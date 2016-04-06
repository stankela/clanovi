namespace Soko.UI
{
    partial class PravljenjeKarticeForm
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
            this.btnNapraviKarticu = new System.Windows.Forms.Button();
            this.cmbClan = new System.Windows.Forms.ComboBox();
            this.txtSifraClana = new System.Windows.Forms.TextBox();
            this.lblClan = new System.Windows.Forms.Label();
            this.btnZatvori = new System.Windows.Forms.Button();
            this.ckbKartica = new System.Windows.Forms.CheckBox();
            this.ckbTestKartica = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnNapraviKarticu
            // 
            this.btnNapraviKarticu.Location = new System.Drawing.Point(151, 91);
            this.btnNapraviKarticu.Name = "btnNapraviKarticu";
            this.btnNapraviKarticu.Size = new System.Drawing.Size(111, 23);
            this.btnNapraviKarticu.TabIndex = 14;
            this.btnNapraviKarticu.Text = "Napravi karticu";
            this.btnNapraviKarticu.UseVisualStyleBackColor = true;
            this.btnNapraviKarticu.Click += new System.EventHandler(this.btnNapraviKarticu_Click);
            // 
            // cmbClan
            // 
            this.cmbClan.FormattingEnabled = true;
            this.cmbClan.Location = new System.Drawing.Point(124, 22);
            this.cmbClan.Name = "cmbClan";
            this.cmbClan.Size = new System.Drawing.Size(232, 21);
            this.cmbClan.TabIndex = 2;
            this.cmbClan.SelectionChangeCommitted += new System.EventHandler(this.cmbClan_SelectionChangeCommitted);
            // 
            // txtSifraClana
            // 
            this.txtSifraClana.Location = new System.Drawing.Point(58, 22);
            this.txtSifraClana.Name = "txtSifraClana";
            this.txtSifraClana.Size = new System.Drawing.Size(48, 20);
            this.txtSifraClana.TabIndex = 0;
            this.txtSifraClana.TextChanged += new System.EventHandler(this.txtSifraClana_TextChanged);
            // 
            // lblClan
            // 
            this.lblClan.AutoSize = true;
            this.lblClan.Location = new System.Drawing.Point(12, 22);
            this.lblClan.Name = "lblClan";
            this.lblClan.Size = new System.Drawing.Size(28, 13);
            this.lblClan.TabIndex = 0;
            this.lblClan.Text = "Clan";
            // 
            // btnZatvori
            // 
            this.btnZatvori.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnZatvori.Location = new System.Drawing.Point(281, 91);
            this.btnZatvori.Name = "btnZatvori";
            this.btnZatvori.Size = new System.Drawing.Size(75, 23);
            this.btnZatvori.TabIndex = 1;
            this.btnZatvori.Text = "Zatvori";
            this.btnZatvori.UseVisualStyleBackColor = true;
            // 
            // ckbKartica
            // 
            this.ckbKartica.AutoSize = true;
            this.ckbKartica.Location = new System.Drawing.Point(15, 74);
            this.ckbKartica.Name = "ckbKartica";
            this.ckbKartica.Size = new System.Drawing.Size(78, 17);
            this.ckbKartica.TabIndex = 15;
            this.ckbKartica.Text = "Ima karticu";
            this.ckbKartica.UseVisualStyleBackColor = true;
            // 
            // ckbTestKartica
            // 
            this.ckbTestKartica.AutoSize = true;
            this.ckbTestKartica.Location = new System.Drawing.Point(15, 97);
            this.ckbTestKartica.Name = "ckbTestKartica";
            this.ckbTestKartica.Size = new System.Drawing.Size(82, 17);
            this.ckbTestKartica.TabIndex = 16;
            this.ckbTestKartica.Text = "Test kartica";
            this.ckbTestKartica.UseVisualStyleBackColor = true;
            this.ckbTestKartica.CheckedChanged += new System.EventHandler(this.ckbTestKartica_CheckedChanged);
            // 
            // PravljenjeKarticeForm
            // 
            this.AcceptButton = this.btnZatvori;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 138);
            this.Controls.Add(this.ckbTestKartica);
            this.Controls.Add(this.ckbKartica);
            this.Controls.Add(this.cmbClan);
            this.Controls.Add(this.btnNapraviKarticu);
            this.Controls.Add(this.txtSifraClana);
            this.Controls.Add(this.btnZatvori);
            this.Controls.Add(this.lblClan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PravljenjeKarticeForm";
            this.ShowInTaskbar = false;
            this.Text = "Unos Clanarine";
            this.Load += new System.EventHandler(this.PravljenjeKarticeForm_Load);
            this.Shown += new System.EventHandler(this.PravljenjeKarticeForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnZatvori;
        private System.Windows.Forms.Label lblClan;
        private System.Windows.Forms.TextBox txtSifraClana;
        private System.Windows.Forms.ComboBox cmbClan;
        private System.Windows.Forms.Button btnNapraviKarticu;
        private System.Windows.Forms.CheckBox ckbKartica;
        private System.Windows.Forms.CheckBox ckbTestKartica;
    }
}