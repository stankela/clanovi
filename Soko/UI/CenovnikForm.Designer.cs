namespace Soko.UI
{
    partial class CenovnikForm
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
            this.cmbGrupa = new System.Windows.Forms.ComboBox();
            this.rbtGrupa = new System.Windows.Forms.RadioButton();
            this.rbtSveGrupe = new System.Windows.Forms.RadioButton();
            this.btnNovaCena = new System.Windows.Forms.Button();
            this.btnBrisi = new System.Windows.Forms.Button();
            this.btnZatvori = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbGrupa);
            this.groupBox1.Controls.Add(this.rbtGrupa);
            this.groupBox1.Controls.Add(this.rbtSveGrupe);
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 88);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cmbGrupa
            // 
            this.cmbGrupa.FormattingEnabled = true;
            this.cmbGrupa.Location = new System.Drawing.Point(96, 48);
            this.cmbGrupa.Name = "cmbGrupa";
            this.cmbGrupa.Size = new System.Drawing.Size(248, 21);
            this.cmbGrupa.TabIndex = 2;
            // 
            // rbtGrupa
            // 
            this.rbtGrupa.AutoSize = true;
            this.rbtGrupa.Location = new System.Drawing.Point(16, 48);
            this.rbtGrupa.Name = "rbtGrupa";
            this.rbtGrupa.Size = new System.Drawing.Size(68, 17);
            this.rbtGrupa.TabIndex = 1;
            this.rbtGrupa.TabStop = true;
            this.rbtGrupa.Text = "Za grupu";
            this.rbtGrupa.UseVisualStyleBackColor = true;
            // 
            // rbtSveGrupe
            // 
            this.rbtSveGrupe.AutoSize = true;
            this.rbtSveGrupe.Location = new System.Drawing.Point(16, 16);
            this.rbtSveGrupe.Name = "rbtSveGrupe";
            this.rbtSveGrupe.Size = new System.Drawing.Size(121, 17);
            this.rbtSveGrupe.TabIndex = 0;
            this.rbtSveGrupe.TabStop = true;
            this.rbtSveGrupe.Text = "Vazeci za sve grupe";
            this.rbtSveGrupe.UseVisualStyleBackColor = true;
            // 
            // btnNovaCena
            // 
            this.btnNovaCena.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNovaCena.Location = new System.Drawing.Point(8, 328);
            this.btnNovaCena.Name = "btnNovaCena";
            this.btnNovaCena.Size = new System.Drawing.Size(80, 23);
            this.btnNovaCena.TabIndex = 2;
            this.btnNovaCena.Text = "Nova cena...";
            this.btnNovaCena.UseVisualStyleBackColor = true;
            this.btnNovaCena.Click += new System.EventHandler(this.btnNovaCena_Click);
            // 
            // btnBrisi
            // 
            this.btnBrisi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBrisi.Location = new System.Drawing.Point(104, 328);
            this.btnBrisi.Name = "btnBrisi";
            this.btnBrisi.Size = new System.Drawing.Size(75, 23);
            this.btnBrisi.TabIndex = 4;
            this.btnBrisi.Text = "Brisi";
            this.btnBrisi.UseVisualStyleBackColor = true;
            this.btnBrisi.Click += new System.EventHandler(this.btnBrisi_Click);
            // 
            // btnZatvori
            // 
            this.btnZatvori.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZatvori.Location = new System.Drawing.Point(192, 328);
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
            this.dataGridView1.Location = new System.Drawing.Point(8, 104);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(426, 216);
            this.dataGridView1.TabIndex = 6;
            // 
            // CenovnikForm
            // 
            this.ClientSize = new System.Drawing.Size(444, 360);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnZatvori);
            this.Controls.Add(this.btnBrisi);
            this.Controls.Add(this.btnNovaCena);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CenovnikForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtSveGrupe;
        private System.Windows.Forms.RadioButton rbtGrupa;
        private System.Windows.Forms.ComboBox cmbGrupa;
        private System.Windows.Forms.Button btnNovaCena;
        private System.Windows.Forms.Button btnBrisi;
        private System.Windows.Forms.Button btnZatvori;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}