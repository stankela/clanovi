using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
            Text = "Opcije";

            object[] sizes = { 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            cmbVelicina.Items.AddRange(sizes);
            Font = Options.Instance.Font;
            int initSize = (int)Math.Round(Font.SizeInPoints);
            cmbVelicina.SelectedIndex = cmbVelicina.Items.IndexOf(initSize);
            cmbVelicina.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbStampacPotvrda.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStampacIzvestaj.DropDownStyle = ComboBoxStyle.DropDownList;

            txtNedostajuceUplate.Text = Options.Instance.NedostajuceUplateStartDate.ToString("dd-MM-yyyy");
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            if (PrinterSettings.InstalledPrinters.Count == 0)
            {
                MessageDialogs.showMessage("Ne postoje instalirani stampaci.", this.Text);
            }
            else
            {
                foreach (string s in PrinterSettings.InstalledPrinters)
                {
                    cmbStampacPotvrda.Items.Add(s);
                    cmbStampacIzvestaj.Items.Add(s);
                }
                cmbStampacPotvrda.SelectedItem = Options.Instance.PrinterNamePotvrda;
                cmbStampacIzvestaj.SelectedItem = Options.Instance.PrinterNameIzvestaj;
            }

            rbtUvekTraziLozinku.Checked = Options.Instance.UvekPitajZaLozinku;
            rbtTraziLozinkuNakon.Checked = !Options.Instance.UvekPitajZaLozinku;
        }

        private void OptionsForm_Shown(object sender, EventArgs e)
        {
            lblVelicina.Focus();
        }

        private void cmbVelicina_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // automatically scales the form
            Font = new Font(Font.FontFamily, (int)cmbVelicina.SelectedItem);
        }

        private void rbtUvekTraziLozinku_CheckedChanged(object sender, EventArgs e)
        {
            rbtUpdate();
        }

        private void rbtTraziLozinkuNakon_CheckedChanged(object sender, EventArgs e)
        {
            rbtUpdate();
        }

        private void rbtUpdate()
        {
            if (rbtUvekTraziLozinku.Checked)
            {
                txtBrojMinuta.Enabled = false;
            }
            else
            {
                txtBrojMinuta.Enabled = true;
                if (txtBrojMinuta.Text == String.Empty)
                {
                    txtBrojMinuta.Text = Options.Instance.LozinkaTimerMinuti.ToString();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Validate

            DateTime dummy;
            string msg = String.Empty;
            if (txtNedostajuceUplate.Text.Trim() == String.Empty)
            {
                msg = "Unesite datum za nedostajuce uplate.";
            }
            else if (!DateTime.TryParse(txtNedostajuceUplate.Text, out dummy))
            {
                msg = "Nepravilan datum za nedostajuce uplate. Datum se unosi u formatu dd-mm-gggg ili dd.mm.gggg.";
            }
            if (msg != String.Empty)
            {
                MessageDialogs.showMessage(msg, this.Text);
                txtNedostajuceUplate.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (rbtTraziLozinkuNakon.Checked)
            {
                int i;
                if (!Int32.TryParse(txtBrojMinuta.Text, out i) || i < 1)
                {
                    MessageDialogs.showMessage("Neispravna vrednost za broj minuta.", this.Text);
                    txtBrojMinuta.Focus();
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            // Update options

            int size;
            if (cmbVelicina.SelectedIndex != -1)
                size = (int)cmbVelicina.SelectedItem;
            else
                size = (int)Math.Round(Font.SizeInPoints);
            Options.Instance.Font = new Font(Font.FontFamily, size);

            if (cmbStampacPotvrda.SelectedItem != null)
                Options.Instance.PrinterNamePotvrda = (string)cmbStampacPotvrda.SelectedItem;
            else
                Options.Instance.PrinterNamePotvrda = null;

            if (cmbStampacIzvestaj.SelectedItem != null)
                Options.Instance.PrinterNameIzvestaj = (string)cmbStampacIzvestaj.SelectedItem;
            else
                Options.Instance.PrinterNameIzvestaj = null;

            Options.Instance.NedostajuceUplateStartDate = DateTime.Parse(txtNedostajuceUplate.Text.Trim());

            Options.Instance.UvekPitajZaLozinku = rbtUvekTraziLozinku.Checked;
            if (!Options.Instance.UvekPitajZaLozinku)
            {
                Options.Instance.LozinkaTimerMinuti = int.Parse(txtBrojMinuta.Text);
            }
        }
    }
}
