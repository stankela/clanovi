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
            Font = Options.Instance.Font;
        }

        private void CitacKarticaDialog_Load(object sender, EventArgs e)
        {
            cmbCOMPortReader.SelectedIndex = Options.Instance.COMPortReader - 1;
            cmbCOMPortWriter.SelectedIndex = Options.Instance.COMPortWriter - 1;
            txtPoslednjiDanZaUplate.Text = Options.Instance.PoslednjiDanZaUplate.ToString();
            txtVelicinaSlova.Text = Options.Instance.VelicinaSlovaZaCitacKartica.ToString();
            ckbPrikaziBoje.Checked = Options.Instance.PrikaziBojeKodOcitavanja;
            ckbPrikaziImeClana.Checked = Options.Instance.PrikaziImeClanaKodOcitavanjaKartice;
            ckbPrikaziDisplejPrekoCelogEkrana.Checked = Options.Instance.PrikaziDisplejPrekoCelogEkrana;
            updatePrikaziDisplejPrekoCelogEkrana();
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
            if (!ckbPrikaziDisplejPrekoCelogEkrana.Checked)
            {
                if (!Int32.TryParse(txtSirinaDispleja.Text, out i) || i < 1)
                {
                    MessageDialogs.showMessage("Neispravna vrednost za sirinu displeja.", this.Text);
                    this.DialogResult = DialogResult.None;
                    return;
                }
                if (!Int32.TryParse(txtVisinaDispleja.Text, out i) || i < 1)
                {
                    MessageDialogs.showMessage("Neispravna vrednost za visinu displeja.", this.Text);
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            
            Options.Instance.COMPortReader = cmbCOMPortReader.SelectedIndex + 1;
            Options.Instance.COMPortWriter = cmbCOMPortWriter.SelectedIndex + 1;
            Options.Instance.PoslednjiDanZaUplate = Int32.Parse(txtPoslednjiDanZaUplate.Text);
            Options.Instance.VelicinaSlovaZaCitacKartica = Int32.Parse(txtVelicinaSlova.Text);
            Options.Instance.PrikaziBojeKodOcitavanja = ckbPrikaziBoje.Checked;
            Options.Instance.PrikaziImeClanaKodOcitavanjaKartice = ckbPrikaziImeClana.Checked;
            Options.Instance.PrikaziDisplejPrekoCelogEkrana = ckbPrikaziDisplejPrekoCelogEkrana.Checked;
            if (!ckbPrikaziDisplejPrekoCelogEkrana.Checked)
            {
                Options.Instance.SirinaDispleja = int.Parse(txtSirinaDispleja.Text);
                Options.Instance.VisinaDispleja = int.Parse(txtVisinaDispleja.Text);
            }

            if (!Options.Instance.JedinstvenProgram && Options.Instance.IsProgramZaClanarinu)
            {
                string msg = "CitacKarticaOpcije";
                msg += " COMPortReader " + Options.Instance.COMPortReader.ToString();
                msg += " COMPortWriter " + Options.Instance.COMPortWriter.ToString();
                msg += " PoslednjiDanZaUplate " + Options.Instance.PoslednjiDanZaUplate.ToString();
                msg += " VelicinaSlovaZaCitacKartica " + Options.Instance.VelicinaSlovaZaCitacKartica.ToString();
                msg += " PrikaziBojeKodOcitavanja " + Options.Instance.PrikaziBojeKodOcitavanja.ToString();
                msg += " PrikaziImeClanaKodOcitavanjaKartice " + Options.Instance.PrikaziImeClanaKodOcitavanjaKartice.ToString();
                msg += " PrikaziDisplejPrekoCelogEkrana " + Options.Instance.PrikaziDisplejPrekoCelogEkrana.ToString();
                msg += " SirinaDispleja " + Options.Instance.SirinaDispleja.ToString();
                msg += " VisinaDispleja " + Options.Instance.VisinaDispleja.ToString();
                Form1.Instance.sendToPipeClient(msg);
            }
        }

        private void ckbPrikaziDisplejPrekoCelogEkrana_CheckedChanged(object sender, EventArgs e)
        {
            updatePrikaziDisplejPrekoCelogEkrana();
        }

        private void updatePrikaziDisplejPrekoCelogEkrana()
        {
            if (!ckbPrikaziDisplejPrekoCelogEkrana.Checked)
            {
                txtSirinaDispleja.Enabled = true;
                txtVisinaDispleja.Enabled = true;
                CitacKarticaForm f = Form1.Instance.CitacKarticaForm;
                if (f != null)
                {
                    txtSirinaDispleja.Text = f.ClientSize.Width.ToString();
                    txtVisinaDispleja.Text = f.ClientSize.Height.ToString();
                }
                else
                {
                    txtSirinaDispleja.Text = Options.Instance.SirinaDispleja.ToString();
                    txtVisinaDispleja.Text = Options.Instance.VisinaDispleja.ToString();
                }
            }
            else
            {
                txtSirinaDispleja.Enabled = false;
                txtVisinaDispleja.Enabled = false;
            }
        }
    }
}
