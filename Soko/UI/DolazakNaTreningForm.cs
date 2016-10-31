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
        private readonly string CLAN = "PrezimeImeBrojDatumRodj";
        private readonly string DATUM_DOLASKA = "DatumDolaska";
        private readonly string VREME_DOLASKA = "VremeDolaska";

        private DateTime currentDatumOd;
        private DateTime currentDatumDo;
        
        private List<Clan> clanovi;

        public DolazakNaTreningForm()
        {
            InitializeComponent();
            initialize(typeof(DolazakNaTrening));

            cmbClan.SelectionChangeCommitted += cmbClan_SelectionChangeCommitted;
        }

        private List<Clan> loadClanovi()
        {
            List<Clan> result = new List<Clan>(DAOFactoryFactory.DAOFactory.GetClanDAO().FindAll());
            Util.sortByPrezimeImeDatumRodjenja(result);
            return result;
        }

        private void setClanovi(List<Clan> clanovi)
        {
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

            cmbClan.DropDownStyle = ComboBoxStyle.DropDownList;

            dtpOd.CustomFormat = "MMMM yyyy";
            dtpOd.Format = DateTimePickerFormat.Custom;
            dtpDo.CustomFormat = "MMMM yyyy";
            dtpDo.Format = DateTimePickerFormat.Custom;
            dtpOd.ShowUpDown = true;
            dtpDo.ShowUpDown = true;

            DateTime firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            dtpOd.Value = firstDayInMonth;
            dtpDo.Value = firstDayInMonth;

            currentDatumOd = dtpOd.Value;
            currentDatumDo = dtpDo.Value;
            dtpOd.ValueChanged += new System.EventHandler(dtpDatum_ValueChanged);
            dtpDo.ValueChanged += new System.EventHandler(dtpDatum_ValueChanged);
        }

        private void dtpDatum_ValueChanged(object sender, EventArgs e)
        {
            // Handle wrapping

            DateTimePicker dateTimePicker = sender as DateTimePicker;
            bool od = object.ReferenceEquals(dateTimePicker, dtpOd);
            DateTime currentDatum = od ? currentDatumOd : currentDatumDo;

            int add = 0;
            if (currentDatum.Month == 12 && dateTimePicker.Value.Month == 1
                && currentDatum.Year == dateTimePicker.Value.Year)
            {
                add = 1;

            }
            else if (currentDatum.Month == 1 && dateTimePicker.Value.Month == 12
                && currentDatum.Year == dateTimePicker.Value.Year)
            {
                add = -1;
            }

            if (add != 0)
            {
                dateTimePicker.ValueChanged -= new System.EventHandler(dtpDatum_ValueChanged);
                dateTimePicker.Value = dateTimePicker.Value.AddYears(add);
                dateTimePicker.ValueChanged += new System.EventHandler(dtpDatum_ValueChanged);
            }

            if (od)
                currentDatumOd = dateTimePicker.Value;
            else
                currentDatumDo = dateTimePicker.Value;
        }

        protected override void addGridColumns()
        {
            AddColumn("Clan", CLAN, 180);
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

            return loadDolasci(SelectedClan, getFromDate(), getToDate());
        }

        private DateTime getFromDate()
        {
            // return first datetime in month
            DateTime from = dtpOd.Value;
            return new DateTime(from.Year, from.Month, 1);
        }

        private DateTime getToDate()
        {
            // return last datetime in month
            DateTime result = dtpDo.Value;
            result = result.AddMonths(1);
            return new DateTime(result.Year, result.Month, 1, 0, 0, 0).AddSeconds(-1);
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

        private void cmbClan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (SelectedClan != null)
            {
                txtSifraClana.Text = SelectedClan.Broj.ToString();
            }
            else
            {
                txtSifraClana.Text = String.Empty;
            }
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void DolazakNaTreningForm_Shown(object sender, EventArgs e)
        {
            clearSelection();
            txtSifraClana.Focus();
        }

        private void btnPrikazi_Click(object sender, EventArgs e)
        {
            // TODO: Ne bi bilo lose da svi kreirani DAO objekti koji se koriste za realizaciju nekog juz-kejsa budu metodi
            // klase Form. Time se izbegava situacija gde se neki DAO dva puta kreira.

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    setEntities(loadDolasci(SelectedClan, getFromDate(), getToDate()));
                    clearSelection();
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

        private void txtSifraClana_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            string text = txtSifraClana.Text;
            Clan clan = null;
            int broj;
            if (int.TryParse(text, out broj))
            {
                clan = findClan(broj);
            }
            else if (text != String.Empty)
            {
                clan = searchForClan(text);
            }
            SelectedClan = clan;
        }

        private Clan findClan(int broj)
        {
            foreach (Clan c in clanovi)
            {
                if (c.Broj == broj)
                    return c;
            }
            return null;
        }

        private Clan searchForClan(string text)
        {
            foreach (Clan c in clanovi)
            {
                if (c.PrezimeIme.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                    return c;
            }
            return null;
        }

    }
}