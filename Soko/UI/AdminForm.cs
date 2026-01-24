using Soko.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            txtBrojOcitavanja.Text = "7";
            txtBrojPonavljanja.Text = Options.Instance.BrojPokusajaCitacKartica.ToString();
            ckbLogToFile.Checked = Options.Instance.LogToFile;
            ckbTraziLozinkuPreOtvaranjaProzora.Checked = Options.Instance.TraziLozinkuPreOtvaranjaProzora;
            txtWritePraznaDataCard.Text = Options.Instance.WritePraznaDataCard.ToString();

            lstLogFiles.SelectionMode = SelectionMode.MultiExtended;
            string[] files = Directory.GetFiles(Sesija.LOG_DIR);
            foreach (string file in files)
            {
                lstLogFiles.Items.Add(Path.GetFileName(file));
            }
        }

        public void newOcitavanje(long elapsedMs)
        {
            if (!ckbPrikaziVremenaOcitavanja.Checked)
                return;

            int brojOcitavanja = int.Parse(txtBrojOcitavanja.Text);
            if (lstCitacKarticeElapsedMs.Items.Count == brojOcitavanja)
            {
                lstCitacKarticeElapsedMs.Items.Clear();
            }
            lstCitacKarticeElapsedMs.Items.Add(elapsedMs);
        }

        private void txtBrojPonavljanja_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int newBrojPonavljanja;
                if (int.TryParse(txtBrojPonavljanja.Text, out newBrojPonavljanja))
                {
                    Options.Instance.BrojPokusajaCitacKartica = newBrojPonavljanja;
                }
            }
        }

        public void newPisanjeKartice(ulong retval, long elapsedMilliseconds)
        {
            if (lstWriteDataCardReturnValue.Items.Count == 7)
            {
                lstWriteDataCardReturnValue.Items.Clear();
            }
            lstWriteDataCardReturnValue.Items.Add(retval.ToString() + "     " + elapsedMilliseconds.ToString() + " ms");
        }

        public void newCitanjeKartice(ulong retval, long elapsedMilliseconds)
        {
            if (lstReadDataCardReturnValue.Items.Count == 7)
            {
                lstReadDataCardReturnValue.Items.Clear();
            }
            lstReadDataCardReturnValue.Items.Add(retval.ToString() + "     " + elapsedMilliseconds.ToString() + " ms");
        }

        private void ckbLogToFile_CheckedChanged(object sender, EventArgs e)
        {
            Options.Instance.LogToFile = ckbLogToFile.Checked;
        }

        private void btnProveriOcitavanja_Click(object sender, EventArgs e)
        {
            if (lstLogFiles.SelectedItems.Count == 0)
                return;

            string message = String.Empty;
            foreach (string fileName in lstLogFiles.SelectedItems)
            {
                string msg;
                Sesija.Instance.proveriOcitavanja(Path.Combine(Sesija.LOG_DIR, fileName), out msg);
                message += msg + "\n";
            }
            MessageBox.Show(message, "Provera ocitavanja");
        }

        private void ckbTraziLozinkuPreOtvaranjaProzora_CheckedChanged(object sender, EventArgs e)
        {
            Options.Instance.TraziLozinkuPreOtvaranjaProzora = ckbTraziLozinkuPreOtvaranjaProzora.Checked;
        }

        private void btnWritePraznaDataCard_Click(object sender, EventArgs e)
        {
            bool newValue = false;
            if (txtWritePraznaDataCard.Text == "True" || txtWritePraznaDataCard.Text == "true")
                newValue = true;
            Options.Instance.WritePraznaDataCard = newValue;
        }
    }
}
