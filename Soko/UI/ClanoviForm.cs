using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Bilten.Dao;

namespace Soko.UI
{
    public partial class ClanoviForm : EntityListForm
    {
        private const string BROJ = "Broj";
        private const string PREZIME = "Prezime";
        private const string IME = "Ime";
        private const string DATUM_RODJENJA = "DatumRodjenja";
        private const string ADRESA = "Adresa";
        private const string MESTO = "NazivMesta";

        public ClanoviForm()
        {
            InitializeComponent();
            initialize(typeof(Clan));
            sortByPrezimeIme();
            updateBrojClanovaLabel();
        }

        private void updateBrojClanovaLabel()
        {
            lblBrojClanova.Text = entities.Count.ToString() + " clanova";
        }

        private void sortByPrezimeIme()
        {
            PropertyDescriptor propDescPrez = TypeDescriptor.GetProperties(typeof(Clan))["Prezime"];
            PropertyDescriptor propDescIme = TypeDescriptor.GetProperties(typeof(Clan))["Ime"];
            PropertyDescriptor propDescDatumRodj = TypeDescriptor.GetProperties(typeof(Clan))["DatumRodjenja"];
            PropertyDescriptor[] propDesc = new PropertyDescriptor[3] { propDescPrez, propDescIme, propDescDatumRodj };
            ListSortDirection[] direction = new ListSortDirection[3] { ListSortDirection.Ascending,
                ListSortDirection.Ascending, ListSortDirection.Ascending };
			
            entities.Sort(new SortComparer<object>(propDesc, direction));
        }

        protected override DataGridView getDataGridView()
        {
            return dataGridView1;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Size = new Size(Size.Width, 450);
            this.Text = "Clanovi";
        }

        protected override void addGridColumns()
        {
            AddColumn("Broj", BROJ, 70, DataGridViewContentAlignment.MiddleCenter);
            AddColumn("Prezime", PREZIME, 120);
            AddColumn("Ime", IME, 90);
            AddColumn("Datum rodjenja", DATUM_RODJENJA, 90, "{0:dd.MM.yyyy}");
            AddColumn("Adresa", ADRESA, 170);
            AddColumn("Mesto", MESTO, 110);
        }

        protected override List<object> loadEntities()
        {
            ClanDAO clanDAO = DAOFactoryFactory.DAOFactory.GetClanDAO();
            return new List<Clan>(clanDAO.FindAll()).ConvertAll<object>(
                delegate(Clan c)
                {
                    return c;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
        {
            return new ClanDialog(entityId);
        }

        private void btnDodaj_Click(object sender, System.EventArgs e)
        {
            addCommand();
            updateBrojClanovaLabel();
        }

        private void btnPromeni_Click(object sender, System.EventArgs e)
        {
            editCommand();
        }

        private void btnBrisi_Click(object sender, System.EventArgs e)
        {
            deleteCommand();
            updateBrojClanovaLabel();
        }

        protected override string deleteConfirmationMessage(DomainObject entity)
        {
            return String.Format("Da li zelite da izbrisete clana \"{0}\"?", entity);
        }

        protected override bool refIntegrityDeleteDlg(DomainObject entity)
        {
            Clan c = (Clan)entity;
            UplataClanarineDAO uplataDao = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();

            if (uplataDao.existsUplataClan(c))
            {
                string msg = "Clana '{0}' nije moguce izbrisati zato sto postoje " +
                    "podaci o uplatama za datog clana.";
                MessageDialogs.showMessage(String.Format(msg, c), this.Text);
                return false;
            }
            return true;
        }

        protected override void delete(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetClanDAO().MakeTransient((Clan)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja clana.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
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
            setSelectedClan(clan);
        }

        private void setSelectedClan(Clan clan)
        {
            // NOTE: Da bi radilo ispravno, Clan mora da implementira Equals i GetHashCode
            List<object> items = (List<object>)dataGridView1.DataSource;
            int index = items.IndexOf(clan);
            if (index >= 0)
                getCurrencyManager().Position = index;
        }

        private Clan findClan(int broj)
        {
            foreach (Clan c in entities)
            {
                if (c.Broj == broj)
                    return c;
            }
            return null;
        }

        private Clan searchForClan(string text)
        {
            foreach (Clan c in entities)
            {
                if (c.PrezimeIme.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                    return c;
            }
            return null;
        }

        private void ClanoviForm_Shown(object sender, EventArgs e)
        {
            txtClan.Focus();
        }
    }
}