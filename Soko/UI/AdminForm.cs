﻿using Soko.Misc;
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
            ckbTraziLozinkuPreOtvaranjaProzora.Checked = Options.Instance.TraziLozinkuPreOtvaranjaProzora;
            txtCitacKarticaThreadInterval.Text = Options.Instance.CitacKarticaThreadInterval.ToString();
            txtCitacKarticaThreadSkipCount.Text = Options.Instance.CitacKarticaThreadSkipCount.ToString();
            txtCitacKarticaThreadVisibleCount.Text = Options.Instance.CitacKarticaThreadVisibleCount.ToString();
            txtCitacKarticaThreadPauzaZaBrisanje.Text = Options.Instance.CitacKarticaThreadPauzaZaBrisanje.ToString();

            lstLogFiles.SelectionMode = SelectionMode.MultiExtended;
            string[] files = Directory.GetFiles(LOG_DIR);
            foreach (string file in files)
            {
                lstLogFiles.Items.Add(Path.GetFileName(file));
            }

            ckbCitacKarticeNaPosebnomThreadu.Checked = Options.Instance.CitacKarticeNaPosebnomThreadu;
            ckbCitacKarticeNaPosebnomThreadu.CheckedChanged += new EventHandler(ckbCitacKarticeNaPosebnomThreadu_CheckedChanged);
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
                if (!Options.Instance.CitacKarticeNaPosebnomThreadu)
                {
                    SingleInstanceApplication.GlavniProzor.zaustaviCitacKartica();
                    SingleInstanceApplication.GlavniProzor.pokreniCitacKartica();
                }
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
            Options.Instance.TraziLozinkuPreOtvaranjaProzora = ckbTraziLozinkuPreOtvaranjaProzora.Checked;
        }

        private void ckbCitacKarticeNaPosebnomThreadu_CheckedChanged(object sender, EventArgs e)
        {
            SingleInstanceApplication.GlavniProzor.zaustaviCitacKartica();
            Options.Instance.CitacKarticeNaPosebnomThreadu = ckbCitacKarticeNaPosebnomThreadu.Checked;
            SingleInstanceApplication.GlavniProzor.pokreniCitacKartica();
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
    }
}
