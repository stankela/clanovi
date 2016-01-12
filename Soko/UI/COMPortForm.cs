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
        private int comPortReader;
        public int COMPortReader
        {
            get { return comPortReader; }
        }

        private int comPortWriter;
        public int COMPortWriter
        {
            get { return comPortWriter; }
        }

        public COMPortForm()
        {
            InitializeComponent();
        }

        private void COMPortForm_Load(object sender, EventArgs e)
        {
            cmbCOMPortReader.SelectedIndex = Options.Instance.COMPortReader - 1;
            cmbCOMPortWriter.SelectedIndex = Options.Instance.COMPortWriter - 1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            comPortReader = cmbCOMPortReader.SelectedIndex + 1;
            comPortWriter = cmbCOMPortWriter.SelectedIndex + 1;
        }
    }
}
