namespace Soko.UI
{
    partial class GrupaDialog
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
            this.chbImaGodisnjuClanarinu = new System.Windows.Forms.CheckBox();
            this.cmbKategorija = new System.Windows.Forms.ComboBox();
            this.lblKategorija = new System.Windows.Forms.Label();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.lblNaziv = new System.Windows.Forms.Label();
            this.txtSifra = new System.Windows.Forms.TextBox();
            this.lblSifra = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnOdustani = new System.Windows.Forms.Button();
            this.lblFinansijskaCelina = new System.Windows.Forms.Label();
            this.cmbFinansijskaCelina = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbFinansijskaCelina);
            this.groupBox1.Controls.Add(this.lblFinansijskaCelina);
            this.groupBox1.Controls.Add(this.chbImaGodisnjuClanarinu);
            this.groupBox1.Controls.Add(this.cmbKategorija);
            this.groupBox1.Controls.Add(this.lblKategorija);
            this.groupBox1.Controls.Add(this.txtNaziv);
            this.groupBox1.Controls.Add(this.lblNaziv);
            this.groupBox1.Controls.Add(this.txtSifra);
            this.groupBox1.Controls.Add(this.lblSifra);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 198);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // chbImaGodisnjuClanarinu
            // 
            this.chbImaGodisnjuClanarinu.AutoSize = true;
            this.chbImaGodisnjuClanarinu.Location = new System.Drawing.Point(11, 166);
            this.chbImaGodisnjuClanarinu.Name = "chbImaGodisnjuClanarinu";
            this.chbImaGodisnjuClanarinu.Size = new System.Drawing.Size(131, 17);
            this.chbImaGodisnjuClanarinu.TabIndex = 11;
            this.chbImaGodisnjuClanarinu.Text = "Ima godisnju clanarinu";
            this.chbImaGodisnjuClanarinu.UseVisualStyleBackColor = true;
            // 
            // cmbKategorija
            // 
            this.cmbKategorija.FormattingEnabled = true;
            this.cmbKategorija.Location = new System.Drawing.Point(72, 80);
            this.cmbKategorija.Name = "cmbKategorija";
            this.cmbKategorija.Size = new System.Drawing.Size(232, 21);
            this.cmbKategorija.TabIndex = 10;
            // 
            // lblKategorija
            // 
            this.lblKategorija.AutoSize = true;
            this.lblKategorija.Location = new System.Drawing.Point(8, 80);
            this.lblKategorija.Name = "lblKategorija";
            this.lblKategorija.Size = new System.Drawing.Size(54, 13);
            this.lblKategorija.TabIndex = 9;
            this.lblKategorija.Text = "Kategorija";
            // 
            // txtNaziv
            // 
            this.txtNaziv.Location = new System.Drawing.Point(72, 48);
            this.txtNaziv.Name = "txtNaziv";
            this.txtNaziv.Size = new System.Drawing.Size(232, 20);
            this.txtNaziv.TabIndex = 3;
            // 
            // lblNaziv
            // 
            this.lblNaziv.AutoSize = true;
            this.lblNaziv.Location = new System.Drawing.Point(8, 48);
            this.lblNaziv.Name = "lblNaziv";
            this.lblNaziv.Size = new System.Drawing.Size(34, 13);
            this.lblNaziv.TabIndex = 2;
            this.lblNaziv.Text = "Naziv";
            // 
            // txtSifra
            // 
            this.txtSifra.Location = new System.Drawing.Point(72, 16);
            this.txtSifra.Name = "txtSifra";
            this.txtSifra.Size = new System.Drawing.Size(72, 20);
            this.txtSifra.TabIndex = 1;
            // 
            // lblSifra
            // 
            this.lblSifra.AutoSize = true;
            this.lblSifra.Location = new System.Drawing.Point(8, 16);
            this.lblSifra.Name = "lblSifra";
            this.lblSifra.Size = new System.Drawing.Size(28, 13);
            this.lblSifra.TabIndex = 8;
            this.lblSifra.Text = "Sifra";
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(163, 229);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnOdustani
            // 
            this.btnOdustani.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOdustani.Location = new System.Drawing.Point(251, 229);
            this.btnOdustani.Name = "btnOdustani";
            this.btnOdustani.Size = new System.Drawing.Size(75, 23);
            this.btnOdustani.TabIndex = 7;
            this.btnOdustani.Text = "Odustani";
            this.btnOdustani.UseVisualStyleBackColor = true;
            this.btnOdustani.Click += new System.EventHandler(this.btnOdustani_Click);
            // 
            // lblFinansijskaCelina
            // 
            this.lblFinansijskaCelina.AutoSize = true;
            this.lblFinansijskaCelina.Location = new System.Drawing.Point(10, 122);
            this.lblFinansijskaCelina.Name = "lblFinansijskaCelina";
            this.lblFinansijskaCelina.Size = new System.Drawing.Size(90, 13);
            this.lblFinansijskaCelina.TabIndex = 12;
            this.lblFinansijskaCelina.Text = "Finansijska celina";
            // 
            // cmbFinansijskaCelina
            // 
            this.cmbFinansijskaCelina.FormattingEnabled = true;
            this.cmbFinansijskaCelina.Location = new System.Drawing.Point(109, 119);
            this.cmbFinansijskaCelina.Name = "cmbFinansijskaCelina";
            this.cmbFinansijskaCelina.Size = new System.Drawing.Size(195, 21);
            this.cmbFinansijskaCelina.TabIndex = 13;
            // 
            // GrupaDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOdustani;
            this.ClientSize = new System.Drawing.Size(338, 269);
            this.Controls.Add(this.btnOdustani);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GrupaDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Grupa";
            this.Shown += new System.EventHandler(this.GrupaDialog_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSifra;
        private System.Windows.Forms.TextBox txtSifra;
        private System.Windows.Forms.Label lblNaziv;
        private System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.Label lblKategorija;
        private System.Windows.Forms.ComboBox cmbKategorija;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnOdustani;
        private System.Windows.Forms.CheckBox chbImaGodisnjuClanarinu;
        private System.Windows.Forms.ComboBox cmbFinansijskaCelina;
        private System.Windows.Forms.Label lblFinansijskaCelina;
    }
}