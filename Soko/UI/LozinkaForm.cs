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
    }
}
