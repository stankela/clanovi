using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using System.Globalization;
using Bilten.Dao;
using System.Runtime.InteropServices;
using System.Threading;
using NHibernate;
using Soko.Data;
using NHibernate.Context;

namespace Soko.UI
{
    public partial class PravljenjeKarticeForm : Form
    {
        [DllImport("PanReaderIf.dll")]
        private static extern ulong ReadDataCard(int comport, ref string sType, ref string sID1, ref string sID2, ref string sName);
        [DllImport("PanReaderIf.dll")]
        private static extern ulong WriteDataCard(int comport, string sType, string sID1, string sID2, string sName);
        [DllImport("PanReaderIf.dll")]
        private static extern ulong WaitDataCard(int comport, int nSecs);
        [DllImport("PanReaderIf.dll")]
        private static extern ulong WaitAndReadDataCard(int comport, int nSecs, ref string sType, ref string sID1, ref string sID2, ref string sName);

        private int nComPort = Soko.UI.Form1.N_COM_PORT;
        
        private List<Clan> clanovi;

        public PravljenjeKarticeForm()
        {
            InitializeComponent();
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    loadData();
                    initUI();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
        }

        private void loadData()
        {
            clanovi = loadClanovi();
        }

        private List<Clan> loadClanovi()
        {
            List<Clan> result = new List<Clan>(DAOFactoryFactory.DAOFactory.GetClanDAO().FindAll());

            PropertyDescriptor propDescPrez = TypeDescriptor.GetProperties(typeof(Clan))["Prezime"];
            PropertyDescriptor propDescIme = TypeDescriptor.GetProperties(typeof(Clan))["Ime"];
            PropertyDescriptor[] propDesc = new PropertyDescriptor[2] { propDescPrez, propDescIme };
            ListSortDirection[] direction = new ListSortDirection[2] 
                { ListSortDirection.Ascending, ListSortDirection.Ascending};

            result.Sort(new SortComparer<Clan>(propDesc, direction));

            return result;
        }

        private void initUI()
        {
            this.Text = "Pravljenje kartice";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            setClanovi(clanovi);
            SelectedClan = null;
        }

        private void setClanovi(List<Clan> clanovi)
        {
            cmbClan.DataSource = clanovi;
            cmbClan.DisplayMember = "BrojPrezimeImeDatumRodjenja";
        }

        private Clan SelectedClan
        {
            get { return cmbClan.SelectedItem as Clan; }
            set { cmbClan.SelectedItem = value; }
        }

        private void PravljenjeKarticeForm_Shown(object sender, EventArgs e)
        {
            txtSifraClana.Focus();
        }

        private void txtSifraClana_TextChanged(object sender, System.EventArgs e)
        {
            int broj;
            try
            {
                broj = int.Parse(txtSifraClana.Text);
                SelectedClan = findClan(broj);
            }
            catch (Exception)
            {
                SelectedClan = null;
            }
        }

        private Clan findClan(int broj)
        {
            foreach (Clan c in clanovi)
            {
                if (c.Broj == broj)
                    return c;
            }
            return null;
        }

        private void cmbClan_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (SelectedClan != null)
            {
                txtSifraClana.Text = SelectedClan.Broj.ToString();
            }
            else
            {
                txtSifraClana.Text = String.Empty;
            }
        }

        private bool napraviKarticuDlg(Clan c)
        {
            string naslov = "Kartica";
            string pitanje = String.Format(
                "Da li zelite da napravite karticu za clana \"{0}\"?", c.BrojPrezimeImeDatumRodjenja);
            return MessageDialogs.queryConfirmation(pitanje, naslov);
        }

        private int getNewBrojKartice()
        {
            return DAOFactoryFactory.DAOFactory.GetClanDAO().getMaxBrojKartice() + 1;
        }

        private void btnNapraviKarticu_Click(object sender, EventArgs e)
        {
            if (SelectedClan == null)
            {
                MessageBox.Show("Izaberite clana.", this.Text);
                return;
            }

            Program.workerObject.RequestStop();
            Program.workerThread.Join();

            if (napraviKarticuDlg(SelectedClan))
            {
                nComPort = 0;
                if (/*this.comboBoxPort.SelectedIndex >= 0*/true)
                {
                    //nComPort = Convert.ToUInt16(this.comboBoxPort.SelectedIndex + 1);
                    nComPort = Soko.UI.Form1.N_COM_PORT;
                }
                if (nComPort <= 0)
                {
                    MessageBox.Show("Potrebno je da podesite COM port za citac kartica.");
                }

                MessageBox.Show("Prislonite karticu na citac i kliknite OK.", "Pravljenje kartice");


                try
                {
                    using (ISession session = NHibernateHelper.OpenSession())
                    using (session.BeginTransaction())
                    {
                        CurrentSessionContext.Bind(session);

                        SelectedClan.BrojKartice = getNewBrojKartice();
                        DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent(SelectedClan);

                        ulong retval = 0;
                        string sType = "";
                        string sID1 = SelectedClan.BrojKartice.ToString();
                        string sID2 = "";
                        string sName = SelectedClan.BrojImePrezime;

                        retval = WriteDataCard(nComPort, sType, sID1, sID2, sName) & 0xFFFFFFFF;

                        if (retval == 0)
                        {
                            throw new Exception("Neuspesno pravljenje kartice. Proverite da li je podesen COM port za citac kartica.");
                        }
                        else
                        {
                            string sType2 = " ";
                            string sID1_2 = "          ";
                            string sID2_2 = "          ";
                            string sName2 = "                                ";

                            retval = ReadDataCard(nComPort, ref sType2, ref sID1_2, ref sID2_2, ref sName2) & 0xFFFFFFFF;

                            // TODO2: Izbacio sam proveru da li su imena jednaka zato sto ne upisuje nasa slova
                            // pravilno na karticu (slova sa kvacicom pretvori u slova bez kvacice). Proveravam
                            // samo broj clana.
                            string broj = sName.Split(' ')[0];
                            string broj2 = sName2.Split(' ')[0];
                            if (retval > 0 && sID1_2 == sID1 && broj == broj2 /*&& sName2 == sName*/)
                            {
                                session.Transaction.Commit();
                                MessageBox.Show(String.Format("Kartica je napravljena.\n\nBroj kartice:   {0}\nIme:   {1}",
                                    sID1_2, sName2));
                            }
                            else
                            {
                                throw new Exception("Greska prilikom pravljenja kartice.");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    //discardChanges();
                    MessageDialogs.showMessage(ex.Message, this.Text);
                }
                finally
                {
                    CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
                }
            }

            Program.workerObject = new CitacKartica();
            Program.workerThread = new Thread(Program.workerObject.DoWork);
            Program.workerThread.Start();
            while (!Program.workerThread.IsAlive) ;
        }
    }
}