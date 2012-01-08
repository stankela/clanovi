using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Soko.Dao;

namespace Soko.UI
{
    public partial class GrupeForm : EntityListForm
    {
        private const string SIFRA = "Sifra";
        private const string NAZIV = "Naziv";
        private const string KATEGORIJA = "Kategorija";

        public GrupeForm()
        {
            InitializeComponent();
            initialize(typeof(Grupa));
            sort(SIFRA);
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
        }

        protected override void addGridColumns()
        {
            AddColumn("Sifra grupe", SIFRA, 50);
            AddColumn("Naziv grupe", NAZIV, 200);
            AddColumn("Naziv kategorije", KATEGORIJA, 170);
        }

        protected override List<object> loadEntities()
        {
            return MapperRegistry.grupaDAO().getAll().ConvertAll<object>(
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
            UplataClanarineDAO uplataDao = MapperRegistry.uplataClanarineDAO();
            MesecnaClanarinaDAO mesecnaClanarinaDao = MapperRegistry.mesecnaClanarinaDAO();

            if (uplataDao.existsUplataGrupa(g.Sifra.Value))
            {
                string msg = "Grupu '{0}' nije moguce izbrisati zato sto postoje " +
                    "podaci o uplatama za datu grupu.";
                MessageDialogs.showMessage(String.Format(msg, g), this.Text);
                return false;
            }
            else if (mesecnaClanarinaDao.existsClanarinaGrupa(g.Sifra.Value))
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
            MapperRegistry.grupaDAO().delete((Grupa)entity);
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