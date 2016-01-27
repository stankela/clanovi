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

        private int clanId;
        public bool PendingWrite = false;
    
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
            ckbKartica.Checked = false;
            cmbClan.DropDownStyle = ComboBoxStyle.DropDownList;

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
            string text = txtSifraClana.Text;
            Clan clan = null;
            int broj;
            if (int.TryParse(text, out broj))
            {
                clan = findClan(broj);
            }
            else if (text != String.Empty)
            {
                clan = searchForClan(text);
            }
            SelectedClan = clan;
            ckbKartica.Checked = SelectedClan != null && SelectedClan.ImaKarticu;
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

        private Clan searchForClan(string text)
        {
            foreach (Clan c in clanovi)
            {
                if (c.PrezimeIme.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                    return c;
            }
            return null;
        }

        private void cmbClan_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (SelectedClan != null)
            {
                txtSifraClana.Text = SelectedClan.Broj.ToString();
                ckbKartica.Checked = SelectedClan.ImaKarticu;
            }
            else
            {
                txtSifraClana.Text = String.Empty;
                ckbKartica.Checked = false;
            }
        }

        private bool napraviKarticuDlg(Clan c)
        {
            string naslov = "Pravljenje kartice";
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
                MessageBox.Show("Izaberite clana.", "Pravljenje kartice");
                return;
            }

            if (napraviKarticuDlg(SelectedClan))
            {
                MessageBox.Show("Prislonite karticu na citac i kliknite OK.", "Pravljenje kartice");
                clanId = SelectedClan.Id;
                PendingWrite = true;
            }
        }

        public void Write()
        {
            if (PendingWrite)
            {
                PendingWrite = false;
                try
                {
                    using (ISession session = NHibernateHelper.OpenSession())
                    using (session.BeginTransaction())
                    {
                        CurrentSessionContext.Bind(session);
                        Clan selectedClan = session.Load<Clan>(clanId);

                        //selectedClan.BrojKartice = getNewBrojKartice();
                        selectedClan.BrojKartice = selectedClan.Broj;
                        DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent(selectedClan);

                        string sType = "";
                        string sID1 = selectedClan.BrojKartice.ToString();
                        string sID2 = "";
                        string sName = CitacKartica.NAME_FIELD;
                        ulong retval = WriteDataCard(Options.Instance.COMPortWriter,
                            sType, sID1, sID2, sName) & 0xFFFFFFFF; 

                        // TODO2: Prvo proveri da li je kartica vazeca, i prikazi upozorenje ako jeste.

                        if (retval == 0)
                        {
                            string msg = "Neuspesno pravljenje kartice. " +
                                "Proverite da li je uredjaj prikljucen, i da li je podesen COM port.";
                            throw new Exception(msg);
                        }
                        else
                        {
                            session.Transaction.Commit();
                            MessageBox.Show(String.Format("Kartica je napravljena.\n\nBroj kartice:   {0}\nClan:   {1}",
                                sID1, selectedClan.BrojPrezimeImeDatumRodjenja), "Pravljenje kartice");
                            ckbKartica.Checked = true;
                        }

                    }
                }
                catch (Exception ex)
                {
                    //discardChanges();
                    MessageDialogs.showMessage(ex.Message, "Pravljenje kartice");
                }
                finally
                {
                    CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
                }
            }
        }
    }
}