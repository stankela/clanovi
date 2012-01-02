using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Soko.Domain;

namespace Soko.UI
{
    public partial class GrupeBezKategorijaDialog : Form
    {
        public GrupeBezKategorijaDialog(List<Grupa> grupe)
        {
            InitializeComponent();

            this.Text = "Grupe bez kategorija";
            Font = Options.Instance.Font;

            grupe.Sort();
            listBox1.Items.Clear();
            foreach (Grupa g in grupe)
            {
                listBox1.Items.Add(g.SifraCrtaNaziv);
            }
        }
    }
}