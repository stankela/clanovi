using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using NHibernate;
using Soko.Data;
using NHibernate.Context;
using Bilten.Dao;
using Soko.Misc;

namespace Soko.UI
{
    public partial class BiracClana : Form
    {
        private int idClana = -1;
        private List<Clan> clanovi;

        public BiracClana(string naslov)
        {
            InitializeComponent();
            this.Text = naslov;
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    clanovi = loadClanovi();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }

            setClanovi(clanovi);
            if (clanovi.Count > 0)
                SelectedClan = clanovi[0];
            else
                SelectedClan = null;

            rbtClan.Checked = true;

            Font = Options.Instance.Font;
        }

        private List<Clan> loadClanovi()
        {
            List<Clan> result = new List<Clan>(DAOFactoryFactory.DAOFactory.GetClanDAO().FindAll());
            Util.sortByPrezimeImeDatumRodjenja(result);
            return result;
        }

        private void setClanovi(List<Clan> clanovi)
        {
            cmbClan.Items.Clear();
            foreach (Clan c in clanovi)
            {
                cmbClan.Items.Add(c.BrojPrezimeImeDatumRodjenja);
            }
        }

        private Clan SelectedClan
        {
            get
            {
                if (cmbClan.SelectedIndex >= 0)
                    return clanovi[cmbClan.SelectedIndex];
                else
                    return null;
            }
            set
            {
                if (value == null || clanovi.IndexOf(value) == -1)
                    cmbClan.SelectedIndex = -1;
                else
                    cmbClan.SelectedIndex = clanovi.IndexOf(value);
            }
        }

        private void rbtCeoIzvestaj_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbtCeoIzvestaj.Checked)
            {
                cmbClan.Enabled = false;
            }
        }

        private void rbtClan_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbtClan.Checked)
            {
                cmbClan.Enabled = true;
            }

        }

        public bool CeoIzvestaj
        {
            get { return rbtCeoIzvestaj.Checked; }
        }

        public int IdClana
        {
            get { return idClana; }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (SelectedClan != null)
                idClana = SelectedClan.Id;
            else
                idClana = -1;
        }
    }
}