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
        private List<Grupa> grupe;

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

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Promeni uplatu";
            cmbGrupa.DropDownStyle = ComboBoxStyle.DropDownList;
            setGrupe(grupe);
            SelectedGrupa = null;
        }

        protected override void updateUIFromEntity(DomainObject entity)
        {
            UplataClanarine uplata = (UplataClanarine)entity;
            SelectedGrupa = uplata.Grupa;
            lblSummary.Text = uplata.PrezimeImeBrojDatumRodj
                + "\n" + uplata.SifraGrupeCrtaNazivGrupe
                + "\n" + uplata.VaziOd.Value.ToString("dd.MM.yyyy")
                + "\n" + uplata.IznosDin;
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
            if (SelectedGrupa == null)
            {
                notification.RegisterMessage("Grupa", "Grupa je obavezna.");
            }
        }

        protected override void setFocus(string propertyName)
        {
            switch (propertyName)
            {
                case "Grupa":
                    cmbGrupa.Focus();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        protected override void updateEntityFromUI(DomainObject entity)
        {
            UplataClanarine uplata = (UplataClanarine)entity;
            uplata.Grupa = SelectedGrupa;
        }

        protected override void updateEntity(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO().MakePersistent((UplataClanarine)entity);
        }

        private void cmbGrupa_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (SelectedGrupa != null)
                txtSifraGrupe.Text = SelectedGrupa.Sifra.Value;
            else
                txtSifraGrupe.Text = String.Empty;
        }

        private void txtSifraGrupe_TextChanged(object sender, System.EventArgs e)
        {
            SifraGrupe sifra;
            if (SifraGrupe.TryParse(txtSifraGrupe.Text.Trim(), out sifra))
                SelectedGrupa = findGrupa(sifra);
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
    }
}
