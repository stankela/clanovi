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
            this.lblVelicinaSlova = new System.Windows.Forms.Label();
            this.txtVelicinaSlova = new System.Windows.Forms.TextBox();
            this.ckbPrikaziBoje = new System.Windows.Forms.CheckBox();
            this.ckbPrikaziImeClana = new System.Windows.Forms.CheckBox();
            this.ckbPrikaziDisplejPrekoCelogEkrana = new System.Windows.Forms.CheckBox();
            this.lblSirinaDispleja = new System.Windows.Forms.Label();
            this.lblVisinaDispleja = new System.Windows.Forms.Label();
            this.txtSirinaDispleja = new System.Windows.Forms.TextBox();
            this.txtVisinaDispleja = new System.Windows.Forms.TextBox();
            this.lblPoslednjiMesecZaGodisnjeClanarine = new System.Windows.Forms.Label();
            this.cmbPoslednjiMesecZaGodisnjeClanarine = new System.Windows.Forms.ComboBox();
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
            this.cmbCOMPortReader.Location = new System.Drawing.Point(21, 182);
            this.cmbCOMPortReader.Name = "cmbCOMPortReader";
            this.cmbCOMPortReader.Size = new System.Drawing.Size(132, 21);
            this.cmbCOMPortReader.TabIndex = 0;
            // 
            // lblCOMPortReader
            // 
            this.lblCOMPortReader.AutoSize = true;
            this.lblCOMPortReader.Location = new System.Drawing.Point(18, 166);
            this.lblCOMPortReader.Name = "lblCOMPortReader";
            this.lblCOMPortReader.Size = new System.Drawing.Size(127, 13);
            this.lblCOMPortReader.TabIndex = 1;
            this.lblCOMPortReader.Text = "COM port za citac kartica";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(147, 395);
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
            this.btnCancel.Location = new System.Drawing.Point(237, 395);
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
            this.cmbCOMPortWriter.Location = new System.Drawing.Point(180, 182);
            this.cmbCOMPortWriter.Name = "cmbCOMPortWriter";
            this.cmbCOMPortWriter.Size = new System.Drawing.Size(132, 21);
            this.cmbCOMPortWriter.TabIndex = 4;
            // 
            // lblCOMPortWriter
            // 
            this.lblCOMPortWriter.AutoSize = true;
            this.lblCOMPortWriter.Location = new System.Drawing.Point(177, 166);
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
            // lblVelicinaSlova
            // 
            this.lblVelicinaSlova.AutoSize = true;
            this.lblVelicinaSlova.Location = new System.Drawing.Point(18, 125);
            this.lblVelicinaSlova.Name = "lblVelicinaSlova";
            this.lblVelicinaSlova.Size = new System.Drawing.Size(125, 13);
            this.lblVelicinaSlova.TabIndex = 9;
            this.lblVelicinaSlova.Text = "Velicina slova na displeju";
            // 
            // txtVelicinaSlova
            // 
            this.txtVelicinaSlova.Location = new System.Drawing.Point(189, 122);
            this.txtVelicinaSlova.Name = "txtVelicinaSlova";
            this.txtVelicinaSlova.Size = new System.Drawing.Size(45, 20);
            this.txtVelicinaSlova.TabIndex = 10;
            // 
            // ckbPrikaziBoje
            // 
            this.ckbPrikaziBoje.AutoSize = true;
            this.ckbPrikaziBoje.Location = new System.Drawing.Point(21, 229);
            this.ckbPrikaziBoje.Name = "ckbPrikaziBoje";
            this.ckbPrikaziBoje.Size = new System.Drawing.Size(237, 17);
            this.ckbPrikaziBoje.TabIndex = 11;
            this.ckbPrikaziBoje.Text = "Prikazi zeleno/crveno kod ocitavanja kartica";
            this.ckbPrikaziBoje.UseVisualStyleBackColor = true;
            // 
            // ckbPrikaziImeClana
            // 
            this.ckbPrikaziImeClana.AutoSize = true;
            this.ckbPrikaziImeClana.Location = new System.Drawing.Point(21, 262);
            this.ckbPrikaziImeClana.Name = "ckbPrikaziImeClana";
            this.ckbPrikaziImeClana.Size = new System.Drawing.Size(105, 17);
            this.ckbPrikaziImeClana.TabIndex = 12;
            this.ckbPrikaziImeClana.Text = "Prikazi ime clana";
            this.ckbPrikaziImeClana.UseVisualStyleBackColor = true;
            // 
            // ckbPrikaziDisplejPrekoCelogEkrana
            // 
            this.ckbPrikaziDisplejPrekoCelogEkrana.AutoSize = true;
            this.ckbPrikaziDisplejPrekoCelogEkrana.Location = new System.Drawing.Point(21, 301);
            this.ckbPrikaziDisplejPrekoCelogEkrana.Name = "ckbPrikaziDisplejPrekoCelogEkrana";
            this.ckbPrikaziDisplejPrekoCelogEkrana.Size = new System.Drawing.Size(184, 17);
            this.ckbPrikaziDisplejPrekoCelogEkrana.TabIndex = 13;
            this.ckbPrikaziDisplejPrekoCelogEkrana.Text = "Prikazi displej preko celog ekrana";
            this.ckbPrikaziDisplejPrekoCelogEkrana.UseVisualStyleBackColor = true;
            this.ckbPrikaziDisplejPrekoCelogEkrana.CheckedChanged += new System.EventHandler(this.ckbPrikaziDisplejPrekoCelogEkrana_CheckedChanged);
            // 
            // lblSirinaDispleja
            // 
            this.lblSirinaDispleja.AutoSize = true;
            this.lblSirinaDispleja.Location = new System.Drawing.Point(55, 325);
            this.lblSirinaDispleja.Name = "lblSirinaDispleja";
            this.lblSirinaDispleja.Size = new System.Drawing.Size(71, 13);
            this.lblSirinaDispleja.TabIndex = 14;
            this.lblSirinaDispleja.Text = "Sirina displeja";
            // 
            // lblVisinaDispleja
            // 
            this.lblVisinaDispleja.AutoSize = true;
            this.lblVisinaDispleja.Location = new System.Drawing.Point(55, 351);
            this.lblVisinaDispleja.Name = "lblVisinaDispleja";
            this.lblVisinaDispleja.Size = new System.Drawing.Size(73, 13);
            this.lblVisinaDispleja.TabIndex = 15;
            this.lblVisinaDispleja.Text = "Visina displeja";
            // 
            // txtSirinaDispleja
            // 
            this.txtSirinaDispleja.Location = new System.Drawing.Point(141, 322);
            this.txtSirinaDispleja.Name = "txtSirinaDispleja";
            this.txtSirinaDispleja.Size = new System.Drawing.Size(42, 20);
            this.txtSirinaDispleja.TabIndex = 16;
            // 
            // txtVisinaDispleja
            // 
            this.txtVisinaDispleja.Location = new System.Drawing.Point(141, 348);
            this.txtVisinaDispleja.Name = "txtVisinaDispleja";
            this.txtVisinaDispleja.Size = new System.Drawing.Size(42, 20);
            this.txtVisinaDispleja.TabIndex = 17;
            // 
            // lblPoslednjiMesecZaGodisnjeClanarine
            // 
            this.lblPoslednjiMesecZaGodisnjeClanarine.AutoSize = true;
            this.lblPoslednjiMesecZaGodisnjeClanarine.Location = new System.Drawing.Point(18, 66);
            this.lblPoslednjiMesecZaGodisnjeClanarine.Name = "lblPoslednjiMesecZaGodisnjeClanarine";
            this.lblPoslednjiMesecZaGodisnjeClanarine.Size = new System.Drawing.Size(233, 13);
            this.lblPoslednjiMesecZaGodisnjeClanarine.TabIndex = 18;
            this.lblPoslednjiMesecZaGodisnjeClanarine.Text = "Poslednji dozvoljen mesec za godisnje clanarine";
            // 
            // cmbPoslednjiMesecZaGodisnjeClanarine
            // 
            this.cmbPoslednjiMesecZaGodisnjeClanarine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPoslednjiMesecZaGodisnjeClanarine.FormattingEnabled = true;
            this.cmbPoslednjiMesecZaGodisnjeClanarine.Location = new System.Drawing.Point(21, 82);
            this.cmbPoslednjiMesecZaGodisnjeClanarine.Name = "cmbPoslednjiMesecZaGodisnjeClanarine";
            this.cmbPoslednjiMesecZaGodisnjeClanarine.Size = new System.Drawing.Size(121, 21);
            this.cmbPoslednjiMesecZaGodisnjeClanarine.TabIndex = 19;
            // 
            // CitacKarticaDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(339, 441);
            this.Controls.Add(this.cmbPoslednjiMesecZaGodisnjeClanarine);
            this.Controls.Add(this.lblPoslednjiMesecZaGodisnjeClanarine);
            this.Controls.Add(this.txtVisinaDispleja);
            this.Controls.Add(this.txtSirinaDispleja);
            this.Controls.Add(this.lblVisinaDispleja);
            this.Controls.Add(this.lblSirinaDispleja);
            this.Controls.Add(this.ckbPrikaziDisplejPrekoCelogEkrana);
            this.Controls.Add(this.ckbPrikaziImeClana);
            this.Controls.Add(this.ckbPrikaziBoje);
            this.Controls.Add(this.txtVelicinaSlova);
            this.Controls.Add(this.lblVelicinaSlova);
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
        private System.Windows.Forms.Label lblVelicinaSlova;
        private System.Windows.Forms.TextBox txtVelicinaSlova;
        private System.Windows.Forms.CheckBox ckbPrikaziBoje;
        private System.Windows.Forms.CheckBox ckbPrikaziImeClana;
        private System.Windows.Forms.CheckBox ckbPrikaziDisplejPrekoCelogEkrana;
        private System.Windows.Forms.Label lblSirinaDispleja;
        private System.Windows.Forms.Label lblVisinaDispleja;
        private System.Windows.Forms.TextBox txtSirinaDispleja;
        private System.Windows.Forms.TextBox txtVisinaDispleja;
        private System.Windows.Forms.Label lblPoslednjiMesecZaGodisnjeClanarine;
        private System.Windows.Forms.ComboBox cmbPoslednjiMesecZaGodisnjeClanarine;
    }
}