namespace Soko.UI
{
    partial class PrintOrExportForm
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
            this.rbtStampaj = new System.Windows.Forms.RadioButton();
            this.rbtEksportuj = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rbtStampaj
            // 
            this.rbtStampaj.AutoSize = true;
            this.rbtStampaj.Location = new System.Drawing.Point(25, 27);
            this.rbtStampaj.Name = "rbtStampaj";
            this.rbtStampaj.Size = new System.Drawing.Size(101, 17);
            this.rbtStampaj.TabIndex = 0;
            this.rbtStampaj.TabStop = true;
            this.rbtStampaj.Text = "Stampaj izvestaj";
            this.rbtStampaj.UseVisualStyleBackColor = true;
            // 
            // rbtEksportuj
            // 
            this.rbtEksportuj.AutoSize = true;
            this.rbtEksportuj.Location = new System.Drawing.Point(25, 59);
            this.rbtEksportuj.Name = "rbtEksportuj";
            this.rbtEksportuj.Size = new System.Drawing.Size(120, 17);
            this.rbtEksportuj.TabIndex = 1;
            this.rbtEksportuj.TabStop = true;
            this.rbtEksportuj.Text = "Eksportuj u tekst fajl";
            this.rbtEksportuj.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(22, 106);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(120, 106);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // PrintOrExportForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(212, 152);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.rbtEksportuj);
            this.Controls.Add(this.rbtStampaj);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintOrExportForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrintOrExportForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtStampaj;
        private System.Windows.Forms.RadioButton rbtEksportuj;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}