using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class BiracDana : Form
    {
        public BiracDana(string caption)
        {
            InitializeComponent();
            this.Text = caption;
            this.dtpDatum.CustomFormat = "d.M.yyyy";
            this.dtpDatum.Format = DateTimePickerFormat.Custom;
            Font = Options.Instance.Font;
        }

        public DateTime Datum
        {
            get
            {
                return dtpDatum.Value.Date;
            }
        }
    }
}