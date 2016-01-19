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
        public CitacKarticaDialog()
        {
            InitializeComponent();
            updateCitacKarticaButtonText();
        }

        private void updateCitacKarticaButtonText()
        {
            if (SingleInstanceApplication.GlavniProzor.CitacKarticaEnabled)
            {
                btnEnableCitacKartica.Text = "Zaustavi citac kartica";
            }
            else
            {
                btnEnableCitacKartica.Text = "Pokreni citac kartica";
            }
        }

        private void CitacKarticaDialog_Load(object sender, EventArgs e)
        {
            cmbCOMPortReader.SelectedIndex = Options.Instance.COMPortReader - 1;
            cmbCOMPortWriter.SelectedIndex = Options.Instance.COMPortWriter - 1;
            txtPoslednjiDanZaUplate.Text = Options.Instance.PoslednjiDanZaUplate.ToString();
            txtVelicinaSlova.Text = Options.Instance.VelicinaSlovaZaCitacKartica.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int i;
            if (!Int32.TryParse(txtPoslednjiDanZaUplate.Text, out i)
                || i < 1 || i > 31)
            {
                MessageDialogs.showMessage("Neispravna vrednost za poslednji dan u mesecu za uplate.", this.Text);
                this.DialogResult = DialogResult.None;
                return;
            }
            if (!Int32.TryParse(txtVelicinaSlova.Text, out i)
                || i < 1 || i > 100)
            {
                MessageDialogs.showMessage("Neispravna vrednost za velicinu slova na displeju.", this.Text);
                this.DialogResult = DialogResult.None;
                return;
            }
            Options.Instance.COMPortReader = cmbCOMPortReader.SelectedIndex + 1;
            Options.Instance.COMPortWriter = cmbCOMPortWriter.SelectedIndex + 1;
            Options.Instance.PoslednjiDanZaUplate = Int32.Parse(txtPoslednjiDanZaUplate.Text);
            Options.Instance.VelicinaSlovaZaCitacKartica = Int32.Parse(txtVelicinaSlova.Text);
        }

        private void btnEnableCitacKartica_Click(object sender, EventArgs e)
        {
            if (SingleInstanceApplication.GlavniProzor.CitacKarticaEnabled)
            {
                SingleInstanceApplication.GlavniProzor.ZaustaviCitacKartica();
            }
            else
            {
                SingleInstanceApplication.GlavniProzor.PokreniCitacKartica();
            }
            updateCitacKarticaButtonText();
        }
    }
}
