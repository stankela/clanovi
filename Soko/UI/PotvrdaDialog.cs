using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class PotvrdaDialog : Form
    {
        public void SetDaText(string daText)
        {
            btnDa.Text = daText;
        }

        public PotvrdaDialog(string naslov, string pitanje)
        {
            InitializeComponent();
            this.Text = naslov;
            lblPitanje.Text = pitanje;

            Font = Options.Instance.Font;
        }

        private void PotvrdaDialog_Shown(object sender, EventArgs e)
        {
            btnNe.Focus();
        }
    }
}