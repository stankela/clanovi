using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using Bilten.Dao;
using Soko.Exceptions;
using NHibernate;
using Soko.Data;
using NHibernate.Context;

namespace Soko.UI
{
    public partial class MesecneClanarineForm : EntityListForm
    {
        private const string GRUPA = "Grupa";
        private const string VAZI_OD = "VaziOd";
        private const string IZNOS = "Iznos";

        private List<Grupa> grupe;

        public MesecneClanarineForm()
        {
            InitializeComponent();
            rbtSveGrupe.Checked = true;
            cmbGrupa.Enabled = false;

            initialize(typeof(MesecnaClanarina));
            sort(GRUPA);

            rbtSveGrupe.CheckedChanged += new System.EventHandler(rbtSveGrupe_CheckedChanged);
            rbtGrupa.CheckedChanged += new System.EventHandler(rbtGrupa_CheckedChanged);
            cmbGrupa.SelectedIndexChanged += new System.EventHandler(cmbGrupa_SelectedIndexChanged);
        }

        protected override DataGridView getDataGridView()
        {
            return dataGridView1;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Size = new Size(Size.Width, 450);
            this.Text = "Cenovnik";
        }

        protected override void addGridColumns()
        {
            AddColumn("Grupa", GRUPA, 220);
            AddColumn("Vazi od", VAZI_OD, 70, DataGridViewContentAlignment.MiddleRight);
            AddColumn("Cena", IZNOS, 70, DataGridViewContentAlignment.MiddleRight, "{0:f2}");
        }

        protected override List<object> loadEntities()
        {
            grupe = loadGrupe();
            setGrupe(grupe);
            if (grupe.Count > 0)
                SelectedGrupa = grupe[0];
            else
                SelectedGrupa = null;

            return loadCenovnik();
        }

        private List<object> loadCenovnik()
        {
            MesecnaClanarinaDAO mesecnaClanarinaDAO = DAOFactoryFactory.DAOFactory.GetMesecnaClanarinaDAO();
            return new List<MesecnaClanarina>(mesecnaClanarinaDAO.getCenovnik()).ConvertAll<object>(
                delegate(MesecnaClanarina mc)
                {
                    return mc;
                });
        }

        private List<Grupa> loadGrupe()
        {
            List<Grupa> result = new List<Grupa>(DAOFactoryFactory.DAOFactory.GetGrupaDAO().FindAll());

            PropertyDescriptor propDesc = TypeDescriptor.GetProperties(typeof(Grupa))["Naziv"];
            result.Sort(new SortComparer<Grupa>(propDesc, ListSortDirection.Ascending));

            return result;
        }

        private void setGrupe(List<Grupa> grupe)
        {
            cmbGrupa.Items.Clear();
            foreach (Grupa g in grupe)
            {
                cmbGrupa.Items.Add(g.SifraNaziv);
            }
        }

        private Grupa SelectedGrupa
        {
            get
            {
                Grupa result = null;
                if (cmbGrupa.SelectedIndex >= 0)
                    result = grupe[cmbGrupa.SelectedIndex];
                return result;
            }
            set
            {
                if (value == null || grupe.IndexOf(value) == -1)
                    cmbGrupa.SelectedIndex = -1;
                else
                    cmbGrupa.SelectedIndex = grupe.IndexOf(value);
            }
        }

        private void rbtSveGrupe_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!rbtSveGrupe.Checked)
                return;

            cmbGrupa.Enabled = false;
            List<object> cenovnik;
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    cenovnik = loadCenovnik();
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
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }

            setEntities(cenovnik);
            sort(GRUPA);
        }

        private void rbtGrupa_CheckedChanged(object sender, System.EventArgs e)
        {
            if (!rbtGrupa.Checked)
                return;

            cmbGrupa.Enabled = true;
            onNewGrupaSelected(SelectedGrupa);
        }

        private void cmbGrupa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            onNewGrupaSelected(SelectedGrupa);
        }

        private void onNewGrupaSelected(Grupa g)
        {
            if (g == null)
            {
                entities.Clear();
                refreshView();
                return;
            }

            List<object> cenovnik;
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    cenovnik = loadCenovnikForGrupa(g);
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
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }

            setEntities(cenovnik);
            sort(VAZI_OD, ListSortDirection.Descending);
        }

        private List<object> loadCenovnikForGrupa(Grupa g)
        {
            MesecnaClanarinaDAO mesecnaClanarinaDAO = DAOFactoryFactory.DAOFactory.GetMesecnaClanarinaDAO();
            return new List<MesecnaClanarina>(mesecnaClanarinaDAO.findForGrupa(g)).ConvertAll<object>(
                delegate(MesecnaClanarina mc)
                {
                    return mc;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
        {
            MesecnaClanarina mc = (MesecnaClanarina)getSelectedEntity();
            SifraGrupe sifra = null;
            if (mc != null)
                sifra = mc.Grupa.Sifra;
            else if (rbtGrupa.Checked && SelectedGrupa != null)
                sifra = SelectedGrupa.Sifra;
            return new CenaDialog(entityId, sifra);
        }

        private void btnNovaCena_Click(object sender, System.EventArgs e)
        {
            addCommand();
        }

        protected override void onEntityAdded(DomainObject entity)
        {
            // najpre izbaci staru clanarinu za datu grupu (pod uslovom da prikazujemo 
            // cenovnik za sve grupe)
            if (rbtSveGrupe.Checked)
            {
                MesecnaClanarina _new = (MesecnaClanarina)entity;
                MesecnaClanarina old = findClanarina(_new.Grupa.Sifra);
                if (old != null)
                    entities.Remove(old);
            }

            // dodaj novu clanarinu
            base.onEntityAdded(entity);
        }

        private MesecnaClanarina findClanarina(SifraGrupe grupa)
        {
            foreach (MesecnaClanarina mc in entities)
            {
                if (mc.Grupa.Sifra == grupa)
                    return mc;
            }
            return null;
        }

        private void btnBrisi_Click(object sender, System.EventArgs e)
        {
            deleteCommand();
        }

        protected override void onEntityDeleted(DomainObject entity)
        {
            entities.Remove(entity);

            // dodaj prethodni cenovnik za izbrisanu grupu
            bool refreshed = false;
            if (rbtSveGrupe.Checked)
            {
                MesecnaClanarina previous = DAOFactoryFactory.DAOFactory.GetMesecnaClanarinaDAO().
                    getVazecaClanarinaForGrupa((entity as MesecnaClanarina).Grupa);
                if (previous != null)
                {
                    entities.Add(previous);
                    sort(sortProperty);
                    refreshed = true;
                }
            }

            if (!refreshed)
                refreshView();
        }

        protected override string deleteConfirmationMessage(DomainObject entity)
        {
            MesecnaClanarina mc = (MesecnaClanarina)entity;
            return String.Format("Da li zelite da izbrisete cenovnik za grupu \"{0}\"" +
                " koji vazi od {1:d}?", mc.Grupa, mc.VaziOd);
        }

        protected override bool refIntegrityDeleteDlg(DomainObject entity)
        {
            return true;
        }

        protected override void delete(DomainObject entity)
        {
            DAOFactoryFactory.DAOFactory.GetMesecnaClanarinaDAO().MakeTransient((MesecnaClanarina)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja cenovnika za grupu.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}