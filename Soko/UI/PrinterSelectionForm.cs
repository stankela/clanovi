using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Soko.UI
{
    public partial class PrinterSelectionForm : Form
    {
        private readonly string NO_PRINTERS_MSG = "Ne postoje instalirani stampaci.";

        private string printerNamePotvrda;
        public string PrinterNamePotvrda
        {
            get { return printerNamePotvrda; }
            set { printerNamePotvrda = value; }
        }

        private string printerNameIzvestaj;
        public string PrinterNameIzvestaj
        {
            get { return printerNameIzvestaj; }
            set { printerNameIzvestaj = value; }
        }

        public PrinterSelectionForm()
        {
            InitializeComponent();

            Text = "Izbor stampaca";
            cmbStampacPotvrda.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStampacIzvestaj.DropDownStyle = ComboBoxStyle.DropDownList;
            Font = Options.Instance.Font;
        }

        private void PrinterSelectionForm_Load(object sender, EventArgs e)
        {
            if (PrinterSettings.InstalledPrinters.Count == 0)
            {
                MessageDialogs.showMessage(NO_PRINTERS_MSG, this.Text);
                Close();
            }

            foreach (string s in PrinterSettings.InstalledPrinters)
            {
                cmbStampacPotvrda.Items.Add(s);
                cmbStampacIzvestaj.Items.Add(s);
            }

            cmbStampacPotvrda.SelectedItem = Options.Instance.PrinterNamePotvrda;
            cmbStampacIzvestaj.SelectedItem = Options.Instance.PrinterNameIzvestaj;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbStampacPotvrda.SelectedItem != null)
                printerNamePotvrda = (string)cmbStampacPotvrda.SelectedItem;
            else
                printerNamePotvrda = null;

            if (cmbStampacIzvestaj.SelectedItem != null)
                printerNameIzvestaj = (string)cmbStampacIzvestaj.SelectedItem;
            else
                printerNameIzvestaj = null;
        }

        private void PrinterSelectionForm_Shown(object sender, EventArgs e)
        {
            lblStampacPotvrda.Focus();
        }
    }
}