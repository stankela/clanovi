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
using Soko.Exceptions;

namespace Soko.UI
{
    public partial class PravljenjeKarticeForm : Form
    {
        private List<Clan> clanovi;

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
            ckbKartica.Visible = false;
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
            ckbKartica.Visible = ckbKartica.Checked;
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
            ckbKartica.Visible = ckbKartica.Checked;
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
            if (!testKartica && SelectedClan == null)
            {
                MessageDialogs.showMessage("Izaberite clana.", "Pravljenje kartice");
                return;
            }

            bool napraviKarticu;
            if (SelectedClan != null && ckbKartica.Checked)
            {
                string naslov = "Duplikat kartice";
                string pitanje = String.Format("Clan \"{0}\" vec ima karticu. Da li zelite da napravite duplikat?",
                    SelectedClan.BrojPrezimeImeDatumRodjenja);
                PotvrdaDialog dlg2 = new PotvrdaDialog(naslov, pitanje);
                dlg2.Width += 100;
                dlg2.SetDaText("Duplikat");
                napraviKarticu = (dlg2.ShowDialog() == DialogResult.Yes);
            }
            else
            {
                napraviKarticu = napraviKarticuDlg(SelectedClan, testKartica);
            }

            if (napraviKarticu)
            {
                MessageDialogs.showMessage("Prislonite karticu na citac i kliknite OK.", "Pravljenje kartice");
                string msg;
                if (!testKartica)
                {
                    handlePisacKarticaWrite(SelectedClan.Id, SelectedClan.Broj.Value, testKartica, out msg);
                }
                else
                {
                    handlePisacKarticaWrite(-1, -1, testKartica, out msg);
                }
                MessageDialogs.showMessage(msg, "Pravljenje kartice");
            }
        }

        private void handlePisacKarticaWrite(int clanId, int brojKartice, bool testKartica, out string msg)
        {
            msg = String.Empty;
            int brojPokusaja = Options.Instance.BrojPokusajaCitacKartica;
            while (brojPokusaja > 0)
            {
                try
                {
                    string okMsg;
                    if (testKartica)
                        WriteTestKartica(out okMsg);
                    else
                        WriteClanKartica(clanId, brojKartice, out okMsg);
                    brojPokusaja = 0;
                    msg = okMsg;
                }
                catch (WriteCardException ex)
                {
                    --brojPokusaja;
                    if (brojPokusaja == 0)
                    {
                        msg = ex.Message;
                    }
                }
            }
        }

        private void WriteTestKartica(out string okMsg)
        {
            okMsg = String.Empty;

            // TODO2: Prvo proveri da li je kartica vazeca, i prikazi upozorenje ako jeste (isto i u WriteClanKartica).
            if (CitacKartica.Instance.writeCard(Options.Instance.COMPortWriter, CitacKartica.TEST_KARTICA_BROJ.ToString()))
            {
                okMsg = OK_MSG_WRITE_TEST;
                return;
            }
            else
            {
                throw new WriteCardException(ERROR_MSG_WRITE_TEST);
            }

        }

        private void WriteClanKartica(int clanId, int brojKartice, out string okMsg)
        {
            okMsg = String.Empty;

            if (!CitacKartica.Instance.writeCard(Options.Instance.COMPortWriter, brojKartice.ToString()))
            {
                throw new WriteCardException(ERROR_MSG_WRITE);
            }

            Clan clan = null;
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    clan = session.Load<Clan>(clanId);
                    clan.BrojKartice = brojKartice;
                    DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent(clan);
                    session.Transaction.Commit();
                    ckbKartica.Checked = true;
                    ckbKartica.Visible = true;

                    okMsg = String.Format(OK_MSG_WRITE, brojKartice.ToString(), clan.BrojPrezimeImeDatumRodjenja);
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }

            if (clan != null)
                CitacKarticaDictionary.Instance.DodajClanaSaKarticom(clan);
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