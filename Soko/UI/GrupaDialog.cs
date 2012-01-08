using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Soko.Dao;
using Soko.Exceptions;

namespace Soko.UI
{
    public partial class GrupaDialog : EntityDetailForm
    {
        private SifraGrupe oldSifra;
        private string oldNaziv;
        private List<Kategorija> kategorije;

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
            return MapperRegistry.grupaDAO().getById(new Key(id));
        }

        protected override void loadData()
        {
            kategorije = loadKategorije();
        }

        private List<Kategorija> loadKategorije()
        {
            List<Kategorija> result = MapperRegistry.kategorijaDAO().getAll();

            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(typeof(Kategorija))["Naziv"];
            result.Sort(new SortComparer<Kategorija>(propDesc, ListSortDirection.Ascending));

            return result;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Grupa";

            this.txtSifra.MaxLength = Grupa.SIFRA_MAX_LENGTH;
            txtSifra.Text = String.Empty;
            txtNaziv.Text = String.Empty;

            setKategorije(kategorije);
            SelectedKategorija = null;
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

        protected override void saveOriginalData(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            oldSifra = g.Sifra;
            oldNaziv = g.Naziv;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            txtSifra.Text = g.Sifra.Value;
            txtNaziv.Text = g.Naziv;
            SelectedKategorija = g.Kategorija;
        }

        private void GrupaDialog_Shown(object sender, EventArgs e)
        {
            btnOdustani.Focus();
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            handleOkClick();
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
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            Notification notification = new Notification();

            GrupaDAO grupaDAO = MapperRegistry.grupaDAO();
            if (grupaDAO.getById(g.Sifra) != null)
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
            MapperRegistry.grupaDAO().insert((Grupa)entity);
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            Notification notification = new Notification();

            GrupaDAO grupaDAO = MapperRegistry.grupaDAO();
            bool sifraChanged = (g.Sifra != oldSifra) ? true : false;
            if (sifraChanged && grupaDAO.getById(g.Sifra) != null)
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
            Grupa g = (Grupa)entity;
            if (g.Sifra == oldSifra)
                MapperRegistry.grupaDAO().update(g);
            else
                MapperRegistry.grupaDAO().update(g, oldSifra);
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }
    }
}