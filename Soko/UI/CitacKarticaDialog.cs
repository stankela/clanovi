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
    public partial class CitacKarticaDialog : Form
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

        private int poslednjiDanZaUplate;
        public int PoslednjiDanZaUplate
        {
            get { return poslednjiDanZaUplate; }
        }

        public CitacKarticaDialog()
        {
            InitializeComponent();
        }

        private void CitacKarticaDialog_Load(object sender, EventArgs e)
        {
            cmbCOMPortReader.SelectedIndex = Options.Instance.COMPortReader - 1;
            cmbCOMPortWriter.SelectedIndex = Options.Instance.COMPortWriter - 1;
            txtPoslednjiDanZaUplate.Text = Options.Instance.PoslednjiDanZaUplate.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int dan;
            if (!Int32.TryParse(txtPoslednjiDanZaUplate.Text, out dan)
                || dan < 1 || dan > 31)
            {
                MessageDialogs.showMessage("Neispravna vrednost za poslednji dan u mesecu za uplate.", this.Text);
                this.DialogResult = DialogResult.None;
                return;
            }
            comPortReader = cmbCOMPortReader.SelectedIndex + 1;
            comPortWriter = cmbCOMPortWriter.SelectedIndex + 1;
            poslednjiDanZaUplate = Int32.Parse(txtPoslednjiDanZaUplate.Text);
        }
    }
}
