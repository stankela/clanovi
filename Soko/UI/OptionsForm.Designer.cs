namespace Soko.UI
{
    partial class OptionsForm
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
            this.rbtUvekTraziLozinku = new System.Windows.Forms.RadioButton();
            this.rbtTraziLozinkuNakon = new System.Windows.Forms.RadioButton();
            this.txtBrojMinuta = new System.Windows.Forms.TextBox();
            this.lblMinuta = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbVelicina = new System.Windows.Forms.ComboBox();
            this.lblVelicina = new System.Windows.Forms.Label();
            this.cmbStampacIzvestaj = new System.Windows.Forms.ComboBox();
            this.lblStampacIzvestaj = new System.Windows.Forms.Label();
            this.cmbStampacPotvrda = new System.Windows.Forms.ComboBox();
            this.lblStampacPotvrda = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rbtUvekTraziLozinku
            // 
            this.rbtUvekTraziLozinku.AutoSize = true;
            this.rbtUvekTraziLozinku.Location = new System.Drawing.Point(22, 272);
            this.rbtUvekTraziLozinku.Name = "rbtUvekTraziLozinku";
            this.rbtUvekTraziLozinku.Size = new System.Drawing.Size(109, 17);
            this.rbtUvekTraziLozinku.TabIndex = 0;
            this.rbtUvekTraziLozinku.TabStop = true;
            this.rbtUvekTraziLozinku.Text = "Uvek trazi lozinku";
            this.rbtUvekTraziLozinku.UseVisualStyleBackColor = true;
            this.rbtUvekTraziLozinku.CheckedChanged += new System.EventHandler(this.rbtUvekTraziLozinku_CheckedChanged);
            // 
            // rbtTraziLozinkuNakon
            // 
            this.rbtTraziLozinkuNakon.AutoSize = true;
            this.rbtTraziLozinkuNakon.Location = new System.Drawing.Point(22, 307);
            this.rbtTraziLozinkuNakon.Name = "rbtTraziLozinkuNakon";
            this.rbtTraziLozinkuNakon.Size = new System.Drawing.Size(117, 17);
            this.rbtTraziLozinkuNakon.TabIndex = 1;
            this.rbtTraziLozinkuNakon.TabStop = true;
            this.rbtTraziLozinkuNakon.Text = "Trazi lozinku nakon";
            this.rbtTraziLozinkuNakon.UseVisualStyleBackColor = true;
            this.rbtTraziLozinkuNakon.CheckedChanged += new System.EventHandler(this.rbtTraziLozinkuNakon_CheckedChanged);
            // 
            // txtBrojMinuta
            // 
            this.txtBrojMinuta.Location = new System.Drawing.Point(145, 306);
            this.txtBrojMinuta.Name = "txtBrojMinuta";
            this.txtBrojMinuta.Size = new System.Drawing.Size(45, 20);
            this.txtBrojMinuta.TabIndex = 2;
            // 
            // lblMinuta
            // 
            this.lblMinuta.AutoSize = true;
            this.lblMinuta.Location = new System.Drawing.Point(205, 309);
            this.lblMinuta.Name = "lblMinuta";
            this.lblMinuta.Size = new System.Drawing.Size(38, 13);
            this.lblMinuta.TabIndex = 3;
            this.lblMinuta.Text = "minuta";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(87, 366);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(178, 366);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cmbVelicina
            // 
            this.cmbVelicina.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVelicina.FormattingEnabled = true;
            this.cmbVelicina.Location = new System.Drawing.Point(116, 36);
            this.cmbVelicina.Name = "cmbVelicina";
            this.cmbVelicina.Size = new System.Drawing.Size(64, 21);
            this.cmbVelicina.TabIndex = 7;
            this.cmbVelicina.SelectionChangeCommitted += new System.EventHandler(this.cmbVelicina_SelectionChangeCommitted);
            // 
            // lblVelicina
            // 
            this.lblVelicina.AutoSize = true;
            this.lblVelicina.Location = new System.Drawing.Point(28, 36);
            this.lblVelicina.Name = "lblVelicina";
            this.lblVelicina.Size = new System.Drawing.Size(72, 13);
            this.lblVelicina.TabIndex = 6;
            this.lblVelicina.Text = "Velicina slova";
            // 
            // cmbStampacIzvestaj
            // 
            this.cmbStampacIzvestaj.FormattingEnabled = true;
            this.cmbStampacIzvestaj.Location = new System.Drawing.Point(29, 160);
            this.cmbStampacIzvestaj.Name = "cmbStampacIzvestaj";
            this.cmbStampacIzvestaj.Size = new System.Drawing.Size(223, 21);
            this.cmbStampacIzvestaj.TabIndex = 11;
            // 
            // lblStampacIzvestaj
            // 
            this.lblStampacIzvestaj.AutoSize = true;
            this.lblStampacIzvestaj.Location = new System.Drawing.Point(26, 144);
            this.lblStampacIzvestaj.Name = "lblStampacIzvestaj";
            this.lblStampacIzvestaj.Size = new System.Drawing.Size(107, 13);
            this.lblStampacIzvestaj.TabIndex = 10;
            this.lblStampacIzvestaj.Text = "Stampac za izvestaje";
            // 
            // cmbStampacPotvrda
            // 
            this.cmbStampacPotvrda.FormattingEnabled = true;
            this.cmbStampacPotvrda.Location = new System.Drawing.Point(29, 109);
            this.cmbStampacPotvrda.Name = "cmbStampacPotvrda";
            this.cmbStampacPotvrda.Size = new System.Drawing.Size(223, 21);
            this.cmbStampacPotvrda.TabIndex = 9;
            // 
            // lblStampacPotvrda
            // 
            this.lblStampacPotvrda.AutoSize = true;
            this.lblStampacPotvrda.Location = new System.Drawing.Point(26, 93);
            this.lblStampacPotvrda.Name = "lblStampacPotvrda";
            this.lblStampacPotvrda.Size = new System.Drawing.Size(102, 13);
            this.lblStampacPotvrda.TabIndex = 8;
            this.lblStampacPotvrda.Text = "Stampac za potvrde";
            // 
            // OptionsForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(278, 410);
            this.Controls.Add(this.cmbStampacIzvestaj);
            this.Controls.Add(this.lblStampacIzvestaj);
            this.Controls.Add(this.cmbStampacPotvrda);
            this.Controls.Add(this.lblStampacPotvrda);
            this.Controls.Add(this.cmbVelicina);
            this.Controls.Add(this.lblVelicina);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblMinuta);
            this.Controls.Add(this.txtBrojMinuta);
            this.Controls.Add(this.rbtTraziLozinkuNakon);
            this.Controls.Add(this.rbtUvekTraziLozinku);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Opcije";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtUvekTraziLozinku;
        private System.Windows.Forms.RadioButton rbtTraziLozinkuNakon;
        private System.Windows.Forms.TextBox txtBrojMinuta;
        private System.Windows.Forms.Label lblMinuta;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbVelicina;
        private System.Windows.Forms.Label lblVelicina;
        private System.Windows.Forms.ComboBox cmbStampacIzvestaj;
        private System.Windows.Forms.Label lblStampacIzvestaj;
        private System.Windows.Forms.ComboBox cmbStampacPotvrda;
        private System.Windows.Forms.Label lblStampacPotvrda;

    }
}