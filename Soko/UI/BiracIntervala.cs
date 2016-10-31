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
using Soko.Misc;

namespace Soko.UI
{
    public partial class BiracIntervala : Form
    {
        private List<Grupa> grupe = null;
        private List<Grupa> sveGrupe;
        private List<Clan> clanovi;
        private bool izborClana;
        private DateTime currentDatumOd;
        private DateTime currentDatumDo;
        private bool months;

        public DateTimePicker DateTimePickerFrom
        {
            get { return dtpOd; }
        }

        public DateTimePicker DateTimePickerTo
        {
            get { return dtpDo; }
        }

        // TODO2: Biranje clana se pojavljuje na vise mesta u programu. Probaj da napravis kontrolu
        // koja to radi, i koja moze da se stavlja na form.

        public BiracIntervala(string naslov, bool izborGrupa, bool izborClana, bool months)
        {
            InitializeComponent();
            this.Text = naslov;
            this.months = months;

            string format;
            if (months)
            {
                format = "MMMM yyyy";
                dtpOd.ShowUpDown = true;
                dtpDo.ShowUpDown = true;
            }
            else
                format = "d.M.yyyy";
            dtpOd.CustomFormat = format;
            dtpOd.Format = DateTimePickerFormat.Custom;
            dtpDo.CustomFormat = format;
            dtpDo.Format = DateTimePickerFormat.Custom;

            DateTime firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            dtpOd.Value = firstDayInMonth;
            dtpDo.Value = firstDayInMonth;

            currentDatumOd = dtpOd.Value;
            currentDatumDo = dtpDo.Value;
            dtpOd.ValueChanged += new System.EventHandler(dtpDatum_ValueChanged);
            dtpDo.ValueChanged += new System.EventHandler(dtpDatum_ValueChanged);

            this.izborClana = izborClana;

            cmbClan.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    sveGrupe = new List<Grupa>(DAOFactoryFactory.DAOFactory.GetGrupaDAO().FindAll());
                    clanovi = loadClanovi();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }

            sveGrupe.Sort();
            fillCheckedListBoxGrupa(sveGrupe);
            rbtSveGrupe.Checked = true;

            setClanovi(clanovi);
            if (clanovi.Count > 0)
                SelectedClan = clanovi[0];
            else
                SelectedClan = null;
            rbtCeoIzvestaj.Checked = true;


            Font = Options.Instance.Font;
            if (!izborClana)
            {
                groupBox3.Visible = false;
                groupBox3.Enabled = false;
                int delta = groupBox3.Bottom - groupBox2.Bottom;
                btnOk.Location = new Point(btnOk.Location.X, btnOk.Location.Y - delta);
                btnOdustani.Location = new Point(btnOdustani.Location.X, btnOdustani.Location.Y - delta);
                this.Size = new Size(Size.Width, Size.Height - delta);
            }
            if (!izborGrupa)
            {
                groupBox2.Visible = false;
                groupBox2.Enabled = false;
                int delta = groupBox2.Bottom - groupBox1.Bottom;
                btnOk.Location = new Point(btnOk.Location.X, btnOk.Location.Y - delta);
                btnOdustani.Location = new Point(btnOdustani.Location.X, btnOdustani.Location.Y - delta);
                this.Size = new Size(Size.Width, Size.Height - delta);
            }
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

        private void BiracIntervala_Shown(object sender, EventArgs e)
        {
            btnOdustani.Focus();
        }

        public DateTime OdDatum
        {
            get
            {                
                DateTime result = dtpOd.Value.Date;
                if (months)
                {
                    // return first datetime in month
                    result = new DateTime(result.Year, result.Month, 1, 0, 0, 0);
                }
                return result;
            }
        }

        public DateTime DoDatum
        {
            get
            {               
                DateTime result = dtpDo.Value.Date;
                if (months)
                {
                    // return last datetime in month
                    result = result.AddMonths(1);
                    result = new DateTime(result.Year, result.Month, 1, 0, 0, 0).AddSeconds(-1);
                }
                return result;
            }
        }

        DateTime truncateSeconds(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
        }

        public DateTime OdDatumVreme
        {
            get { return truncateSeconds(dtpOd.Value); }
        }

        public DateTime DoDatumVreme
        {
            get { return truncateSeconds(dtpDo.Value); }
        }

        public List<Grupa> Grupe
        {
            get { return grupe; }
        }

        public Nullable<int> ClanId
        {
            get
            {
                if (!izborClana || rbtCeoIzvestaj.Checked)
                    return null;

                if (SelectedClan != null)
                    return SelectedClan.Id;
                else
                    return null;
            }
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

        private void rbtCeoIzvestaj_CheckedChanged(object sender, EventArgs e)
        {
            onRbtClanChanged();
        }

        private void rbtClan_CheckedChanged(object sender, EventArgs e)
        {
            onRbtClanChanged();
        }

        private void onRbtClanChanged()
        {
            txtClan.Enabled = rbtClan.Checked;
            cmbClan.Enabled = rbtClan.Checked;
            if (rbtClan.Checked)
                txtClan.Focus();
        }

        private void txtClan_TextChanged(object sender, EventArgs e)
        {
            string text = txtClan.Text;
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

        private void cmbClan_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (SelectedClan != null)
            {
                txtClan.Text = SelectedClan.Broj.ToString();
            }
            else
            {
                txtClan.Text = String.Empty;
            }
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
    }
}