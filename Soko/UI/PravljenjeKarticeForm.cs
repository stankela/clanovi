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
using Soko.Misc;

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

        public bool PendingWrite = false;    
        private List<Clan> clanovi;

        public PravljenjeKarticeForm()
        {
            InitializeComponent();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    loadData();
                    initUI();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }
        }

        private void loadData()
        {
            clanovi = loadClanovi();
        }

        private List<Clan> loadClanovi()
        {
            List<Clan> result = new List<Clan>(DAOFactoryFactory.DAOFactory.GetClanDAO().FindAll());
            Util.sortByPrezimeImeDatumRodjenja(result);
            return result;
        }

        private void initUI()
        {
            this.Text = "Pravljenje kartice";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            Font = Options.Instance.Font;

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

        private bool napraviKarticuDlg(Clan c, bool testKartica)
        {
            string naslov = "Pravljenje kartice";
            string pitanje;
            if (testKartica)
            {
                pitanje = "Da li zelite da napravite TEST KARTICU?";
            }
            else
            {
                pitanje = String.Format(
                "Da li zelite da napravite karticu za clana \"{0}\"?", c.BrojPrezimeImeDatumRodjenja);
            }
            return MessageDialogs.queryConfirmation(pitanje, naslov);
        }

        private void btnNapraviKarticu_Click(object sender, EventArgs e)
        {
            bool testKartica = ckbTestKartica.Checked;
            if (SelectedClan == null && !testKartica)
            {
                MessageBox.Show("Izaberite clana.", "Pravljenje kartice");
                return;
            }

            if (napraviKarticuDlg(SelectedClan, testKartica))
            {
                MessageBox.Show("Prislonite karticu na citac i kliknite OK.", "Pravljenje kartice");
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
                    string sType = "";
                    string sID1;
                    string sID2 = "";
                    string sName = CitacKartica.NAME_FIELD;
                    ulong retval;

                    if (ckbTestKartica.Checked)
                    {
                        sID1 = CitacKartica.TEST_KARTICA_BROJ.ToString();
                        retval = WriteDataCard(Options.Instance.COMPortWriter,
                            sType, sID1, sID2, sName) & 0xFFFFFFFF;

                        // TODO2: Prvo proveri da li je kartica vazeca, i prikazi upozorenje ako jeste (isto i dole).
                        if (retval == 0)
                        {
                            string msg = "Neuspesno pravljenje TEST KARTICE. " +
                                "Proverite da li je uredjaj prikljucen, i da li je podesen COM port.";
                            throw new Exception(msg);
                        }
                        else
                        {
                            MessageBox.Show("TEST KARTICA je napravljena.", "Pravljenje kartice");
                        }
                        return;
                    }

                    int brojKartice = SelectedClan.Broj.Value;
                    sID1 = brojKartice.ToString();
                    retval = WriteDataCard(Options.Instance.COMPortWriter,
                        sType, sID1, sID2, sName) & 0xFFFFFFFF;

                    if (retval == 0)
                    {
                        string msg = "Neuspesno pravljenje kartice. " +
                            "Proverite da li je uredjaj prikljucen, i da li je podesen COM port.";
                        throw new Exception(msg);
                    }
                    else
                    {
                        using (ISession session = NHibernateHelper.Instance.OpenSession())
                        using (session.BeginTransaction())
                        {
                            CurrentSessionContext.Bind(session);
                            Clan clan = session.Load<Clan>(SelectedClan.Id);
                            clan.BrojKartice = brojKartice;
                            DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent(clan);
                            session.Transaction.Commit();

                            MessageBox.Show(String.Format("Kartica je napravljena.\n\nBroj kartice:   {0}\nClan:   {1}",
                                sID1, clan.BrojPrezimeImeDatumRodjenja), "Pravljenje kartice");
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
                    CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                }
            }
        }

        private void ckbTestKartica_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbTestKartica.Checked)
            {
                txtSifraClana.Text = String.Empty;
            }
            txtSifraClana.Enabled = !ckbTestKartica.Checked;
            cmbClan.Enabled = !ckbTestKartica.Checked;
        }
    }
}