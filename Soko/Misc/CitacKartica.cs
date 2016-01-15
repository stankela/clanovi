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

        public const int POSLEDNJI_DAN_ZA_UPLATE = 10;

        private Font font = new Font("Arial", 40, FontStyle.Bold);

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
                if (Int32.TryParse(sID1, out broj) && broj > 0)
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
            string name;
            if (!readCard(Options.Instance.COMPortReader, false, out broj, out name))
                return false;
            
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    Clan clan = DAOFactoryFactory.DAOFactory.GetClanDAO().findForBrojKartice(broj);
                    if (clan == null)
                        return false;

                    UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
                    List<UplataClanarine> uplate = new List<UplataClanarine>(uplataClanarineDAO.findUplate(clan));

                    UplataClanarine poslednjaUplata = null;
                    if (uplate.Count > 0)
                    {
                        Util.sortByDatumVremeUplateDesc(uplate);
                        poslednjaUplata = uplate[0];
                    }

                    DolazakNaTrening dolazak = new DolazakNaTrening();
                    dolazak.Clan = clan;
                    dolazak.DatumVremeDolaska = DateTime.Now;
                    if (poslednjaUplata != null)
                    {
                        dolazak.Grupa = poslednjaUplata.Grupa;
                        dolazak.DatumPoslednjeUplate = poslednjaUplata.DatumVremeUplate;
                    }
                    else
                    {
                        dolazak.Grupa = null;
                        dolazak.DatumPoslednjeUplate = null;
                    }

                    DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO().MakePersistent(dolazak);
                    session.Transaction.Commit();

                    bool okForTrening = false;
                    if (poslednjaUplata != null)
                    {
                        // Najpre proveri da li postoji uplata u ovom mesecu.
                        okForTrening =
                            poslednjaUplata.DatumVremeUplate.Value.Year == DateTime.Now.Year
                            && poslednjaUplata.DatumVremeUplate.Value.Month == DateTime.Now.Month;
                        if (!okForTrening)
                        {
                            if (DateTime.Now.Day > CitacKartica.POSLEDNJI_DAN_ZA_UPLATE)
                            {
                                okForTrening = false;
                            }
                            else
                            {
                                // Proveri da li postoji uplata u prethodnom mesecu.
                                /*DateTime prevMonth = DateTime.Today.AddMonths(-1);
                                okForTrening =
                                    poslednjaUplata.DatumVremeUplate.Value.Year == prevMonth.Year
                                    && poslednjaUplata.DatumVremeUplate.Value.Month == prevMonth.Month;

                                DateTime from = new DateTime(prevMonth.Year, prevMonth.Month, 1);
                                DateTime to = DateTime.Now;
                                okForTrening = poslednjaUplata.DatumVremeUplate >= from
                                    && poslednjaUplata.DatumVremeUplate <= to;*/

                                okForTrening = true;
                            }
                        }
                    }

                    string msg = broj.ToString();
                    if (poslednjaUplata != null)
                    {
                        msg += "\n" + poslednjaUplata.Grupa.Naziv;
                    }
                    Graphics g = SingleInstanceApplication.GlavniProzor.CitacKarticaForm.CreateGraphics();
                    if (okForTrening)
                    {
                        g.Clear(Color.Green);
                    }
                    else
                    {
                        g.Clear(Color.Red);
                    }
                    g.DrawString(msg, font, Brushes.Black, 0, 0);
                    g.Dispose();
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
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }
    }
}
