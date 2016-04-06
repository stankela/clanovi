using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class LozinkaForm : Form
    {
        public string Lozinka
        {
            get { return txtLozinka.Text; }
        }

        public LozinkaForm()
        {
            InitializeComponent();
        }

        private void LozinkaForm_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.AllScreens[0];
            this.Location = new Point((screen.Bounds.Width - this.Width) / 2, (screen.Bounds.Height - this.Height) / 2);
        }
    }
}
