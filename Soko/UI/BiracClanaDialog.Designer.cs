namespace Soko.UI
{
    partial class BiracClanaDialog
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
            this.cmbClan = new System.Windows.Forms.ComboBox();
            this.txtSifraClana = new System.Windows.Forms.TextBox();
            this.lblClan = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(190, 77);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(281, 77);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // BiracClanaDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(373, 117);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbClan);
            this.Controls.Add(this.txtSifraClana);
            this.Controls.Add(this.lblClan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BiracClanaDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unos Clanarine";
            this.Shown += new System.EventHandler(this.BiracClanaDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClan;
        private System.Windows.Forms.TextBox txtSifraClana;
        private System.Windows.Forms.ComboBox cmbClan;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}