using System;
using System.Threading;
using System.Drawing;
using NHibernate;
using Soko.Data;
using Bilten.Dao;
using Soko.Domain;
using Soko.UI;
using Soko.Misc;
using System.Diagnostics;
using Soko.Exceptions;
using Bilten.Dao.NHibernate;
using RFID.NativeInterface;

namespace Soko
{
    public abstract class CitacKartica
    {
        protected Object readAndWriteLock = new Object();
        protected static Object createLock = new Object();

        public enum TipKartice
        {
            Prazna,
            Panonit,
            NoviFormat
        }

        public enum TipCitaca
        {
            Panonit,
            OMNIKEY5422,
            R10A
        }

        public static TipCitaca[] GetTipoviCitaca()
        {
            return (TipCitaca[])Enum.GetValues(typeof(TipCitaca));
        }

        private static CitacKartica treningInstance;
        public static CitacKartica TreningInstance
        {
            get
            {
                lock (createLock)
                {
                    if (treningInstance == null)
                    {
                        InitTreningInstanceFromOptions();
                    }
                    return treningInstance;
                }
            }
        }

        private static CitacKartica uplateInstance;
        public static CitacKartica UplateInstance
        {
            get
            {
                lock (createLock)
                {
                    if (uplateInstance == null)
                    {
                        InitUplateInstanceFromOptions();
                    }
                    return uplateInstance;
                }
            }
        }

        public static void UpdateUplateInstanceFromOptions()
        {
            InitUplateInstanceFromOptions();
        }

        private static void InitUplateInstanceFromOptions()
        {
            TipCitaca tipCitaca = CitacKartica.GetTipoviCitaca()[Options.Instance.CitacKarticaUplate];
            switch (tipCitaca)
            {
                case TipCitaca.Panonit:
                    uplateInstance = new PanonitCitacKartica(Options.Instance.COMPortWriter);
                    break;
                case TipCitaca.OMNIKEY5422:
                    uplateInstance = new OMNIKEY5422CitacKartica();
                    break;
                case TipCitaca.R10A:
                    throw new Exception("Citac kartica R10A ne podrzava upisivanje na karticu");
            }
        }

        public static void UpdateTreningInstanceFromOptions()
        {
            StopTreningInstance();

            CitacKarticaForm citacKarticaForm = Form1.Instance.CitacKarticaForm;
            if (citacKarticaForm != null)
            {
                citacKarticaForm.Clear();
            }

            InitTreningInstanceFromOptions();

            Form1.Instance.pokreniCitacKarticaThread();
        }

        private static void InitTreningInstanceFromOptions()
        {
            TipCitaca tipCitaca = CitacKartica.GetTipoviCitaca()[Options.Instance.CitacKarticaTrening];
            switch (tipCitaca)
            {
                case TipCitaca.Panonit:
                    treningInstance = new PanonitCitacKartica(Options.Instance.COMPortReader);
                    break;
                case TipCitaca.OMNIKEY5422:
                    treningInstance = new OMNIKEY5422CitacKartica();
                    break;
                case TipCitaca.R10A:
                    treningInstance = new R10ACitacKartica();
                    break;
            }
        }

        private static void StopTreningInstance()
        {
            lock (createLock)
            {
                if (treningInstance != null)
                {
                    treningInstance.RequestStop();
                    while (treningInstance.IsRunning())
                    {
                        Thread.Sleep(500);
                    }
                    treningInstance.Cleanup();
                }
                treningInstance = null;
            }
        }

        // TODO4: Ovo bi trebalo pomocu Dispose
        public virtual void Cleanup()
        {
        }

        public virtual void readCard(out int broj, out string serijskiBroj)
        {
            string sID;
            string name;
            broj = -1;

            AdminForm af = Form1.Instance.AdminForm;
            bool measureTime = af != null;

            Stopwatch watch = null;
            if (measureTime)
            {
                watch = Stopwatch.StartNew();
            }

            ulong retval;
            lock (readAndWriteLock)
            {
                retval = ReadDataCard(out sID, out name, out serijskiBroj);
            }

            if (measureTime)
            {
                watch.Stop();
                af.newCitanjeKartice(retval, watch.ElapsedMilliseconds);
            }

            if (retval == 1)
            {
                if (!dobroFormatiranaKartica(sID, name, out broj))
                {
                    throw new ReadCardException("Lose formatirana kartica.");
                }
            }
            else if (retval == 2)
            {
                string msg = "Kartica je ili prazna ili u starom formatu. " +
                    "Idite u meni 'Pravljenje kartice' i napravite karticu.";
                throw new ReadCardException(msg);
            }
            else
            {
                string msg = "Neuspesno citanje kartice. " +
                    "Proverite da li je uredjaj prikljucen, i da li je podesen COM port.";
                throw new ReadCardException(msg);
            }
        }

        public static bool ValidateCitacUplate(TipCitaca citacUplate, out string errorMsg)
        {
            errorMsg = "";
            if (citacUplate != TipCitaca.R10A)
                return true;
            errorMsg = "Citac kartica R10A ne podrzava upisivanje kartica.";
            return false;
        }

        public static bool ValidateCitacTrening(TipCitaca citacTrening, out string errorMsg)
        {
            errorMsg = "";
            return true;
        }

        protected virtual ulong ReadDataCard(out string sID, out string sName, out string serialNumber)
        {
            throw new Exception("Method ReadDataCard should be overriden by the derived class");
        }

        public virtual bool writeCard(string sID, out Int64 serialCardNo)
        {
            string sName = CitacKartica.NAME_FIELD;

            AdminForm af = Form1.Instance.AdminForm;
            bool measureTime = af != null;

            Stopwatch watch = null;
            if (measureTime)
            {
                watch = Stopwatch.StartNew();
            }

            ulong retval;
            lock (readAndWriteLock)
            {
                retval = WriteDataCard(sID, sName, out serialCardNo);
            }

            if (measureTime)
            {
                watch.Stop();
                af.newPisanjeKartice(retval, watch.ElapsedMilliseconds);
            }

            return retval == 1;
        }

        protected virtual ulong WriteDataCard(string sID, string sName, out Int64 serialCardNo)
        {
            throw new Exception("Method WriteDataCard should be overriden by the derived class");
        }

        public virtual bool tryReadCard(out int broj, out string serijskiBroj)
        {
            string sID;
            string name;
            broj = -1;

            ulong retval;
            lock (readAndWriteLock)
            {
                retval = ReadDataCard(out sID, out name, out serijskiBroj);
            }

            // Sesija.Instance.Log("C READ: " + retval.ToString());

            return retval == 1 && dobroFormatiranaKartica(sID, name, out broj);
        }

        private bool running = false;
        public bool IsRunning()
        {
            return running;
        }

        public static readonly string NAME_FIELD = "SDV";
        public static readonly int TEST_KARTICA_BROJ = 100000;
        public static readonly string TEST_KARTICA_NAME = "TEST KARTICA";

        protected static readonly string DEFAULT_KEY = "FFFFFFFFFFFF";
        protected static readonly string KEY = "13072004abcd";
        protected static readonly string SDV_BLOCK = "53445600000000000000000000000000";
        protected static readonly string SDV2023_BLOCK = "53445632303233000000000000000000";
        protected static readonly string ZERO_BLOCK = "00000000000000000000000000000000";

        // The structure of the key block is as follows: KEY is the A key for the sector. FF078069 are access bits
        // for that sector (they define what data is modifiable with which key). DEFAULT_KEY is the B key.
        protected static readonly string KEY_BLOCK = KEY + "FF078069" + DEFAULT_KEY;
        protected static readonly string DEFAULT_KEY_BLOCK = DEFAULT_KEY + "FF078069" + DEFAULT_KEY;

        protected TipKartice getTipKartice(string block12Data)
        {
            if (String.IsNullOrEmpty(block12Data))
                return TipKartice.Prazna;
            if (block12Data == SDV2023_BLOCK)
                return TipKartice.NoviFormat;
            else if (block12Data == ZERO_BLOCK)
                return TipKartice.Prazna;
            else
                return TipKartice.Panonit;
        }

        protected bool dobroFormatiranaKartica(string sID, string name, out int broj)
        {
            return Int32.TryParse(sID, out broj) && broj > 0 && name == NAME_FIELD;
        }

        // Name is a string which is encoded hexadecimally, so "SDV" -> "53445600"
        protected string DecodeName(string encodedName)
        {
            String res = String.Empty;
            for (int i = 0; i < encodedName.Length - 1; i = i + 2)
            {
                if (encodedName[i] == '0' && encodedName[i + 1] == '0')
                    break;
                int index = 0;
                res += Uri.HexUnescape("%" + encodedName.Substring(i, 2), ref index);
                if (index == 123)
                    break;
            }
            return res;
        }

        protected string DecodeID(string encryptedID, string CurrentCardNo)
        {
            byte[] encryptedBytes = RFIDReader.ToDigitsBytes(encryptedID);
            byte[] decryptedBytes = EncryptionHelper.Decrypt(encryptedBytes);
            string encodedID = RFIDReader.ToHexString(decryptedBytes);

            // Format je "n6 n1 n5 id1 n7 n0 id0 n2 n4 n3" + ostatak id
            char[] cardNo = new char[8];
            cardNo[6] = encodedID[0];
            cardNo[1] = encodedID[1];
            cardNo[5] = encodedID[2];
            cardNo[7] = encodedID[4];
            cardNo[0] = encodedID[5];
            cardNo[2] = encodedID[7];
            cardNo[4] = encodedID[8];
            cardNo[3] = encodedID[9];
            if (new String(cardNo) != CurrentCardNo.Substring(0, 8))
                throw new ReadCardException("Lose formatirana kartica.");

            String hex_encoded_id_digit = String.Empty;
            hex_encoded_id_digit += encodedID[6];
            hex_encoded_id_digit += encodedID[3];
            String res = String.Empty;
            int index = 0;
            res += Uri.HexUnescape("%" + hex_encoded_id_digit, ref index);
            for (int i = 10; i < encodedID.Length - 1; i += 2)
            {
                if (encodedID[i] == '0' && encodedID[i + 1] == '0')
                    break;
                if ((i / 2) % 2 == 0)
                {
                    hex_encoded_id_digit = encodedID.Substring(i, 2);
                }
                else
                {
                    hex_encoded_id_digit = String.Empty;
                    hex_encoded_id_digit += encodedID[i + 1];
                    hex_encoded_id_digit += encodedID[i];
                }
                index = 0;
                res += Uri.HexUnescape("%" + hex_encoded_id_digit, ref index);
            }
            return res;
        }

        protected string EncodeID(string sID, string CurrentCardNo)
        {
            string value = ZERO_BLOCK;
            char[] value2 = value.ToCharArray();

            // Format je "n6 n1 n5 id1 n7 n0 id0 n2 n4 n3" + ostatak id
            string hex = Uri.HexEscape(sID[0]);
            value2[0] = CurrentCardNo[6];
            value2[1] = CurrentCardNo[1];
            value2[2] = CurrentCardNo[5];
            value2[3] = hex[2];
            value2[4] = CurrentCardNo[7];
            value2[5] = CurrentCardNo[0];
            value2[6] = hex[1];
            value2[7] = CurrentCardNo[2];
            value2[8] = CurrentCardNo[4];
            value2[9] = CurrentCardNo[3];

            int j = 10;
            for (int i = 1; i < sID.Length; ++i, j += 2)
            {
                hex = Uri.HexEscape(sID[i]);
                // hex[0] contains "%"
                if (i % 2 == 0)
                {
                    value2[j] = hex[1];
                    value2[j + 1] = hex[2];
                }
                else
                {
                    value2[j] = hex[2];
                    value2[j + 1] = hex[1];
                }
            }
            // Encrypt the value
            byte[] hexUnescapedBytes = RFIDReader.ToDigitsBytes(new String(value2));
            byte[] encryptedBytes = EncryptionHelper.Encrypt(hexUnescapedBytes);
            return RFIDReader.ToHexString(encryptedBytes);
        }

        // Volatile is used as hint to the compiler that this data 
        // member will be accessed by multiple threads. 
        private volatile bool _shouldStop = false;

        public void RequestStop()
        {
            _shouldStop = true;
        }

        public void ReadLoop()
        {
            // TODO4: Proveri da li je sve u ovom metodu thread safe.
            running = true;

            // Koliko dugo treba da bude vidljivo ocitavanje
            int visibleCount = 0;
            // Koliko dugo treba sacekati pre nego sto bude moguce novo ocitavanje
            int skipCount = 0;
            // Da li prilikom prikazivanja novog ocitavanja postoji prethodno ocitavanje koje treba obrisati
            bool pendingClear = false;

            // Kod pretpostavlja da je CitacKarticaThreadSkipCount <= CitacKarticaThreadVisibleCount
            if (Options.Instance.CitacKarticaThreadSkipCount > Options.Instance.CitacKarticaThreadVisibleCount)
                throw new Exception("Greska u programu");

            while (true)
            {
                if (skipCount > 0)
                {
                    --skipCount;
                }
                if (visibleCount > 0)
                {
                    --visibleCount;
                }
                if (skipCount == 0)
                {
                    // Prestani sa ocitavanjem ako je stigao StopRequest
                    if (!_shouldStop && TryReadDolazakNaTrening(pendingClear))
                    {
                        skipCount = Options.Instance.CitacKarticaThreadSkipCount;
                        visibleCount = Options.Instance.CitacKarticaThreadVisibleCount;
                        pendingClear = true;
                    }
                    else if (visibleCount == 0)
                    {
                        // Zavrsi sa ReadLoop tek nakon sto je poslednje ocitavanje kompletiralo svoj interval na
                        // displeju. citacKarticaForm.Clear() ce biti pozvan u UI threadu, u
                        // UpdateTreningInstanceFromOptions() (imao sam problema kada sam pozivao u ReadLoop threadu).
                        if (_shouldStop) break;

                        if (pendingClear)
                        {
                            CitacKarticaForm citacKarticaForm = Form1.Instance.CitacKarticaForm;
                            if (citacKarticaForm != null)
                            {
                                citacKarticaForm.Clear();
                            }
                            pendingClear = false;
                        }
                    }
                }
                // inace (ako je skipCount > 0) ne radimo nista jer znamo da je visibleCount > 0

                Thread.Sleep(TimeSpan.FromMilliseconds(Options.Instance.CitacKarticaThreadInterval));
            }
            running = false;
        }

        private bool TryReadDolazakNaTrening(bool obrisiPrePrikazivanja)
        {
            AdminForm af = Form1.Instance.AdminForm;
            bool measureTime = af != null;

            Stopwatch watch = null;
            if (measureTime)
            {
                watch = Stopwatch.StartNew();
            }

            int broj;
            string serijskiBroj;
            bool result = tryReadCard(out broj, out serijskiBroj);

            if (measureTime)
            {
                watch.Stop();
                af.newOcitavanje(watch.ElapsedMilliseconds);
            }

            return result && handleOcitavanjeKarticeTrening(broj, DateTime.Now, obrisiPrePrikazivanja);
        }

        public static bool handleOcitavanjeKarticeTrening(int broj, DateTime vremeOcitavanja,
            bool obrisiPrePrikazivanja)
        {
            CitacKarticaForm citacKarticaForm = Form1.Instance.CitacKarticaForm;
            if (obrisiPrePrikazivanja)
            {
                // Kartica je ocitana, a na displeju je prikaz prethodnog ocitavanja. Obrisi prethodno ocitavanje
                // i pauziraj (tako da se vidi prazan displej), pre nego sto prikazes naredno ocitavanje.
                if (citacKarticaForm != null)
                {
                    citacKarticaForm.Clear();
                    Thread.Sleep(Options.Instance.CitacKarticaThreadPauzaZaBrisanje);
                }
            }

            if (broj == TEST_KARTICA_BROJ)
            {
                if (citacKarticaForm != null)
                {
                    string msg = FormatMessage(broj, null, TEST_KARTICA_NAME);
                    citacKarticaForm.Prikazi(msg, Options.Instance.PozadinaCitacaKartica);
                }
                return true;
            }

            Sesija.Instance.OnOcitavanjeKartice(broj, vremeOcitavanja);

            Clan clan = CitacKarticaDictionary.Instance.findClan(broj);
            if (clan == null)
                return false;

            // Odmah prikazi ocitavanje, da bi se momentalno videlo na ekranu nakon zvuka ocitavanja kartice.
            UplataClanarine uplata;
            prikaziOcitavanje(clan, vremeOcitavanja, out uplata);

            unesiOcitavanje(clan, vremeOcitavanja, uplata);
            return true;
        }

        private static void unesiOcitavanje(Clan clan, DateTime vremeOcitavanja, UplataClanarine uplata)
        {
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    // NOTE: DolazakNaTreningDAO (vidi dole) ne uzima session iz CurrentSessionContext zato sto planiram
                    // da metod unesiOcitavanje izvrsavam u posebnom threadu.

                    // CurrentSessionContext.Bind(session);

                    DolazakNaTrening dolazak = new DolazakNaTrening();
                    dolazak.Clan = clan;
                    dolazak.DatumVremeDolaska = vremeOcitavanja;
                    if (uplata != null && !clan.NeplacaClanarinu)
                    {
                        dolazak.Grupa = uplata.Grupa;
                    }
                    else
                    {
                        dolazak.Grupa = null;
                    }

                    DolazakNaTreningDAOImpl dolazakNaTreningDAO =
                        DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO() as DolazakNaTreningDAOImpl;
                    dolazakNaTreningDAO.Session = session;
                    dolazakNaTreningDAO.MakePersistent(dolazak);

                    if (CitacKarticaDictionary.Instance.DanasnjaOcitavanja.Add(clan.Id))
                    {
                        DolazakNaTreningMesecniDAOImpl dolazakNaTreningMesecniDAO
                            = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningMesecniDAO() as DolazakNaTreningMesecniDAOImpl;
                        dolazakNaTreningMesecniDAO.Session = session;
                        DolazakNaTreningMesecni dolazakMesecni = dolazakNaTreningMesecniDAO.getDolazakNaTrening(dolazak.Clan,
                            dolazak.DatumDolaska.Value.Year, dolazak.DatumDolaska.Value.Month);
                        if (dolazakMesecni == null)
                        {
                            dolazakMesecni = new DolazakNaTreningMesecni();
                            dolazakMesecni.Clan = clan;
                            dolazakMesecni.Godina = vremeOcitavanja.Year;
                            dolazakMesecni.Mesec = vremeOcitavanja.Month;
                            dolazakMesecni.BrojDolazaka = 1;
                        }
                        else
                        {
                            ++dolazakMesecni.BrojDolazaka;
                        }
                        dolazakNaTreningMesecniDAO.MakePersistent(dolazakMesecni);
                    }
                    session.Transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(ex.Message, "Citac kartica");
            }
            finally
            {
                // CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }
        }

        private static void prikaziOcitavanje(Clan clan, DateTime vremeOcitavanja, out UplataClanarine uplata)
        {
            uplata = CitacKarticaDictionary.Instance.findUplata(clan);

            bool okForTrening = false;
            if (uplata != null)
            {
                // Moguce da je clan uplatio clanarinu npr. pre 3 meseca, a tek ovog meseca mu je promenjen status
                // u neplaca clanarinu. U tom slucaju treba da svetli zeleno.
                if (clan.NeplacaClanarinu)
                    okForTrening = true;
                else if (uplata.Grupa.ImaGodisnjuClanarinu)
                    okForTrening = uplata.VaziOd.Value.Year == vremeOcitavanja.Year;
                else
                {
                    // Proveri da li postoji uplata za ovaj ili sledeci mesec.
                    okForTrening = uplata.VaziOd.Value.Year == vremeOcitavanja.Year
                        && uplata.VaziOd.Value.Month == vremeOcitavanja.Month;
                    if (!okForTrening)
                    {
                        DateTime sledeciMesec = DateTime.Now.AddMonths(1);
                        okForTrening = uplata.VaziOd.Value.Year == sledeciMesec.Year
                            && uplata.VaziOd.Value.Month == sledeciMesec.Month;
                    }
                }
            }
            else
            {
                okForTrening = clan.NeplacaClanarinu;
            }

            // Tolerisi do odredjenog dana u mesecu, ali ne i za godisnje clanarine.
            if (!okForTrening)
            {
                if (uplata != null && uplata.Grupa.ImaGodisnjuClanarinu)
                {
                    okForTrening = vremeOcitavanja.Month <= Options.Instance.PoslednjiMesecZaGodisnjeClanarine;
                }
                else
                    okForTrening = vremeOcitavanja.Day <= Options.Instance.PoslednjiDanZaUplate;
            }

            string grupa = null;
            if (uplata != null)
            {
                grupa = uplata.Grupa.Naziv;
            }
            string msg = FormatMessage(clan.BrojKartice.Value, clan, grupa);

            // Posto ocitavanje kartice traje relativno dugo (oko 374 ms), moguce je da je prozor
            // zatvoren bas u trenutku dok se kartica ocitava. Korisnik je u tom slucaju cuo zvuk
            // da je kartica ocitana ali se na displeju ne prikazuje da je kartica ocitana.
            CitacKarticaForm form = Form1.Instance.CitacKarticaForm;
            if (form != null)
            {
                Color color = Options.Instance.PozadinaCitacaKartica;
                if (Options.Instance.PrikaziBojeKodOcitavanja)
                {
                    if (okForTrening)
                    {
                        color = Color.SpringGreen;
                    }
                    else
                    {
                        color = Color.Red;
                    }
                }
                form.Prikazi(msg, color);
            }
        }

        public static string FormatMessage(int broj, Clan clan, string grupa)
        {
            string result = String.Empty;
            if (Options.Instance.PrikaziBrojClanaKodOcitavanjaKartice)
            {
                result = broj.ToString();
            }
            if (clan != null && Options.Instance.PrikaziImeClanaKodOcitavanjaKartice)
            {
                if (result != String.Empty)
                    result += "   ";
                result += clan.PrezimeIme;
            }
            if (grupa != null)
            {
                if (result != String.Empty)
                    result += "\n";
                result += grupa;
            }
            return result;
        }
    }
}
