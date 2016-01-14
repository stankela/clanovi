using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;
using NHibernate;
using Soko.Data;
using NHibernate.Context;
using Bilten.Dao;

namespace Soko.UI
{
    public partial class BiracIntervala : Form
    {
        private List<Grupa> grupe = null;
        private List<Grupa> sveGrupe;

        public DateTimePicker DateTimePickerFrom
        {
            get { return dtpOd; }
        }

        public DateTimePicker DateTimePickerTo
        {
            get { return dtpDo; }
        }

        public BiracIntervala(string naslov, bool izborGrupa)
        {
            InitializeComponent();

            this.Text = naslov;
            this.dtpOd.CustomFormat = "d.M.yyyy";
            this.dtpOd.Format = DateTimePickerFormat.Custom;
            this.dtpDo.CustomFormat = "d.M.yyyy";
            this.dtpDo.Format = DateTimePickerFormat.Custom;

            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    sveGrupe = new List<Grupa>(DAOFactoryFactory.DAOFactory.GetGrupaDAO().FindAll());
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }

            sveGrupe.Sort();
            fillCheckedListBoxGrupa(sveGrupe);

            rbtSveGrupe.Checked = true;

            Font = Options.Instance.Font;
            if (!izborGrupa)
            {
                groupBox2.Visible = false;
                groupBox2.Enabled = false;
                int height = groupBox1.Height + btnOk.Height * 3 + (int)Math.Round(groupBox1.Location.Y * 1.5f);
                this.Size = new Size(Size.Width, height);
                btnOk.Location = new Point(btnOk.Location.X, groupBox1.Height + 3 * groupBox1.Location.Y);
                btnOdustani.Location = new Point(btnOdustani.Location.X, btnOk.Location.Y);
            }
        }

        private void BiracIntervala_Shown(object sender, EventArgs e)
        {
            btnOdustani.Focus();
        }

        public DateTime OdDatum
        {
            get { return dtpOd.Value.Date; }
        }

        public DateTime DoDatum
        {
            get { return dtpDo.Value.Date; }
        }

        public DateTime OdDatumVreme
        {
            get { return dtpOd.Value; }
        }

        public DateTime DoDatumVreme
        {
            get { return dtpDo.Value; }
        }

        public List<Grupa> Grupe
        {
            get { return grupe; }
        }

        private void fillCheckedListBoxGrupa(List<Grupa> grupe)
        {
            foreach (Grupa g in grupe)
            {
                checkedListBoxGrupe.Items.Add(g.SifraNaziv);
            }
        }

        private void rbtSveGrupe_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbtSveGrupe.Checked)
            {
                checkedListBoxGrupe.Enabled = false;
                uncheckAll(checkedListBoxGrupe);
                listBoxGrupe.Enabled = false;
                listBoxGrupe.Items.Clear();
                lblSelGrupe.Visible = false;
            }
        }

        private void rbtGrupe_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbtGrupe.Checked)
            {
                checkedListBoxGrupe.Enabled = true;
                listBoxGrupe.Enabled = true;
                lblSelGrupe.Visible = true;
            }
        }

        private void checkedListBoxGrupe_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                listBoxGrupe.Items.Clear();
                for (int i = 0; i < checkedListBoxGrupe.Items.Count; i++)
                {
                    if (checkedListBoxGrupe.GetItemChecked(i) || i == e.Index)
                    {
                        listBoxGrupe.Items.Add(checkedListBoxGrupe.Items[i]);
                    }
                }
            }
            else
            {
                listBoxGrupe.Items.Remove(checkedListBoxGrupe.Items[e.Index]);
            }
        }

        private void uncheckAll(CheckedListBox checkedListBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                if (checkedListBox.GetItemChecked(i))
                {
                    checkedListBox.SetItemChecked(i, false);
                }
            }
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            if (rbtGrupe.Checked && checkedListBoxGrupe.CheckedItems.Count == 0)
            {
                MessageDialogs.showMessage("Izaberite grupe.", this.Text);
                this.DialogResult = DialogResult.None;
            }
        }

        private void BiracIntervala_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (rbtSveGrupe.Checked)
            {
                grupe = null;
            }
            else
            {
                grupe = new List<Grupa>();
                for (int i = 0; i < checkedListBoxGrupe.Items.Count; i++)
                {
                    if (checkedListBoxGrupe.GetItemChecked(i))
                        grupe.Add(sveGrupe[i]);
                }
            }
        }
    }
}