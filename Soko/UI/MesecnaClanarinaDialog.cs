using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using System.Globalization;
using Soko.Exceptions;
using Bilten.Dao;

namespace Soko.UI
{
    public partial class MesecnaClanarinaDialog : EntityDetailForm
    {
        private List<Grupa> grupe;
        private SifraGrupe pocetnaSifraGrupe;

        public MesecnaClanarinaDialog(Nullable<int> entityId, SifraGrupe pocetnaSifraGrupe)
        {
            if (entityId != null)
                throw new ArgumentException("CenaDialog radi samo u add modu.");

            InitializeComponent();
            this.pocetnaSifraGrupe = pocetnaSifraGrupe;
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new MesecnaClanarina();
        }

        protected override void loadData()
        {
            grupe = loadGrupe();
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
            this.Text = "Cena";

            txtIznos.Text = String.Empty;

            dateTimePickerVaziOd.CustomFormat = "d.M.yyyy";
            dateTimePickerVaziOd.Format = DateTimePickerFormat.Custom;
            dateTimePickerVaziOd.Value = DateTime.Today;

            setGrupe(grupe);
            if (pocetnaSifraGrupe != null)
                SelectedGrupa = findGrupa(pocetnaSifraGrupe);
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

        private void CenaDialog_Shown(object sender, EventArgs e)
        {
            btnOdustani.Focus();
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            handleOkClick();
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            decimal dummy;
            if (SelectedGrupa == null)
            {
                notification.RegisterMessage("Grupa", "Grupa je obavezna.");
            }

            bool correctIznos = true;
            if (txtIznos.Text.Trim() == String.Empty)
            {
                correctIznos = false;
                notification.RegisterMessage(
                    "Iznos", "Iznos clanarine je obavezan.");
            }
            else if (!decimal.TryParse(txtIznos.Text, NumberStyles.Float, null, out dummy))
            {
                // NOTE: NumberStyles.Float sprecava da greskom unesem tacku kao
                // decimalni separator, a da program to protumaci kao znak kojim se
                // razdvajaju hiljade i da ceo broj tretira kao integer.
                correctIznos = false;
                notification.RegisterMessage(
                    "Iznos", "Neispravan format za iznos clanarine.");
            }

            if (correctIznos)
                txtIznos.Text = decimal.Parse(txtIznos.Text).ToString("F2");
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Grupa":
                    cmbGrupa.Focus();
                    break;

                case "VaziOd":
                    dateTimePickerVaziOd.Focus();
                    break;

                case "Iznos":
                    txtIznos.Focus();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            MesecnaClanarina mc = (MesecnaClanarina)entity;
            mc.Grupa = SelectedGrupa;
            mc.VaziOd = dateTimePickerVaziOd.Value.Date;
            mc.Iznos = decimal.Parse(txtIznos.Text);
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            MesecnaClanarina mc = (MesecnaClanarina)entity;
            Notification notification = new Notification();

            MesecnaClanarinaDAO mcDAO = DAOFactoryFactory.DAOFactory.GetMesecnaClanarinaDAO();
            if (mcDAO.findForGrupaVaziOd(mc.Grupa, mc.VaziOd) != null)
            {
                notification.RegisterMessage("Grupa", "Vec postoji clanarina za izabranu grupu i datum.");
                throw new BusinessException(notification);
            }
        }

        protected override void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetMesecnaClanarinaDAO().MakePersistent((MesecnaClanarina)entity);
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }
    }
}