using Bilten.Dao;
using Soko.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class UplataDialogAdmin : EntityDetailForm
    {
        public UplataDialogAdmin(Nullable<int> entityId)
        {
            InitializeComponent();
            if (entityId == null)
                throw new Exception("UplataDialogAdmin radi samo u edit modu");
            initialize(entityId, true);
        }
    
        protected override DomainObject getEntityById(int id)
        {
            return DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO().FindById(id);
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Promeni uplatu";

            txtClan.ReadOnly = true;
            txtIznos.ReadOnly = true;
            txtDatumUplate.ReadOnly = true;
            txtGrupa.ReadOnly = true;
            txtNapomena.ReadOnly = true;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            UplataClanarine uplata = (UplataClanarine)entity;
            txtClan.Text = uplata.PrezimeImeBrojDatumRodj;
            txtIznos.Text = uplata.IznosDin;
            txtDatumUplate.Text = uplata.DatumVremeUplate.Value.ToString("dd.MM.yyyy HH:mm:ss");
            txtGrupa.Text = uplata.SifraGrupeCrtaNazivGrupe;
            txtZaMesec.Text = uplata.VaziOd.Value.ToString("dd.MM.yyyy");
            txtNapomena.Text = uplata.Napomena;
        }

        private void UplataDialogAdmin_Shown(object sender, EventArgs e)
        {
            btnCancel.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            handleOkClick();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            handleCancelClick();
        }

        protected override void requiredFieldsAndFormatValidation(Notification notification)
        {
            DateTime dummy;
            if (txtZaMesec.Text.Trim() == String.Empty)
            {
                notification.RegisterMessage("ZaMesec", "Unesite od kada vazi uplata.");
            }
            else if (!DateTime.TryParse(txtZaMesec.Text, out dummy))
            {
                notification.RegisterMessage(
                    "ZaMesec", "Nepravilan datum. Datum se unosi u formatu dd-mm-gggg ili dd.mm.gggg");
            }
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "ZaMesec":
                    txtZaMesec.Focus();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            UplataClanarine uplata = (UplataClanarine)entity;
            uplata.VaziOd = DateTime.Parse(txtZaMesec.Text);
        }

        protected override void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO().MakePersistent((UplataClanarine)entity);
        }

    }
}
