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
    public partial class MestoDialog : EntityDetailForm
    {
        private string oldNaziv;

        public MestoDialog(Nullable<int> entityId)
        {
            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new Mesto();
        }

        protected override DomainObject getEntityById(int id)
        {
            return DAOFactoryFactory.DAOFactory.GetMestoDAO().FindById(id);
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Mesto";

            txtPostanskiBroj.Text = String.Empty;
            txtNaziv.Text = String.Empty;
        }

        protected override void saveOriginalData(DomainObject entity)
        {
            Mesto m = (Mesto)entity;
            oldNaziv = m.Naziv;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            Mesto m = (Mesto)entity;
            txtPostanskiBroj.Text = m.Zip;
            txtNaziv.Text = m.Naziv;
        }

        private void MestoDialog_Shown(object sender, EventArgs e)
        {
            btnOdustani.Focus();
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            handleOkClick();
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            if (txtPostanskiBroj.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "Zip", "Postanski broj je obavezan.");
            }
            if (txtNaziv.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage(
                    "Naziv", "Naziv mesta je obavezan.");
            }
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Zip":
                    txtPostanskiBroj.Focus();
                    break;

                case "Naziv":
                    txtNaziv.Focus();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            Mesto m = (Mesto)entity;
            m.Zip = txtPostanskiBroj.Text.Trim();
            m.Naziv = txtNaziv.Text.Trim();
        }

        protected override void checkBusinessRulesOnAdd(DomainObject entity)
        {
            Mesto m = (Mesto)entity;
            Notification notification = new Notification();

            MestoDAO mestoDAO = DAOFactoryFactory.DAOFactory.GetMestoDAO();
            if (mestoDAO.existsMestoNaziv(m.Naziv))
            {
                notification.RegisterMessage("Naziv", "Mesto sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void insertEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetMestoDAO().MakePersistent((Mesto)entity);
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            Mesto m = (Mesto)entity;
            Notification notification = new Notification();

            MestoDAO mestoDAO = DAOFactoryFactory.DAOFactory.GetMestoDAO();
            bool nazivChanged = (m.Naziv.ToUpper() != oldNaziv.ToUpper()) ? true : false;
            if (nazivChanged && mestoDAO.existsMestoNaziv(m.Naziv))
            {
                notification.RegisterMessage("Naziv", "Mesto sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetMestoDAO().MakePersistent((Mesto)entity);
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }

        private void MestoDialog_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.AllScreens[0];
            this.Location = new Point((screen.Bounds.Width - this.Width) / 2, (screen.Bounds.Height - this.Height) / 2);
        }
    }
}
