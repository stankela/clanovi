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
    public partial class COMPortForm : Form
    {
        private int comPort;
        public int COMPort
        {
            get { return comPort; }
        }

        public COMPortForm()
        {
            InitializeComponent();
        }

        private void COMPortForm_Load(object sender, EventArgs e)
        {
            cmbCOMPort.SelectedIndex = Options.Instance.COMPort - 1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            comPort = cmbCOMPort.SelectedIndex + 1;
        }
    }
}
