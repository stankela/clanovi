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
        
        private CitacKartica()
        {

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

        public void readCard(int comPort, out int broj)
        {
            string sType = " ";
            string sID1 = "          ";
            string sID2 = "          ";
            string name = "                                ";
            broj = -1;

            AdminForm af = SingleInstanceApplication.GlavniProzor.AdminForm;
            bool measureTime = af != null;

            Sesija.Instance.Log("BEFORE P READ");
            
            Stopwatch watch = null;
            if (measureTime)
            {
                watch = Stopwatch.StartNew();
            }

            ulong retval;
            lock (readAndWriteLock)
            {
                retval = ReadDataCard(comPort, ref sType, ref sID1, ref sID2, ref name) & 0xFFFFFFFF;
            }
            
            if (measureTime)
            {
                watch.Stop();
                af.newCitanjeKartice(retval, watch.ElapsedMilliseconds);
            }

            Sesija.Instance.Log("P READ: " + retval.ToString());

            if (retval == 1)
            {
                if (!dobroFormatiranaKartica(sID1, name, out broj))
                {
                    throw new ReadCardException("Lose formatirana kartica.");
                }
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

            Sesija.Instance.Log("BEFORE C READ");

            ulong retval;
            lock (readAndWriteLock)
            {
                retval = ReadDataCard(comPort, ref sType, ref sID1, ref sID2, ref name) & 0xFFFFFFFF;
            }

            Sesija.Instance.Log("C READ: " + retval.ToString());

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
            
            AdminForm af = SingleInstanceApplication.GlavniProzor.AdminForm;
            bool measureTime = af != null;

            Sesija.Instance.Log("BEFORE P WRITE");

            Stopwatch watch = null;
            if (measureTime)
            {
                watch = Stopwatch.StartNew();
            }

            ulong retval;
            lock (readAndWriteLock)
            {
                retval = WriteDataCard(comPort, sType, sID1, sID2, sName) & 0xFFFFFFFF;
            }

            if (measureTime)
            {
                watch.Stop();
                af.newPisanjeKartice(retval, watch.ElapsedMilliseconds);
            }

            Sesija.Instance.Log("P WRITE: " + retval.ToString());

            return retval == 1;
        }

        public bool TryReadDolazakNaTrening(int comPort)
        {
            AdminForm af = SingleInstanceApplication.GlavniProzor.AdminForm;
            bool measureTime = af != null;

            Stopwatch watch = null;
            if (measureTime)
            {
                watch = Stopwatch.StartNew();
            }

            int broj;
            bool result = tryReadCard(comPort, out broj);

            /*result = true;
            broj = 5464;*/

            if (measureTime)
            {
                watch.Stop();
                af.newOcitavanje(watch.ElapsedMilliseconds);
            }

            return result && handleDolazakNaTrening(broj, DateTime.Now);
        }

        public void WaitAndReadLoop()
        {
            // TODO2: Proveri da li je sve u ovom metodu thread safe.
            while (!_shouldStop)
            {
                // NOTE: Izabran je mali vremenski interval (a ne recimo 20 sec), zato sto kada se program zatvori
                // WaitAndReadDataCard je i dalje aktivan dok ne istekne interval, a samim tim i proces je i dalje
                // aktivan, i nije moguce ponovo restartovanje programa (ili je moguce ali imamo istovremeno dva
                // procesa).
                int nSecs = 2;

                string sType = " ";
                string sID1 = "          ";
                string sID2 = "          ";

                string name = "                                ";
                int broj = -1;

                ulong retval;
                lock (readAndWriteLock)
                {
                    retval = WaitAndReadDataCard(Options.Instance.COMPortReader, nSecs,
                       ref sType, ref sID1, ref sID2, ref name) & 0xFFFFFFFF;
                }

                /*retval = 2;
                sID1 = "5504";
                name = NAME_FIELD;*/

                if (retval > 1)
                {
                    if (dobroFormatiranaKartica(sID1, name, out broj) && handleDolazakNaTrening(broj, DateTime.Now))
                    {
                        CitacKarticaForm citacKarticaForm = SingleInstanceApplication.GlavniProzor.CitacKarticaForm;
                        if (citacKarticaForm != null)
                        {
                            Thread.Sleep(1500);
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
            while (!_shouldStop)
            {
                if (CitacKartica.Instance.TryReadDolazakNaTrening(Options.Instance.COMPortReader))
                {
                    CitacKarticaForm citacKarticaForm = SingleInstanceApplication.GlavniProzor.CitacKarticaForm;
                    if (citacKarticaForm != null)
                    {
                        Thread.Sleep(1500);
                        citacKarticaForm.Clear();
                    }
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }
        
        public bool handleDolazakNaTrening(int broj, DateTime vremeOcitavanja)
        {
            if (broj == TEST_KARTICA_BROJ)
            {
                string msg = FormatMessage(broj, null, TEST_KARTICA_NAME);
                CitacKarticaForm form = SingleInstanceApplication.GlavniProzor.CitacKarticaForm;
                if (form != null)
                {
                    form.Prikazi(msg, Options.Instance.PozadinaCitacaKartica);
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
                // Najpre proveri godisnju clanarinu.
                okForTrening = Util.isGodisnjaClanarina(uplata.Grupa.Naziv);

                // Proveri da li postoji uplata za ovaj mesec.
                if (!okForTrening)
                {
                    okForTrening =
                        uplata.VaziOd.Value.Year == vremeOcitavanja.Year
                        && uplata.VaziOd.Value.Month == vremeOcitavanja.Month;
                }
            }
            else
            {
                okForTrening = clan.NeplacaClanarinu;
            }

            // Tolerisi do odredjenog dana u mesecu.
            if (!okForTrening)
            {
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
            CitacKarticaForm form = SingleInstanceApplication.GlavniProzor.CitacKarticaForm;
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
