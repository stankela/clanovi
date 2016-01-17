namespace Soko.UI
{
    partial class CitacKarticaDialog
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
            this.cmbCOMPortReader = new System.Windows.Forms.ComboBox();
            this.lblCOMPortReader = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbCOMPortWriter = new System.Windows.Forms.ComboBox();
            this.lblCOMPortWriter = new System.Windows.Forms.Label();
            this.lblPoslednjiDanZaUplate = new System.Windows.Forms.Label();
            this.txtPoslednjiDanZaUplate = new System.Windows.Forms.TextBox();
            this.btnEnableCitacKartica = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbCOMPortReader
            // 
            this.cmbCOMPortReader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCOMPortReader.FormattingEnabled = true;
            this.cmbCOMPortReader.Items.AddRange(new object[] {
            "COM 1",
            "COM 2",
            "COM 3",
            "COM 4",
            "COM 5",
            "COM 6",
            "COM 7",
            "COM 8",
            "COM 9",
            "COM 10",
            "COM 11",
            "COM 12",
            "COM 13",
            "COM 14",
            "COM 15",
            "COM 16",
            "COM 17",
            "COM 18",
            "COM 19",
            "COM 20",
            "COM 21",
            "COM 22",
            "COM 23",
            "COM 24",
            "COM 25",
            "COM 26",
            "COM 27",
            "COM 28",
            "COM 29",
            "COM 30"});
            this.cmbCOMPortReader.Location = new System.Drawing.Point(21, 75);
            this.cmbCOMPortReader.Name = "cmbCOMPortReader";
            this.cmbCOMPortReader.Size = new System.Drawing.Size(132, 21);
            this.cmbCOMPortReader.TabIndex = 0;
            // 
            // lblCOMPortReader
            // 
            this.lblCOMPortReader.AutoSize = true;
            this.lblCOMPortReader.Location = new System.Drawing.Point(18, 59);
            this.lblCOMPortReader.Name = "lblCOMPortReader";
            this.lblCOMPortReader.Size = new System.Drawing.Size(127, 13);
            this.lblCOMPortReader.TabIndex = 1;
            this.lblCOMPortReader.Text = "COM port za citac kartica";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(90, 246);
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
            this.btnCancel.Location = new System.Drawing.Point(180, 246);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cmbCOMPortWriter
            // 
            this.cmbCOMPortWriter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCOMPortWriter.FormattingEnabled = true;
            this.cmbCOMPortWriter.Items.AddRange(new object[] {
            "COM 1",
            "COM 2",
            "COM 3",
            "COM 4",
            "COM 5",
            "COM 6",
            "COM 7",
            "COM 8",
            "COM 9",
            "COM 10",
            "COM 11",
            "COM 12",
            "COM 13",
            "COM 14",
            "COM 15",
            "COM 16",
            "COM 17",
            "COM 18",
            "COM 19",
            "COM 20",
            "COM 21",
            "COM 22",
            "COM 23",
            "COM 24",
            "COM 25",
            "COM 26",
            "COM 27",
            "COM 28",
            "COM 29",
            "COM 30"});
            this.cmbCOMPortWriter.Location = new System.Drawing.Point(21, 129);
            this.cmbCOMPortWriter.Name = "cmbCOMPortWriter";
            this.cmbCOMPortWriter.Size = new System.Drawing.Size(132, 21);
            this.cmbCOMPortWriter.TabIndex = 4;
            // 
            // lblCOMPortWriter
            // 
            this.lblCOMPortWriter.AutoSize = true;
            this.lblCOMPortWriter.Location = new System.Drawing.Point(18, 113);
            this.lblCOMPortWriter.Name = "lblCOMPortWriter";
            this.lblCOMPortWriter.Size = new System.Drawing.Size(129, 13);
            this.lblCOMPortWriter.TabIndex = 5;
            this.lblCOMPortWriter.Text = "COM port za pisac kartica";
            // 
            // lblPoslednjiDanZaUplate
            // 
            this.lblPoslednjiDanZaUplate.AutoSize = true;
            this.lblPoslednjiDanZaUplate.Location = new System.Drawing.Point(18, 24);
            this.lblPoslednjiDanZaUplate.Name = "lblPoslednjiDanZaUplate";
            this.lblPoslednjiDanZaUplate.Size = new System.Drawing.Size(165, 13);
            this.lblPoslednjiDanZaUplate.TabIndex = 6;
            this.lblPoslednjiDanZaUplate.Text = "Poslednji dan u mesecu za uplate";
            // 
            // txtPoslednjiDanZaUplate
            // 
            this.txtPoslednjiDanZaUplate.Location = new System.Drawing.Point(189, 21);
            this.txtPoslednjiDanZaUplate.Name = "txtPoslednjiDanZaUplate";
            this.txtPoslednjiDanZaUplate.Size = new System.Drawing.Size(45, 20);
            this.txtPoslednjiDanZaUplate.TabIndex = 7;
            // 
            // btnEnableCitacKartica
            // 
            this.btnEnableCitacKartica.Location = new System.Drawing.Point(21, 182);
            this.btnEnableCitacKartica.Name = "btnEnableCitacKartica";
            this.btnEnableCitacKartica.Size = new System.Drawing.Size(132, 23);
            this.btnEnableCitacKartica.TabIndex = 8;
            this.btnEnableCitacKartica.Text = "button1";
            this.btnEnableCitacKartica.UseVisualStyleBackColor = true;
            this.btnEnableCitacKartica.Click += new System.EventHandler(this.btnEnableCitacKartica_Click);
            // 
            // CitacKarticaDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(267, 281);
            this.Controls.Add(this.btnEnableCitacKartica);
            this.Controls.Add(this.txtPoslednjiDanZaUplate);
            this.Controls.Add(this.lblPoslednjiDanZaUplate);
            this.Controls.Add(this.lblCOMPortWriter);
            this.Controls.Add(this.cmbCOMPortWriter);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblCOMPortReader);
            this.Controls.Add(this.cmbCOMPortReader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CitacKarticaDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Citac kartica";
            this.Load += new System.EventHandler(this.CitacKarticaDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCOMPortReader;
        private System.Windows.Forms.Label lblCOMPortReader;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbCOMPortWriter;
        private System.Windows.Forms.Label lblCOMPortWriter;
        private System.Windows.Forms.Label lblPoslednjiDanZaUplate;
        private System.Windows.Forms.TextBox txtPoslednjiDanZaUplate;
        private System.Windows.Forms.Button btnEnableCitacKartica;
    }
}