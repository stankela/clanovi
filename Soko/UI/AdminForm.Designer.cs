﻿namespace Soko.UI
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
            this.lblBrojPonavljanja = new System.Windows.Forms.Label();
            this.txtBrojPonavljanja = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstWriteDataCardReturnValue = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstReadDataCardReturnValue = new System.Windows.Forms.ListBox();
            this.ckbLogToFile = new System.Windows.Forms.CheckBox();
            this.btnProveriOcitavanja = new System.Windows.Forms.Button();
            this.lstLogFiles = new System.Windows.Forms.ListBox();
            this.ckbTraziLozinkuPreOtvaranjaProzora = new System.Windows.Forms.CheckBox();
            this.lblCitacKarticaThreadInterval = new System.Windows.Forms.Label();
            this.txtCitacKarticaThreadInterval = new System.Windows.Forms.TextBox();
            this.btnPromeniCitacKarticaThreadInterval = new System.Windows.Forms.Button();
            this.lblCitacKarticaThreadSkipCount = new System.Windows.Forms.Label();
            this.txtCitacKarticaThreadSkipCount = new System.Windows.Forms.TextBox();
            this.btnPromeniCitacKarticaThreadSkipCount = new System.Windows.Forms.Button();
            this.lblCitacKarticaThreadVisibleCount = new System.Windows.Forms.Label();
            this.txtCitacKarticaThreadVisibleCount = new System.Windows.Forms.TextBox();
            this.btnPromeniCitacKarticaThreadVisibleCount = new System.Windows.Forms.Button();
            this.lblCitacKarticaThreadPauzaZaBrisanje = new System.Windows.Forms.Label();
            this.txtCitacKarticaThreadPauzaZaBrisanje = new System.Windows.Forms.TextBox();
            this.btnPromeniCitacKarticaThreadPauzaZaBrisanje = new System.Windows.Forms.Button();
            this.lblWritePraznaDataCard = new System.Windows.Forms.Label();
            this.txtWritePraznaDataCard = new System.Windows.Forms.TextBox();
            this.btnWritePraznaDataCard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstCitacKarticeElapsedMs
            // 
            this.lstCitacKarticeElapsedMs.FormattingEnabled = true;
            this.lstCitacKarticeElapsedMs.Location = new System.Drawing.Point(12, 68);
            this.lstCitacKarticeElapsedMs.Name = "lstCitacKarticeElapsedMs";
            this.lstCitacKarticeElapsedMs.Size = new System.Drawing.Size(120, 95);
            this.lstCitacKarticeElapsedMs.TabIndex = 1;
            // 
            // ckbPrikaziVremenaOcitavanja
            // 
            this.ckbPrikaziVremenaOcitavanja.AutoSize = true;
            this.ckbPrikaziVremenaOcitavanja.Location = new System.Drawing.Point(12, 12);
            this.ckbPrikaziVremenaOcitavanja.Name = "ckbPrikaziVremenaOcitavanja";
            this.ckbPrikaziVremenaOcitavanja.Size = new System.Drawing.Size(153, 17);
            this.ckbPrikaziVremenaOcitavanja.TabIndex = 2;
            this.ckbPrikaziVremenaOcitavanja.Text = "Prikazi vremena ocitavanja";
            this.ckbPrikaziVremenaOcitavanja.UseVisualStyleBackColor = true;
            // 
            // txtBrojOcitavanja
            // 
            this.txtBrojOcitavanja.Location = new System.Drawing.Point(92, 42);
            this.txtBrojOcitavanja.Name = "txtBrojOcitavanja";
            this.txtBrojOcitavanja.Size = new System.Drawing.Size(40, 20);
            this.txtBrojOcitavanja.TabIndex = 3;
            // 
            // lblBrojOcitavanja
            // 
            this.lblBrojOcitavanja.AutoSize = true;
            this.lblBrojOcitavanja.Location = new System.Drawing.Point(9, 45);
            this.lblBrojOcitavanja.Name = "lblBrojOcitavanja";
            this.lblBrojOcitavanja.Size = new System.Drawing.Size(77, 13);
            this.lblBrojOcitavanja.TabIndex = 4;
            this.lblBrojOcitavanja.Text = "Broj ocitavanja";
            // 
            // lblBrojPonavljanja
            // 
            this.lblBrojPonavljanja.AutoSize = true;
            this.lblBrojPonavljanja.Location = new System.Drawing.Point(11, 181);
            this.lblBrojPonavljanja.Name = "lblBrojPonavljanja";
            this.lblBrojPonavljanja.Size = new System.Drawing.Size(154, 13);
            this.lblBrojPonavljanja.TabIndex = 8;
            this.lblBrojPonavljanja.Text = "Broj ponavljanja citanja/pisanja";
            // 
            // txtBrojPonavljanja
            // 
            this.txtBrojPonavljanja.Location = new System.Drawing.Point(14, 197);
            this.txtBrojPonavljanja.Name = "txtBrojPonavljanja";
            this.txtBrojPonavljanja.Size = new System.Drawing.Size(74, 20);
            this.txtBrojPonavljanja.TabIndex = 9;
            this.txtBrojPonavljanja.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBrojPonavljanja_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(417, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "WriteDataCard return value";
            // 
            // lstWriteDataCardReturnValue
            // 
            this.lstWriteDataCardReturnValue.FormattingEnabled = true;
            this.lstWriteDataCardReturnValue.Location = new System.Drawing.Point(420, 137);
            this.lstWriteDataCardReturnValue.Name = "lstWriteDataCardReturnValue";
            this.lstWriteDataCardReturnValue.Size = new System.Drawing.Size(120, 95);
            this.lstWriteDataCardReturnValue.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(417, 262);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "ReadDataCard return value";
            // 
            // lstReadDataCardReturnValue
            // 
            this.lstReadDataCardReturnValue.FormattingEnabled = true;
            this.lstReadDataCardReturnValue.Location = new System.Drawing.Point(420, 278);
            this.lstReadDataCardReturnValue.Name = "lstReadDataCardReturnValue";
            this.lstReadDataCardReturnValue.Size = new System.Drawing.Size(120, 95);
            this.lstReadDataCardReturnValue.TabIndex = 13;
            // 
            // ckbLogToFile
            // 
            this.ckbLogToFile.AutoSize = true;
            this.ckbLogToFile.Location = new System.Drawing.Point(12, 235);
            this.ckbLogToFile.Name = "ckbLogToFile";
            this.ckbLogToFile.Size = new System.Drawing.Size(79, 17);
            this.ckbLogToFile.TabIndex = 14;
            this.ckbLogToFile.Text = "Log To File";
            this.ckbLogToFile.UseVisualStyleBackColor = true;
            this.ckbLogToFile.CheckedChanged += new System.EventHandler(this.ckbLogToFile_CheckedChanged);
            // 
            // btnProveriOcitavanja
            // 
            this.btnProveriOcitavanja.Location = new System.Drawing.Point(619, 118);
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
            this.lstLogFiles.Location = new System.Drawing.Point(619, 147);
            this.lstLogFiles.Name = "lstLogFiles";
            this.lstLogFiles.Size = new System.Drawing.Size(221, 212);
            this.lstLogFiles.TabIndex = 17;
            // 
            // ckbTraziLozinkuPreOtvaranjaProzora
            // 
            this.ckbTraziLozinkuPreOtvaranjaProzora.AutoSize = true;
            this.ckbTraziLozinkuPreOtvaranjaProzora.Location = new System.Drawing.Point(11, 268);
            this.ckbTraziLozinkuPreOtvaranjaProzora.Name = "ckbTraziLozinkuPreOtvaranjaProzora";
            this.ckbTraziLozinkuPreOtvaranjaProzora.Size = new System.Drawing.Size(188, 17);
            this.ckbTraziLozinkuPreOtvaranjaProzora.TabIndex = 18;
            this.ckbTraziLozinkuPreOtvaranjaProzora.Text = "Trazi lozinku pre otvaranja prozora";
            this.ckbTraziLozinkuPreOtvaranjaProzora.UseVisualStyleBackColor = true;
            this.ckbTraziLozinkuPreOtvaranjaProzora.CheckedChanged += new System.EventHandler(this.ckbTraziLozinkuPreOtvaranjaProzora_CheckedChanged);
            // 
            // lblCitacKarticaThreadInterval
            // 
            this.lblCitacKarticaThreadInterval.AutoSize = true;
            this.lblCitacKarticaThreadInterval.Location = new System.Drawing.Point(13, 341);
            this.lblCitacKarticaThreadInterval.Name = "lblCitacKarticaThreadInterval";
            this.lblCitacKarticaThreadInterval.Size = new System.Drawing.Size(136, 13);
            this.lblCitacKarticaThreadInterval.TabIndex = 20;
            this.lblCitacKarticaThreadInterval.Text = "Citac kartica thread interval";
            // 
            // txtCitacKarticaThreadInterval
            // 
            this.txtCitacKarticaThreadInterval.Location = new System.Drawing.Point(208, 338);
            this.txtCitacKarticaThreadInterval.Name = "txtCitacKarticaThreadInterval";
            this.txtCitacKarticaThreadInterval.Size = new System.Drawing.Size(75, 20);
            this.txtCitacKarticaThreadInterval.TabIndex = 21;
            // 
            // btnPromeniCitacKarticaThreadInterval
            // 
            this.btnPromeniCitacKarticaThreadInterval.Location = new System.Drawing.Point(301, 336);
            this.btnPromeniCitacKarticaThreadInterval.Name = "btnPromeniCitacKarticaThreadInterval";
            this.btnPromeniCitacKarticaThreadInterval.Size = new System.Drawing.Size(75, 23);
            this.btnPromeniCitacKarticaThreadInterval.TabIndex = 22;
            this.btnPromeniCitacKarticaThreadInterval.Text = "Promeni";
            this.btnPromeniCitacKarticaThreadInterval.UseVisualStyleBackColor = true;
            this.btnPromeniCitacKarticaThreadInterval.Click += new System.EventHandler(this.btnPromeniCitacKarticaThreadInterval_Click);
            // 
            // lblCitacKarticaThreadSkipCount
            // 
            this.lblCitacKarticaThreadSkipCount.AutoSize = true;
            this.lblCitacKarticaThreadSkipCount.Location = new System.Drawing.Point(13, 382);
            this.lblCitacKarticaThreadSkipCount.Name = "lblCitacKarticaThreadSkipCount";
            this.lblCitacKarticaThreadSkipCount.Size = new System.Drawing.Size(151, 13);
            this.lblCitacKarticaThreadSkipCount.TabIndex = 23;
            this.lblCitacKarticaThreadSkipCount.Text = "Citac kartica thread skip count";
            // 
            // txtCitacKarticaThreadSkipCount
            // 
            this.txtCitacKarticaThreadSkipCount.Location = new System.Drawing.Point(208, 375);
            this.txtCitacKarticaThreadSkipCount.Name = "txtCitacKarticaThreadSkipCount";
            this.txtCitacKarticaThreadSkipCount.Size = new System.Drawing.Size(75, 20);
            this.txtCitacKarticaThreadSkipCount.TabIndex = 24;
            // 
            // btnPromeniCitacKarticaThreadSkipCount
            // 
            this.btnPromeniCitacKarticaThreadSkipCount.Location = new System.Drawing.Point(302, 373);
            this.btnPromeniCitacKarticaThreadSkipCount.Name = "btnPromeniCitacKarticaThreadSkipCount";
            this.btnPromeniCitacKarticaThreadSkipCount.Size = new System.Drawing.Size(75, 23);
            this.btnPromeniCitacKarticaThreadSkipCount.TabIndex = 25;
            this.btnPromeniCitacKarticaThreadSkipCount.Text = "Promeni";
            this.btnPromeniCitacKarticaThreadSkipCount.UseVisualStyleBackColor = true;
            this.btnPromeniCitacKarticaThreadSkipCount.Click += new System.EventHandler(this.btnPromeniCitacKarticaThreadSkipCount_Click);
            // 
            // lblCitacKarticaThreadVisibleCount
            // 
            this.lblCitacKarticaThreadVisibleCount.AutoSize = true;
            this.lblCitacKarticaThreadVisibleCount.Location = new System.Drawing.Point(11, 422);
            this.lblCitacKarticaThreadVisibleCount.Name = "lblCitacKarticaThreadVisibleCount";
            this.lblCitacKarticaThreadVisibleCount.Size = new System.Drawing.Size(161, 13);
            this.lblCitacKarticaThreadVisibleCount.TabIndex = 26;
            this.lblCitacKarticaThreadVisibleCount.Text = "Citac kartica thread visible count";
            // 
            // txtCitacKarticaThreadVisibleCount
            // 
            this.txtCitacKarticaThreadVisibleCount.Location = new System.Drawing.Point(208, 419);
            this.txtCitacKarticaThreadVisibleCount.Name = "txtCitacKarticaThreadVisibleCount";
            this.txtCitacKarticaThreadVisibleCount.Size = new System.Drawing.Size(75, 20);
            this.txtCitacKarticaThreadVisibleCount.TabIndex = 27;
            // 
            // btnPromeniCitacKarticaThreadVisibleCount
            // 
            this.btnPromeniCitacKarticaThreadVisibleCount.Location = new System.Drawing.Point(301, 417);
            this.btnPromeniCitacKarticaThreadVisibleCount.Name = "btnPromeniCitacKarticaThreadVisibleCount";
            this.btnPromeniCitacKarticaThreadVisibleCount.Size = new System.Drawing.Size(75, 23);
            this.btnPromeniCitacKarticaThreadVisibleCount.TabIndex = 28;
            this.btnPromeniCitacKarticaThreadVisibleCount.Text = "Promeni";
            this.btnPromeniCitacKarticaThreadVisibleCount.UseVisualStyleBackColor = true;
            this.btnPromeniCitacKarticaThreadVisibleCount.Click += new System.EventHandler(this.btnPromeniCitacKarticaThreadVisibleCount_Click);
            // 
            // lblCitacKarticaThreadPauzaZaBrisanje
            // 
            this.lblCitacKarticaThreadPauzaZaBrisanje.AutoSize = true;
            this.lblCitacKarticaThreadPauzaZaBrisanje.Location = new System.Drawing.Point(15, 463);
            this.lblCitacKarticaThreadPauzaZaBrisanje.Name = "lblCitacKarticaThreadPauzaZaBrisanje";
            this.lblCitacKarticaThreadPauzaZaBrisanje.Size = new System.Drawing.Size(184, 13);
            this.lblCitacKarticaThreadPauzaZaBrisanje.TabIndex = 29;
            this.lblCitacKarticaThreadPauzaZaBrisanje.Text = "Citac kartica thread pauza za brisanje";
            // 
            // txtCitacKarticaThreadPauzaZaBrisanje
            // 
            this.txtCitacKarticaThreadPauzaZaBrisanje.Location = new System.Drawing.Point(208, 460);
            this.txtCitacKarticaThreadPauzaZaBrisanje.Name = "txtCitacKarticaThreadPauzaZaBrisanje";
            this.txtCitacKarticaThreadPauzaZaBrisanje.Size = new System.Drawing.Size(75, 20);
            this.txtCitacKarticaThreadPauzaZaBrisanje.TabIndex = 30;
            // 
            // btnPromeniCitacKarticaThreadPauzaZaBrisanje
            // 
            this.btnPromeniCitacKarticaThreadPauzaZaBrisanje.Location = new System.Drawing.Point(302, 458);
            this.btnPromeniCitacKarticaThreadPauzaZaBrisanje.Name = "btnPromeniCitacKarticaThreadPauzaZaBrisanje";
            this.btnPromeniCitacKarticaThreadPauzaZaBrisanje.Size = new System.Drawing.Size(75, 23);
            this.btnPromeniCitacKarticaThreadPauzaZaBrisanje.TabIndex = 31;
            this.btnPromeniCitacKarticaThreadPauzaZaBrisanje.Text = "Promeni";
            this.btnPromeniCitacKarticaThreadPauzaZaBrisanje.UseVisualStyleBackColor = true;
            this.btnPromeniCitacKarticaThreadPauzaZaBrisanje.Click += new System.EventHandler(this.btnPromeniCitacKarticaThreadPauzaZaBrisanje_Click);
            // 
            // lblWritePraznaDataCard
            // 
            this.lblWritePraznaDataCard.AutoSize = true;
            this.lblWritePraznaDataCard.Location = new System.Drawing.Point(277, 86);
            this.lblWritePraznaDataCard.Name = "lblWritePraznaDataCard";
            this.lblWritePraznaDataCard.Size = new System.Drawing.Size(115, 13);
            this.lblWritePraznaDataCard.TabIndex = 42;
            this.lblWritePraznaDataCard.Text = "Write prazna data card";
            // 
            // txtWritePraznaDataCard
            // 
            this.txtWritePraznaDataCard.Location = new System.Drawing.Point(416, 79);
            this.txtWritePraznaDataCard.Name = "txtWritePraznaDataCard";
            this.txtWritePraznaDataCard.Size = new System.Drawing.Size(72, 20);
            this.txtWritePraznaDataCard.TabIndex = 43;
            // 
            // btnWritePraznaDataCard
            // 
            this.btnWritePraznaDataCard.Location = new System.Drawing.Point(509, 77);
            this.btnWritePraznaDataCard.Name = "btnWritePraznaDataCard";
            this.btnWritePraznaDataCard.Size = new System.Drawing.Size(75, 23);
            this.btnWritePraznaDataCard.TabIndex = 44;
            this.btnWritePraznaDataCard.Text = "Promeni";
            this.btnWritePraznaDataCard.UseVisualStyleBackColor = true;
            this.btnWritePraznaDataCard.Click += new System.EventHandler(this.btnWritePraznaDataCard_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 525);
            this.Controls.Add(this.btnWritePraznaDataCard);
            this.Controls.Add(this.txtWritePraznaDataCard);
            this.Controls.Add(this.lblWritePraznaDataCard);
            this.Controls.Add(this.btnPromeniCitacKarticaThreadPauzaZaBrisanje);
            this.Controls.Add(this.txtCitacKarticaThreadPauzaZaBrisanje);
            this.Controls.Add(this.lblCitacKarticaThreadPauzaZaBrisanje);
            this.Controls.Add(this.btnPromeniCitacKarticaThreadVisibleCount);
            this.Controls.Add(this.txtCitacKarticaThreadVisibleCount);
            this.Controls.Add(this.lblCitacKarticaThreadVisibleCount);
            this.Controls.Add(this.btnPromeniCitacKarticaThreadSkipCount);
            this.Controls.Add(this.txtCitacKarticaThreadSkipCount);
            this.Controls.Add(this.lblCitacKarticaThreadSkipCount);
            this.Controls.Add(this.btnPromeniCitacKarticaThreadInterval);
            this.Controls.Add(this.txtCitacKarticaThreadInterval);
            this.Controls.Add(this.lblCitacKarticaThreadInterval);
            this.Controls.Add(this.ckbTraziLozinkuPreOtvaranjaProzora);
            this.Controls.Add(this.lstLogFiles);
            this.Controls.Add(this.btnProveriOcitavanja);
            this.Controls.Add(this.ckbLogToFile);
            this.Controls.Add(this.lstReadDataCardReturnValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstWriteDataCardReturnValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBrojPonavljanja);
            this.Controls.Add(this.lblBrojPonavljanja);
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
        private System.Windows.Forms.Label lblBrojPonavljanja;
        private System.Windows.Forms.TextBox txtBrojPonavljanja;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstWriteDataCardReturnValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstReadDataCardReturnValue;
        private System.Windows.Forms.CheckBox ckbLogToFile;
        private System.Windows.Forms.Button btnProveriOcitavanja;
        private System.Windows.Forms.ListBox lstLogFiles;
        private System.Windows.Forms.CheckBox ckbTraziLozinkuPreOtvaranjaProzora;
        private System.Windows.Forms.Label lblCitacKarticaThreadInterval;
        private System.Windows.Forms.TextBox txtCitacKarticaThreadInterval;
        private System.Windows.Forms.Button btnPromeniCitacKarticaThreadInterval;
        private System.Windows.Forms.Label lblCitacKarticaThreadSkipCount;
        private System.Windows.Forms.TextBox txtCitacKarticaThreadSkipCount;
        private System.Windows.Forms.Button btnPromeniCitacKarticaThreadSkipCount;
        private System.Windows.Forms.Label lblCitacKarticaThreadVisibleCount;
        private System.Windows.Forms.TextBox txtCitacKarticaThreadVisibleCount;
        private System.Windows.Forms.Button btnPromeniCitacKarticaThreadVisibleCount;
        private System.Windows.Forms.Label lblCitacKarticaThreadPauzaZaBrisanje;
        private System.Windows.Forms.TextBox txtCitacKarticaThreadPauzaZaBrisanje;
        private System.Windows.Forms.Button btnPromeniCitacKarticaThreadPauzaZaBrisanje;
        private System.Windows.Forms.Label lblWritePraznaDataCard;
        private System.Windows.Forms.TextBox txtWritePraznaDataCard;
        private System.Windows.Forms.Button btnWritePraznaDataCard;
    }
}