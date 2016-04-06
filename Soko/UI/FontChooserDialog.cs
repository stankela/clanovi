using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class FontChooserDialog : Form
    {
        private int initSize;

        public FontChooserDialog()
        {
            InitializeComponent();

            object[] sizes = { 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            cmbVelicina.Items.AddRange(sizes);
            Font = Options.Instance.Font;
            initSize = (int)Math.Round(Font.SizeInPoints);
            cmbVelicina.SelectedIndex = cmbVelicina.Items.IndexOf(initSize);
        }

        private void cmbVelicina_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            // automatically scales the form
            Font = new Font(Font.FontFamily, (int)cmbVelicina.SelectedItem);
        }

        public Font SelectedFont
        {
            get
            {
                int size;
                if (cmbVelicina.SelectedIndex != -1)
                    size = (int)cmbVelicina.SelectedItem;
                else
                    size = (int)Math.Round(Font.SizeInPoints);
                return new Font(Font.FontFamily, size);
            }
        }

        private void FontChooserDialog_Load(object sender, EventArgs e)
        {
            Screen screen = Screen.AllScreens[0];
            this.Location = new Point((screen.Bounds.Width - this.Width) / 2, (screen.Bounds.Height - this.Height) / 2);
        }
    }
}