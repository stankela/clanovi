using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Bilten.Dao;
using NHibernate;
using NHibernate.Context;
using Soko.Data;
using Soko.Exceptions;

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
            MestoDAO mestoDAO = DAOFactoryFactory.DAOFactory.GetMestoDAO();
            return new List<Mesto>(mestoDAO.FindAll()).ConvertAll<object>(
                delegate(Mesto m)
                {
                    return m;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
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
            ClanDAO clanDao = DAOFactoryFactory.DAOFactory.GetClanDAO();
            InstitucijaDAO instDao = DAOFactoryFactory.DAOFactory.GetInstitucijaDAO();

            if (clanDao.existsClanMesto(m))
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
            else if (instDao.existsInstitucijaMesto(m))
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

        protected override void delete(DomainObject entity)
        {
            MestoDAO mestoDAO = DAOFactoryFactory.DAOFactory.GetMestoDAO();
            mestoDAO.MakeTransient((Mesto)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja mesta.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void MestaForm_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.AllScreens[0];
            this.Location = new Point((screen.Bounds.Width - this.Width) / 2, (screen.Bounds.Height - this.Height) / 2);
        }
    }
}