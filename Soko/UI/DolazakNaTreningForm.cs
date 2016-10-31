using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Soko.Report;
using Bilten.Dao;
using Soko.Exceptions;
using NHibernate;
using Soko.Data;
using NHibernate.Context;
using Soko.Misc;

namespace Soko.UI
{
    public partial class DolazakNaTreningForm : EntityListForm
    {
        private readonly string DATUM_DOLASKA = "DatumDolaska";
        private readonly string VREME_DOLASKA = "VremeDolaska";

        private List<Clan> clanovi;

        public DolazakNaTreningForm()
        {
            InitializeComponent();
            initialize(typeof(DolazakNaTrening));

            cmbClan.SelectedIndexChanged += new System.EventHandler(cmbClan_SelectedIndexChanged);

            //sortByDatumVreme();
        }

        private List<Clan> loadClanovi()
        {
            List<Clan> result = new List<Clan>(DAOFactoryFactory.DAOFactory.GetClanDAO().FindAll());
            Util.sortByPrezimeImeDatumRodjenja(result);
            return result;
        }

        private void setClanovi(List<Clan> clanovi)
        {
            cmbClan.DropDownStyle = ComboBoxStyle.DropDown;
            cmbClan.DataSource = clanovi;
            cmbClan.DisplayMember = "BrojPrezimeImeDatumRodjenja";
        }

        private Clan SelectedClan
        {
            get { return cmbClan.SelectedItem as Clan; }
            set { cmbClan.SelectedItem = value; }
        }

        protected override DataGridView getDataGridView()
        {
            return dataGridView1;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Text = "Dolazak na trening";
            this.Size = new Size(Size.Width, 450);

            this.dtpOd.CustomFormat = "d.M.yyyy";
            this.dtpOd.Format = DateTimePickerFormat.Custom;
            this.dtpDo.CustomFormat = "d.M.yyyy";
            this.dtpDo.Format = DateTimePickerFormat.Custom;
        }

        private void sortByDatumVreme()
        {
            PropertyDescriptor propDescDatum =
                TypeDescriptor.GetProperties(typeof(DolazakNaTrening))["DatumDolaska"];
            PropertyDescriptor propDescVreme =
                TypeDescriptor.GetProperties(typeof(DolazakNaTrening))["VremeDolaska"];
            PropertyDescriptor[] propDesc = new PropertyDescriptor[2] { propDescDatum, propDescVreme };
            ListSortDirection[] direction = new ListSortDirection[2] { ListSortDirection.Ascending, ListSortDirection.Ascending };

            entities.Sort(new SortComparer<object>(propDesc, direction));
        }

        protected override void addGridColumns()
        {
            AddColumn("Datum dolaska", DATUM_DOLASKA, 105, DataGridViewContentAlignment.MiddleCenter, "{0:dd.MM.yyyy}");
            AddColumn("Vreme dolaska", VREME_DOLASKA, 105, DataGridViewContentAlignment.MiddleCenter, "{0:t}");
        }

        protected override List<object> loadEntities()
        {
            clanovi = loadClanovi();
            setClanovi(clanovi);
            if (clanovi.Count > 0)
                SelectedClan = clanovi[0];
            else
                SelectedClan = null;

            DateTime from = getFromDate();
            DateTime to = getToDate();
            return loadDolasci(SelectedClan, from, to);
        }

        private DateTime getFromDate()
        {
            DateTime from = dtpOd.Value;
            return new DateTime(from.Year, from.Month, from.Day);
        }

        private DateTime getToDate()
        {
            DateTime to = dtpDo.Value;
            DateTime result = new DateTime(to.Year, to.Month, to.Day);
            result = result.AddDays(1).AddSeconds(-1);
            return result;
        }

        private List<object> loadDolasci(Clan c, DateTime from, DateTime to)
        {
            if (c == null)
                return new List<object>();

            DolazakNaTreningDAO dolazakNaTreningDAO = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO();
            return new List<DolazakNaTrening>(dolazakNaTreningDAO.getDolazakNaTrening(c, from, to)).ConvertAll<object>(
                delegate(DolazakNaTrening u)
                {
                    return u;
                });
        }

        private void cmbClan_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // TODO: Ne bi bilo lose da svi kreirani DAO objekti koji se koriste za realizaciju nekog juz-kejsa budu metodi
            // klase Form. Time se izbegava situacija gde se neki DAO dva puta kreira.

            updateGrid();
        }

        private void showDolasci(Clan c, DateTime from, DateTime to)
        {
            setEntities(loadDolasci(c, from, to));
            //sortByDatumVreme();
        }

        private void updateGrid()
        {
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    DateTime from = getFromDate();
                    DateTime to = getToDate();
                    showDolasci(SelectedClan, from, to);
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                Close();
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                Close();
                return;
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void DolazakNaTreningForm_Shown(object sender, EventArgs e)
        {
            btnZatvori.Focus();
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            MessageBox.Show("");
            //updateGrid();
        }
    }
}