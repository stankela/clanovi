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
    public partial class InstitucijaDialog : EntityDetailForm
    {
        private List<Mesto> mesta;
        private string oldNaziv;
        private const string PRAZNO = "<<prazno>>";

        public InstitucijaDialog(Key entityId)
        {
            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new Institucija();
        }

        protected override DomainObject getEntityById(Key id)
        {
            return MapperRegistry.institucijaDAO().getById(id);
        }

        protected override void loadData()
        {
            mesta = loadMesta();
        }

        private List<Mesto> loadMesta()
        {
            List<Mesto> result = MapperRegistry.mestoDAO().getAll();

            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(typeof(Mesto))["Naziv"];
            result.Sort(new SortComparer<Mesto>(propDesc, ListSortDirection.Ascending));

            return result;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Institucija";

            txtNaziv.Text = String.Empty;
            txtAdresa.Text = String.Empty;

            cmbMesto.DropDownStyle = ComboBoxStyle.DropDownList;
            setMesta(mesta);
            SelectedMesto = null;
        }

        private void setMesta(List<Mesto> mesta)
        {
            cmbMesto.Items.Clear();
            cmbMesto.Items.Add(PRAZNO);
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
                if (cmbMesto.SelectedIndex > 0)
                    return mesta[cmbMesto.SelectedIndex - 1];
                else
                    return null;
            }
            set
            {
                if (value == null || mesta.IndexOf(value) == -1)
                    cmbMesto.SelectedIndex = -1;
                else
                    cmbMesto.SelectedIndex = mesta.IndexOf(value) + 1;
            }
        }

        protected override void saveOriginalData(DomainObject entity)
        {
            Institucija inst = (Institucija)entity;
            oldNaziv = inst.Naziv;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            Institucija inst = (Institucija)entity;
            txtNaziv.Text = inst.Naziv;
            txtAdresa.Text = inst.Adresa;
            SelectedMesto = inst.Mesto;
        }

        private void InstitucijaDialog_Shown(object sender, EventArgs e)
        {
            btnOdustani.Focus();
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            handleOkClick();
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            if (txtNaziv.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "Naziv", "Naziv institucije je obavezan.");
            }
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Naziv":
                    txtNaziv.Focus();
                    break;

                case "Adresa":
                    txtAdresa.Focus();
                    break;

                case "Mesto":
                    cmbMesto.Focus();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            Institucija inst = (Institucija)entity;
            inst.Naziv = txtNaziv.Text.Trim();
            inst.Adresa = txtAdresa.Text.Trim();
            inst.Mesto = SelectedMesto;
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            Institucija inst = (Institucija)entity;
            Notification notification = new Notification();

            InstitucijaDAO instDAO = MapperRegistry.institucijaDAO();
            if (instDAO.existsInstitucijaNaziv(inst.Naziv))
            {
                notification.RegisterMessage("Naziv", "Institucija sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override bool insertEntity(DomainObject entity)
        {
            return MapperRegistry.institucijaDAO().insert((Institucija)entity);
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            Institucija inst = (Institucija)entity;
            Notification notification = new Notification();
            InstitucijaDAO instDAO = MapperRegistry.institucijaDAO();

            bool nazivChanged = (inst.Naziv.ToUpper() != oldNaziv.ToUpper()) ? true : false;
            if (nazivChanged && instDAO.existsInstitucijaNaziv(inst.Naziv))
            {
                notification.RegisterMessage("Naziv", "Institucija sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override bool updateEntity(DomainObject entity)
        {
            return MapperRegistry.institucijaDAO().update((Institucija)entity);
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }
    }
}