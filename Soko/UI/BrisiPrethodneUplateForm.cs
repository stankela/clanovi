using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class BrisiPrethodneUplateForm : Form
    {
        DateTime datum;
        public DateTime Datum
        {
            get { return datum.Date; }
        }

        public BrisiPrethodneUplateForm(string caption)
        {
            InitializeComponent();
            this.Text = caption;
            Font = Options.Instance.Font;
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.CustomFormat = "MMMM yyyy";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            datum = dateTimePicker1.Value;
        }
    }
}