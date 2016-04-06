using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Bilten.Dao;

namespace Soko.UI
{
    public partial class KategorijeForm : EntityListForm
    {
        private const string NAZIV = "Naziv";

        public KategorijeForm()
        {
            InitializeComponent();
            initialize(typeof(Kategorija));
            sort(NAZIV);
        }

        protected override DataGridView getDataGridView()
        {
            return dataGridView1;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Size = new Size(Size.Width, 350);
            this.Text = "Kategorije";
        }

        protected override void addGridColumns()
        {
            AddColumn("Naziv kategorije", NAZIV, 170);
        }

        protected override List<object> loadEntities()
        {
            KategorijaDAO kategorijaDAO = DAOFactoryFactory.DAOFactory.GetKategorijaDAO();
            return new List<Kategorija>(kategorijaDAO.FindAll()).ConvertAll<object>(
                delegate(Kategorija kat)
                {
                    return kat;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
        {
            return new KategorijaDialog(entityId);
        }

        private void btnDodaj_Click(object sender, System.EventArgs e)
        {
            addCommand();
        }

        private void btnPromeni_Click(object sender, System.EventArgs e)
        {
            editCommand();
        }

        private void btnBrisi_Click(object sender, System.EventArgs e)
        {
            deleteCommand();
        }

        protected override string deleteConfirmationMessage(DomainObject entity)
        {
            return String.Format("Da li zelite da izbrisete kategoriju \"{0}\"?", entity);
        }

        protected override bool refIntegrityDeleteDlg(DomainObject entity)
        {
            Kategorija k = (Kategorija)entity;
            GrupaDAO grupaDao = DAOFactoryFactory.DAOFactory.GetGrupaDAO();

            if (grupaDao.existsGrupa(k))
            {
                string msg = "Kategoriju '{0}' nije moguce izbrisati zato sto postoje " +
                    "grupe za datu kategoriju. \n\nDa bi neka kategorija mogla da se " +
                    "izbrise, uslov je da ne postoje grupe za tu kategoriju. To " +
                    "znaci da morate najpre da pronadjete sve grupe za datu " +
                    "kategoriju, i da zatim, u prozoru u kome " +
                    "se menjaju podaci o grupi, polje za kategoriju ostavite prazno. " +
                    "Nakon sto ste ovo uradili za sve " +
                    "grupe za datu kategoriju, moci cete da izbrisete kategoriju. ";
                MessageDialogs.showMessage(String.Format(msg, k), this.Text);
                return false;
            }
            return true;
        }

        protected override void delete(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetKategorijaDAO().MakeTransient((Kategorija)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja kategorije.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void KategorijeForm_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.AllScreens[0];
            this.Location = new Point((screen.Bounds.Width - this.Width) / 2, (screen.Bounds.Height - this.Height) / 2);
        }
    }
}