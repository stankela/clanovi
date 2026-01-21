using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using NHibernate;
using Soko.Data;
using NHibernate.Context;
using Bilten.Dao;
using Soko.Domain;
using Soko.UI;
using Soko.Misc;
using System.Diagnostics;
using Soko.Exceptions;
using Bilten.Dao.NHibernate;
using RFID.NativeInterface;
using RFID.Utils;
using RawInput_dll;
using System.Collections.Concurrent;
using HidGlobal.OK.Readers;
using HidGlobal.OK.Readers.Components;
using HidGlobal.OK.Readers.AViatoR.Components;
using HidGlobal.OK.SampleCodes.Utilities;

namespace Soko
{
    public abstract class CitacKartica
    {
        protected Object readAndWriteLock = new Object();

        public enum TipKartice
        {
            Prazna,
            Panonit,
            NoviFormat
        }

        private static CitacKartica treningInstance;
        public static CitacKartica TreningInstance
        {
            get
            {
                if (treningInstance == null)
                    //treningInstance = new PanonitCitacKartica(Options.Instance.COMPortReader);
                    treningInstance = new R10ACitacKartica();
                    //treningInstance = new OMNIKEY5422CitacKartica();
                return treningInstance;
            }
        }

        private static CitacKartica uplateInstance;
        public static CitacKartica UplateInstance
        {
            get
            {
                if (uplateInstance == null)
                    //uplateInstance = new PanonitCitacKartica(Options.Instance.COMPortWriter);
                    uplateInstance = new OMNIKEY5422CitacKartica();
                return uplateInstance;
            }
        }

        public virtual void readCard(out int broj, out string serijskiBroj)
        {
            string sID = "";
            string name = "";
            broj = -1;
            serijskiBroj = "-1";

            AdminForm af = Form1.Instance.AdminForm;
            bool measureTime = af != null;

            Stopwatch watch = null;
            if (measureTime)
            {
                watch = Stopwatch.StartNew();
            }

            ulong retval;
            if (Options.Instance.JedinstvenProgram)
            {
                lock (readAndWriteLock)
                {
                    retval = ReadDataCard(ref sID, ref name, ref serijskiBroj);
                }
            }
            else
            {
                retval = ReadDataCard(ref sID, ref name, ref serijskiBroj);
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

        protected virtual ulong ReadDataCard(ref string sID, ref string sName, ref string serialNumber)
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
            if (Options.Instance.JedinstvenProgram)
            {
                lock (readAndWriteLock)
                {
                    retval = WriteDataCard(sID, sName, out serialCardNo);
                }
            }
            else
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
            string sID = "";
            string name = "";
            broj = -1;
            serijskiBroj = "-1";

            ulong retval;
            if (Options.Instance.JedinstvenProgram)
            {
                lock (readAndWriteLock)
                {
                    retval = ReadDataCard(ref sID, ref name, ref serijskiBroj);
                }
            }
            else
            {
                retval = ReadDataCard(ref sID, ref name, ref serijskiBroj);
            }

            // Sesija.Instance.Log("C READ: " + retval.ToString());

            return retval == 1 && dobroFormatiranaKartica(sID, name, out broj);
        }

        public virtual void SetComPort(int comPort)
        {
        }

        public static readonly string NAME_FIELD = "SDV";
        public static readonly int TEST_KARTICA_BROJ = 100000;
        public static readonly string TEST_KARTICA_NAME = "TEST KARTICA";

        protected string DEFAULT_KEY = "FFFFFFFFFFFF";
        protected string KEY = "13072004abcd";

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
            string value = "00000000000000000000000000000000";
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
            // TODO2: Proveri da li je sve u ovom metodu thread safe.
            bool lastRead = false;
            bool pendingClear = false;
            int visibleCount = 0;
            int skipCount = 0;

            while (!_shouldStop)
            {
                if (lastRead)
                {
                    // Preskoci narednih CitacKarticaThreadSkipCount ocitavanja.
                    // Kod radi samo za CitacKarticaThreadSkipCount > 0.
                    lastRead = false;
                    skipCount = 1;
                }
                else
                {
                    if (skipCount > 0 && ++skipCount > Options.Instance.CitacKarticaThreadSkipCount)
                    {
                        skipCount = 0;
                    }
                    if (skipCount == 0)
                    {
                        lastRead = TryReadDolazakNaTrening(pendingClear);
                    }
                }

                if (lastRead)
                {
                    // Prikazivanje treba da bude vidljivo tokom trajanja CitacKarticaThreadVisibleCount intervala.
                    pendingClear = true;
                    visibleCount = 1;
                }
                else if (pendingClear)
                {
                    ++visibleCount;
                    if (visibleCount > Options.Instance.CitacKarticaThreadVisibleCount)
                    {
                        CitacKarticaForm citacKarticaForm = Form1.Instance.CitacKarticaForm;
                        if (citacKarticaForm != null)
                        {
                            citacKarticaForm.Clear();
                        }
                        pendingClear = false;
                    }
                }

                Thread.Sleep(Options.Instance.CitacKarticaThreadInterval);
            }
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
