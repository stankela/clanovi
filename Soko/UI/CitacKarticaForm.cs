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
    public partial class CitacKarticaForm : Form
    {
        public CitacKarticaForm()
        {
            InitializeComponent();
        }

        private void CitacKarticaForm_Shown(object sender, EventArgs e)
        {
            SingleInstanceApplication.GlavniProzor.Activate();
        }
    }
}
