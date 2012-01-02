using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Soko.Dao;

namespace Soko.UI
{
    public partial class BiracClana : Form
    {
        private int idClana = -1;
        private List<Clan> clanovi;

        public BiracClana(string naslov)
        {
            InitializeComponent();
            MapperRegistry.initialize();

            this.Text = naslov;

            clanovi = loadClanovi();
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
            List<Clan> result = MapperRegistry.clanDAO().getAll();

            PropertyDescriptor propDescPrez = TypeDescriptor.GetProperties(typeof(Clan))["Prezime"];
            PropertyDescriptor propDescIme = TypeDescriptor.GetProperties(typeof(Clan))["Ime"];
            PropertyDescriptor[] propDesc = new PropertyDescriptor[2] { propDescPrez, propDescIme };
            ListSortDirection[] direction = new ListSortDirection[2] 
                { ListSortDirection.Ascending, ListSortDirection.Ascending};
			
            result.Sort(new SortComparer<Clan>(propDesc, direction));

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
                idClana = SelectedClan.Key.intValue();
            else
                idClana = -1;
        }
    }
}