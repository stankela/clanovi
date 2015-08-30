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
        
        // This method will be called when the thread is started.
        public void DoWork()
        {
            Thread.Sleep(3000);
            int i = 0;
            int j = 0;
            while (!_shouldStop)
            {
                ulong retval = 0;
                int nSecs = 1;
          		int nComPort = Soko.UI.Form1.N_COM_PORT;

                string sType = " ";
                string sID1 = "          ";
                string sID2 = "          ";
                string sName = "                                ";

                retval = WaitAndReadDataCard(nComPort, nSecs, ref sType, ref sID1, ref sID2, ref sName) & 0xFFFFFFFF;
                if (_shouldStop)
                    break;

                int notUsed;
                if (retval > 1 && Int32.TryParse(sID1, out notUsed))
                {
                    ++j;
                    SingleInstanceApplication.GlavniProzor.setStatusBarText(
                        String.Format("Card detected {0}     Broj kartice: {1}   Ime: {2}", j, sID1, sName));

                    try
                    {
                        using (ISession session = NHibernateHelper.OpenSession())
                        using (session.BeginTransaction())
                        {
                            CurrentSessionContext.Bind(session);

                            Clan clan = DAOFactoryFactory.DAOFactory.GetClanDAO().findForBrojKartice(Int32.Parse(sID1));
                            if (clan != null)
                            {
                                int prevMonth = DateTime.Today.AddMonths(-1).Month;
                                DateTime from = new DateTime(DateTime.Today.Year, prevMonth, 1);
                                DateTime to = DateTime.Now;
                                //DateTime firstDayInMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
                                
                                UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
                                IList<UplataClanarine> uplate = uplataClanarineDAO.findUplate(clan, from, to);
                                if (uplate.Count > 0)
                                {
                                    SingleInstanceApplication.GlavniProzor.CitacKarticaForm.BackColor = Color.Green;
                                    Thread.Sleep(2000);
                                    SingleInstanceApplication.GlavniProzor.CitacKarticaForm.BackColor = Color.Yellow;
                                }
                                else
                                {
                                    SingleInstanceApplication.GlavniProzor.CitacKarticaForm.BackColor = Color.Red;
                                    Thread.Sleep(2000);
                                    SingleInstanceApplication.GlavniProzor.CitacKarticaForm.BackColor = Color.Yellow;
                                }

                                // izgleda da ova linija srusi program kada se program zatvori odmah nakon sto je kartica
                                // detektovana.
                                //SingleInstanceApplication.GlavniProzor.setStatusBarText("");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //discardChanges();
                        MessageDialogs.showMessage(ex.Message, "Citac kartica");
                    }
                    finally
                    {
                        CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
                    }
                }
                else if (retval == 1)
                {
                    ++i;
                    SingleInstanceApplication.GlavniProzor.setStatusBarText(
                        String.Format("Waiting time elapsed {0}", i));
                    //MessageBox.Show("Waiting time elapsed!");
                }
                else
                {
                    //MessageBox.Show("Wrong card!");
                }
            }
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }

        // Volatile is used as hint to the compiler that this data 
        // member will be accessed by multiple threads. 
        private volatile bool _shouldStop = false;
    }
}
