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

        private DateTime currentDatum;
        
        public BrisiPrethodneUplateForm(string caption)
        {
            InitializeComponent();
            this.Text = caption;
            Font = Options.Instance.Font;
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker1.CustomFormat = "MMMM yyyy";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.Value = DateTime.Now;
            currentDatum = dateTimePicker1.Value;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
        }

        void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Handle wrapping
            // TODO: Ovo je slican metod kao u dtpDatum_ValueChanged.
            int add = 0;
            if (currentDatum.Month == 12 && dateTimePicker1.Value.Month == 1
                && currentDatum.Year == dateTimePicker1.Value.Year)
            {
                add = 1;

            }
            else if (currentDatum.Month == 1 && dateTimePicker1.Value.Month == 12
                && currentDatum.Year == dateTimePicker1.Value.Year)
            {
                add = -1;
            }

            if (add != 0)
            {
                dateTimePicker1.ValueChanged -= new System.EventHandler(dateTimePicker1_ValueChanged);
                dateTimePicker1.Value = dateTimePicker1.Value.AddYears(add);
                dateTimePicker1.ValueChanged += new System.EventHandler(dateTimePicker1_ValueChanged);
            }
            currentDatum = dateTimePicker1.Value;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // TODO3: Ponudi samo godinu za biranje, i onda ukloni ovaj workaround.
            if (dateTimePicker1.Value.Month != 1)
            {
                MessageDialogs.showMessage("Izaberite januar mesec. Jedino to je trenutno podrzano.", this.Text);
                this.DialogResult = DialogResult.None;
            }

            datum = dateTimePicker1.Value;
        }
    }
}