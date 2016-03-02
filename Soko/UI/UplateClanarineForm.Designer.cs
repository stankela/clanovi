namespace Soko.UI
{
    partial class UplateClanarineForm
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
            this.dtpDo = new System.Windows.Forms.DateTimePicker();
            this.lblDo = new System.Windows.Forms.Label();
            this.dtpOd = new System.Windows.Forms.DateTimePicker();
            this.lblOd = new System.Windows.Forms.Label();
            this.rbtInterval = new System.Windows.Forms.RadioButton();
            this.cmbClan = new System.Windows.Forms.ComboBox();
            this.rbtClan = new System.Windows.Forms.RadioButton();
            this.btnStampaj = new System.Windows.Forms.Button();
            this.btnZatvori = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnPromeni = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpDo);
            this.groupBox1.Controls.Add(this.lblDo);
            this.groupBox1.Controls.Add(this.dtpOd);
            this.groupBox1.Controls.Add(this.lblOd);
            this.groupBox1.Controls.Add(this.rbtInterval);
            this.groupBox1.Controls.Add(this.cmbClan);
            this.groupBox1.Controls.Add(this.rbtClan);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 112);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // dtpDo
            // 
            this.dtpDo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDo.Location = new System.Drawing.Point(168, 80);
            this.dtpDo.Name = "dtpDo";
            this.dtpDo.Size = new System.Drawing.Size(104, 20);
            this.dtpDo.TabIndex = 8;
            this.dtpDo.CloseUp += new System.EventHandler(this.dtpDo_CloseUp);
            // 
            // lblDo
            // 
            this.lblDo.AutoSize = true;
            this.lblDo.Location = new System.Drawing.Point(141, 84);
            this.lblDo.Name = "lblDo";
            this.lblDo.Size = new System.Drawing.Size(21, 13);
            this.lblDo.TabIndex = 10;
            this.lblDo.Text = "Do";
            // 
            // dtpOd
            // 
            this.dtpOd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpOd.Location = new System.Drawing.Point(168, 48);
            this.dtpOd.Name = "dtpOd";
            this.dtpOd.Size = new System.Drawing.Size(104, 20);
            this.dtpOd.TabIndex = 7;
            this.dtpOd.CloseUp += new System.EventHandler(this.dtpOd_CloseUp);
            // 
            // lblOd
            // 
            this.lblOd.AutoSize = true;
            this.lblOd.Location = new System.Drawing.Point(141, 52);
            this.lblOd.Name = "lblOd";
            this.lblOd.Size = new System.Drawing.Size(21, 13);
            this.lblOd.TabIndex = 9;
            this.lblOd.Text = "Od";
            // 
            // rbtInterval
            // 
            this.rbtInterval.AutoSize = true;
            this.rbtInterval.Location = new System.Drawing.Point(16, 50);
            this.rbtInterval.Name = "rbtInterval";
            this.rbtInterval.Size = new System.Drawing.Size(106, 17);
            this.rbtInterval.TabIndex = 5;
            this.rbtInterval.TabStop = true;
            this.rbtInterval.Text = "Vremenski period";
            this.rbtInterval.UseVisualStyleBackColor = true;
            // 
            // cmbClan
            // 
            this.cmbClan.FormattingEnabled = true;
            this.cmbClan.Location = new System.Drawing.Point(88, 16);
            this.cmbClan.Name = "cmbClan";
            this.cmbClan.Size = new System.Drawing.Size(240, 21);
            this.cmbClan.TabIndex = 6;
            // 
            // rbtClan
            // 
            this.rbtClan.AutoSize = true;
            this.rbtClan.Location = new System.Drawing.Point(16, 16);
            this.rbtClan.Name = "rbtClan";
            this.rbtClan.Size = new System.Drawing.Size(46, 17);
            this.rbtClan.TabIndex = 4;
            this.rbtClan.TabStop = true;
            this.rbtClan.Text = "Clan";
            this.rbtClan.UseVisualStyleBackColor = true;
            // 
            // btnStampaj
            // 
            this.btnStampaj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStampaj.Location = new System.Drawing.Point(8, 328);
            this.btnStampaj.Name = "btnStampaj";
            this.btnStampaj.Size = new System.Drawing.Size(104, 23);
            this.btnStampaj.TabIndex = 2;
            this.btnStampaj.Text = "Stampaj potvrdu";
            this.btnStampaj.UseVisualStyleBackColor = true;
            this.btnStampaj.Click += new System.EventHandler(this.btnStampaj_Click);
            // 
            // btnZatvori
            // 
            this.btnZatvori.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZatvori.Location = new System.Drawing.Point(120, 328);
            this.btnZatvori.Name = "btnZatvori";
            this.btnZatvori.Size = new System.Drawing.Size(75, 23);
            this.btnZatvori.TabIndex = 5;
            this.btnZatvori.Text = "Zatvori";
            this.btnZatvori.UseVisualStyleBackColor = true;
            this.btnZatvori.Click += new System.EventHandler(this.btnZatvori_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(8, 128);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(650, 192);
            this.dataGridView1.TabIndex = 6;
            // 
            // btnPromeni
            // 
            this.btnPromeni.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPromeni.Location = new System.Drawing.Point(579, 328);
            this.btnPromeni.Name = "btnPromeni";
            this.btnPromeni.Size = new System.Drawing.Size(75, 23);
            this.btnPromeni.TabIndex = 7;
            this.btnPromeni.Text = "Promeni";
            this.btnPromeni.UseVisualStyleBackColor = true;
            this.btnPromeni.Click += new System.EventHandler(this.btnPromeni_Click);
            // 
            // UplateClanarineForm
            // 
            this.ClientSize = new System.Drawing.Size(666, 360);
            this.Controls.Add(this.btnPromeni);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnZatvori);
            this.Controls.Add(this.btnStampaj);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UplateClanarineForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Shown += new System.EventHandler(this.PotvrdaUplateForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtClan;
        private System.Windows.Forms.ComboBox cmbClan;
        private System.Windows.Forms.RadioButton rbtInterval;
        private System.Windows.Forms.Label lblOd;
        private System.Windows.Forms.DateTimePicker dtpOd;
        private System.Windows.Forms.Label lblDo;
        private System.Windows.Forms.DateTimePicker dtpDo;
        private System.Windows.Forms.Button btnStampaj;
        private System.Windows.Forms.Button btnZatvori;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnPromeni;
    }
}