using Bilten.Dao;
using NHibernate;
using NHibernate.Context;
using Soko.Data;
using Soko.Domain;
using Soko.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class SimulatorCitacaKarticaForm : Form
    {
        private List<Clan> clanovi;
        
        public SimulatorCitacaKarticaForm()
        {
            InitializeComponent();
            dtpVremeOcitavanja.Format = DateTimePickerFormat.Custom;
            dtpVremeOcitavanja.CustomFormat = "dd.MM.yyyy HH:mm:ss";

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    clanovi = loadClanovi();
                    setClanovi(clanovi);
                    SelectedClan = null;
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }
        }

        private List<Clan> loadClanovi()
        {
            List<Clan> result = new List<Clan>(DAOFactoryFactory.DAOFactory.GetClanDAO().FindAll());
            Util.sortByPrezimeImeDatumRodjenja(result);
            return result;
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

        private void txtClan_TextChanged(object sender, EventArgs e)
        {
            string text = txtClan.Text;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbClan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (SelectedClan != null)
            {
                txtClan.Text = SelectedClan.Broj.ToString();
            }
            else
            {
                txtClan.Text = String.Empty;
            }
        }

        private void btnOcitajKarticu_Click(object sender, EventArgs e)
        {
            if (SelectedClan != null)
            {
                CitacKartica.Instance.handleDolazakNaTrening(SelectedClan.Broj.Value, dtpVremeOcitavanja.Value);
            }
        }

    }
}
