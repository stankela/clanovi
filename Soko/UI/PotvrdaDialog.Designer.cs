namespace Soko.UI
{
    partial class PotvrdaDialog
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
            this.lblPitanje = new System.Windows.Forms.Label();
            this.btnDa = new System.Windows.Forms.Button();
            this.btnNe = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPitanje
            // 
            this.lblPitanje.Location = new System.Drawing.Point(8, 8);
            this.lblPitanje.Name = "lblPitanje";
            this.lblPitanje.Size = new System.Drawing.Size(192, 32);
            this.lblPitanje.TabIndex = 2;
            // 
            // btnDa
            // 
            this.btnDa.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnDa.Location = new System.Drawing.Point(8, 48);
            this.btnDa.Name = "btnDa";
            this.btnDa.Size = new System.Drawing.Size(75, 23);
            this.btnDa.TabIndex = 1;
            this.btnDa.Text = "Da";
            this.btnDa.UseVisualStyleBackColor = true;
            // 
            // btnNe
            // 
            this.btnNe.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNe.Location = new System.Drawing.Point(104, 48);
            this.btnNe.Name = "btnNe";
            this.btnNe.Size = new System.Drawing.Size(75, 23);
            this.btnNe.TabIndex = 2;
            this.btnNe.Text = "Ne";
            this.btnNe.UseVisualStyleBackColor = true;
            // 
            // PotvrdaDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 80);
            this.Controls.Add(this.btnNe);
            this.Controls.Add(this.btnDa);
            this.Controls.Add(this.lblPitanje);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PotvrdaDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PotvrdaDialog";
            this.Shown += new System.EventHandler(this.PotvrdaDialog_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPitanje;
        private System.Windows.Forms.Button btnDa;
        private System.Windows.Forms.Button btnNe;
    }
}