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
    public partial class FinansijskaCelinaDialog : EntityDetailForm
    {
        private string oldNaziv;

        public FinansijskaCelinaDialog(Nullable<int> entityId)
        {
            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new FinansijskaCelina();
        }

        protected override DomainObject getEntityById(int id)
        {
            return DAOFactoryFactory.DAOFactory.GetFinansijskaCelinaDAO().FindById(id);
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Finansijska celina";

            txtNaziv.Text = String.Empty;
        }

        protected override void saveOriginalData(DomainObject entity)
        {
            FinansijskaCelina f = (FinansijskaCelina)entity;
            oldNaziv = f.Naziv;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            FinansijskaCelina f = (FinansijskaCelina)entity;
            txtNaziv.Text = f.Naziv;
        }

        private void FinansijskaCelinaDialog_Shown(object sender, EventArgs e)
        {
            if (!editMode)
                txtNaziv.Focus();
            else
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
                    "Naziv", "Naziv je obavezan.");
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
            FinansijskaCelina f = (FinansijskaCelina)entity;
            f.Naziv = txtNaziv.Text.Trim();
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            FinansijskaCelina f = (FinansijskaCelina)entity;
            Notification notification = new Notification();

            FinansijskaCelinaDAO finCelDAO = DAOFactoryFactory.DAOFactory.GetFinansijskaCelinaDAO();
            if (finCelDAO.existsFinansijskaCelinaNaziv(f.Naziv))
            {
                notification.RegisterMessage("Naziv", "Finansijska celina sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetFinansijskaCelinaDAO().MakePersistent((FinansijskaCelina)entity);
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            FinansijskaCelina f = (FinansijskaCelina)entity;
            Notification notification = new Notification();

            FinansijskaCelinaDAO finCelDAO = DAOFactoryFactory.DAOFactory.GetFinansijskaCelinaDAO();
            bool nazivChanged = (f.Naziv.ToUpper() != oldNaziv.ToUpper()) ? true : false;
            if (nazivChanged && finCelDAO.existsFinansijskaCelinaNaziv(f.Naziv))
            {
                notification.RegisterMessage("Naziv", "Finansijska celina sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetFinansijskaCelinaDAO().MakePersistent((FinansijskaCelina)entity);
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }
    }
}