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
            txtCitacKarticaThreadInterval.Text = Options.Instance.CitacKarticaThreadInterval.ToString();
            txtCitacKarticaThreadSkipCount.Text = Options.Instance.CitacKarticaThreadSkipCount.ToString();
            txtCitacKarticaThreadVisibleCount.Text = Options.Instance.CitacKarticaThreadVisibleCount.ToString();
            txtCitacKarticaThreadPauzaZaBrisanje.Text = Options.Instance.CitacKarticaThreadPauzaZaBrisanje.ToString();
            ckbUseWaitAndReadLoop.Checked = Options.Instance.UseWaitAndReadLoop;

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

        private void btnPromeniCitacKarticaThreadInterval_Click(object sender, EventArgs e)
        {
            int newValue;
            if (int.TryParse(txtCitacKarticaThreadInterval.Text, out newValue))
            {
                Options.Instance.CitacKarticaThreadInterval = newValue;
            }
        }

        private void btnPromeniCitacKarticaThreadSkipCount_Click(object sender, EventArgs e)
        {
            int newValue;
            if (int.TryParse(txtCitacKarticaThreadSkipCount.Text, out newValue))
            {
                Options.Instance.CitacKarticaThreadSkipCount = newValue;
            }
        }

        private void btnPromeniCitacKarticaThreadVisibleCount_Click(object sender, EventArgs e)
        {
            int newValue;
            if (int.TryParse(txtCitacKarticaThreadVisibleCount.Text, out newValue))
            {
                Options.Instance.CitacKarticaThreadVisibleCount = newValue;
            }
        }

        private void btnPromeniCitacKarticaThreadPauzaZaBrisanje_Click(object sender, EventArgs e)
        {
            int newValue;
            if (int.TryParse(txtCitacKarticaThreadPauzaZaBrisanje.Text, out newValue))
            {
                Options.Instance.CitacKarticaThreadPauzaZaBrisanje = newValue;
            }
        }

        private void ckbUseWaitAndReadLoop_CheckedChanged(object sender, EventArgs e)
        {
            Options.Instance.UseWaitAndReadLoop = ckbUseWaitAndReadLoop.Checked;
        }
    }
}
