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
            
            dtpOd.CustomFormat = "d.M.yyyy";
            dtpOd.Format = DateTimePickerFormat.Custom;
            dtpDo.CustomFormat = "d.M.yyyy";
            dtpDo.Format = DateTimePickerFormat.Custom;
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
                    DateTime from = getFromDate();
                    DateTime to = getToDate();

                    setEntities(loadDolasci(SelectedClan, from, to));
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