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
        public bool PendingWrite = false;    
        private List<Clan> clanovi;

        private bool testKartica;
        private int clanId;
        private int brojKartice;

        private const string OK_MSG_WRITE_TEST = "TEST KARTICA je napravljena.";
        private const string OK_MSG_WRITE = "Kartica je napravljena.\n\nBroj kartice:   {0}\nClan:   {1}";
        private const string ERROR_MSG_WRITE_TEST = "Neuspesno pravljenje TEST KARTICE. " +
            "Proverite da li je uredjaj prikljucen, i da li je podesen COM port.";
        private const string ERROR_MSG_WRITE = "Neuspesno pravljenje kartice. " +
            "Proverite da li je uredjaj prikljucen, i da li je podesen COM port.";

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
            testKartica = ckbTestKartica.Checked;
            if (!testKartica && SelectedClan == null)
            {
                MessageDialogs.showMessage("Izaberite clana.", "Pravljenje kartice");
                return;
            }

            if (napraviKarticuDlg(SelectedClan, testKartica))
            {
                MessageDialogs.showMessage("Prislonite karticu na citac i kliknite OK.", "Pravljenje kartice");
                if (!testKartica)
                {
                    clanId = SelectedClan.Id;
                    brojKartice = SelectedClan.Broj.Value;
                }
                PendingWrite = true;
            }
        }

        public void WriteKartica(out string okMsg)
        {
            okMsg = String.Empty;
            if (!PendingWrite)
                return;
            PendingWrite = false;

            if (testKartica)
            {
                // TODO2: Prvo proveri da li je kartica vazeca, i prikazi upozorenje ako jeste (isto i dole).
                CitacKartica.Instance.writeCard(Options.Instance.COMPortWriter,
                    CitacKartica.TEST_KARTICA_BROJ.ToString(), ERROR_MSG_WRITE_TEST);
                okMsg = OK_MSG_WRITE_TEST;
                return;
            }

            CitacKartica.Instance.writeCard(Options.Instance.COMPortWriter,
                brojKartice.ToString(), ERROR_MSG_WRITE);

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    Clan clan = session.Load<Clan>(clanId);
                    clan.BrojKartice = brojKartice;
                    DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent(clan);
                    session.Transaction.Commit();
                    ckbKartica.Checked = true;

                    okMsg = String.Format(OK_MSG_WRITE, brojKartice.ToString(), clan.BrojPrezimeImeDatumRodjenja);

                    CitacKarticaDictionary.Instance.DodajClanaSaKarticom(clan);
                    return;
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
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