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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            txtBrojOcitavanja.Text = "7";
            txtVremenskiIntervalZaCitacKartica.Text = Options.Instance.CitacKarticaTimerInterval.ToString();
            txtBrojPonavljanja.Text = Options.Instance.BrojPokusajaCitacKartica.ToString();
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

        private void txtVremenskiIntervalZaCitacKartica_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int newInterval;
                if (int.TryParse(txtVremenskiIntervalZaCitacKartica.Text, out newInterval))
                {
                    Options.Instance.CitacKarticaTimerInterval = newInterval;
                    SingleInstanceApplication.GlavniProzor.initKarticaTimer();
                }
            }
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
    }
}
