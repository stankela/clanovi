namespace Soko.UI
{
    partial class PrinterSelectionForm
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
            this.lblStampacPotvrda = new System.Windows.Forms.Label();
            this.cmbStampacPotvrda = new System.Windows.Forms.ComboBox();
            this.lblStampacIzvestaj = new System.Windows.Forms.Label();
            this.cmbStampacIzvestaj = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblStampacPotvrda
            // 
            this.lblStampacPotvrda.AutoSize = true;
            this.lblStampacPotvrda.Location = new System.Drawing.Point(23, 21);
            this.lblStampacPotvrda.Name = "lblStampacPotvrda";
            this.lblStampacPotvrda.Size = new System.Drawing.Size(102, 13);
            this.lblStampacPotvrda.TabIndex = 0;
            this.lblStampacPotvrda.Text = "Stampac za potvrde";
            // 
            // cmbStampacPotvrda
            // 
            this.cmbStampacPotvrda.FormattingEnabled = true;
            this.cmbStampacPotvrda.Location = new System.Drawing.Point(26, 37);
            this.cmbStampacPotvrda.Name = "cmbStampacPotvrda";
            this.cmbStampacPotvrda.Size = new System.Drawing.Size(223, 21);
            this.cmbStampacPotvrda.TabIndex = 1;
            // 
            // lblStampacIzvestaj
            // 
            this.lblStampacIzvestaj.AutoSize = true;
            this.lblStampacIzvestaj.Location = new System.Drawing.Point(23, 72);
            this.lblStampacIzvestaj.Name = "lblStampacIzvestaj";
            this.lblStampacIzvestaj.Size = new System.Drawing.Size(107, 13);
            this.lblStampacIzvestaj.TabIndex = 2;
            this.lblStampacIzvestaj.Text = "Stampac za izvestaje";
            // 
            // cmbStampacIzvestaj
            // 
            this.cmbStampacIzvestaj.FormattingEnabled = true;
            this.cmbStampacIzvestaj.Location = new System.Drawing.Point(26, 88);
            this.cmbStampacIzvestaj.Name = "cmbStampacIzvestaj";
            this.cmbStampacIzvestaj.Size = new System.Drawing.Size(223, 21);
            this.cmbStampacIzvestaj.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(84, 130);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(174, 130);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Odustani";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // PrinterSelectionForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(271, 171);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbStampacIzvestaj);
            this.Controls.Add(this.lblStampacIzvestaj);
            this.Controls.Add(this.cmbStampacPotvrda);
            this.Controls.Add(this.lblStampacPotvrda);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrinterSelectionForm";
            this.ShowInTaskbar = false;
            this.Text = "PrinterSelectionForm";
            this.Load += new System.EventHandler(this.PrinterSelectionForm_Load);
            this.Shown += new System.EventHandler(this.PrinterSelectionForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStampacPotvrda;
        private System.Windows.Forms.ComboBox cmbStampacPotvrda;
        private System.Windows.Forms.Label lblStampacIzvestaj;
        private System.Windows.Forms.ComboBox cmbStampacIzvestaj;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}