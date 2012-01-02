using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Soko.Dao;
using Soko.Report;

namespace Soko.UI
{
    public partial class PotvrdaUplateForm : EntityListForm
    {
        private readonly string CLAN = "Clan";
        private readonly string IZNOS = "Iznos";
        private readonly string DATUM_UPLATE = "DatumUplate";
        private readonly string VREME_UPLATE = "VremeUplate";
        private readonly string GRUPA = "Grupa";
        private readonly string VAZI_OD = "VaziOd";
        private readonly string NAPOMENA = "Napomena";
        private readonly string KORISNIK = "Korisnik";

        private List<Clan> clanovi;

        public PotvrdaUplateForm()
        {
            InitializeComponent();

            clanovi = loadClanovi();
            setClanovi(clanovi);
            if (clanovi.Count > 0)
                SelectedClan = clanovi[0];

            // potrebno je da se radio buttons podese pre poziva initialize, da bi
            // loadEntities pravilno radio
            rbtInterval.Checked = true;
            cmbClan.Enabled = false;
            rbtClan.CheckedChanged += new System.EventHandler(rbtClan_CheckedChanged);
            rbtInterval.CheckedChanged += new System.EventHandler(rbtInterval_CheckedChanged);
            cmbClan.SelectedIndexChanged += new System.EventHandler(cmbClan_SelectedIndexChanged);

            initialize(typeof(UplataDTO));
            //sortByDatumVreme();
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

        protected override DataGridView getDataGridView()
        {
            return dataGridView1;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Potvrda o uplati";
            this.Size = new Size(Size.Width, 450);

            this.dtpOd.CustomFormat = "d.M.yyyy";
            this.dtpOd.Format = DateTimePickerFormat.Custom;
            this.dtpDo.CustomFormat = "d.M.yyyy";
            this.dtpDo.Format = DateTimePickerFormat.Custom;
        }

        private void sortByDatumVreme()
        {
            PropertyDescriptor propDescDatum =
                TypeDescriptor.GetProperties(typeof(UplataDTO))["DatumUplate"];
            PropertyDescriptor propDescVreme =
                TypeDescriptor.GetProperties(typeof(UplataDTO))["VremeUplate"];
            PropertyDescriptor[] propDesc = new PropertyDescriptor[2] { propDescDatum, propDescVreme };
            ListSortDirection[] direction = new ListSortDirection[2] { ListSortDirection.Descending, ListSortDirection.Descending };

            entities.Sort(new SortComparer<object>(propDesc, direction));
        }

        protected override void addGridColumns()
        {
            AddColumn("Clan", CLAN, 180);
            AddColumn("Iznos", IZNOS, DataGridViewContentAlignment.MiddleRight, "{0:f2}");
            AddColumn("Datum uplate", DATUM_UPLATE, DataGridViewContentAlignment.MiddleRight);
            AddColumn("Vreme uplate", VREME_UPLATE, DataGridViewContentAlignment.MiddleCenter, "{0:t}");
            AddColumn("Grupa", GRUPA, 220);
            AddColumn("Vazi od", VAZI_OD, DataGridViewContentAlignment.MiddleRight);
            AddColumn("Napomena", NAPOMENA, 150, DataGridViewContentAlignment.MiddleCenter);
            AddColumn("Korisnik", KORISNIK, DataGridViewContentAlignment.MiddleCenter);
        }

        protected override List<object> loadEntities()
        {
            if (rbtClan.Checked)
                return loadUplateForClan(SelectedClan);
            else
            {
                return loadUplateForInterval(dtpOd.Value.Date, dtpDo.Value.Date);
            }
        }

        private List<object> loadUplateForInterval(DateTime from, DateTime to)
        {
            return MapperRegistry.uplataClanarineDAO().getUplate(from, to).ConvertAll<object>(
                delegate(UplataDTO u)
                {
                    return u;
                });
        }

        private List<object> loadUplateForClan(Clan c)
        {
            if (c == null)
                return new List<object>();

            return MapperRegistry.uplataClanarineDAO().getUplate(c.Key.intValue()).ConvertAll<object>(
                delegate(UplataDTO u)
                {
                    return u;
                });
        }

        private void cmbClan_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // nema potrebe da se startuje nova sesija zato sto form ne menja podatke
            //MapperRegistry.initialize();

            updateGrid();
        }

        private void showUplateForClan(Clan c)
        {
            setEntities(loadUplateForClan(c));
            //sortByDatumVreme();
        }

        private void updateGrid()
        {
            if (rbtClan.Checked)
                showUplateForClan(SelectedClan);
            else
                showUplateForInterval(dtpOd.Value.Date, dtpDo.Value.Date);
        }

        private void showUplateForInterval(DateTime from, DateTime to)
        {
            setEntities(loadUplateForInterval(from, to));
            //sortByDatumVreme();
        }

        private void rbtClan_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbtClan.Checked)
            {
                cmbClan.Enabled = true;
                dtpOd.Enabled = false;
                dtpDo.Enabled = false;
                lblOd.Enabled = false;
                lblDo.Enabled = false;
                updateGrid();
            }
        }

        private void rbtInterval_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbtInterval.Checked)
            {
                cmbClan.Enabled = false;
                dtpOd.Enabled = true;
                dtpDo.Enabled = true;
                lblOd.Enabled = true;
                lblDo.Enabled = true;

                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                updateGrid();

                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void dtpOd_CloseUp(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            updateGrid();

            Cursor.Hide();
            Cursor.Current = Cursors.Arrow;
        }

        private void dtpDo_CloseUp(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            updateGrid();

            Cursor.Hide();
            Cursor.Current = Cursors.Arrow;
        }

        private void btnStampaj_Click(object sender, System.EventArgs e)
        {
            // TODO: Probaj da promenis i EntityListForm i EntityDetailForm tako da 
            // rade za bilo koji objekt (da mogu da se koriste i za npr. DTO objekte)

            UplataDTO uplata = (UplataDTO)getSelectedEntity();
            if (uplata == null)
                return;

            PreviewDialog p = new PreviewDialog();
            p.setIzvestaj(new PotvrdaIzvestaj(uplata.IdStavke));
            p.ShowDialog();
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void PotvrdaUplateForm_Shown(object sender, EventArgs e)
        {
            btnZatvori.Focus();
        }
    }
}