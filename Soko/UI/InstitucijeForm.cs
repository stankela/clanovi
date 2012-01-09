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
    public partial class InstitucijeForm : EntityListForm
    {
        private const string NAZIV = "Naziv";
        private const string ADRESA = "Adresa";
        private const string MESTO_NAZIV = "MestoNaziv";

        public InstitucijeForm()
        {
            InitializeComponent();
            initialize(typeof(Institucija));
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
            this.Text = "Institucije";
        }

        protected override void addGridColumns()
        {
            AddColumn("Naziv institucije", NAZIV, 220);
            AddColumn("Adresa", ADRESA, 175);
            AddColumn("Mesto", MESTO_NAZIV);
        }

        protected override List<object> loadEntities()
        {
            InstitucijaDAO institucijaDAO = DAOFactoryFactory.DAOFactory.GetInstitucijaDAO();
            return new List<Institucija>(institucijaDAO.FindAll()).ConvertAll<object>(
                delegate(Institucija i)
                {
                    return i;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
        {
            return new InstitucijaDialog(entityId);
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
            return String.Format("Da li zelite da izbrisete instituciju \"{0}\"?", entity);
        }

        protected override bool refIntegrityDeleteDlg(DomainObject entity)
        {
            Institucija inst = (Institucija)entity;
            ClanDAO clanDao = DAOFactoryFactory.DAOFactory.GetClanDAO();

            if (clanDao.existsClanInstitucija(inst))
            {
                string msg = "Instituciju '{0}' nije moguce izbrisati zato sto postoje " +
                    "clanovi iz date institucije. \n\nDa bi neka institucija mogla da se izbrise, " +
                    "uslov je da ne postoje clanovi iz date institucije. To znaci da morate " +
                    "najpre da pronadjete sve clanove iz date institucije, i da zatim, u " +
                    "prozoru u kome " +
                    "se menjaju podaci o clanu, polje za instituciju ostavite prazno. " +
                    "Nakon sto ste ovo uradili za sve " +
                    "clanove iz date institucije, moci cete da izbrisete instituciju. ";
                MessageDialogs.showMessage(String.Format(msg, inst), this.Text);
                return false;
            }
            return true;
        }

        protected override void delete(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetInstitucijaDAO().MakeTransient((Institucija)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja institucije.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}