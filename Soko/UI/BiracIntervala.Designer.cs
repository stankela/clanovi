namespace Soko.UI
{
    partial class BiracIntervala
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpDo = new System.Windows.Forms.DateTimePicker();
            this.lblDo = new System.Windows.Forms.Label();
            this.dtpOd = new System.Windows.Forms.DateTimePicker();
            this.lblOd = new System.Windows.Forms.Label();
            this.lblInterval = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxGrupe = new System.Windows.Forms.ListBox();
            this.lblSelGrupe = new System.Windows.Forms.Label();
            this.checkedListBoxGrupe = new System.Windows.Forms.CheckedListBox();
            this.rbtGrupe = new System.Windows.Forms.RadioButton();
            this.rbtSveGrupe = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnOdustani = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbClan = new System.Windows.Forms.ComboBox();
            this.txtClan = new System.Windows.Forms.TextBox();
            this.rbtClan = new System.Windows.Forms.RadioButton();
            this.rbtCeoIzvestaj = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpDo);
            this.groupBox1.Controls.Add(this.lblDo);
            this.groupBox1.Controls.Add(this.dtpOd);
            this.groupBox1.Controls.Add(this.lblOd);
            this.groupBox1.Controls.Add(this.lblInterval);
            this.groupBox1.Location = new System.Drawing.Point(8, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // dtpDo
            // 
            this.dtpDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDo.Location = new System.Drawing.Point(302, 32);
            this.dtpDo.Name = "dtpDo";
            this.dtpDo.Size = new System.Drawing.Size(123, 20);
            this.dtpDo.TabIndex = 3;
            // 
            // lblDo
            // 
            this.lblDo.AutoSize = true;
            this.lblDo.Location = new System.Drawing.Point(275, 36);
            this.lblDo.Name = "lblDo";
            this.lblDo.Size = new System.Drawing.Size(21, 13);
            this.lblDo.TabIndex = 5;
            this.lblDo.Text = "Do";
            // 
            // dtpOd
            // 
            this.dtpOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpOd.Location = new System.Drawing.Point(136, 32);
            this.dtpOd.Name = "dtpOd";
            this.dtpOd.Size = new System.Drawing.Size(123, 20);
            this.dtpOd.TabIndex = 2;
            // 
            // lblOd
            // 
            this.lblOd.AutoSize = true;
            this.lblOd.Location = new System.Drawing.Point(109, 36);
            this.lblOd.Name = "lblOd";
            this.lblOd.Size = new System.Drawing.Size(21, 13);
            this.lblOd.TabIndex = 4;
            this.lblOd.Text = "Od";
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = true;
            this.lblInterval.Location = new System.Drawing.Point(14, 36);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(89, 13);
            this.lblInterval.TabIndex = 6;
            this.lblInterval.Text = "Izvestaj za period";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxGrupe);
            this.groupBox2.Controls.Add(this.lblSelGrupe);
            this.groupBox2.Controls.Add(this.checkedListBoxGrupe);
            this.groupBox2.Controls.Add(this.rbtGrupe);
            this.groupBox2.Controls.Add(this.rbtSveGrupe);
            this.groupBox2.Location = new System.Drawing.Point(8, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(441, 224);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // listBoxGrupe
            // 
            this.listBoxGrupe.FormattingEnabled = true;
            this.listBoxGrupe.Location = new System.Drawing.Point(112, 120);
            this.listBoxGrupe.Name = "listBoxGrupe";
            this.listBoxGrupe.Size = new System.Drawing.Size(313, 95);
            this.listBoxGrupe.TabIndex = 7;
            // 
            // lblSelGrupe
            // 
            this.lblSelGrupe.AutoSize = true;
            this.lblSelGrupe.Location = new System.Drawing.Point(8, 120);
            this.lblSelGrupe.Name = "lblSelGrupe";
            this.lblSelGrupe.Size = new System.Drawing.Size(97, 13);
            this.lblSelGrupe.TabIndex = 8;
            this.lblSelGrupe.Text = "Selektovane grupe";
            // 
            // checkedListBoxGrupe
            // 
            this.checkedListBoxGrupe.FormattingEnabled = true;
            this.checkedListBoxGrupe.Location = new System.Drawing.Point(112, 16);
            this.checkedListBoxGrupe.Name = "checkedListBoxGrupe";
            this.checkedListBoxGrupe.Size = new System.Drawing.Size(313, 94);
            this.checkedListBoxGrupe.TabIndex = 4;
            this.checkedListBoxGrupe.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxGrupe_ItemCheck);
            // 
            // rbtGrupe
            // 
            this.rbtGrupe.AutoSize = true;
            this.rbtGrupe.Location = new System.Drawing.Point(8, 48);
            this.rbtGrupe.Name = "rbtGrupe";
            this.rbtGrupe.Size = new System.Drawing.Size(96, 17);
            this.rbtGrupe.TabIndex = 6;
            this.rbtGrupe.TabStop = true;
            this.rbtGrupe.Text = "Samo za grupe";
            this.rbtGrupe.UseVisualStyleBackColor = true;
            this.rbtGrupe.CheckedChanged += new System.EventHandler(this.rbtGrupe_CheckedChanged);
            // 
            // rbtSveGrupe
            // 
            this.rbtSveGrupe.AutoSize = true;
            this.rbtSveGrupe.Location = new System.Drawing.Point(8, 16);
            this.rbtSveGrupe.Name = "rbtSveGrupe";
            this.rbtSveGrupe.Size = new System.Drawing.Size(74, 17);
            this.rbtSveGrupe.TabIndex = 5;
            this.rbtSveGrupe.TabStop = true;
            this.rbtSveGrupe.Text = "Sve grupe";
            this.rbtSveGrupe.UseVisualStyleBackColor = true;
            this.rbtSveGrupe.CheckedChanged += new System.EventHandler(this.rbtSveGrupe_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(286, 424);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnOdustani
            // 
            this.btnOdustani.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOdustani.Location = new System.Drawing.Point(374, 424);
            this.btnOdustani.Name = "btnOdustani";
            this.btnOdustani.Size = new System.Drawing.Size(75, 23);
            this.btnOdustani.TabIndex = 2;
            this.btnOdustani.Text = "Odustani";
            this.btnOdustani.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbClan);
            this.groupBox3.Controls.Add(this.txtClan);
            this.groupBox3.Controls.Add(this.rbtClan);
            this.groupBox3.Controls.Add(this.rbtCeoIzvestaj);
            this.groupBox3.Location = new System.Drawing.Point(8, 310);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(441, 91);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // cmbClan
            // 
            this.cmbClan.FormattingEnabled = true;
            this.cmbClan.Location = new System.Drawing.Point(213, 51);
            this.cmbClan.Name = "cmbClan";
            this.cmbClan.Size = new System.Drawing.Size(212, 21);
            this.cmbClan.TabIndex = 3;
            this.cmbClan.SelectionChangeCommitted += new System.EventHandler(this.cmbClan_SelectionChangeCommitted);
            // 
            // txtClan
            // 
            this.txtClan.Location = new System.Drawing.Point(112, 51);
            this.txtClan.Name = "txtClan";
            this.txtClan.Size = new System.Drawing.Size(82, 20);
            this.txtClan.TabIndex = 2;
            this.txtClan.TextChanged += new System.EventHandler(this.txtClan_TextChanged);
            // 
            // rbtClan
            // 
            this.rbtClan.AutoSize = true;
            this.rbtClan.Location = new System.Drawing.Point(8, 51);
            this.rbtClan.Name = "rbtClan";
            this.rbtClan.Size = new System.Drawing.Size(95, 17);
            this.rbtClan.TabIndex = 1;
            this.rbtClan.TabStop = true;
            this.rbtClan.Text = "Samo za clana";
            this.rbtClan.UseVisualStyleBackColor = true;
            this.rbtClan.CheckedChanged += new System.EventHandler(this.rbtClan_CheckedChanged);
            // 
            // rbtCeoIzvestaj
            // 
            this.rbtCeoIzvestaj.AutoSize = true;
            this.rbtCeoIzvestaj.Location = new System.Drawing.Point(8, 19);
            this.rbtCeoIzvestaj.Name = "rbtCeoIzvestaj";
            this.rbtCeoIzvestaj.Size = new System.Drawing.Size(82, 17);
            this.rbtCeoIzvestaj.TabIndex = 0;
            this.rbtCeoIzvestaj.TabStop = true;
            this.rbtCeoIzvestaj.Text = "Ceo izvestaj";
            this.rbtCeoIzvestaj.UseVisualStyleBackColor = true;
            this.rbtCeoIzvestaj.CheckedChanged += new System.EventHandler(this.rbtCeoIzvestaj_CheckedChanged);
            // 
            // BiracIntervala
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOdustani;
            this.ClientSize = new System.Drawing.Size(461, 466);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnOdustani);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BiracIntervala";
            this.ShowInTaskbar = false;
            this.Text = "BiracIntervala";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BiracIntervala_FormClosed);
            this.Load += new System.EventHandler(this.BiracIntervala_Load);
            this.Shown += new System.EventHandler(this.BiracIntervala_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblInterval;
        private System.Windows.Forms.Label lblOd;
        private System.Windows.Forms.DateTimePicker dtpOd;
        private System.Windows.Forms.Label lblDo;
        private System.Windows.Forms.DateTimePicker dtpDo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtSveGrupe;
        private System.Windows.Forms.RadioButton rbtGrupe;
        private System.Windows.Forms.CheckedListBox checkedListBoxGrupe;
        private System.Windows.Forms.Label lblSelGrupe;
        private System.Windows.Forms.ListBox listBoxGrupe;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnOdustani;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbtClan;
        private System.Windows.Forms.RadioButton rbtCeoIzvestaj;
        private System.Windows.Forms.ComboBox cmbClan;
        private System.Windows.Forms.TextBox txtClan;
    }
}