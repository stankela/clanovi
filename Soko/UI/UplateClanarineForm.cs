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
    public partial class UplateClanarineForm : EntityListForm
    {
        private readonly string CLAN = "PrezimeImeBrojDatumRodj";
        private readonly string IZNOS = "Iznos";
        private readonly string DATUM_UPLATE = "DatumUplate";
        private readonly string VREME_UPLATE = "VremeUplate";
        private readonly string GRUPA = "SifraGrupeCrtaNazivGrupe";
        private readonly string VAZI_OD = "VaziOd";
        private readonly string NAPOMENA = "Napomena";
        private readonly string KORISNIK = "Korisnik";

        private List<Clan> clanovi;

        public UplateClanarineForm()
        {
            InitializeComponent();
            initialize(typeof(UplataClanarine));

            rbtClan.CheckedChanged += new System.EventHandler(rbtClan_CheckedChanged);
            rbtInterval.CheckedChanged += new System.EventHandler(rbtInterval_CheckedChanged);
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
            this.Text = "Potvrda o uplati";
            this.Size = new Size(Size.Width, 450);

            this.dtpOd.CustomFormat = "d.M.yyyy";
            this.dtpOd.Format = DateTimePickerFormat.Custom;
            this.dtpDo.CustomFormat = "d.M.yyyy";
            this.dtpDo.Format = DateTimePickerFormat.Custom;

            rbtInterval.Checked = true;
            cmbClan.Enabled = false;

            btnPromeni.Visible = Options.Instance.AdminMode;
            btnPromeni.Enabled = Options.Instance.AdminMode;
        }

        private void sortByDatumVreme()
        {
            PropertyDescriptor propDescDatum =
                TypeDescriptor.GetProperties(typeof(UplataClanarine))["DatumUplate"];
            PropertyDescriptor propDescVreme =
                TypeDescriptor.GetProperties(typeof(UplataClanarine))["VremeUplate"];
            PropertyDescriptor[] propDesc = new PropertyDescriptor[2] { propDescDatum, propDescVreme };
            ListSortDirection[] direction = new ListSortDirection[2] { ListSortDirection.Descending, ListSortDirection.Descending };

            entities.Sort(new SortComparer<object>(propDesc, direction));
        }

        protected override void addGridColumns()
        {
            AddColumn("Clan", CLAN, 180);
            AddColumn("Iznos", IZNOS, DataGridViewContentAlignment.MiddleRight, "{0:f2}");
            AddColumn("Datum uplate", DATUM_UPLATE, DataGridViewContentAlignment.MiddleRight);
            AddColumn("Vreme uplate", VREME_UPLATE, DataGridViewContentAlignment.MiddleCenter, "{0:t}");
            AddColumn("Grupa", GRUPA, 220);
            AddColumn("Za mesec", VAZI_OD, DataGridViewContentAlignment.MiddleRight, "{0:MMMM yyyy}");
            AddColumn("Napomena", NAPOMENA, 150, DataGridViewContentAlignment.MiddleCenter);
            AddColumn("Korisnik", KORISNIK, DataGridViewContentAlignment.MiddleCenter);
        }

        protected override List<object> loadEntities()
        {
            clanovi = loadClanovi();
            setClanovi(clanovi);
            if (clanovi.Count > 0)
                SelectedClan = clanovi[0];
            else
                SelectedClan = null;

            if (rbtClan.Checked)
                return loadUplateForClan(SelectedClan);
            else
                return loadUplateForInterval(dtpOd.Value.Date, dtpDo.Value.Date);
        }

        private List<object> loadUplateForInterval(DateTime from, DateTime to)
        {
            UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
            return new List<UplataClanarine>(uplataClanarineDAO.findUplate(from, to)).ConvertAll<object>(
                delegate(UplataClanarine u)
                {
                    return u;
                });
        }

        private List<object> loadUplateForClan(Clan c)
        {
            if (c == null)
                return new List<object>();

            UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
            return new List<UplataClanarine>(uplataClanarineDAO.findUplate(c)).ConvertAll<object>(
                delegate(UplataClanarine u)
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

        private void showUplateForClan(Clan c)
        {
            setEntities(loadUplateForClan(c));
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
                    if (rbtClan.Checked)
                        showUplateForClan(SelectedClan);
                    else
                        showUplateForInterval(dtpOd.Value.Date, dtpDo.Value.Date);
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

        private void showUplateForInterval(DateTime from, DateTime to)
        {
            setEntities(loadUplateForInterval(from, to));
            //sortByDatumVreme();
        }

        private void rbtClan_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbtClan.Checked)
            {
                cmbClan.Enabled = true;
                dtpOd.Enabled = false;
                dtpDo.Enabled = false;
                lblOd.Enabled = false;
                lblDo.Enabled = false;
                updateGrid();
            }
        }

        private void rbtInterval_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbtInterval.Checked)
            {
                cmbClan.Enabled = false;
                dtpOd.Enabled = true;
                dtpDo.Enabled = true;
                lblOd.Enabled = true;
                lblDo.Enabled = true;

                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                updateGrid();

                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void dtpOd_CloseUp(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            updateGrid();

            Cursor.Hide();
            Cursor.Current = Cursors.Arrow;
        }

        private void dtpDo_CloseUp(object sender, System.EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            updateGrid();

            Cursor.Hide();
            Cursor.Current = Cursors.Arrow;
        }

        private void btnStampaj_Click(object sender, System.EventArgs e)
        {
            // TODO: Probaj da promenis i EntityListForm i EntityDetailForm tako da 
            // rade za bilo koji objekt (da mogu da se koriste i za npr. DTO objekte)

            UplataClanarine uplata = (UplataClanarine)getSelectedEntity();
            if (uplata == null)
                return;

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    List<int> idList = new List<int>();
                    idList.Add(uplata.Id);
                    p.setIzvestaj(new PotvrdaIzvestaj(idList));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
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

        private void PotvrdaUplateForm_Shown(object sender, EventArgs e)
        {
            btnZatvori.Focus();
        }

        private void btnPromeni_Click(object sender, EventArgs e)
        {
            editCommand();
        }
        
        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
        {
            return new UplataDialogAdmin(entityId);
        }

    }
}