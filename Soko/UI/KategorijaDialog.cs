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
    public partial class KategorijaDialog : EntityDetailForm
    {
        private string oldNaziv;

        public KategorijaDialog(Nullable<int> entityId)
        {
            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new Kategorija();
        }

        protected override DomainObject getEntityById(int id)
        {
            return DAOFactoryFactory.DAOFactory.GetKategorijaDAO().FindById(id);
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Kategorija";

            txtNaziv.Text = String.Empty;
        }

        protected override void saveOriginalData(DomainObject entity)
        {
            Kategorija k = (Kategorija)entity;
            oldNaziv = k.Naziv;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            Kategorija k = (Kategorija)entity;
            txtNaziv.Text = k.Naziv;
        }

        private void KategorijaDialog_Shown(object sender, EventArgs e)
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
                    "Naziv", "Naziv kategorije je obavezan.");
            }
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Naziv":
                    txtNaziv.Focus();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            Kategorija k = (Kategorija)entity;
            k.Naziv = txtNaziv.Text.Trim();
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            Kategorija k = (Kategorija)entity;
            Notification notification = new Notification();

            KategorijaDAO katDAO = DAOFactoryFactory.DAOFactory.GetKategorijaDAO();
            if (katDAO.existsKategorijaNaziv(k.Naziv))
            {
                notification.RegisterMessage("Naziv", "Kategorija sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetKategorijaDAO().MakePersistent((Kategorija)entity);
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            Kategorija k = (Kategorija)entity;
            Notification notification = new Notification();

            KategorijaDAO katDAO = DAOFactoryFactory.DAOFactory.GetKategorijaDAO();
            bool nazivChanged = (k.Naziv.ToUpper() != oldNaziv.ToUpper()) ? true : false;
            if (nazivChanged && katDAO.existsKategorijaNaziv(k.Naziv))
            {
                notification.RegisterMessage("Naziv", "Kategorija sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetKategorijaDAO().MakePersistent((Kategorija)entity);
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }
    }
}