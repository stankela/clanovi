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
        private const string LOG_DIR = @"..\Log";

        public AdminForm()
        {
            InitializeComponent();
            txtBrojOcitavanja.Text = "7";
            txtVremenskiIntervalZaCitacKartica.Text = Options.Instance.CitacKarticaTimerInterval.ToString();
            txtBrojPonavljanja.Text = Options.Instance.BrojPokusajaCitacKartica.ToString();
            ckbLogToFile.Checked = Options.Instance.LogToFile;
            ckbTraziLozinkuPreOtvaranjaProzora.Checked = Options.Instance.TraziLozinkuPreOtvatanjaProzora;

            lstLogFiles.SelectionMode = SelectionMode.MultiExtended;
            string[] files = Directory.GetFiles(LOG_DIR);
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

        private void btnPromeniVremenskiInterval_Click(object sender, EventArgs e)
        {
            int newInterval;
            if (int.TryParse(txtVremenskiIntervalZaCitacKartica.Text, out newInterval))
            {
                Options.Instance.CitacKarticaTimerInterval = newInterval;
                SingleInstanceApplication.GlavniProzor.initKarticaTimer();
            }
        }

        private void btnProveriOcitavanja_Click(object sender, EventArgs e)
        {
            if (lstLogFiles.SelectedItems.Count == 0)
                return;

            string message = String.Empty;
            foreach (string fileName in lstLogFiles.SelectedItems)
            {
                string msg;
                Sesija.Instance.proveriOcitavanja(Path.Combine(LOG_DIR, fileName), out msg);
                message += msg + "\n";
            }
            MessageBox.Show(message, "Provera ocitavanja");
        }

        private void ckbTraziLozinkuPreOtvaranjaProzora_CheckedChanged(object sender, EventArgs e)
        {
            Options.Instance.TraziLozinkuPreOtvatanjaProzora = ckbTraziLozinkuPreOtvaranjaProzora.Checked;
        }
    }
}
