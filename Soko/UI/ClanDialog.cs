using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Bilten.Dao;
using Soko.Exceptions;
using Soko.Misc;

namespace Soko.UI
{
    public partial class ClanDialog : EntityDetailForm
    {
        private List<Mesto> mesta;
        private List<Institucija> institucije;
        private string oldIme;
        private string oldPrezime;
        private Nullable<DateTime> oldDatumRodjenja;

        public ClanDialog(Nullable<int> entityId)
        {
            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new Clan();
        }

        protected override DomainObject getEntityById(int id)
        {
            return DAOFactoryFactory.DAOFactory.GetClanDAO().FindById(id);
        }

        protected override void saveOriginalData(DomainObject entity)
        {
            Clan clan = (Clan)entity;
            oldIme = clan.Ime;
            oldPrezime = clan.Prezime;
            oldDatumRodjenja = clan.DatumRodjenja;
        }

        protected override void loadData()
        {
            mesta = loadMesta();
            institucije = loadInstitucije();
        }

        private List<Mesto> loadMesta()
        {
            List<Mesto> result = new List<Mesto>(DAOFactoryFactory.DAOFactory.GetMestoDAO().FindAll());

            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(typeof(Mesto))["Naziv"];
            result.Sort(new SortComparer<Mesto>(propDesc, ListSortDirection.Ascending));

            return result;
        }

        private List<Institucija> loadInstitucije()
        {
            List<Institucija> result = new List<Institucija>(DAOFactoryFactory.DAOFactory.GetInstitucijaDAO().FindAll());

            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(typeof(Institucija))["Naziv"];
            result.Sort(new SortComparer<Institucija>(propDesc, ListSortDirection.Ascending));

            return result;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Clan";

            if (!editMode)
            {
                ckbKartica.Visible = false;
                ckbKartica.Enabled = false;
            }

            txtBroj.Text = String.Empty;
            txtBroj.ReadOnly = true;
            txtBroj.BackColor = SystemColors.Window;
            if (!editMode)
            {
                txtBroj.Text = getNewBroj().ToString();
            }
            txtIme.Text = String.Empty;
            txtPrezime.Text = String.Empty;
            txtDatumRodjenja.Text = String.Empty;
            txtAdresa.Text = String.Empty;
            txtNazivMesta.Text = String.Empty;
            txtTelefon1.Text = String.Empty;
            txtTelefon2.Text = String.Empty;
            txtNapomena.Text = String.Empty;
            ckbPristupnica.Checked = false;
            ckbKartica.Checked = false;

            setMesta(mesta);
            SelectedMesto = null;

            setInstitucije(institucije);
            SelectedInstitucija = null;
        }

        private void setMesta(List<Mesto> mesta)
        {
            cmbMesto.Items.Clear();
            foreach (Mesto m in mesta)
            {
                string item = m.Zip + "   " + m.Naziv;
                cmbMesto.Items.Add(item);
            }
        }

        private Mesto SelectedMesto
        {
            get
            {
                if (cmbMesto.SelectedIndex >= 0)
                    return mesta[cmbMesto.SelectedIndex];
                else
                    return null;
            }
            set
            {
                if (value == null || mesta.IndexOf(value) == -1)
                    cmbMesto.SelectedIndex = -1;
                else
                    cmbMesto.SelectedIndex = mesta.IndexOf(value);
            }
        }

        private void setInstitucije(List<Institucija> institucije)
        {
            cmbInstitucija.Items.Clear();
            foreach (Institucija i in institucije)
            {
                cmbInstitucija.Items.Add(i.NazivAdresaMesto);
            }
        }

        private Institucija SelectedInstitucija
        {
            get
            {
                if (cmbInstitucija.SelectedIndex >= 0)
                    return institucije[cmbInstitucija.SelectedIndex];
                else
                    return null;
            }
            set
            {
                if (value == null || institucije.IndexOf(value) == -1)
                    cmbInstitucija.SelectedIndex = -1;
                else
                    cmbInstitucija.SelectedIndex = institucije.IndexOf(value);
            }
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            Clan c = (Clan)entity;
            if (c == null)
              c = new Clan();

            txtBroj.Text = c.Broj.ToString();
            txtIme.Text = c.Ime;
            txtPrezime.Text = c.Prezime;

            txtDatumRodjenja.Text = String.Empty;
            if (c.DatumRodjenja != null)
                txtDatumRodjenja.Text = c.DatumRodjenja.Value.ToString("dd-MM-yyyy");
            
            txtAdresa.Text = c.Adresa;
            SelectedMesto = c.Mesto;
            txtTelefon1.Text = c.Telefon1;
            txtTelefon2.Text = c.Telefon2;
            SelectedInstitucija = c.Institucija;
            txtNapomena.Text = c.Napomena;
            ckbPristupnica.Checked = c.ImaPristupnicu;
            ckbKartica.Checked = c.ImaKarticu;
        }

        private void ClanDialog_Shown(object sender, EventArgs e)
        {
            if (!editMode)
                txtIme.Focus();
            else
                btnOdustani.Focus();
        }

        private int getNewBroj()
        {
            return DAOFactoryFactory.DAOFactory.GetClanDAO().getMaxBroj() + 1;
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            handleOkClick();
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            DateTime dummy;
            if (txtIme.Text.Trim() == String.Empty && txtPrezime.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "Ime", "Ime ili prezime je obavezno.");
            }
            if (txtDatumRodjenja.Text.Trim() != String.Empty
            && !DateTime.TryParse(txtDatumRodjenja.Text, out dummy))
            {
                notification.RegisterMessage(
                    "DatumRodjenja", "Nepravilan datum. Datum se unosi u formatu dd-mm-gggg ili dd.mm.gggg");
            }
            if (SelectedMesto == null && txtNazivMesta.Text.Trim() != String.Empty)
            {
                notification.RegisterMessage(
                    "Mesto", "Uneli ste nepostojeci naziv mesta.");
            }
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Broj":
                    txtBroj.Focus();
                    break;

                case "Ime":
                    txtIme.Focus();
                    break;

                case "Prezime":
                    txtPrezime.Focus();
                    break;

                case "DatumRodjenja":
                    txtDatumRodjenja.Focus();
                    break;

                case "Adresa":
                    txtAdresa.Focus();
                    break;

                case "Mesto":
                    txtNazivMesta.Focus();
                    break;

                case "Telefon1":
                    txtTelefon1.Focus();
                    break;

                case "Telefon2":
                    txtTelefon2.Focus();
                    break;

                case "Institucija":
                    cmbInstitucija.Focus();
                    break;

                case "Napomena":
                    txtNapomena.Focus();
                    break;

                case "Pristupnica":
                    ckbPristupnica.Focus();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            Clan c = (Clan)entity;
            c.Broj = int.Parse(txtBroj.Text);

            string ime = txtIme.Text.Trim();
            if (ime != String.Empty)
                c.Ime = ime;
            else
                c.Ime = null;

            string prezime = txtPrezime.Text.Trim();
            if (prezime != String.Empty)
                c.Prezime = prezime;
            else
                c.Prezime = null;

            string datumRodj = txtDatumRodjenja.Text.Trim();
            if (datumRodj != String.Empty)
                c.DatumRodjenja = DateTime.Parse(datumRodj);
            else
                c.DatumRodjenja = null;

            string adresa = txtAdresa.Text.Trim();
            if (adresa != String.Empty)
                c.Adresa = adresa;
            else
                c.Adresa = null;

            c.Mesto = SelectedMesto;

            string telefon1 = txtTelefon1.Text.Trim();
            if (telefon1 != String.Empty)
                c.Telefon1 = telefon1;
            else
                c.Telefon1 = null;

            string telefon2 = txtTelefon2.Text.Trim();
            if (telefon2 != String.Empty)
                c.Telefon2 = telefon2;
            else
                c.Telefon2 = null;

            c.Institucija = SelectedInstitucija;

            string napomena = txtNapomena.Text.Trim();
            if (napomena != String.Empty)
                c.Napomena = napomena;
            else
                c.Napomena = null;

            c.ImaPristupnicu = ckbPristupnica.Checked;
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            Clan clan = (Clan)entity;
            Notification notification = new Notification();

            ClanDAO clanDAO = DAOFactoryFactory.DAOFactory.GetClanDAO();
            if (clanDAO.existsClanImePrezimeDatumRodjenja(clan))
            {
                notification.RegisterMessage("Ime",
                    "Clan sa datim imenom, prezimenom i datumom rodjenja vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            Clan clan = (Clan)entity;
            Notification notification = new Notification();

            ClanDAO clanDAO = DAOFactoryFactory.DAOFactory.GetClanDAO();
            if (hasImePrezimeDatumRodjenjaChanged(clan)
            && clanDAO.existsClanImePrezimeDatumRodjenja(clan))
            {
                notification.RegisterMessage("Ime",
                    "Clan sa datim imenom, prezimenom i datumom rodjenja vec postoji.");
                throw new BusinessException(notification);
            }
        }

        private bool hasImePrezimeDatumRodjenjaChanged(Clan c)
        {
            bool equals = string.IsNullOrEmpty(c.Ime) && string.IsNullOrEmpty(oldIme)
                            || (!string.IsNullOrEmpty(c.Ime)
                                && !string.IsNullOrEmpty(oldIme)
                                && c.Ime.ToUpper() == oldIme.ToUpper());
            if (!equals)
                return true;

            equals = string.IsNullOrEmpty(c.Prezime) && string.IsNullOrEmpty(oldPrezime)
                            || (!string.IsNullOrEmpty(c.Prezime)
                                && !string.IsNullOrEmpty(oldPrezime)
                                && c.Prezime.ToUpper() == oldPrezime.ToUpper());
            if (!equals)
                return true;

            equals = c.DatumRodjenja == null && oldDatumRodjenja == null
                            || (c.DatumRodjenja != null
                                && oldDatumRodjenja != null
                                && c.DatumRodjenja.Value == oldDatumRodjenja.Value);
            return !equals;
        }

        protected override void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent((Clan)entity);
        }

        protected override void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent((Clan)entity);
        }

        private void txtNazivMesta_TextChanged(object sender, System.EventArgs e)
        {
            SelectedMesto = findMesto(txtNazivMesta.Text.Trim());
        }

        private Mesto findMesto(string naziv)
        {
            foreach (Mesto m in mesta)
            {
                if (m.Naziv.ToUpper() == naziv.ToUpper())
                    return m;
            }
            return null;
        }

        private void cmbMesto_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (SelectedMesto != null)
                txtNazivMesta.Text = SelectedMesto.Naziv;
            else
                txtNazivMesta.Text = String.Empty;
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }

        private void ClanDialog_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.AllScreens[0];
            this.Location = new Point((screen.Bounds.Width - this.Width) / 2, (screen.Bounds.Height - this.Height) / 2);
        }
    }
}