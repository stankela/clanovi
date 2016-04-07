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
    public partial class LozinkaOptionsForm : Form
    {
        public LozinkaOptionsForm()
        {
            InitializeComponent();
        }

        private void LozinkaOptionsForm_Load(object sender, EventArgs e)
        {
            rbtUvekTraziLozinku.Checked = Options.Instance.UvekPitajZaLozinku;
            rbtTraziLozinkuNakon.Checked = !Options.Instance.UvekPitajZaLozinku;
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

            Options.Instance.UvekPitajZaLozinku = rbtUvekTraziLozinku.Checked;
            if (!Options.Instance.UvekPitajZaLozinku)
            {
                Options.Instance.LozinkaTimerMinuti = int.Parse(txtBrojMinuta.Text);
            }
        }
    }
}
