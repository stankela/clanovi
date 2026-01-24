using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Soko.CitacKartica;

namespace Soko.UI
{
    public partial class CitacKarticaDialog : Form
    {
        private int oldCitacUplateIndex;
        private int oldCitacTreningIndex;

        public CitacKarticaDialog()
        {
            InitializeComponent();
            Font = Options.Instance.Font;
            cmbPoslednjiMesecZaGodisnjeClanarine.Items.AddRange(new object[] {
            "Januar",
            "Februar",
            "Mart",
            "April",
            "Maj",
            "Jun",
            "Jul",
            "Avgust",
            "Septembar",
            "Oktobar",
            "Novembar",
            "Decembar"});

            cmbCitacKarticaUplate.Items.AddRange(
                // Convert TipCitaca[] to object[]
                Array.ConvertAll(CitacKartica.GetTipoviCitaca(), x => (object)x));
            cmbCitacKarticaTrening.Items.AddRange(Array.ConvertAll(CitacKartica.GetTipoviCitaca(), x => (object)x));
        }

        private void CitacKarticaDialog_Load(object sender, EventArgs e)
        {
            cmbCitacKarticaUplate.SelectedIndex = Options.Instance.CitacKarticaUplate;
            cmbCitacKarticaTrening.SelectedIndex = Options.Instance.CitacKarticaTrening;
            oldCitacUplateIndex = cmbCitacKarticaUplate.SelectedIndex;
            oldCitacTreningIndex = cmbCitacKarticaTrening.SelectedIndex;

            updateCOMPortVisibility();
            cmbCOMPortReader.SelectedIndex = Options.Instance.COMPortReader - 1;
            cmbCOMPortWriter.SelectedIndex = Options.Instance.COMPortWriter - 1;

            cmbPoslednjiMesecZaGodisnjeClanarine.SelectedIndex = Options.Instance.PoslednjiMesecZaGodisnjeClanarine - 1;
            txtPoslednjiDanZaUplate.Text = Options.Instance.PoslednjiDanZaUplate.ToString();
            txtVelicinaSlova.Text = Options.Instance.VelicinaSlovaZaCitacKartica.ToString();
            ckbPrikaziBoje.Checked = Options.Instance.PrikaziBojeKodOcitavanja;
            ckbPrikaziImeClana.Checked = Options.Instance.PrikaziImeClanaKodOcitavanjaKartice;
            ckbPrikaziDisplejPrekoCelogEkrana.Checked = Options.Instance.PrikaziDisplejPrekoCelogEkrana;
            updatePrikaziDisplejPrekoCelogEkrana();
        }

        private void updateCOMPortVisibility()
        {
            cmbCOMPortWriter.Enabled = cmbCitacKarticaUplate.SelectedItem != null
                && cmbCitacKarticaUplate.SelectedItem.ToString() == TipCitaca.Panonit.ToString();
            cmbCOMPortWriter.Visible = cmbCOMPortWriter.Enabled;
            lblCOMPortWriter.Visible = cmbCOMPortWriter.Visible;
            cmbCOMPortReader.Enabled = cmbCitacKarticaTrening.SelectedItem != null
                && cmbCitacKarticaTrening.SelectedItem.ToString() == TipCitaca.Panonit.ToString();
            cmbCOMPortReader.Visible = cmbCOMPortReader.Enabled;
            lblCOMPortReader.Visible = cmbCOMPortReader.Visible;
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
            string errorMsg;
            if (oldCitacUplateIndex != cmbCitacKarticaUplate.SelectedIndex)
            {
                TipCitaca citacUplate = CitacKartica.GetTipoviCitaca()[cmbCitacKarticaUplate.SelectedIndex];
                if (!CitacKartica.ValidateCitacUplate(citacUplate, out errorMsg))
                {
                    MessageDialogs.showMessage("Neispravna vrednost za citac kartica za uplate.\n\n" + errorMsg,
                        this.Text);
                    cmbCitacKarticaUplate.Focus();
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }
            if (oldCitacTreningIndex != cmbCitacKarticaTrening.SelectedIndex)
            {
                TipCitaca citacTrening = CitacKartica.GetTipoviCitaca()[cmbCitacKarticaTrening.SelectedIndex];
                if (!CitacKartica.ValidateCitacTrening(citacTrening, out errorMsg))
                {
                    MessageDialogs.showMessage("Neispravna vrednost za citac kartica za trening.\n\n" + errorMsg,
                        this.Text);
                    cmbCitacKarticaTrening.Focus();
                    this.DialogResult = DialogResult.None;
                    return;
                }
            }

            Options.Instance.CitacKarticaUplate = cmbCitacKarticaUplate.SelectedIndex;
            Options.Instance.CitacKarticaTrening = cmbCitacKarticaTrening.SelectedIndex;    
            Options.Instance.COMPortReader = cmbCOMPortReader.SelectedIndex + 1;
            Options.Instance.COMPortWriter = cmbCOMPortWriter.SelectedIndex + 1;
            Options.Instance.PoslednjiMesecZaGodisnjeClanarine = cmbPoslednjiMesecZaGodisnjeClanarine.SelectedIndex + 1;
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

            if (oldCitacUplateIndex != cmbCitacKarticaUplate.SelectedIndex)
            {
                CitacKartica.UpdateUplateInstanceFromOptions();
            }
            if (oldCitacTreningIndex != cmbCitacKarticaTrening.SelectedIndex)
            {
                CitacKartica.UpdateTreningInstanceFromOptions();
            }

            if (!Options.Instance.JedinstvenProgram && Options.Instance.IsProgramZaClanarinu)
            {
                string msg = "CitacKarticaOpcije";
                msg += " CitacKarticaUplate " + Options.Instance.CitacKarticaUplate.ToString();
                msg += " CitacKarticaTrening " + Options.Instance.CitacKarticaTrening.ToString();
                msg += " COMPortReader " + Options.Instance.COMPortReader.ToString();
                msg += " COMPortWriter " + Options.Instance.COMPortWriter.ToString();
                msg += " PoslednjiDanZaUplate " + Options.Instance.PoslednjiDanZaUplate.ToString();
                msg += " PoslednjiMesecZaGodisnjeClanarine " + Options.Instance.PoslednjiMesecZaGodisnjeClanarine.ToString();
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

        private void cmbCitacKarticaUplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateCOMPortVisibility();
        }

        private void cmbCitacKarticaTrening_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateCOMPortVisibility();
        }
    }
}
