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
using NHibernate;
using Soko.Data;
using NHibernate.Context;
using Soko.Misc;
using Soko.Exceptions;

namespace Soko.UI
{
    public partial class UplataClanarineDialog : EntityDetailForm
    {
        private List<Clan> clanovi;
        private List<Grupa> grupe;
        private DateTime currentDatumClanarine;
        public bool PendingRead = false;

        private List<UplataClanarine> uplateList = new List<UplataClanarine>();
        public List<UplataClanarine> Uplate
        {
            get { return uplateList; }
        }
        
        public UplataClanarineDialog(Nullable<int> entityId)
        {
            if (entityId != null)
                throw new ArgumentException("UplataClanarineDialog radi samo u add modu.");

            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            // koristi se uplateList
            return null;
        }

        protected override void loadData()
        {
            clanovi = loadClanovi();
            grupe = loadGrupe();
        }

        private List<Clan> loadClanovi()
        {
            List<Clan> result = new List<Clan>(DAOFactoryFactory.DAOFactory.GetClanDAO().FindAll());
            Util.sortByPrezimeImeDatumRodjenja(result);
            result.Add(createTestClan());
            return result;
        }

        private Clan createTestClan()
        {
            Clan result = new Clan();
            result.Broj = CitacKartica.TEST_KARTICA_BROJ;
            result.Prezime = CitacKartica.TEST_KARTICA_NAME;
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

            // Font podesavam ovde da bi se uzeo u obzir skalirani font (koji se podesava u base.initUI).
            lblUkupnoIznos.Font = new Font(Font.FontFamily.Name, Font.Size, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            lblUkupno.Font = new Font(Font.FontFamily.Name, Font.Size, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

            // NOTE: DateTimePicker controla izgleda ne reaguje na CurrentCulture
            // pa mora rucno da se podesi srpski format
            this.dateTimePickerDatumClanarine.CustomFormat = "MMMM yyyy";
            this.dateTimePickerDatumClanarine.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerDatumClanarine.ShowUpDown = true;

            DateTime firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            dateTimePickerDatumClanarine.Value = firstDayInMonth;
            currentDatumClanarine = dateTimePickerDatumClanarine.Value;
            dateTimePickerDatumClanarine.ValueChanged += new System.EventHandler(dateTimePickerDatumClanarine_ValueChanged);

            txtIznos.Text = String.Empty;
            txtNapomena.Text = String.Empty;
            ckbPristupnica.Checked = false;
            ckbKartica.Checked = false;
            cmbClan.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGrupa.DropDownStyle = ComboBoxStyle.DropDownList;

            listViewPrethodneUplate.View = View.Details;
            listViewPrethodneUplate.HeaderStyle = ColumnHeaderStyle.None;
            listViewPrethodneUplate.FullRowSelect = true;
            listViewPrethodneUplate.Columns.Add("Mesec");
            listViewPrethodneUplate.Columns.Add("Godina");
            listViewPrethodneUplate.Columns.Add("Iznos");
            // TODO2: Kolona Napomena je verovatno privremena i trebala bi da se ukloni nakon sto
            // se uhoda nov sistem unosa clanarine koji omogucava da se unosi po mesecima.
            listViewPrethodneUplate.Columns.Add("Napomena");
            listViewPrethodneUplate.Columns.Add("Grupa");
            listViewPrethodneUplate.Columns[0].TextAlign = HorizontalAlignment.Right;
            listViewPrethodneUplate.Columns[1].TextAlign = HorizontalAlignment.Right;
            listViewPrethodneUplate.Columns[2].TextAlign = HorizontalAlignment.Right;
            listViewPrethodneUplate.Columns[3].TextAlign = HorizontalAlignment.Left;
            listViewPrethodneUplate.Columns[4].TextAlign = HorizontalAlignment.Left;

            listViewNoveUplate.View = View.Details;
            listViewNoveUplate.HeaderStyle = ColumnHeaderStyle.None;
            listViewNoveUplate.FullRowSelect = true;
            listViewNoveUplate.Columns.Add("Iznos");
            listViewNoveUplate.Columns.Add("Mesec");
            listViewNoveUplate.Columns.Add("Godina");
            listViewNoveUplate.Columns[0].TextAlign = HorizontalAlignment.Right;
            listViewNoveUplate.Columns[1].TextAlign = HorizontalAlignment.Right;
            listViewNoveUplate.Columns[2].TextAlign = HorizontalAlignment.Right;

            // TODO2: Proveri i prikazi da li clan ima uplate za sve mesece na kojima je bio na treningu.

            setClanovi(clanovi);
            SelectedClan = null;

            setGrupe(grupe);
            SelectedGrupa = null;

            updateUkupnoIznos();
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
            txtBrojClana.Focus();
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (SelectedClan != null && SelectedClan.Broj == CitacKartica.TEST_KARTICA_BROJ)
            {
                DialogResult = DialogResult.Cancel;
                handleCancelClick();
                return;
            }
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
            if (SelectedClan == null)
            {
                notification.RegisterMessage("Clan", "Clan je obavezan.");
            }

            if (SelectedGrupa == null)
            {
                notification.RegisterMessage("Grupa", "Grupa je obavezna.");
            }

            if (listViewNoveUplate.Items.Count == 0)
            {
                notification.RegisterMessage("Iznos", "Unesite uplatu.");            
            }
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
            List<UplataItem> uplateItems = getUplataItems();
            uplateList.Clear();

            DateTime vremeUplate = DateTime.Now;
            for (int i = 0; i < uplateItems.Count; ++i)
            {
                UplataClanarine u = new UplataClanarine();
                u.Clan = SelectedClan;
                u.Grupa = SelectedGrupa;
                u.DatumVremeUplate = new DateTime(
                    vremeUplate.Year, vremeUplate.Month, vremeUplate.Day,
                    vremeUplate.Hour, vremeUplate.Minute, vremeUplate.Second);

                UplataItem uplataItem = uplateItems[i];
                u.VaziOd = uplataItem.VaziOd;
                u.Iznos = uplataItem.Iznos;

                if (txtNapomena.Text.Trim() != String.Empty)
                    u.Napomena = txtNapomena.Text.Trim();
                else
                    u.Napomena = null; // u Access bazi je specifikovano da ne prihvata
                // stringove duzine nula za Napomenu
                u.Korisnik = "Admin";

                uplateList.Add(u);
                vremeUplate = vremeUplate.AddSeconds(1);
            }
        }

        protected override void validateEntity(DomainObject entity)
        {
            Notification notification = new Notification();
            foreach (UplataClanarine u in uplateList)
            {
                u.validate(notification);
            }
            if (!notification.IsValid())
                throw new BusinessException(notification);
        }

        protected override void insertEntity(DomainObject entity)
        {
            foreach (UplataClanarine u in uplateList)
            {
                DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO().MakePersistent(u);
            }

            if (SelectedClan.ImaPristupnicu != ckbPristupnica.Checked)
            {
                SelectedClan.ImaPristupnicu = ckbPristupnica.Checked;
                DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent(SelectedClan);
            }
        }

        private void txtBrojClana_TextChanged(object sender, System.EventArgs e)
        {
            listViewPrethodneUplate.Items.Clear();
            string text = txtBrojClana.Text;
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
            ckbPristupnica.Checked = SelectedClan != null && SelectedClan.ImaPristupnicu;
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

        private void cmbClan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            listViewPrethodneUplate.Items.Clear();
            if (SelectedClan != null)
            {
                txtBrojClana.Text = SelectedClan.Broj.ToString();
                ckbPristupnica.Checked = SelectedClan.ImaPristupnicu;
                ckbKartica.Checked = SelectedClan.ImaKarticu;
            }
            else
            {
                txtBrojClana.Text = String.Empty;
                ckbPristupnica.Checked = false;
                ckbKartica.Checked = false;
            }
        }
        
        private void cmbGrupa_SelectionChangeCommitted(object sender, EventArgs e)
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

        private void btnOcitajKarticu_Click(object sender, EventArgs e)
        {
            PendingRead = true;
            btnOk.Focus();
        }

        public void Read()
        {
            if (PendingRead)
            {
                PendingRead = false;
                int broj;
                string notUsed;
                if (CitacKartica.getCitacKartica().readCard(Options.Instance.COMPortWriter, true, out broj, out notUsed))
                {
                    // SelectedClan will be updated in txtBrojClana_TextChanged
                    txtBrojClana.Text = broj.ToString();
                }
                else
                    txtBrojClana.Text = String.Empty;

                List<UplataClanarine> uplate = getUplate(SelectedClan);
                updateGrupaFromUplate(uplate);
                updatePrethodneUplate(uplate);
            }
        }

        private void updateGrupaFromUplate(List<UplataClanarine> uplate)
        {
            if (uplate != null && uplate.Count > 0)
            {
                txtSifraGrupe.Text = uplate[0].Grupa.Sifra.Value;
            }
            else
            {
                txtSifraGrupe.Text = String.Empty;
            }            
        }

        private List<UplataClanarine> getUplate(Clan c)
        {
            if (c == null || c.Broj == CitacKartica.TEST_KARTICA_BROJ)
                return null;

            List<UplataClanarine> uplate = null;
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
                    uplate = new List<UplataClanarine>(uplataClanarineDAO.findUplate(c));
                }
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(ex.Message, "Uplata clanarine");
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }

            if (uplate != null)
                Util.sortByVaziOdDesc(uplate);
            return uplate;
        }

        private void txtSifraGrupe_Enter(object sender, EventArgs e)
        {
            if (txtSifraGrupe.Text == String.Empty || SelectedGrupa == null)
            {
                List<UplataClanarine> uplate = getUplate(SelectedClan);
                updateGrupaFromUplate(uplate);
                updatePrethodneUplate(uplate);
            }
        }

        private void btnPrethodneUplate_Click(object sender, EventArgs e)
        {
            List<UplataClanarine> uplate = getUplate(SelectedClan);
            updatePrethodneUplate(uplate);
            if (txtSifraGrupe.Text == String.Empty || SelectedGrupa == null)
            {
                updateGrupaFromUplate(uplate);
            }
        }

        private void updatePrethodneUplate(List<UplataClanarine> uplate)
        {
            if (uplate == null || uplate.Count == 0)
            {
                listViewPrethodneUplate.Items.Clear();
                return;
            }
            
            ListViewItem[] items = new ListViewItem[uplate.Count];
            for (int i = 0; i < uplate.Count; ++i)
            {
                UplataClanarine u = uplate[i];
                items[i] = new ListViewItem(new string[] {
                            u.VaziOd.Value.ToString("MMM"), u.VaziOd.Value.ToString("yyyy"),
                            u.IznosDin, u.Napomena, u.Grupa.Naziv });
            }
            listViewPrethodneUplate.Items.Clear();
            listViewPrethodneUplate.Items.AddRange(items);
            listViewPrethodneUplate.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void btnUnesiUplatu_Click(object sender, EventArgs e)
        {
            if (unesiUplatu())
            {
                txtIznos.Text = String.Empty;
                dateTimePickerDatumClanarine.Value = dateTimePickerDatumClanarine.Value.AddMonths(1);
                txtIznos.Focus();
            }
        }

        private bool unesiUplatu()
        {
            Notification notification = new Notification();
            validateUplata(notification);
            if (!notification.IsValid())
            {
                NotificationMessage msg = notification.FirstMessage;
                MessageDialogs.showMessage(msg.Message, this.Text);
                setFocus(msg.FieldName);
                return false;
            }

            DateTime vaziOd = dateTimePickerDatumClanarine.Value.Date;
            string iznos = decimal.Parse(txtIznos.Text).ToString("F2");
            listViewNoveUplate.Items.Add(new ListViewItem(new string[] {
                            iznos, vaziOd.ToString("MMM"), vaziOd.ToString("yyyy") }));
            listViewNoveUplate.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            updateUkupnoIznos();
            return true;
        }

        private void validateUplata(Notification notification)
        {
            double dummy;
            if (txtIznos.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage("Iznos", "Unesite iznos uplate.");
            }
            else if (!double.TryParse(txtIznos.Text, NumberStyles.Float, null, out dummy))
            {
                notification.RegisterMessage("Iznos", "Neispravan format za iznos uplate.");
            }
            else if (double.Parse(txtIznos.Text, NumberStyles.Float) < 0.0)
            {
                notification.RegisterMessage("Iznos", "Neispravan vrednost za iznos uplate.");
            }

            foreach (UplataItem i in getUplataItems())
            {
                if (dateTimePickerDatumClanarine.Value.Date == i.VaziOd.Date)
                {
                    notification.RegisterMessage("VaziOd", "Vec ste uneli uplatu za dati mesec.");
                    break;
                }
            }
        }

        private List<UplataItem> getUplataItems()
        {
            List<UplataItem> result = new List<UplataItem>();
            for (int i = 0; i < listViewNoveUplate.Items.Count; ++i)
            {
                ListViewItem item = listViewNoveUplate.Items[i];
                UplataItem uplataItem = new UplataItem();
                uplataItem.Iznos = decimal.Parse(item.SubItems[0].Text);

                string mesec = item.SubItems[1].Text;
                string godina = item.SubItems[2].Text;
                uplataItem.VaziOd = DateTime.Parse(mesec + " " + godina);

                result.Add(uplataItem);
            }
            return result;
        }

        private void updateUkupnoIznos()
        {
            decimal total = 0;
            foreach (UplataItem item in getUplataItems())
            {
                total += item.Iznos;
            }
            lblUkupnoIznos.Text = total.ToString("F2") + " Din";
        }

        private void txtIznos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (unesiUplatu())
                {
                    txtIznos.Text = String.Empty;
                    dateTimePickerDatumClanarine.Value = dateTimePickerDatumClanarine.Value.AddMonths(1);
                    txtIznos.Focus();
                }
            }
        }

        private void btnBrisiUplatu_Click(object sender, EventArgs e)
        {
            if (listViewNoveUplate.SelectedItems.Count == 0)
            {
                MessageDialogs.showMessage("Izaberite uplatu koju zelite da izbrisete.", "Uplata clanarine");
                return;
            }

            for (int i = listViewNoveUplate.Items.Count - 1; i >= 0; --i)
            {
                ListViewItem item = listViewNoveUplate.Items[i];
                if (item.Selected)
                {
                    listViewNoveUplate.Items.Remove(item);
                }
            }
            listViewNoveUplate.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

            updateUkupnoIznos();
        }

        private void dateTimePickerDatumClanarine_ValueChanged(object sender, EventArgs e)
        {
            if (currentDatumClanarine.Month == 12 && dateTimePickerDatumClanarine.Value.Month == 1
                && currentDatumClanarine.Year == dateTimePickerDatumClanarine.Value.Year)
            {
                dateTimePickerDatumClanarine.ValueChanged -= new System.EventHandler(dateTimePickerDatumClanarine_ValueChanged);
                dateTimePickerDatumClanarine.Value = dateTimePickerDatumClanarine.Value.AddYears(1);
                dateTimePickerDatumClanarine.ValueChanged += new System.EventHandler(dateTimePickerDatumClanarine_ValueChanged);

            }
            else if (currentDatumClanarine.Month == 1 && dateTimePickerDatumClanarine.Value.Month == 12
                && currentDatumClanarine.Year == dateTimePickerDatumClanarine.Value.Year)
            {
                dateTimePickerDatumClanarine.ValueChanged -= new System.EventHandler(dateTimePickerDatumClanarine_ValueChanged);
                dateTimePickerDatumClanarine.Value = dateTimePickerDatumClanarine.Value.AddYears(-1);
                dateTimePickerDatumClanarine.ValueChanged += new System.EventHandler(dateTimePickerDatumClanarine_ValueChanged);
            }
            currentDatumClanarine = dateTimePickerDatumClanarine.Value;
        }
    }
}