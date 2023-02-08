﻿using System;
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

namespace Soko
{
    public class CitacKartica
    {
        [DllImport("PanReaderIf.dll")]
        private static extern ulong ReadDataCard(int comport, ref string sType, ref string sID1, ref string sID2, ref string sName);
        [DllImport("PanReaderIf.dll")]
        private static extern ulong WriteDataCard(int comport, string sType, string sID1, string sID2, string sName);
        [DllImport("PanReaderIf.dll")]
        private static extern ulong WaitDataCard(int comport, int nSecs);
        [DllImport("PanReaderIf.dll")]
        private static extern ulong WaitAndReadDataCard(int comport, int nSecs, ref string sType, ref string sID1, ref string sID2, ref string sName);

        private Object readAndWriteLock = new Object();

        // Volatile is used as hint to the compiler that this data 
        // member will be accessed by multiple threads. 
        private volatile bool _shouldStop = false;

        private RFIDReader mReader;
        private string DEFAULT_KEY = "FFFFFFFFFFFF";
        private string KEY = "13072004abcd";

        private enum TipKartice
        {
            Prazna,
            Panonit,
            NoviFormat
        }

        private CitacKartica()
        {
            mReader = RFIDReader.OpenRFIDReader();
        }

        private static CitacKartica instance;
        public static CitacKartica Instance
        {
            get
            {
                if (instance == null)
                    instance = new CitacKartica();
                return instance;
            }
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

        public static readonly string NAME_FIELD = "SDV";
        public static readonly int TEST_KARTICA_BROJ = 100000;
        public static readonly string TEST_KARTICA_NAME = "TEST KARTICA";

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

        private string DecodeID1(string encodedID1)
        {
            return DecodeName(encodedID1);
        }

        private ulong NewReadDataCard(int comport, ref string sID1, ref string sName)
        {
            if (!mReader.ConnectDevice(comport) || !mReader.BindCard(false))
                return 0;

            //setCardNo(mReader.CurrentCardNo);

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
            byte[] encryptedBytes = RFIDReader.ToDigitsBytes(data.Block2);
            byte[] decryptedBytes = EncryptionHelper.Decrypt(encryptedBytes);
            sID1 = DecodeID1(RFIDReader.ToHexString(decryptedBytes));
            Log.Info("sID1", sID1);
      
            return 1;
        }

        private ulong MyReadDataCard(int comport, ref string sType, ref string sID1, ref string sID2, ref string sName)
        {
            if (Options.Instance.UseNewCardFormat)
            {
                ulong retval = NewReadDataCard(comport, ref sID1, ref sName);
                if (retval == 1)
                    mReader.Beep();
                return retval;
            }
            else
                return ReadDataCard(comport, ref sType, ref sID1, ref sID2, ref sName) & 0xFFFFFFFF;
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

        private string EncryptID1(string sID1)
        {
            string value = "00000000000000000000000000000000";
            char[] value2 = value.ToCharArray();
            for (int i = 0; i < sID1.Length; ++i)
            {
                string hex = Uri.HexEscape(sID1[i]);
                // hex[0] contains "%"
                value2[2 * i] = hex[1];
                value2[2 * i + 1] = hex[2];
            }
            byte[] hexUnescapedBytes = RFIDReader.ToDigitsBytes(new String(value2));
            byte[] encryptedBytes = EncryptionHelper.Encrypt(hexUnescapedBytes);
            return RFIDReader.ToHexString(encryptedBytes);
        }

        private ulong SetData(string sID1, string sName)
        {
            // sID1

            // write encrypted sID1 to block 6
            string encryptedValue = EncryptID1(sID1);
            Log.Info("Write data to block 6:", encryptedValue);
            if (!mReader.WriteDataToBlock(1, 2, false, KEY, encryptedValue))
                return 0;

            // sName

            // write "SDV" to block 20
            if (sName != "SDV")
                throw new Exception("Greska u programu.");
            string value = "53445600000000000000000000000000";
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

        private ulong NewWriteDataCard(int comport, string sID1, string sName)
        {
            //mReader.EnableCheck = false;
            //RFIDReader.Sleep(200);

            if (!mReader.ConnectDevice(comport) || !mReader.BindCard(false))
                return 0;

            if (Options.Instance.WritePanonitDataCard)
                return WritePanonitDataCard();
            else if (Options.Instance.WritePraznaDataCard)
                return WritePraznaDataCard();

            if (SetKeys() == 1)  // Prazna kartica
                return SetData(sID1, sName);

            // NoviFormat ili Panonit kartica.

            RFIDReader.Sleep(200);
            if (!mReader.ConnectDevice(comport) || !mReader.BindCard(false))
                return 0;
            return SetData(sID1, sName);

            //mReader.EnableCheck = true;  // TODO4: RAII ?
        }

        // Konvertuje iz TipKartice.NoviFormat u TipKartice.Panonit
        private ulong WritePanonitDataCard()
        {
            string value = "0000000EBA0A74770000000000000000";
            Log.Info("Write data to block 6:", value);
            if (!mReader.WriteDataToBlock(1, 2, false, KEY, value))
                return 0;

            value = "6FB602642150981F9ADAECC8713CDC07";
            Log.Info("Write data to block 12:", value);
            if (!mReader.WriteDataToBlock(3, 0, false, KEY, value))
                return 0;

            value = "53445600000000000000000000000000";
            Log.Info("Write data to block 20:", value);
            if (!mReader.WriteDataToBlock(5, 0, false, KEY, value))
                return 0;

            //mReader.EnableCheck = true;  // TODO4: RAII ?
            return 1;
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

        private ulong MyWriteDataCard(int comport, string sType, string sID1, string sID2, string sName)
        {
            if (Options.Instance.UseNewCardFormat)
            {
                ulong retval = NewWriteDataCard(comport, sID1, sName);
                if (retval == 1)
                    mReader.Beep();
                return retval;
            }
            else
                return WriteDataCard(comport, sType, sID1, sID2, sName) & 0xFFFFFFFF;
        }

        public void readCard(int comPort, out int broj)
        {
            string sType = " ";
            string sID1 = "          ";
            string sID2 = "          ";
            string name = "                                ";
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
                    retval = MyReadDataCard(comPort, ref sType, ref sID1, ref sID2, ref name);
                }
            }
            else
            {
                retval = MyReadDataCard(comPort, ref sType, ref sID1, ref sID2, ref name);
            }
            
            if (measureTime)
            {
                watch.Stop();
                af.newCitanjeKartice(retval, watch.ElapsedMilliseconds);
            }

            if (retval == 1)
            {
                if (!dobroFormatiranaKartica(sID1, name, out broj))
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

        public bool tryReadCard(int comPort, out int broj)
        {
            string sType = " ";
            string sID1 = "          ";
            string sID2 = "          ";
            string name = "                                ";
            broj = -1;

            ulong retval;
            if (Options.Instance.JedinstvenProgram)
            {
                lock (readAndWriteLock)
                {
                    retval = MyReadDataCard(comPort, ref sType, ref sID1, ref sID2, ref name);
                }
            }
            else
            {
                retval = MyReadDataCard(comPort, ref sType, ref sID1, ref sID2, ref name);
            }

            // Sesija.Instance.Log("C READ: " + retval.ToString());

            return retval == 1 && dobroFormatiranaKartica(sID1, name, out broj);
        }

        private bool dobroFormatiranaKartica(string sID1, string name, out int broj)
        {
            return Int32.TryParse(sID1, out broj) && broj > 0 && name == NAME_FIELD;
        }

        public bool writeCard(int comPort, string sID1)
        {
            string sType = "";
            string sID2 = "";
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
                    retval = MyWriteDataCard(comPort, sType, sID1, sID2, sName);
                }
            }
            else
            {
                retval = MyWriteDataCard(comPort, sType, sID1, sID2, sName);
            }

            if (measureTime)
            {
                watch.Stop();
                af.newPisanjeKartice(retval, watch.ElapsedMilliseconds);
            }

            return retval == 1;
        }

        public bool TryReadDolazakNaTrening(int comPort, bool obrisiPrePrikazivanja)
        {
            AdminForm af = Form1.Instance.AdminForm;
            bool measureTime = af != null;

            Stopwatch watch = null;
            if (measureTime)
            {
                watch = Stopwatch.StartNew();
            }

            int broj;
            bool result = tryReadCard(comPort, out broj);

            if (measureTime)
            {
                watch.Stop();
                af.newOcitavanje(watch.ElapsedMilliseconds);
            }

            return result && handleOcitavanjeKarticeTrening(broj, DateTime.Now, obrisiPrePrikazivanja);
        }

        public void WaitAndReadLoop()
        {
            // TODO2: Proveri da li je sve u ovom metodu thread safe.
            while (!_shouldStop)
            {
                // NOTE: Izabran je mali vremenski interval 2 sec (a ne recimo 10 sec), zato sto kada se program zatvori
                // WaitAndReadDataCard je i dalje aktivan dok ne istekne interval, a samim tim i proces je i dalje
                // aktivan, i nije moguce ponovo restartovanje programa (ili je moguce ali imamo istovremeno dva
                // procesa).

                int nSecs = Options.Instance.NumSecondsWaitAndRead;

                string sType = " ";
                string sID1 = "          ";
                string sID2 = "          ";

                string name = "                                ";
                int broj = -1;

                ulong retval;
                if (Options.Instance.JedinstvenProgram)
                {
                    lock (readAndWriteLock)
                    {
                        retval = WaitAndReadDataCard(Options.Instance.COMPortReader, nSecs,
                           ref sType, ref sID1, ref sID2, ref name) & 0xFFFFFFFF;
                    }
                }
                else
                {
                    retval = WaitAndReadDataCard(Options.Instance.COMPortReader, nSecs,
                       ref sType, ref sID1, ref sID2, ref name) & 0xFFFFFFFF;
                }

                /*retval = 2;
                sID1 = "5504";
                name = NAME_FIELD;*/

                if (retval > 1)
                {
                    if (dobroFormatiranaKartica(sID1, name, out broj) && handleOcitavanjeKarticeTrening(broj, DateTime.Now, false))
                    {
                        CitacKarticaForm citacKarticaForm = Form1.Instance.CitacKarticaForm;
                        if (citacKarticaForm != null)
                        {
                            Thread.Sleep(Options.Instance.CitacKarticaThreadVisibleCount 
                                * Options.Instance.CitacKarticaThreadInterval);
                            citacKarticaForm.Clear();
                        }
                    }
                }
                else if (retval == 1)
                {
                    // Waiting time elapsed
                }
                else
                {
                    // Wrong card
                }
            }
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
                        lastRead = CitacKartica.Instance.TryReadDolazakNaTrening(Options.Instance.COMPortReader, pendingClear);
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

        public void RequestStop()
        {
            _shouldStop = true;
        }

        public bool handleOcitavanjeKarticeTrening(int broj, DateTime vremeOcitavanja, bool obrisiPrePrikazivanja)
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

        private void unesiOcitavanje(Clan clan, DateTime vremeOcitavanja, UplataClanarine uplata)
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

        private void prikaziOcitavanje(Clan clan, DateTime vremeOcitavanja, out UplataClanarine uplata)
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

        public string FormatMessage(int broj, Clan clan, string grupa)
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
