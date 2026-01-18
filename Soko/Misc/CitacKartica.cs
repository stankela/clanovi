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

namespace Soko
{
    public abstract class CitacKartica
    {
        private static CitacKartica treningInstance;
        public static CitacKartica TreningInstance
        {
            get
            {
                if (treningInstance == null)
                    //treningInstance = new PanonitCitacKartica(Options.Instance.COMPortReader);
                    treningInstance = new R10ACitacKartica();
                return treningInstance;
            }
        }

        private static CitacKartica uplateInstance;
        public static CitacKartica UplateInstance
        {
            get
            {
                if (uplateInstance == null)
                    uplateInstance = new PanonitCitacKartica(Options.Instance.COMPortWriter);
                return uplateInstance;
            }
        }

        public abstract void readCard(out int broj);
        public abstract bool tryReadCard(out int broj);
        public abstract bool writeCard(string sID, out Int64 serialCardNo);

        public virtual void SetComPort(int comPort)
        {
        }

        public static readonly string NAME_FIELD = "SDV";
        public static readonly int TEST_KARTICA_BROJ = 100000;
        public static readonly string TEST_KARTICA_NAME = "TEST KARTICA";

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
            bool result = tryReadCard(out broj);

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

    public class PanonitCitacKartica : CitacKartica
    {
        private Object readAndWriteLock = new Object();

        private RFIDReader mReader;
        private string DEFAULT_KEY = "FFFFFFFFFFFF";
        private string KEY = "13072004abcd";

        private int comPort;

        public override void SetComPort(int comPort)
        {
            this.comPort = comPort;
        }

        private enum TipKartice
        {
            Prazna,
            Panonit,
            NoviFormat
        }

        public PanonitCitacKartica(int comPort)
        {
            this.comPort = comPort;
            mReader = RFIDReader.OpenRFIDReader();
        }

        // NOTE: Thread safe creation
        /*private static Object creationLock = new Object();
        public static CitacKartica Instance
        {
            get
            {
                lock (creationLock)
                {
                    if (instance == null)
                        instance = new CitacKartica();
                    return instance;
                }
            }
        }*/

        private void LogSector(int sectorNo, RFIDReader.RFIDSectorData data)
        {
            Log.Info("", sectorNo.ToString() + "----------------------------");
            Log.Info("", data.Block0);
            Log.Info("", data.Block1);
            Log.Info("", data.Block2);
            Log.Info("", data.PasswordA + " " + data.Control + " " + data.PasswordB);
        }

        // Name is a string which is encoded hexadecimally, so "SDV" -> "53445600"
        private string DecodeName(string encodedName)
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

        private string DecodeID(string encryptedID, string CurrentCardNo)
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

        private ulong ReadDataCardImpl(ref string sID, ref string sName)
        {
            if (!mReader.ConnectDevice(comPort) || !mReader.BindCard(false))
                return 0;

            TipKartice tipKartice;
            int sectorNo = 3;
            RFIDReader.RFIDSectorData data = mReader.ReadData(sectorNo, false, KEY);
            if (data == null)
                tipKartice = TipKartice.Prazna;
            else
            {
                LogSector(sectorNo, data);
                if (data.Block0 == "53445632303233000000000000000000")
                    tipKartice = TipKartice.NoviFormat;
                else
                    tipKartice = TipKartice.Panonit;
            }

            if (tipKartice == TipKartice.Prazna || tipKartice == TipKartice.Panonit)
                return 2;

            // TipKartice je NoviFormat

            sectorNo = 5;
            data = mReader.ReadData(sectorNo, false, KEY);
            if (data == null)
                return 0;
            LogSector(sectorNo, data);
            sName = DecodeName(data.Block0);
            Log.Info("sName", sName);  // TODO4: Iskljuci Log od readera kada sve podesis

            sectorNo = 1;
            data = mReader.ReadData(sectorNo, false, KEY);
            if (data == null)
                return 0;
            LogSector(sectorNo, data);
            sID = DecodeID(data.Block2, mReader.CurrentCardNo);
            Log.Info("sID", sID);
      
            return 1;
        }

        private ulong ReadDataCard(ref string sID, ref string sName)
        {
            ulong retval = ReadDataCardImpl(ref sID, ref sName);
            if (retval == 1)
                mReader.Beep();
            return retval;
        }

        private ulong SetKeys()
        {
            // write key to blocks 7, 15 and 23
            string keyBlock = KEY + "FF078069" + DEFAULT_KEY;
            if (!mReader.WriteDataToBlock(1, 3, false, DEFAULT_KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 7:", keyBlock);
            if (!mReader.WriteDataToBlock(3, 3, false, DEFAULT_KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 15:", keyBlock);
            if (!mReader.WriteDataToBlock(5, 3, false, DEFAULT_KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 23:", keyBlock);
            return 1;
        }

        private string EncodeID(string sID, string CurrentCardNo)
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

        private ulong SetData(string sID, string sName, string CurrentCardNo)
        {
            // sID

            // write encrypted sID to block 6
            string value = EncodeID(sID, CurrentCardNo);
            Log.Info("Write data to block 6:", value);
            if (!mReader.WriteDataToBlock(1, 2, false, KEY, value))
                return 0;

            // sName

            // write "SDV" to block 20
            if (sName != "SDV")
                throw new Exception("Greska u programu.");
            value = "53445600000000000000000000000000";
            Log.Info("Write data to block 20:", value);
            if (!mReader.WriteDataToBlock(5, 0, false, KEY, value))
                return 0;

            // Napravi da kartica bude u formatu TipKartice.NoviFormat - write "SDV2023" to block 12
            value = "53445632303233000000000000000000";
            Log.Info("Write data to block 12:", value);
            if (!mReader.WriteDataToBlock(3, 0, false, KEY, value))
                return 0;

            return 1;
        }

        private ulong WriteDataCardImpl(string sID, string sName, out Int64 serialCardNo)
        {
            //mReader.EnableCheck = false;
            //RFIDReader.Sleep(200);

            serialCardNo = 0;

            if (!mReader.ConnectDevice(comPort) || !mReader.BindCard(false))
                return 0;

            if (Options.Instance.WritePraznaDataCard)
                return WritePraznaDataCard();

            serialCardNo = Convert.ToInt64(mReader.CurrentCardNo, 16);

            if (SetKeys() == 1)  // Prazna kartica
                return SetData(sID, sName, mReader.CurrentCardNo);

            // NoviFormat ili Panonit kartica.

            RFIDReader.Sleep(200);
            if (!mReader.ConnectDevice(comPort) || !mReader.BindCard(false))
                return 0;
            return SetData(sID, sName, mReader.CurrentCardNo);

            //mReader.EnableCheck = true;  // TODO4: RAII ?
        }

        // Konvertuje iz TipKartice.NoviFormat u TipKartice.Prazna
        private ulong WritePraznaDataCard()
        {
            // write default key to blocks 7, 15 and 23
            string keyBlock = DEFAULT_KEY + "FF078069" + DEFAULT_KEY;
            if (!mReader.WriteDataToBlock(1, 3, false, KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 7:", keyBlock);
            if (!mReader.WriteDataToBlock(3, 3, false, KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 15:", keyBlock);
            if (!mReader.WriteDataToBlock(5, 3, false, KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 23:", keyBlock);

            // write zero data to blocks 6, 12 and 20
            string value = "00000000000000000000000000000000";
            Log.Info("Write data to block 6:", value);
            if (!mReader.WriteDataToBlock(1, 2, false, DEFAULT_KEY, value))
                return 0;
            Log.Info("Write data to block 12:", value);
            if (!mReader.WriteDataToBlock(3, 0, false, DEFAULT_KEY, value))
                return 0;
            Log.Info("Write data to block 20:", value);
            if (!mReader.WriteDataToBlock(5, 0, false, DEFAULT_KEY, value))
                return 0;

            //mReader.EnableCheck = true;  // TODO4: RAII ?
            return 1;
        }

        private ulong WriteDataCard(string sID, string sName, out Int64 serialCardNo)
        {
            serialCardNo = 0;
            ulong retval = WriteDataCardImpl(sID, sName, out serialCardNo);
            if (retval == 1)
                mReader.Beep();
            return retval;
        }

        public override void readCard(out int broj)
        {
            string sID = "";
            string name = "";
            broj = -1;

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
                    retval = ReadDataCard(ref sID, ref name);
                }
            }
            else
            {
                retval = ReadDataCard(ref sID, ref name);
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

        public override bool tryReadCard(out int broj)
        {
            string sID = "";
            string name = "";
            broj = -1;

            ulong retval;
            if (Options.Instance.JedinstvenProgram)
            {
                lock (readAndWriteLock)
                {
                    retval = ReadDataCard(ref sID, ref name);
                }
            }
            else
            {
                retval = ReadDataCard(ref sID, ref name);
            }

            // Sesija.Instance.Log("C READ: " + retval.ToString());

            return retval == 1 && dobroFormatiranaKartica(sID, name, out broj);
        }

        private bool dobroFormatiranaKartica(string sID, string name, out int broj)
        {
            return Int32.TryParse(sID, out broj) && broj > 0 && name == NAME_FIELD;
        }

        public override bool writeCard(string sID, out Int64 serialCardNo)
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
    }

    public class R10ACitacKartica : CitacKartica
    {
        public R10ACitacKartica()
        {
            Form1.Instance.CitacKarticaForm.GetRawInput().KeyPressed += OnKeyPressed;
            // TODO4: Potrebno je negde odraditi GetRawInput().KeyPressed -= OnKeyPressed;
        }

        class KeyEvent
        {
            public string VKeyName;
            public string KeyPressState;

            public KeyEvent(string name, string state)
            {
                VKeyName = name;
                KeyPressState = state;
            }
        }

        private int index = 0;
        private KeyEvent[] buffer = null;  // circular buffer of last 28 digits
        private static ConcurrentQueue<string> concurrentQueue = new ConcurrentQueue<string>();

        private void OnKeyPressed(object sender, RawInputEventArg e)
        {
            if (buffer == null)
            {
                buffer = new KeyEvent[28];  // circular buffer of last 28 digits
                for (int i = 0; i < 28; ++i)
                {
                    buffer[i] = new KeyEvent("", "");
                }
            }
            buffer[index] = new KeyEvent(e.KeyPressEvent.VKeyName, e.KeyPressEvent.KeyPressState);
            index = (index + 1) % 28;
            if (e.KeyPressEvent.VKeyName != "ENTER" || e.KeyPressEvent.KeyPressState != "BREAK")
            {
                return;
            }
            // Detektovali smo zadnji karakter u nizu. Proveri svih 28 karaktera
            string input = checkInput(index);
            if (input != "")
            {
                concurrentQueue.Enqueue(input);
            }
        }

        private string checkInput(int index)
        {
            // Primer korektnog inputa je ";0722287837?", plus ENTER na kraju. Za svaki karakter imamo MAKE i BREAK.
            // MAKE predstavlja pritisak tastera, BREAK predstavlja otpustanje. Znaku '?' prethodi desni SHIFT
            // (tj. RSHIFT)
            string result = "";
            string[] vKeyNames = new string[28] {
                "OEMSEMICOLON",
                "OEMSEMICOLON",
                /* proizvoljnih deset cifara. svaka cifra se ponavlja dva puta (pritisak tastera i otpustanje */
                "D0","D0","D1","D1","D2","D2","D3","D3","D4","D4","D5","D5","D6","D6","D7","D7","D8","D8","D9","D9",
                "RSHIFT", "OEMQUESTION", "RSHIFT", "OEMQUESTION", "ENTER", "ENTER"
            };
            string[] vKeyPressStates = new string[28] {
                "MAKE",
                "BREAK",
                "MAKE","BREAK","MAKE","BREAK","MAKE","BREAK","MAKE","BREAK","MAKE","BREAK",
                "MAKE","BREAK","MAKE","BREAK","MAKE","BREAK","MAKE","BREAK","MAKE","BREAK",
                "MAKE", "MAKE", "BREAK", "BREAK", "MAKE", "BREAK"
            };
            for (int i = 0; i < 28; ++i, index = (index + 1) % 28)
            {
                KeyEvent e = buffer[index];
                if (e.KeyPressState != vKeyPressStates[i])
                    return "";
                if (i < 2 || i >= 22)
                {
                    if (i == 23 || i == 25)
                    {
                        // Ovaj slucaj obradjujem posebno, zato sto je na srpskoj tastaturi znak '?' u stvari '-',
                        // tj. umesto OEMQUESTION imamo OEMMINUS
                        // TODO4: Mozda da izostavim proveru za ova dva indeksa (23 i 25), zato sto bi za neku
                        // tastaturu koja nije ni srpska ni engleska mogla da se pojavi neka treca vrednost. Takodje,
                        // i za znak ';' bi nesto slicno moglo da se desi.
                        if (e.VKeyName != "OEMQUESTION" && e.VKeyName != "OEMMINUS")
                            return "";
                    }
                    else if (e.VKeyName != vKeyNames[i])
                        return "";
                }
                else
                {
                    if (!isDigit(e.VKeyName))
                        return "";
                    if (e.KeyPressState == "MAKE")
                    {
                        int nextIndex = (index + 1) % 28;
                        KeyEvent nextEv = buffer[nextIndex];
                        if (nextEv.KeyPressState != "BREAK")
                            return "";
                    }
                    else if (e.KeyPressState == "BREAK")
                    {
                        int prevIndex = index - 1;
                        if (prevIndex == -1)
                            prevIndex = 27;
                        KeyEvent prevEv = buffer[prevIndex];
                        if (prevEv.KeyPressState != "MAKE")
                            return "";
                        char digit = getDigit(e.VKeyName);
                        if (digit != getDigit(prevEv.VKeyName))
                            return "";
                        result += digit;
                    }
                    else
                        return "";
                }
            }
            return result;
        }

        private bool isDigit(string vKeyName)
        {
            if (vKeyName.Length != 2 || vKeyName[0] != 'D')
                return false;
            switch (vKeyName[1])
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return true;
                default:
                    return false;
            }
        }

        private char getDigit(string vKeyName)
        {
            return vKeyName[1];
        }

        public override bool tryReadCard(out int broj)
        {
            broj = 9241;
            string item;
            if (concurrentQueue.TryDequeue(out item))
            {
                return true;
            }
            return false;
        }

        public override void readCard(out int broj)
        {
            throw new NotImplementedException();
        }

        public override bool writeCard(string sID, out long serialCardNo)
        {
            throw new NotImplementedException();
        }
    }
}
