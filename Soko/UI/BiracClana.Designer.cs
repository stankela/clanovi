namespace Soko.UI
{
    partial class BiracClana
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
            this.cmbClan = new System.Windows.Forms.ComboBox();
            this.rbtClan = new System.Windows.Forms.RadioButton();
            this.rbtCeoIzvestaj = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnOdustani = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbClan);
            this.groupBox1.Controls.Add(this.rbtClan);
            this.groupBox1.Controls.Add(this.rbtCeoIzvestaj);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(272, 120);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // cmbClan
            // 
            this.cmbClan.FormattingEnabled = true;
            this.cmbClan.Location = new System.Drawing.Point(24, 80);
            this.cmbClan.Name = "cmbClan";
            this.cmbClan.Size = new System.Drawing.Size(232, 21);
            this.cmbClan.TabIndex = 3;
            // 
            // rbtClan
            // 
            this.rbtClan.AutoSize = true;
            this.rbtClan.Location = new System.Drawing.Point(8, 48);
            this.rbtClan.Name = "rbtClan";
            this.rbtClan.Size = new System.Drawing.Size(95, 17);
            this.rbtClan.TabIndex = 2;
            this.rbtClan.TabStop = true;
            this.rbtClan.Text = "Samo za clana";
            this.rbtClan.UseVisualStyleBackColor = true;
            this.rbtClan.CheckedChanged += new System.EventHandler(this.rbtClan_CheckedChanged);
            // 
            // rbtCeoIzvestaj
            // 
            this.rbtCeoIzvestaj.AutoSize = true;
            this.rbtCeoIzvestaj.Location = new System.Drawing.Point(8, 16);
            this.rbtCeoIzvestaj.Name = "rbtCeoIzvestaj";
            this.rbtCeoIzvestaj.Size = new System.Drawing.Size(82, 17);
            this.rbtCeoIzvestaj.TabIndex = 1;
            this.rbtCeoIzvestaj.TabStop = true;
            this.rbtCeoIzvestaj.Text = "Ceo izvestaj";
            this.rbtCeoIzvestaj.UseVisualStyleBackColor = true;
            this.rbtCeoIzvestaj.CheckedChanged += new System.EventHandler(this.rbtCeoIzvestaj_CheckedChanged);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(112, 136);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnOdustani
            // 
            this.btnOdustani.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOdustani.Location = new System.Drawing.Point(200, 136);
            this.btnOdustani.Name = "btnOdustani";
            this.btnOdustani.Size = new System.Drawing.Size(75, 23);
            this.btnOdustani.TabIndex = 5;
            this.btnOdustani.Text = "Odustani";
            this.btnOdustani.UseVisualStyleBackColor = true;
            // 
            // BiracClana
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOdustani;
            this.ClientSize = new System.Drawing.Size(290, 176);
            this.Controls.Add(this.btnOdustani);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BiracClana";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BiracClana";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtCeoIzvestaj;
        private System.Windows.Forms.RadioButton rbtClan;
        private System.Windows.Forms.ComboBox cmbClan;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnOdustani;
    }
}