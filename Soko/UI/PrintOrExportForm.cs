﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class PrintOrExportForm : Form
    {
        private bool eksportuj;
        public bool Eksportuj
        {
            get { return eksportuj; }
        }

        public PrintOrExportForm()
        {
            InitializeComponent();
            Text = "Izvestaj";
            rbtStampaj.Checked = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            eksportuj = rbtEksportuj.Checked;
        }
    }
}
