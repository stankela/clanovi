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

        private void CitacKarticaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Request that the worker thread stop itself:
            Program.workerObject.RequestStop();

            // Use the Join method to block the current thread  
            // until the object's thread terminates.
            Program.workerThread.Join();

            //workerThread.Abort();
        }

        private void CitacKarticaForm_Shown(object sender, EventArgs e)
        {
            SingleInstanceApplication.GlavniProzor.Activate();
        }
    }
}
