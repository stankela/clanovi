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
    public partial class MestoDialog : EntityDetailForm
    {
        private string oldZip;
        private string oldNaziv;

        public MestoDialog(Key entityId)
        {
            InitializeComponent();
            initialize(entityId, true);
        }

        protected override DomainObject createNewEntity()
        {
            return new Mesto();
        }

        protected override DomainObject getEntityById(Key id)
        {
            return MapperRegistry.mestoDAO().getById(id);
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
            oldZip = m.Zip;
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

            MestoDAO mestoDAO = MapperRegistry.mestoDAO();
            if (mestoDAO.getById(m.Zip) != null)
            {
                notification.RegisterMessage("Zip", "Mesto sa datim postanskim brojem vec postoji.");
                throw new BusinessException(notification);
            }
            if (mestoDAO.existsMestoNaziv(m.Naziv))
            {
                notification.RegisterMessage("Naziv", "Mesto sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override bool insertEntity(DomainObject entity)
        {
            return MapperRegistry.mestoDAO().insert((Mesto)entity);
        }

        protected override void checkBusinessRulesOnUpdate(DomainObject entity)
        {
            Mesto m = (Mesto)entity;
            Notification notification = new Notification();

            MestoDAO mestoDAO = MapperRegistry.mestoDAO();
            bool zipChanged = (m.Zip.ToUpper() != oldZip.ToUpper()) ? true : false;
            if (zipChanged && mestoDAO.getById(m.Zip) != null)
            {
                notification.RegisterMessage("Zip", "Mesto sa datim postanskim brojem vec postoji.");
                throw new BusinessException(notification);
            }

            bool nazivChanged = (m.Naziv.ToUpper() != oldNaziv.ToUpper()) ? true : false;
            if (nazivChanged && mestoDAO.existsMestoNaziv(m.Naziv))
            {
                notification.RegisterMessage("Naziv", "Mesto sa datim nazivom vec postoji.");
                throw new BusinessException(notification);
            }
        }

        protected override bool updateEntity(DomainObject entity)
        {
            Mesto m = (Mesto)entity;
            if (m.Zip == oldZip)
                return MapperRegistry.mestoDAO().update(m);
            else
                return MapperRegistry.mestoDAO().update(m, oldZip);
        }

        private void btnOdustani_Click(object sender, System.EventArgs e)
        {
            handleCancelClick();
        }
    }
}