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
    public partial class CenovnikForm : EntityListForm
    {
        private const string GRUPA = "Grupa";
        private const string VAZI_OD = "VaziOd";
        private const string IZNOS = "Iznos";

        private List<Grupa> grupe;

        public CenovnikForm()
        {
            InitializeComponent();
            initialize(typeof(MesecnaClanarina));
            sort(GRUPA);

            grupe = loadGrupe();
            setGrupe(grupe);
            if (grupe.Count > 0)
                SelectedGrupa = grupe[0];
            else
                SelectedGrupa = null;

            rbtSveGrupe.Checked = true;
            cmbGrupa.Enabled = false;
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
            return loadCenovnik();
        }

        private List<object> loadCenovnik()
        {
            return MapperRegistry.mesecnaClanarinaDAO().getCenovnik().ConvertAll<object>(
                delegate(MesecnaClanarina mc)
                {
                    return mc;
                });
        }

        private List<Grupa> loadGrupe()
        {
            List<Grupa> result = MapperRegistry.grupaDAO().getAll();

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
            if (rbtSveGrupe.Checked)
            {
                MapperRegistry.initialize();  // nova sesija
                onRbtSveGrupeChecked();
            }
        }

        private void onRbtSveGrupeChecked()
        {
            cmbGrupa.Enabled = false;
            showCenovnik();
        }

        private void rbtGrupa_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbtGrupa.Checked)
            {
                MapperRegistry.initialize();  // nova sesija
                onRbtGrupaChecked();
            }
        }

        private void onRbtGrupaChecked()
        {
            cmbGrupa.Enabled = true;
            showCenovnikForGrupa(SelectedGrupa);
        }

        private void cmbGrupa_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            MapperRegistry.initialize();  // nova sesija
            showCenovnikForGrupa(SelectedGrupa);
        }

        private void showCenovnik()
        {
            setEntities(loadCenovnik());
            sort(GRUPA);
        }

        private void showCenovnikForGrupa(Grupa g)
        {
            if (g != null)
            {
                setEntities(loadCenovnikForGrupa(g));
                sort(VAZI_OD, ListSortDirection.Descending);
            }
            else
            {
                entities.Clear();
                refreshView();
            }
        }

        private List<object> loadCenovnikForGrupa(Grupa g)
        {
            return MapperRegistry.mesecnaClanarinaDAO().getAllForGrupa(g.Sifra.Value).ConvertAll<object>(
                delegate(MesecnaClanarina mc)
                {
                    return mc;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Key entityId)
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
                MesecnaClanarina previous = MapperRegistry.mesecnaClanarinaDAO().
                    getVazeciForGrupa((entity as MesecnaClanarina).Grupa.Sifra.Value);
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

        protected override bool delete(DomainObject entity)
        {
            return MapperRegistry.mesecnaClanarinaDAO().delete((MesecnaClanarina)entity);
        }

        protected override string deleteErrorMessage(DomainObject entity)
        {
            return "Greska prilikom brisanja cenovnika za grupu.";
        }

        protected override string deleteConcurrencyErrorMessage(DomainObject entity)
        {
            return "Neuspesno brisanje cenovnika za grupu.";
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}