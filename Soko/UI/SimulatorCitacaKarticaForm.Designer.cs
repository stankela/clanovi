namespace Soko.UI
{
    partial class SimulatorCitacaKarticaForm
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
            this.txtClan = new System.Windows.Forms.TextBox();
            this.cmbClan = new System.Windows.Forms.ComboBox();
            this.dtpVremeOcitavanja = new System.Windows.Forms.DateTimePicker();
            this.btnOcitajKarticu = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblVremeOcitavanja = new System.Windows.Forms.Label();
            this.lblClan = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtClan
            // 
            this.txtClan.Location = new System.Drawing.Point(33, 39);
            this.txtClan.Name = "txtClan";
            this.txtClan.Size = new System.Drawing.Size(68, 20);
            this.txtClan.TabIndex = 0;
            this.txtClan.TextChanged += new System.EventHandler(this.txtClan_TextChanged);
            // 
            // cmbClan
            // 
            this.cmbClan.FormattingEnabled = true;
            this.cmbClan.Location = new System.Drawing.Point(124, 39);
            this.cmbClan.Name = "cmbClan";
            this.cmbClan.Size = new System.Drawing.Size(230, 21);
            this.cmbClan.TabIndex = 1;
            this.cmbClan.SelectionChangeCommitted += new System.EventHandler(this.cmbClan_SelectionChangeCommitted);
            // 
            // dtpVremeOcitavanja
            // 
            this.dtpVremeOcitavanja.Location = new System.Drawing.Point(33, 90);
            this.dtpVremeOcitavanja.Name = "dtpVremeOcitavanja";
            this.dtpVremeOcitavanja.Size = new System.Drawing.Size(200, 20);
            this.dtpVremeOcitavanja.TabIndex = 2;
            // 
            // btnOcitajKarticu
            // 
            this.btnOcitajKarticu.Location = new System.Drawing.Point(169, 162);
            this.btnOcitajKarticu.Name = "btnOcitajKarticu";
            this.btnOcitajKarticu.Size = new System.Drawing.Size(92, 23);
            this.btnOcitajKarticu.TabIndex = 3;
            this.btnOcitajKarticu.Text = "Ocitaj karticu";
            this.btnOcitajKarticu.UseVisualStyleBackColor = true;
            this.btnOcitajKarticu.Click += new System.EventHandler(this.btnOcitajKarticu_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(279, 162);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Zatvori";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblVremeOcitavanja
            // 
            this.lblVremeOcitavanja.AutoSize = true;
            this.lblVremeOcitavanja.Location = new System.Drawing.Point(30, 74);
            this.lblVremeOcitavanja.Name = "lblVremeOcitavanja";
            this.lblVremeOcitavanja.Size = new System.Drawing.Size(89, 13);
            this.lblVremeOcitavanja.TabIndex = 5;
            this.lblVremeOcitavanja.Text = "Vreme ocitavanja";
            // 
            // lblClan
            // 
            this.lblClan.AutoSize = true;
            this.lblClan.Location = new System.Drawing.Point(30, 23);
            this.lblClan.Name = "lblClan";
            this.lblClan.Size = new System.Drawing.Size(28, 13);
            this.lblClan.TabIndex = 6;
            this.lblClan.Text = "Clan";
            // 
            // SimulatorCitacaKarticaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 202);
            this.Controls.Add(this.lblClan);
            this.Controls.Add(this.lblVremeOcitavanja);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOcitajKarticu);
            this.Controls.Add(this.dtpVremeOcitavanja);
            this.Controls.Add(this.cmbClan);
            this.Controls.Add(this.txtClan);
            this.Name = "SimulatorCitacaKarticaForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SimulatorCitacaKarticaForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtClan;
        private System.Windows.Forms.ComboBox cmbClan;
        private System.Windows.Forms.DateTimePicker dtpVremeOcitavanja;
        private System.Windows.Forms.Button btnOcitajKarticu;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblVremeOcitavanja;
        private System.Windows.Forms.Label lblClan;
    }
}