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
    public partial class FinansijskeCelineForm : EntityListForm
    {
        private const string NAZIV = "Naziv";

        public FinansijskeCelineForm()
        {
            InitializeComponent();
            this.Shown += FinansijskeCelineForm_Shown;
            initialize(typeof(FinansijskaCelina));
        }

        void FinansijskeCelineForm_Shown(object sender, EventArgs e)
        {
            clearSelection();
        }

        protected override DataGridView getDataGridView()
        {
            return dataGridView1;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Size = new Size(Size.Width, 350);
            this.Text = "Finansijske celine";
        }

        protected override void addGridColumns()
        {
            AddColumn("Naziv finansijske celine", NAZIV, 170);
        }

        protected override List<object> loadEntities()
        {
            FinansijskaCelinaDAO finansijskaCelinaDAO = DAOFactoryFactory.DAOFactory.GetFinansijskaCelinaDAO();
            return new List<FinansijskaCelina>(finansijskaCelinaDAO.FindAllSortById()).ConvertAll<object>(
                delegate(FinansijskaCelina f)
                {
                    return f;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
        {
            return new FinansijskaCelinaDialog(entityId);
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
            return String.Format("Da li zelite da izbrisete finansijsku celinu \"{0}\"?", entity);
        }

        protected override bool refIntegrityDeleteDlg(DomainObject entity)
        {
            FinansijskaCelina f = (FinansijskaCelina)entity;
            GrupaDAO grupaDao = DAOFactoryFactory.DAOFactory.GetGrupaDAO();

            if (grupaDao.existsGrupa(f))
            {
                string msg = "Finansijsku celinu '{0}' nije moguce izbrisati zato sto postoje " +
                    "grupe za datu finansijsku celinu. \n\nDa bi neka finansijska celina mogla da se " +
                    "izbrise, uslov je da ne postoje grupe za tu finansijsku celinu. To " +
                    "znaci da morate najpre da pronadjete sve grupe za datu " +
                    "finansijsku celinu, i da zatim, u prozoru u kome " +
                    "se menjaju podaci o grupi, promenite finansijsku celinu za datu grupu. " +
                    "Nakon sto ste ovo uradili za sve " +
                    "grupe za datu finansijsku celinu, moci cete da izbrisete finansijsku celinu. ";
                MessageDialogs.showMessage(String.Format(msg, f), this.Text);
                return false;
            }
            return true;
        }

        protected override void delete(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetFinansijskaCelinaDAO().MakeTransient((FinansijskaCelina)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja finansijske celine.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}