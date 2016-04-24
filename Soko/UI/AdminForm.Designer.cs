namespace Soko.UI
{
    partial class AdminForm
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
            this.lstCitacKarticeElapsedMs = new System.Windows.Forms.ListBox();
            this.ckbPrikaziVremenaOcitavanja = new System.Windows.Forms.CheckBox();
            this.txtBrojOcitavanja = new System.Windows.Forms.TextBox();
            this.lblBrojOcitavanja = new System.Windows.Forms.Label();
            this.lblVremenskiIntervalZaCitacKartica = new System.Windows.Forms.Label();
            this.txtVremenskiIntervalZaCitacKartica = new System.Windows.Forms.TextBox();
            this.lblBrojPonavljanja = new System.Windows.Forms.Label();
            this.txtBrojPonavljanja = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstWriteDataCardReturnValue = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstReadDataCardReturnValue = new System.Windows.Forms.ListBox();
            this.ckbLogToFile = new System.Windows.Forms.CheckBox();
            this.btnPromeniVremenskiInterval = new System.Windows.Forms.Button();
            this.btnProveriOcitavanja = new System.Windows.Forms.Button();
            this.lstLogFiles = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstCitacKarticeElapsedMs
            // 
            this.lstCitacKarticeElapsedMs.FormattingEnabled = true;
            this.lstCitacKarticeElapsedMs.Location = new System.Drawing.Point(26, 98);
            this.lstCitacKarticeElapsedMs.Name = "lstCitacKarticeElapsedMs";
            this.lstCitacKarticeElapsedMs.Size = new System.Drawing.Size(120, 95);
            this.lstCitacKarticeElapsedMs.TabIndex = 1;
            // 
            // ckbPrikaziVremenaOcitavanja
            // 
            this.ckbPrikaziVremenaOcitavanja.AutoSize = true;
            this.ckbPrikaziVremenaOcitavanja.Location = new System.Drawing.Point(26, 42);
            this.ckbPrikaziVremenaOcitavanja.Name = "ckbPrikaziVremenaOcitavanja";
            this.ckbPrikaziVremenaOcitavanja.Size = new System.Drawing.Size(153, 17);
            this.ckbPrikaziVremenaOcitavanja.TabIndex = 2;
            this.ckbPrikaziVremenaOcitavanja.Text = "Prikazi vremena ocitavanja";
            this.ckbPrikaziVremenaOcitavanja.UseVisualStyleBackColor = true;
            // 
            // txtBrojOcitavanja
            // 
            this.txtBrojOcitavanja.Location = new System.Drawing.Point(106, 72);
            this.txtBrojOcitavanja.Name = "txtBrojOcitavanja";
            this.txtBrojOcitavanja.Size = new System.Drawing.Size(40, 20);
            this.txtBrojOcitavanja.TabIndex = 3;
            // 
            // lblBrojOcitavanja
            // 
            this.lblBrojOcitavanja.AutoSize = true;
            this.lblBrojOcitavanja.Location = new System.Drawing.Point(23, 75);
            this.lblBrojOcitavanja.Name = "lblBrojOcitavanja";
            this.lblBrojOcitavanja.Size = new System.Drawing.Size(77, 13);
            this.lblBrojOcitavanja.TabIndex = 4;
            this.lblBrojOcitavanja.Text = "Broj ocitavanja";
            // 
            // lblVremenskiIntervalZaCitacKartica
            // 
            this.lblVremenskiIntervalZaCitacKartica.AutoSize = true;
            this.lblVremenskiIntervalZaCitacKartica.Location = new System.Drawing.Point(244, 42);
            this.lblVremenskiIntervalZaCitacKartica.Name = "lblVremenskiIntervalZaCitacKartica";
            this.lblVremenskiIntervalZaCitacKartica.Size = new System.Drawing.Size(190, 13);
            this.lblVremenskiIntervalZaCitacKartica.TabIndex = 5;
            this.lblVremenskiIntervalZaCitacKartica.Text = "Vremenski interval za citac kartica (ms)";
            // 
            // txtVremenskiIntervalZaCitacKartica
            // 
            this.txtVremenskiIntervalZaCitacKartica.Location = new System.Drawing.Point(247, 68);
            this.txtVremenskiIntervalZaCitacKartica.Name = "txtVremenskiIntervalZaCitacKartica";
            this.txtVremenskiIntervalZaCitacKartica.Size = new System.Drawing.Size(59, 20);
            this.txtVremenskiIntervalZaCitacKartica.TabIndex = 6;
            // 
            // lblBrojPonavljanja
            // 
            this.lblBrojPonavljanja.AutoSize = true;
            this.lblBrojPonavljanja.Location = new System.Drawing.Point(23, 231);
            this.lblBrojPonavljanja.Name = "lblBrojPonavljanja";
            this.lblBrojPonavljanja.Size = new System.Drawing.Size(154, 13);
            this.lblBrojPonavljanja.TabIndex = 8;
            this.lblBrojPonavljanja.Text = "Broj ponavljanja citanja/pisanja";
            // 
            // txtBrojPonavljanja
            // 
            this.txtBrojPonavljanja.Location = new System.Drawing.Point(26, 247);
            this.txtBrojPonavljanja.Name = "txtBrojPonavljanja";
            this.txtBrojPonavljanja.Size = new System.Drawing.Size(74, 20);
            this.txtBrojPonavljanja.TabIndex = 9;
            this.txtBrojPonavljanja.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBrojPonavljanja_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(298, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "WriteDataCard return value";
            // 
            // lstWriteDataCardReturnValue
            // 
            this.lstWriteDataCardReturnValue.FormattingEnabled = true;
            this.lstWriteDataCardReturnValue.Location = new System.Drawing.Point(301, 152);
            this.lstWriteDataCardReturnValue.Name = "lstWriteDataCardReturnValue";
            this.lstWriteDataCardReturnValue.Size = new System.Drawing.Size(120, 95);
            this.lstWriteDataCardReturnValue.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "ReadDataCard return value";
            // 
            // lstReadDataCardReturnValue
            // 
            this.lstReadDataCardReturnValue.FormattingEnabled = true;
            this.lstReadDataCardReturnValue.Location = new System.Drawing.Point(301, 293);
            this.lstReadDataCardReturnValue.Name = "lstReadDataCardReturnValue";
            this.lstReadDataCardReturnValue.Size = new System.Drawing.Size(120, 95);
            this.lstReadDataCardReturnValue.TabIndex = 13;
            // 
            // ckbLogToFile
            // 
            this.ckbLogToFile.AutoSize = true;
            this.ckbLogToFile.Location = new System.Drawing.Point(26, 322);
            this.ckbLogToFile.Name = "ckbLogToFile";
            this.ckbLogToFile.Size = new System.Drawing.Size(79, 17);
            this.ckbLogToFile.TabIndex = 14;
            this.ckbLogToFile.Text = "Log To File";
            this.ckbLogToFile.UseVisualStyleBackColor = true;
            this.ckbLogToFile.CheckedChanged += new System.EventHandler(this.ckbLogToFile_CheckedChanged);
            // 
            // btnPromeniVremenskiInterval
            // 
            this.btnPromeniVremenskiInterval.Location = new System.Drawing.Point(323, 66);
            this.btnPromeniVremenskiInterval.Name = "btnPromeniVremenskiInterval";
            this.btnPromeniVremenskiInterval.Size = new System.Drawing.Size(75, 23);
            this.btnPromeniVremenskiInterval.TabIndex = 15;
            this.btnPromeniVremenskiInterval.Text = "Promeni";
            this.btnPromeniVremenskiInterval.UseVisualStyleBackColor = true;
            this.btnPromeniVremenskiInterval.Click += new System.EventHandler(this.btnPromeniVremenskiInterval_Click);
            // 
            // btnProveriOcitavanja
            // 
            this.btnProveriOcitavanja.Location = new System.Drawing.Point(546, 52);
            this.btnProveriOcitavanja.Name = "btnProveriOcitavanja";
            this.btnProveriOcitavanja.Size = new System.Drawing.Size(105, 23);
            this.btnProveriOcitavanja.TabIndex = 16;
            this.btnProveriOcitavanja.Text = "Proveri ocitavanja";
            this.btnProveriOcitavanja.UseVisualStyleBackColor = true;
            this.btnProveriOcitavanja.Click += new System.EventHandler(this.btnProveriOcitavanja_Click);
            // 
            // lstLogFiles
            // 
            this.lstLogFiles.FormattingEnabled = true;
            this.lstLogFiles.Location = new System.Drawing.Point(546, 81);
            this.lstLogFiles.Name = "lstLogFiles";
            this.lstLogFiles.Size = new System.Drawing.Size(221, 316);
            this.lstLogFiles.TabIndex = 17;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 465);
            this.Controls.Add(this.lstLogFiles);
            this.Controls.Add(this.btnProveriOcitavanja);
            this.Controls.Add(this.btnPromeniVremenskiInterval);
            this.Controls.Add(this.ckbLogToFile);
            this.Controls.Add(this.lstReadDataCardReturnValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstWriteDataCardReturnValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBrojPonavljanja);
            this.Controls.Add(this.lblBrojPonavljanja);
            this.Controls.Add(this.txtVremenskiIntervalZaCitacKartica);
            this.Controls.Add(this.lblVremenskiIntervalZaCitacKartica);
            this.Controls.Add(this.lblBrojOcitavanja);
            this.Controls.Add(this.txtBrojOcitavanja);
            this.Controls.Add(this.ckbPrikaziVremenaOcitavanja);
            this.Controls.Add(this.lstCitacKarticeElapsedMs);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstCitacKarticeElapsedMs;
        private System.Windows.Forms.CheckBox ckbPrikaziVremenaOcitavanja;
        private System.Windows.Forms.TextBox txtBrojOcitavanja;
        private System.Windows.Forms.Label lblBrojOcitavanja;
        private System.Windows.Forms.Label lblVremenskiIntervalZaCitacKartica;
        private System.Windows.Forms.TextBox txtVremenskiIntervalZaCitacKartica;
        private System.Windows.Forms.Label lblBrojPonavljanja;
        private System.Windows.Forms.TextBox txtBrojPonavljanja;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstWriteDataCardReturnValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstReadDataCardReturnValue;
        private System.Windows.Forms.CheckBox ckbLogToFile;
        private System.Windows.Forms.Button btnPromeniVremenskiInterval;
        private System.Windows.Forms.Button btnProveriOcitavanja;
        private System.Windows.Forms.ListBox lstLogFiles;
    }
}