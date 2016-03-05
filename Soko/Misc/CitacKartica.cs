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

        private CitacKartica()
        { 
        
        }

        private static CitacKartica citacKartica = new CitacKartica();

        public static CitacKartica getCitacKartica()
        {
            return citacKartica;
        }

        public static readonly string NAME_FIELD = "SDV";
        public static readonly int TEST_KARTICA_BROJ = 100000;
        public static readonly string TEST_KARTICA_NAME = "TEST KARTICA";

        public bool readCard(int comPort, bool showErrorMessages, out int broj, out string name)
        {
            string sType = " ";
            string sID1 = "          ";
            string sID2 = "          ";
            name = "                                ";
            broj = -1;

            ulong retval = ReadDataCard(comPort, ref sType, ref sID1, ref sID2, ref name) & 0xFFFFFFFF;
            if (retval == 1)
            {
                if (Int32.TryParse(sID1, out broj) && broj > 0 && name == NAME_FIELD)
                {
                    return true;
                }
                else
                {
                    if (showErrorMessages)
                    {
                  
                        string msg = "Lose formatirana kartica.";
                        MessageBox.Show(msg, "Ocitavanje kartice");
                    }
                    return false;
                }
            }
            else
            {
                if (showErrorMessages)
                {
                    string msg = "Neuspesno citanje kartice. " +
                        "Proverite da li je uredjaj prikljucen, i da li je podesen COM port.";
                    MessageBox.Show(msg, "Ocitavanje kartice");
                }
                return false;
            }
        }

        public bool Read()
        {
            int broj;
            string name;  // not used

            //Stopwatch watch = Stopwatch.StartNew();
            if (!readCard(Options.Instance.COMPortReader, false, out broj, out name))
                return false;
            //watch.Stop();
            //long elapsedMs = watch.ElapsedMilliseconds;

            return handleOcitanaKartica(broj, DateTime.Now);
        }

        public bool handleOcitanaKartica(int broj, DateTime vremeOcitavanja)
        {
            if (broj == TEST_KARTICA_BROJ)
            {
                string msg = FormatMessage(broj, TEST_KARTICA_NAME);
                CitacKarticaForm form = SingleInstanceApplication.GlavniProzor.CitacKarticaForm;
                if (form != null)
                {
                    form.PrikaziOcitavanje(msg, Options.Instance.PozadinaCitacaKartica);
                }
                return true;
            }

            UplataClanarine poslednjaUplata;
            if (!unesiOcitavanje(broj, vremeOcitavanja, out poslednjaUplata))
                return false;

            prikaziOcitavanje(broj, vremeOcitavanja, poslednjaUplata);
            return true;
        }

        private bool unesiOcitavanje(int broj, DateTime vremeOcitavanja, out UplataClanarine poslednjaUplata)
        {
            poslednjaUplata = null;
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    Clan clan = DAOFactoryFactory.DAOFactory.GetClanDAO().findForBrojKartice(broj);
                    if (clan == null)
                        return false;

                    UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
                    List<UplataClanarine> uplate = new List<UplataClanarine>(uplataClanarineDAO.findUplate(clan));

                    if (uplate.Count > 0)
                    {
                        Util.sortByVaziOdDesc(uplate);
                        for (int i = 0; i < uplate.Count; ++i)
                        {
                            UplataClanarine u = uplate[i];
                            if (u.VaziOd.Value.Year == vremeOcitavanja.Year && u.VaziOd.Value.Month == vremeOcitavanja.Month)
                            {
                                poslednjaUplata = u;
                                break;
                            }
                        }
                        if (poslednjaUplata == null)
                            poslednjaUplata = uplate[0];
                    }

                    DolazakNaTrening dolazak = new DolazakNaTrening();
                    dolazak.Clan = clan;
                    dolazak.DatumVremeDolaska = vremeOcitavanja;
                    if (poslednjaUplata != null)
                    {
                        dolazak.Grupa = poslednjaUplata.Grupa;
                    }
                    else
                    {
                        dolazak.Grupa = null;
                    }

                    DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO().MakePersistent(dolazak);
                    session.Transaction.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(ex.Message, "Citac kartica");
                return false;
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }
        }

        private void prikaziOcitavanje(int broj, DateTime vremeOcitavanja, UplataClanarine poslednjaUplata)
        {
            bool okForTrening = false;
            if (poslednjaUplata != null)
            {
                // Najpre proveri da li postoji uplata za ovaj mesec.
                okForTrening =
                    poslednjaUplata.VaziOd.Value.Year == vremeOcitavanja.Year
                    && poslednjaUplata.VaziOd.Value.Month == vremeOcitavanja.Month;
                if (!okForTrening)
                {
                    okForTrening = vremeOcitavanja.Day <= Options.Instance.PoslednjiDanZaUplate;
                }
            }

            string grupa = null;
            if (poslednjaUplata != null)
            {
                grupa = poslednjaUplata.Grupa.Naziv;
            }
            string msg = FormatMessage(broj, grupa);

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
                form.PrikaziOcitavanje(msg, color);
            }
        }

        public string FormatMessage(int broj, string grupa)
        {
            string result = String.Empty;
            if (Options.Instance.PrikaziBrojClanaKodOcitavanjaKartice)
            {
                result = broj.ToString() + "\n";
            }
            if (grupa != null)
            {
                result += grupa;
            }
            return result;
        }
    }
}
