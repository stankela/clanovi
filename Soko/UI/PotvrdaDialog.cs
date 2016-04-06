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

        private void PotvrdaDialog_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.AllScreens[0];
            this.Location = new Point((screen.Bounds.Width - this.Width) / 2, (screen.Bounds.Height - this.Height) / 2);
        }
    }
}