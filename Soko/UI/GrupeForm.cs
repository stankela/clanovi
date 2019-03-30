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
    public partial class GrupeForm : EntityListForm
    {
        private const string SIFRA = "Sifra";
        private const string NAZIV = "Naziv";
        private const string KATEGORIJA = "Kategorija";
        private const string FINANSIJSKA_CELINA = "FinansijskaCelina";
        private const string IMA_GODISNJU_CLANARINU = "ImaGodisnjuClanarinu";

        public GrupeForm()
        {
            InitializeComponent();
            this.Shown += GrupeForm_Shown;
            initialize(typeof(Grupa));
            sort(SIFRA);
        }

        void GrupeForm_Shown(object sender, EventArgs e)
        {
            // Ponisti selekciju za prvo prikazivanje
            // TODO3: Uradi ovo i na ostalim mestima.
            clearSelection();
        }

        protected override DataGridView getDataGridView()
        {
            return dataGridView1;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Size = new Size(Size.Width, 450);
            this.Text = "Grupe";
            dataGridView1.CellFormatting += new DataGridViewCellFormattingEventHandler(dataGridView1_CellFormatting);
        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // This method formats only boolean columns. Additional formating can be
            // specified in the AddColumn method.
            DataGridView dgw = (DataGridView)sender;
            if (e.Value == null)
                return;
            if (e.Value.GetType() == typeof(bool))
            {
                if ((bool)e.Value == true)
                    e.Value = "Da";
                else
                    e.Value = "";
                e.FormattingApplied = true;
            }
        }

        protected override void addGridColumns()
        {
            AddColumn("Sifra grupe", SIFRA, 50);
            AddColumn("Naziv grupe", NAZIV, 200);
            AddColumn("Kategorija", KATEGORIJA, 170);
            AddColumn("Finansijska celina", FINANSIJSKA_CELINA, 170);
            AddColumn("Ima godisnju clanarinu", IMA_GODISNJU_CLANARINU, 100);
        }

        protected override List<object> loadEntities()
        {
            GrupaDAO grupaDAO = DAOFactoryFactory.DAOFactory.GetGrupaDAO();
            return new List<Grupa>(grupaDAO.FindAll()).ConvertAll<object>(
                delegate(Grupa g)
                {
                    return g;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
        {
            return new GrupaDialog(entityId);
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
            return String.Format("Da li zelite da izbrisete grupu \"{0}\"?", entity);
        }

        protected override bool refIntegrityDeleteDlg(DomainObject entity)
        {
            Grupa g = (Grupa)entity;
            UplataClanarineDAO uplataDao = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
            MesecnaClanarinaDAO mesecnaClanarinaDao = DAOFactoryFactory.DAOFactory.GetMesecnaClanarinaDAO();

            if (uplataDao.existsUplataGrupa(g))
            {
                string msg = "Grupu '{0}' nije moguce izbrisati zato sto postoje " +
                    "podaci o uplatama za datu grupu.";
                MessageDialogs.showMessage(String.Format(msg, g), this.Text);
                return false;
            }
            else if (mesecnaClanarinaDao.existsClanarinaGrupa(g))
            {
                string msg = "Grupu '{0}' nije moguce izbrisati zato sto postoji " +
                    "cenovnik za datu grupu. \n\nDa bi mogli da izbrisete neku grupu, " +
                    "morate najpre da izbrisete cenovnik za tu grupu. ";
                MessageDialogs.showMessage(String.Format(msg, g), this.Text);
                return false;
            }
            return true;
        }

        protected override void delete(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetGrupaDAO().MakeTransient((Grupa)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja grupe.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}