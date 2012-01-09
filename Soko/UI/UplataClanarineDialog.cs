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

namespace Soko.UI
{
    public partial class UplataClanarineDialog : EntityDetailForm
    {
        private List<Clan> clanovi;
        private List<Grupa> grupe;

        public UplataClanarineDialog(Nullable<int> entityId)
        {
            if (entityId != null)
                throw new ArgumentException("UplataClanarineDialog radi samo u add modu.");

            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new UplataClanarine();
        }

        protected override void loadData()
        {
            clanovi = loadClanovi();
            grupe = loadGrupe();
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

        private List<Grupa> loadGrupe()
        {
            List<Grupa> result = new List<Grupa>(DAOFactoryFactory.DAOFactory.GetGrupaDAO().FindAll());

            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(typeof(Grupa))["Naziv"];
            result.Sort(new SortComparer<Grupa>(propDesc, ListSortDirection.Ascending));

            return result;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Unos Clanarine";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // NOTE: DateTimePicker controla izgleda ne reaguje na CurrentCulture
            // pa mora rucno da se podesi srpski format
            this.dateTimePickerDatumClanarine.CustomFormat = "d.M.yyyy";
            this.dateTimePickerDatumClanarine.Format = DateTimePickerFormat.Custom;

            txtDatumUplate.ReadOnly = true;
            txtDatumUplate.Text = DateTime.Today.ToShortDateString();

            DateTime firstDayInMonth = DateTime.Today.AddDays(-(DateTime.Today.Day - 1));
            dateTimePickerDatumClanarine.Value = firstDayInMonth;

            txtIznos.Text = String.Empty;
            txtNapomena.Text = String.Empty;

            setClanovi(clanovi);
            SelectedClan = null;

            setGrupe(grupe);
            SelectedGrupa = null;
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

        private void setGrupe(List<Grupa> grupe)
        {
            cmbGrupa.DataSource = grupe;
            cmbGrupa.DisplayMember = "SifraNaziv";
        }

        private Grupa SelectedGrupa
        {
            get { return cmbGrupa.SelectedItem as Grupa; }
            set { cmbGrupa.SelectedItem = value; }
        }

        private void UplataClanarineDialog_Shown(object sender, EventArgs e)
        {
            txtSifraClana.Focus();
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            handleOkClick();
        }

        protected override bool beforePersistDlg(DomainObject entity)
        {
            string naslov = "Potvrda uplate";
            string pitanje = "Da li zelite da unesete ovu uplatu? Unos uplate je nepovratna operacija.";
            PotvrdaDialog dlg = new PotvrdaDialog(naslov, pitanje);
            dlg.StartPosition = FormStartPosition.Manual;
            Point location = new Point(txtNapomena.Location.X, txtNapomena.Location.Y);
            dlg.Location = this.PointToScreen(location);
            return dlg.ShowDialog() == DialogResult.Yes;
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            double dummy;

            if (SelectedClan == null)
            {
                notification.RegisterMessage("Clan", "Clan je obavezan.");
            }

            if (SelectedGrupa == null)
            {
                notification.RegisterMessage("Grupa", "Grupa je obavezna.");
            }

            bool correctIznos = true;
            if (txtIznos.Text.Trim() == String.Empty)
            {
                correctIznos = false;
                notification.RegisterMessage(
                    "Iznos", "Iznos uplate je obavezan.");
            }
            else if (!double.TryParse(txtIznos.Text, NumberStyles.Float, null, out dummy))
            {
                correctIznos = false;
                notification.RegisterMessage(
                    "Iznos", "Neispravan format za iznos uplate.");
            }

            if (correctIznos)
                txtIznos.Text = double.Parse(txtIznos.Text).ToString("F2");
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Clan":
                    cmbClan.Focus();
                    break;

                case "Grupa":
                    cmbGrupa.Focus();
                    break;

                case "DatumUplate":
                    txtDatumUplate.Focus();
                    break;

                case "VaziOd":
                    dateTimePickerDatumClanarine.Focus();
                    break;

                case "Iznos":
                    txtIznos.Focus();
                    break;

                case "Napomena":
                    txtNapomena.Focus();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            UplataClanarine uc = (UplataClanarine)entity;
            uc.Clan = SelectedClan;
            uc.Grupa = SelectedGrupa;
            DateTime datumUplate = DateTime.Parse(txtDatumUplate.Text);
            TimeSpan vremeUplate = DateTime.Now.TimeOfDay;
            uc.DatumVremeUplate = new DateTime(
                datumUplate.Year, datumUplate.Month, datumUplate.Day,
                vremeUplate.Hours, vremeUplate.Minutes, vremeUplate.Seconds);
            uc.VaziOd = dateTimePickerDatumClanarine.Value.Date;
            uc.Iznos = decimal.Parse(txtIznos.Text);
            if (txtNapomena.Text.Trim() != String.Empty)
                uc.Napomena = txtNapomena.Text.Trim();
            else
                uc.Napomena = null; // u Access bazi je specifikovano da ne prihvata
            // stringove duzine nula za Napomenu
            uc.Korisnik = "Admin";
        }

        protected override void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO().MakePersistent((UplataClanarine)entity);
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

        private void txtSifraGrupe_TextChanged(object sender, System.EventArgs e)
        {
            SifraGrupe sifra;
            if (SifraGrupe.TryParse(txtSifraGrupe.Text.Trim(), out sifra))
                SelectedGrupa = findGrupa(sifra);
            else
                SelectedGrupa = null;
        }

        private Grupa findGrupa(SifraGrupe sifra)
        {
            foreach (Grupa g in grupe)
            {
                if (g.Sifra == sifra)
                    return g;
            }
            return null;
        }

        private void cmbClan_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (SelectedClan != null)
                txtSifraClana.Text = SelectedClan.Broj.ToString();
            else
                txtSifraClana.Text = String.Empty;
        }

        private void cmbGrupa_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (SelectedGrupa != null)
                txtSifraGrupe.Text = SelectedGrupa.Sifra.Value;
            else
                txtSifraGrupe.Text = String.Empty;
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }
    }
}