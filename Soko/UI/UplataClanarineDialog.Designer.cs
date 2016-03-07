namespace Soko.UI
{
    partial class UplataClanarineDialog
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
            this.btnOdustani = new System.Windows.Forms.Button();
            this.btnOcitajKarticu = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblUkupnoIznos = new System.Windows.Forms.Label();
            this.lblUkupno = new System.Windows.Forms.Label();
            this.listViewNoveUplate = new System.Windows.Forms.ListView();
            this.btnBrisiUplatu = new System.Windows.Forms.Button();
            this.btnUnesiUplatu = new System.Windows.Forms.Button();
            this.txtNapomena = new System.Windows.Forms.TextBox();
            this.btnPrethodneUplate = new System.Windows.Forms.Button();
            this.listViewPrethodneUplate = new System.Windows.Forms.ListView();
            this.ckbKartica = new System.Windows.Forms.CheckBox();
            this.ckbPristupnica = new System.Windows.Forms.CheckBox();
            this.lblNapomena = new System.Windows.Forms.Label();
            this.txtIznos = new System.Windows.Forms.TextBox();
            this.lblIznos = new System.Windows.Forms.Label();
            this.dateTimePickerDatumClanarine = new System.Windows.Forms.DateTimePicker();
            this.lblDatumClanarine = new System.Windows.Forms.Label();
            this.cmbGrupa = new System.Windows.Forms.ComboBox();
            this.txtSifraGrupe = new System.Windows.Forms.TextBox();
            this.lblGrupa = new System.Windows.Forms.Label();
            this.cmbClan = new System.Windows.Forms.ComboBox();
            this.txtBrojClana = new System.Windows.Forms.TextBox();
            this.lblClan = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOdustani
            // 
            this.btnOdustani.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOdustani.Location = new System.Drawing.Point(535, 299);
            this.btnOdustani.Name = "btnOdustani";
            this.btnOdustani.Size = new System.Drawing.Size(75, 23);
            this.btnOdustani.TabIndex = 2;
            this.btnOdustani.Text = "Odustani";
            this.btnOdustani.UseVisualStyleBackColor = true;
            this.btnOdustani.Click += new System.EventHandler(this.btnOdustani_Click);
            // 
            // btnOcitajKarticu
            // 
            this.btnOcitajKarticu.Location = new System.Drawing.Point(12, 299);
            this.btnOcitajKarticu.Name = "btnOcitajKarticu";
            this.btnOcitajKarticu.Size = new System.Drawing.Size(96, 23);
            this.btnOcitajKarticu.TabIndex = 3;
            this.btnOcitajKarticu.Text = "Ocitaj karticu";
            this.btnOcitajKarticu.UseVisualStyleBackColor = true;
            this.btnOcitajKarticu.Click += new System.EventHandler(this.btnOcitajKarticu_Click);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(447, 299);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblUkupnoIznos);
            this.groupBox1.Controls.Add(this.lblUkupno);
            this.groupBox1.Controls.Add(this.listViewNoveUplate);
            this.groupBox1.Controls.Add(this.btnBrisiUplatu);
            this.groupBox1.Controls.Add(this.btnUnesiUplatu);
            this.groupBox1.Controls.Add(this.txtNapomena);
            this.groupBox1.Controls.Add(this.btnPrethodneUplate);
            this.groupBox1.Controls.Add(this.listViewPrethodneUplate);
            this.groupBox1.Controls.Add(this.ckbKartica);
            this.groupBox1.Controls.Add(this.ckbPristupnica);
            this.groupBox1.Controls.Add(this.lblNapomena);
            this.groupBox1.Controls.Add(this.txtIznos);
            this.groupBox1.Controls.Add(this.lblIznos);
            this.groupBox1.Controls.Add(this.dateTimePickerDatumClanarine);
            this.groupBox1.Controls.Add(this.lblDatumClanarine);
            this.groupBox1.Controls.Add(this.cmbGrupa);
            this.groupBox1.Controls.Add(this.txtSifraGrupe);
            this.groupBox1.Controls.Add(this.lblGrupa);
            this.groupBox1.Controls.Add(this.cmbClan);
            this.groupBox1.Controls.Add(this.txtBrojClana);
            this.groupBox1.Controls.Add(this.lblClan);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(602, 280);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lblUkupnoIznos
            // 
            this.lblUkupnoIznos.AutoSize = true;
            this.lblUkupnoIznos.Location = new System.Drawing.Point(84, 249);
            this.lblUkupnoIznos.Name = "lblUkupnoIznos";
            this.lblUkupnoIznos.Size = new System.Drawing.Size(32, 13);
            this.lblUkupnoIznos.TabIndex = 22;
            this.lblUkupnoIznos.Text = "Iznos";
            // 
            // lblUkupno
            // 
            this.lblUkupno.AutoSize = true;
            this.lblUkupno.Location = new System.Drawing.Point(20, 249);
            this.lblUkupno.Name = "lblUkupno";
            this.lblUkupno.Size = new System.Drawing.Size(45, 13);
            this.lblUkupno.TabIndex = 21;
            this.lblUkupno.Text = "Ukupno";
            // 
            // listViewNoveUplate
            // 
            this.listViewNoveUplate.Location = new System.Drawing.Point(19, 159);
            this.listViewNoveUplate.Name = "listViewNoveUplate";
            this.listViewNoveUplate.Size = new System.Drawing.Size(156, 81);
            this.listViewNoveUplate.TabIndex = 20;
            this.listViewNoveUplate.UseCompatibleStateImageBehavior = false;
            // 
            // btnBrisiUplatu
            // 
            this.btnBrisiUplatu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrisiUplatu.Location = new System.Drawing.Point(100, 130);
            this.btnBrisiUplatu.Name = "btnBrisiUplatu";
            this.btnBrisiUplatu.Size = new System.Drawing.Size(75, 23);
            this.btnBrisiUplatu.TabIndex = 19;
            this.btnBrisiUplatu.Text = "Brisi uplatu";
            this.btnBrisiUplatu.UseVisualStyleBackColor = true;
            this.btnBrisiUplatu.Click += new System.EventHandler(this.btnBrisiUplatu_Click);
            // 
            // btnUnesiUplatu
            // 
            this.btnUnesiUplatu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnUnesiUplatu.Location = new System.Drawing.Point(19, 130);
            this.btnUnesiUplatu.Name = "btnUnesiUplatu";
            this.btnUnesiUplatu.Size = new System.Drawing.Size(75, 23);
            this.btnUnesiUplatu.TabIndex = 18;
            this.btnUnesiUplatu.Text = "Unesi uplatu";
            this.btnUnesiUplatu.UseVisualStyleBackColor = true;
            this.btnUnesiUplatu.Click += new System.EventHandler(this.btnUnesiUplatu_Click);
            // 
            // txtNapomena
            // 
            this.txtNapomena.Location = new System.Drawing.Point(273, 240);
            this.txtNapomena.Name = "txtNapomena";
            this.txtNapomena.Size = new System.Drawing.Size(313, 20);
            this.txtNapomena.TabIndex = 4;
            // 
            // btnPrethodneUplate
            // 
            this.btnPrethodneUplate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPrethodneUplate.Location = new System.Drawing.Point(211, 90);
            this.btnPrethodneUplate.Name = "btnPrethodneUplate";
            this.btnPrethodneUplate.Size = new System.Drawing.Size(100, 23);
            this.btnPrethodneUplate.TabIndex = 16;
            this.btnPrethodneUplate.Text = "Prethodne uplate";
            this.btnPrethodneUplate.UseVisualStyleBackColor = true;
            this.btnPrethodneUplate.Click += new System.EventHandler(this.btnPrethodneUplate_Click);
            // 
            // listViewPrethodneUplate
            // 
            this.listViewPrethodneUplate.Location = new System.Drawing.Point(211, 119);
            this.listViewPrethodneUplate.Name = "listViewPrethodneUplate";
            this.listViewPrethodneUplate.Size = new System.Drawing.Size(375, 105);
            this.listViewPrethodneUplate.TabIndex = 15;
            this.listViewPrethodneUplate.UseCompatibleStateImageBehavior = false;
            // 
            // ckbKartica
            // 
            this.ckbKartica.AutoSize = true;
            this.ckbKartica.Location = new System.Drawing.Point(406, 52);
            this.ckbKartica.Name = "ckbKartica";
            this.ckbKartica.Size = new System.Drawing.Size(59, 17);
            this.ckbKartica.TabIndex = 14;
            this.ckbKartica.Text = "Kartica";
            this.ckbKartica.UseVisualStyleBackColor = true;
            // 
            // ckbPristupnica
            // 
            this.ckbPristupnica.AutoSize = true;
            this.ckbPristupnica.Location = new System.Drawing.Point(406, 20);
            this.ckbPristupnica.Name = "ckbPristupnica";
            this.ckbPristupnica.Size = new System.Drawing.Size(78, 17);
            this.ckbPristupnica.TabIndex = 13;
            this.ckbPristupnica.TabStop = false;
            this.ckbPristupnica.Text = "Pristupnica";
            this.ckbPristupnica.UseVisualStyleBackColor = true;
            // 
            // lblNapomena
            // 
            this.lblNapomena.AutoSize = true;
            this.lblNapomena.Location = new System.Drawing.Point(208, 243);
            this.lblNapomena.Name = "lblNapomena";
            this.lblNapomena.Size = new System.Drawing.Size(59, 13);
            this.lblNapomena.TabIndex = 9;
            this.lblNapomena.Text = "Napomena";
            // 
            // txtIznos
            // 
            this.txtIznos.Location = new System.Drawing.Point(19, 96);
            this.txtIznos.Name = "txtIznos";
            this.txtIznos.Size = new System.Drawing.Size(50, 20);
            this.txtIznos.TabIndex = 2;
            this.txtIznos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIznos_KeyDown);
            // 
            // lblIznos
            // 
            this.lblIznos.AutoSize = true;
            this.lblIznos.Location = new System.Drawing.Point(20, 80);
            this.lblIznos.Name = "lblIznos";
            this.lblIznos.Size = new System.Drawing.Size(32, 13);
            this.lblIznos.TabIndex = 7;
            this.lblIznos.Text = "Iznos";
            // 
            // dateTimePickerDatumClanarine
            // 
            this.dateTimePickerDatumClanarine.CustomFormat = "MMMM yyy";
            this.dateTimePickerDatumClanarine.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerDatumClanarine.Location = new System.Drawing.Point(75, 96);
            this.dateTimePickerDatumClanarine.Name = "dateTimePickerDatumClanarine";
            this.dateTimePickerDatumClanarine.ShowUpDown = true;
            this.dateTimePickerDatumClanarine.Size = new System.Drawing.Size(106, 20);
            this.dateTimePickerDatumClanarine.TabIndex = 3;
            this.dateTimePickerDatumClanarine.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimePickerDatumClanarine_KeyDown);
            // 
            // lblDatumClanarine
            // 
            this.lblDatumClanarine.AutoSize = true;
            this.lblDatumClanarine.Location = new System.Drawing.Point(75, 80);
            this.lblDatumClanarine.Name = "lblDatumClanarine";
            this.lblDatumClanarine.Size = new System.Drawing.Size(39, 13);
            this.lblDatumClanarine.TabIndex = 4;
            this.lblDatumClanarine.Text = "Mesec";
            // 
            // cmbGrupa
            // 
            this.cmbGrupa.FormattingEnabled = true;
            this.cmbGrupa.Location = new System.Drawing.Point(118, 48);
            this.cmbGrupa.Name = "cmbGrupa";
            this.cmbGrupa.Size = new System.Drawing.Size(232, 21);
            this.cmbGrupa.TabIndex = 7;
            this.cmbGrupa.SelectionChangeCommitted += new System.EventHandler(this.cmbGrupa_SelectionChangeCommitted);
            // 
            // txtSifraGrupe
            // 
            this.txtSifraGrupe.Location = new System.Drawing.Point(62, 48);
            this.txtSifraGrupe.Name = "txtSifraGrupe";
            this.txtSifraGrupe.Size = new System.Drawing.Size(48, 20);
            this.txtSifraGrupe.TabIndex = 1;
            this.txtSifraGrupe.TextChanged += new System.EventHandler(this.txtSifraGrupe_TextChanged);
            this.txtSifraGrupe.Enter += new System.EventHandler(this.txtSifraGrupe_Enter);
            // 
            // lblGrupa
            // 
            this.lblGrupa.AutoSize = true;
            this.lblGrupa.Location = new System.Drawing.Point(16, 48);
            this.lblGrupa.Name = "lblGrupa";
            this.lblGrupa.Size = new System.Drawing.Size(36, 13);
            this.lblGrupa.TabIndex = 1;
            this.lblGrupa.Text = "Grupa";
            // 
            // cmbClan
            // 
            this.cmbClan.FormattingEnabled = true;
            this.cmbClan.Location = new System.Drawing.Point(118, 16);
            this.cmbClan.Name = "cmbClan";
            this.cmbClan.Size = new System.Drawing.Size(232, 21);
            this.cmbClan.TabIndex = 6;
            this.cmbClan.SelectionChangeCommitted += new System.EventHandler(this.cmbClan_SelectionChangeCommitted);
            // 
            // txtBrojClana
            // 
            this.txtBrojClana.Location = new System.Drawing.Point(62, 16);
            this.txtBrojClana.Name = "txtBrojClana";
            this.txtBrojClana.Size = new System.Drawing.Size(48, 20);
            this.txtBrojClana.TabIndex = 0;
            this.txtBrojClana.TextChanged += new System.EventHandler(this.txtBrojClana_TextChanged);
            // 
            // lblClan
            // 
            this.lblClan.AutoSize = true;
            this.lblClan.Location = new System.Drawing.Point(16, 16);
            this.lblClan.Name = "lblClan";
            this.lblClan.Size = new System.Drawing.Size(28, 13);
            this.lblClan.TabIndex = 0;
            this.lblClan.Text = "Clan";
            // 
            // UplataClanarineDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOdustani;
            this.ClientSize = new System.Drawing.Size(622, 335);
            this.Controls.Add(this.btnOcitajKarticu);
            this.Controls.Add(this.btnOdustani);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UplataClanarineDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unos Clanarine";
            this.Shown += new System.EventHandler(this.UplataClanarineDialog_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnOdustani;
        private System.Windows.Forms.Label lblClan;
        private System.Windows.Forms.TextBox txtBrojClana;
        private System.Windows.Forms.ComboBox cmbClan;
        private System.Windows.Forms.Label lblGrupa;
        private System.Windows.Forms.TextBox txtSifraGrupe;
        private System.Windows.Forms.ComboBox cmbGrupa;
        private System.Windows.Forms.Label lblDatumClanarine;
        private System.Windows.Forms.DateTimePicker dateTimePickerDatumClanarine;
        private System.Windows.Forms.Label lblIznos;
        private System.Windows.Forms.TextBox txtIznos;
        private System.Windows.Forms.Label lblNapomena;
        private System.Windows.Forms.CheckBox ckbPristupnica;
        private System.Windows.Forms.Button btnOcitajKarticu;
        private System.Windows.Forms.CheckBox ckbKartica;
        private System.Windows.Forms.ListView listViewPrethodneUplate;
        private System.Windows.Forms.Button btnPrethodneUplate;
        private System.Windows.Forms.TextBox txtNapomena;
        private System.Windows.Forms.Button btnBrisiUplatu;
        private System.Windows.Forms.Button btnUnesiUplatu;
        private System.Windows.Forms.ListView listViewNoveUplate;
        private System.Windows.Forms.Label lblUkupnoIznos;
        private System.Windows.Forms.Label lblUkupno;
    }
}