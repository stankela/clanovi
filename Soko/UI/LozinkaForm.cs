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
        private string lozinka;

        public LozinkaForm(string lozinka, bool usePasswordChar)
        {
            InitializeComponent();
            this.lozinka = lozinka;
            if (usePasswordChar)
            {
                txtLozinka.PasswordChar = '*';
                txtLozinka.UseSystemPasswordChar = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtLozinka.Text != lozinka)
            {
                MessageDialogs.showMessage("Neispravna lozinka", this.Text);
                txtLozinka.Clear();
                this.DialogResult = DialogResult.None;
                return;
            }
        }
    }
}
