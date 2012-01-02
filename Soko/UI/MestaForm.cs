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
    public partial class MestaForm : EntityListForm
    {
        private const string ZIP = "Zip";
        private const string NAZIV = "Naziv";

        public MestaForm()
        {
            InitializeComponent();
            initialize(typeof(Mesto));
            sort(NAZIV);
        }

        protected override DataGridView getDataGridView()
        {
            return dataGridView1;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Size = new Size(Size.Width, 450);
            this.Text = "Mesta";
        }

        protected override void addGridColumns()
        {
            AddColumn("Postanski broj", ZIP);
            AddColumn("Naziv mesta", NAZIV, 150);
        }

        protected override List<object> loadEntities()
        {
            return MapperRegistry.mestoDAO().getAll().ConvertAll<object>(
                delegate(Mesto m)
                {
                    return m;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Key entityId)
        {
            return new MestoDialog(entityId);
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
            return String.Format("Da li zelite da izbrisete mesto \"{0}\"?", entity);
        }

        protected override bool refIntegrityDeleteDlg(DomainObject entity)
        {
            Mesto m = (Mesto)entity;
            ClanDAO clanDao = MapperRegistry.clanDAO();
            InstitucijaDAO instDao = MapperRegistry.institucijaDAO();

            if (clanDao.existsClanMesto(m.Zip))
            {
                string msg = "Mesto '{0}' nije moguce izbrisati zato sto postoje " +
                    "clanovi iz datog mesta. \n\nDa bi neko mesto moglo da se izbrise, " +
                    "uslov je da ne postoje clanovi iz datog mesta. To znaci da morate " +
                    "najpre da pronadjete sve clanove iz datog mesta, i da zatim, u " +
                    "prozoru u kome " +
                    "se menjaju podaci o clanu, polje za mesto ostavite prazno. " +
                    "Nakon sto ste ovo uradili za sve " +
                    "clanove iz datog mesta, moci cete da izbrisete mesto. ";
                MessageDialogs.showMessage(String.Format(msg, m), this.Text);
                return false;
            }
            else if (instDao.existsInstitucijaZip(m.Zip))
            {
                string msg = "Mesto '{0}' nije moguce izbrisati zato sto postoje " +
                    "institucije iz datog mesta. \n\nDa bi neko mesto moglo da se izbrise, " +
                    "uslov je da ne postoje institucije iz datog mesta. To znaci da morate " +
                    "najpre da pronadjete sve institucije iz datog mesta, i da zatim, u " +
                    "prozoru u kome " +
                    "se menjaju podaci o instituciji, polje za mesto ostavite prazno. " +
                    "Nakon sto ste ovo uradili za sve " +
                    "institucije iz datog mesta, moci cete da izbrisete mesto. ";
                MessageDialogs.showMessage(String.Format(msg, m), this.Text);
                return false;
            }
            return true;
        }

        protected override bool delete(DomainObject entity)
        {
            return MapperRegistry.mestoDAO().delete((Mesto)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja mesta.";
        }

        protected override string deleteConcurrencyErrorMessage(DomainObject entity)
        {
            return "Neuspesno brisanje mesta.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}