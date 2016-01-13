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

        public static bool readCard(int comPort, bool showErrorMessages, out int broj, out string name)
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

        public static void Read()
        {
            int broj;
            string name;
            if (!CitacKartica.readCard(Options.Instance.COMPortReader, false, out broj, out name))
                return;
            
            SingleInstanceApplication.GlavniProzor.setStatusBarText(
                String.Format("Broj kartice: {0}   Ime: {1}", broj, name));

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    Clan clan = DAOFactoryFactory.DAOFactory.GetClanDAO().findForBrojKartice(broj);
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
                            Thread.Sleep(1000);
                            SingleInstanceApplication.GlavniProzor.CitacKarticaForm.BackColor = Color.Yellow;
                        }
                        else
                        {
                            SingleInstanceApplication.GlavniProzor.CitacKarticaForm.BackColor = Color.Red;
                            Thread.Sleep(1000);
                            SingleInstanceApplication.GlavniProzor.CitacKarticaForm.BackColor = Color.Yellow;
                        }

                        // izgleda da ova linija srusi program kada se program zatvori odmah nakon sto je kartica
                        // detektovana.
                        SingleInstanceApplication.GlavniProzor.setStatusBarText("");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(ex.Message, "Citac kartica");
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }
    }
}
