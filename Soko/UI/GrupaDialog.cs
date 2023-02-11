using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Soko.Exceptions;
using Bilten.Dao;

namespace Soko.UI
{
    public partial class GrupaDialog : EntityDetailForm
    {
        private SifraGrupe oldSifra;
        private string oldNaziv;
        private List<Kategorija> kategorije;
        private List<FinansijskaCelina> finansijskeCeline;
        private bool oldImaGodisnjuClanarinu;

        public GrupaDialog(Nullable<int> entityId)
        {
            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new Grupa();
        }

        protected override DomainObject getEntityById(int id)
        {
            return DAOFactoryFactory.DAOFactory.GetGrupaDAO().FindById(id);
        }

        protected override void loadData()
        {
            kategorije = loadKategorije();
            finansijskeCeline = loadFinansijskeCeline();
        }

        private List<Kategorija> loadKategorije()
        {
            List<Kategorija> result = new List<Kategorija>(DAOFactoryFactory.DAOFactory.GetKategorijaDAO().FindAll());

            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(typeof(Kategorija))["Naziv"];
            result.Sort(new SortComparer<Kategorija>(propDesc, ListSortDirection.Ascending));

            return result;
        }

        private List<FinansijskaCelina> loadFinansijskeCeline()
        {
            List<FinansijskaCelina> result = new List<FinansijskaCelina>(
                DAOFactoryFactory.DAOFactory.GetFinansijskaCelinaDAO().FindAllSortById());
            return result;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Grupa";
            cmbFinansijskaCelina.DropDownStyle = ComboBoxStyle.DropDownList;

            this.txtSifra.MaxLength = Grupa.SIFRA_MAX_LENGTH;
            txtSifra.Text = String.Empty;
            txtNaziv.Text = String.Empty;
            chbImaGodisnjuClanarinu.Checked = false;

            setKategorije(kategorije);
            SelectedKategorija = null;

            setFinCeline(finansijskeCeline);
            SelectedFinCelina = null;
        }

        private void setKategorije(List<Kategorija> kategorije)
        {
            cmbKategorija.Items.Clear();
            foreach (Kategorija k in kategorije)
            {
                cmbKategorija.Items.Add(k.Naziv);
            }
        }

        private Kategorija SelectedKategorija
        {
            get
            {
                if (cmbKategorija.SelectedIndex >= 0)
                    return kategorije[cmbKategorija.SelectedIndex];
                else
                    return null;
            }
            set
            {
                if (value == null || kategorije.IndexOf(value) == -1)
                    cmbKategorija.SelectedIndex = -1;
                else
                    cmbKategorija.SelectedIndex = kategorije.IndexOf(value);
            }
        }

        private void setFinCeline(List<FinansijskaCelina> finCeline)
        {
            cmbFinansijskaCelina.Items.Clear();
            foreach (FinansijskaCelina f in finCeline)
            {
                cmbFinansijskaCelina.Items.Add(f.Naziv);
            }
        }

        private FinansijskaCelina SelectedFinCelina
        {
            get
            {
                if (cmbFinansijskaCelina.SelectedIndex >= 0)
                    return finansijskeCeline[cmbFinansijskaCelina.SelectedIndex];
                else
                    return null;
            }
            set
            {
                if (value == null || finansijskeCeline.IndexOf(value) == -1)
                    cmbFinansijskaCelina.SelectedIndex = -1;
                else
                    cmbFinansijskaCelina.SelectedIndex = finansijskeCeline.IndexOf(value);
            }
        }

        protected override void saveOriginalData(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            oldSifra = g.Sifra;
            oldNaziv = g.Naziv;
            oldImaGodisnjuClanarinu = g.ImaGodisnjuClanarinu;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            txtSifra.Text = g.Sifra.Value;
            txtNaziv.Text = g.Naziv;
            chbImaGodisnjuClanarinu.Checked = g.ImaGodisnjuClanarinu;
            SelectedKategorija = g.Kategorija;
            SelectedFinCelina = g.FinansijskaCelina;
        }

        private void GrupaDialog_Shown(object sender, EventArgs e)
        {
            if (!editMode)
                txtSifra.Focus();
            else
                btnOdustani.Focus();
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            handleOkClick();
            Grupa g = (Grupa)entity;
            bool ImaGodisnjuClanarinuChanged = (editMode && g.ImaGodisnjuClanarinu != oldImaGodisnjuClanarinu)
                || !editMode && g.ImaGodisnjuClanarinu;
            if (ImaGodisnjuClanarinuChanged)
            {
                MessageDialogs.showMessage("Promenili ste godisnju clanarinu za grupu. Morate da restartujete "
                    + "program da bi izmene bile vidljive prilikom ocitavanja kartica.", "");
            }
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            SifraGrupe dummy;
            if (txtSifra.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage("Sifra", "Sifra grupe je obavezan.");
            }
            else if (!SifraGrupe.TryParse(txtSifra.Text, out dummy))
            {
                notification.RegisterMessage(
                    "Sifra", String.Format("Neispravan format za sifru grupe. Sifra " +
                    "grupe mora da zapocne sa brojem, i moze da sadrzi " +
                    "maksimalno {0} znakova.", Grupa.SIFRA_MAX_LENGTH));
            }

            if (txtNaziv.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "Naziv", "Naziv grupe je obavezan.");
            }

            if (SelectedFinCelina == null && finansijskeCeline.Count > 0)
            {
                notification.RegisterMessage(
                    "FinansijskaCelina", "Finansijska celina je obavezna.");
            }
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Sifra":
                    txtSifra.Focus();
                    break;

                case "Naziv":
                    txtNaziv.Focus();
                    break;

                case "Kategorija":
                    cmbKategorija.Focus();
                    break;

                case "FinansijskaCelina":
                    cmbFinansijskaCelina.Focus();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            g.Sifra = SifraGrupe.Parse(txtSifra.Text.Trim());
            g.Naziv = txtNaziv.Text.Trim();
            g.Kategorija = SelectedKategorija;
            g.FinansijskaCelina = SelectedFinCelina;
            g.ImaGodisnjuClanarinu = chbImaGodisnjuClanarinu.Checked;
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            Notification notification = new Notification();

            GrupaDAO grupaDAO = DAOFactoryFactory.DAOFactory.GetGrupaDAO();
            if (grupaDAO.existsGrupaSifra(g.Sifra))
            {
                notification.RegisterMessage("Sifra", "Grupa sa datom sifrom vec postoji.");
                throw new BusinessException(notification);
            }
            if (grupaDAO.existsGrupaNaziv(g.Naziv))
            {
                notification.RegisterMessage("Naziv", "Grupa sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetGrupaDAO().MakePersistent((Grupa)entity);
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            Notification notification = new Notification();

            GrupaDAO grupaDAO = DAOFactoryFactory.DAOFactory.GetGrupaDAO();
            bool sifraChanged = (g.Sifra != oldSifra) ? true : false;
            if (sifraChanged && grupaDAO.existsGrupaSifra(g.Sifra))
            {
                notification.RegisterMessage("Sifra", "Grupa sa datom sifrom vec postoji.");
                throw new BusinessException(notification);
            }

            bool nazivChanged = (g.Naziv.ToUpper() != oldNaziv.ToUpper()) ? true : false;
            if (nazivChanged && grupaDAO.existsGrupaNaziv(g.Naziv))
            {
                notification.RegisterMessage("Naziv", "Grupa sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetGrupaDAO().MakePersistent((Grupa)entity);
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }
    }
}