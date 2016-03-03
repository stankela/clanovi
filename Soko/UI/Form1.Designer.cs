namespace Soko.UI
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnDatoteka = new System.Windows.Forms.ToolStripMenuItem();
            this.mnKraj = new System.Windows.Forms.ToolStripMenuItem();
            this.mnClanovi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnUplataClanarine = new System.Windows.Forms.ToolStripMenuItem();
            this.mnClanoviClanovi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPretrazivanjeClanova = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPravljenjeKartice = new System.Windows.Forms.ToolStripMenuItem();
            this.mnCenovnik = new System.Windows.Forms.ToolStripMenuItem();
            this.mnGrupe = new System.Windows.Forms.ToolStripMenuItem();
            this.mnKategorije = new System.Windows.Forms.ToolStripMenuItem();
            this.mnMesta = new System.Windows.Forms.ToolStripMenuItem();
            this.mnInstitucije = new System.Windows.Forms.ToolStripMenuItem();
            this.mnUplate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnDuplikatiClanova = new System.Windows.Forms.ToolStripMenuItem();
            this.mnIzvestaji = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiDnevniKategorije = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiDnevniGrupe = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiDnevniClanovi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiPeriodicni = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiPeriodicniClanovi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiMesecni = new System.Windows.Forms.ToolStripMenuItem();
            this.mnIzvestajiCenovnik = new System.Windows.Forms.ToolStripMenuItem();
            this.mnUplateClanova = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAktivniClanovi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnAktivniClanoviGrupe = new System.Windows.Forms.ToolStripMenuItem();
            this.mnEvidencijaPrisustvaNaTreningu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPodesavanja = new System.Windows.Forms.ToolStripMenuItem();
            this.mnFont = new System.Windows.Forms.ToolStripMenuItem();
            this.mnStampaci = new System.Windows.Forms.ToolStripMenuItem();
            this.mnCitacKartica = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDatoteka,
            this.mnClanovi,
            this.mnIzvestaji,
            this.mnPodesavanja});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(466, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnDatoteka
            // 
            this.mnDatoteka.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnKraj});
            this.mnDatoteka.Name = "mnDatoteka";
            this.mnDatoteka.Size = new System.Drawing.Size(66, 20);
            this.mnDatoteka.Text = "Datoteka";
            // 
            // mnKraj
            // 
            this.mnKraj.Name = "mnKraj";
            this.mnKraj.Size = new System.Drawing.Size(94, 22);
            this.mnKraj.Text = "Kraj";
            this.mnKraj.Click += new System.EventHandler(this.mnKraj_Click);
            // 
            // mnClanovi
            // 
            this.mnClanovi.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnUplataClanarine,
            this.mnClanoviClanovi,
            this.mnPretrazivanjeClanova,
            this.mnPravljenjeKartice,
            this.mnCenovnik,
            this.mnGrupe,
            this.mnKategorije,
            this.mnMesta,
            this.mnInstitucije,
            this.mnUplate,
            this.mnDuplikatiClanova});
            this.mnClanovi.Name = "mnClanovi";
            this.mnClanovi.Size = new System.Drawing.Size(59, 20);
            this.mnClanovi.Text = "Clanovi";
            // 
            // mnUplataClanarine
            // 
            this.mnUplataClanarine.Name = "mnUplataClanarine";
            this.mnUplataClanarine.Size = new System.Drawing.Size(185, 22);
            this.mnUplataClanarine.Text = "Uplata clanarine";
            this.mnUplataClanarine.Click += new System.EventHandler(this.mnUplataClanarine_Click);
            // 
            // mnClanoviClanovi
            // 
            this.mnClanoviClanovi.Name = "mnClanoviClanovi";
            this.mnClanoviClanovi.Size = new System.Drawing.Size(185, 22);
            this.mnClanoviClanovi.Text = "Clanovi";
            this.mnClanoviClanovi.Click += new System.EventHandler(this.mnClanoviClanovi_Click);
            // 
            // mnPretrazivanjeClanova
            // 
            this.mnPretrazivanjeClanova.Name = "mnPretrazivanjeClanova";
            this.mnPretrazivanjeClanova.Size = new System.Drawing.Size(185, 22);
            this.mnPretrazivanjeClanova.Text = "Pretrazivanje clanova";
            this.mnPretrazivanjeClanova.Click += new System.EventHandler(this.mnPretrazivanjeClanova_Click);
            // 
            // mnPravljenjeKartice
            // 
            this.mnPravljenjeKartice.Name = "mnPravljenjeKartice";
            this.mnPravljenjeKartice.Size = new System.Drawing.Size(185, 22);
            this.mnPravljenjeKartice.Text = "Pravljenje kartice";
            this.mnPravljenjeKartice.Click += new System.EventHandler(this.mnPravljenjeKartice_Click);
            // 
            // mnCenovnik
            // 
            this.mnCenovnik.Name = "mnCenovnik";
            this.mnCenovnik.Size = new System.Drawing.Size(185, 22);
            this.mnCenovnik.Text = "Cenovnik";
            this.mnCenovnik.Click += new System.EventHandler(this.mnCenovnik_Click);
            // 
            // mnGrupe
            // 
            this.mnGrupe.Name = "mnGrupe";
            this.mnGrupe.Size = new System.Drawing.Size(185, 22);
            this.mnGrupe.Text = "Grupe";
            this.mnGrupe.Click += new System.EventHandler(this.mnGrupe_Click);
            // 
            // mnKategorije
            // 
            this.mnKategorije.Name = "mnKategorije";
            this.mnKategorije.Size = new System.Drawing.Size(185, 22);
            this.mnKategorije.Text = "Kategorije";
            this.mnKategorije.Click += new System.EventHandler(this.mnKategorije_Click);
            // 
            // mnMesta
            // 
            this.mnMesta.Name = "mnMesta";
            this.mnMesta.Size = new System.Drawing.Size(185, 22);
            this.mnMesta.Text = "Mesta";
            this.mnMesta.Click += new System.EventHandler(this.mnMesta_Click);
            // 
            // mnInstitucije
            // 
            this.mnInstitucije.Name = "mnInstitucije";
            this.mnInstitucije.Size = new System.Drawing.Size(185, 22);
            this.mnInstitucije.Text = "Institucije";
            this.mnInstitucije.Click += new System.EventHandler(this.mnInstitucije_Click);
            // 
            // mnUplate
            // 
            this.mnUplate.Name = "mnUplate";
            this.mnUplate.Size = new System.Drawing.Size(185, 22);
            this.mnUplate.Text = "Uplate";
            this.mnUplate.Click += new System.EventHandler(this.mnUplate_Click);
            // 
            // mnDuplikatiClanova
            // 
            this.mnDuplikatiClanova.Name = "mnDuplikatiClanova";
            this.mnDuplikatiClanova.Size = new System.Drawing.Size(185, 22);
            this.mnDuplikatiClanova.Text = "Duplikati clanova";
            this.mnDuplikatiClanova.Click += new System.EventHandler(this.mnDuplikatiClanova_Click);
            // 
            // mnIzvestaji
            // 
            this.mnIzvestaji.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnPrihodiDnevniKategorije,
            this.mnPrihodiDnevniGrupe,
            this.mnPrihodiDnevniClanovi,
            this.mnPrihodiPeriodicni,
            this.mnPrihodiPeriodicniClanovi,
            this.mnPrihodiMesecni,
            this.mnIzvestajiCenovnik,
            this.mnUplateClanova,
            this.mnAktivniClanovi,
            this.mnAktivniClanoviGrupe,
            this.mnEvidencijaPrisustvaNaTreningu});
            this.mnIzvestaji.Name = "mnIzvestaji";
            this.mnIzvestaji.Size = new System.Drawing.Size(60, 20);
            this.mnIzvestaji.Text = "Izvestaji";
            // 
            // mnPrihodiDnevniKategorije
            // 
            this.mnPrihodiDnevniKategorije.Name = "mnPrihodiDnevniKategorije";
            this.mnPrihodiDnevniKategorije.Size = new System.Drawing.Size(241, 22);
            this.mnPrihodiDnevniKategorije.Text = "Dnevni - kategorije";
            this.mnPrihodiDnevniKategorije.Click += new System.EventHandler(this.mnPrihodiDnevniKategorije_Click);
            // 
            // mnPrihodiDnevniGrupe
            // 
            this.mnPrihodiDnevniGrupe.Name = "mnPrihodiDnevniGrupe";
            this.mnPrihodiDnevniGrupe.Size = new System.Drawing.Size(241, 22);
            this.mnPrihodiDnevniGrupe.Text = "Dnevni - grupe";
            this.mnPrihodiDnevniGrupe.Click += new System.EventHandler(this.mnPrihodiDnevniGrupe_Click);
            // 
            // mnPrihodiDnevniClanovi
            // 
            this.mnPrihodiDnevniClanovi.Name = "mnPrihodiDnevniClanovi";
            this.mnPrihodiDnevniClanovi.Size = new System.Drawing.Size(241, 22);
            this.mnPrihodiDnevniClanovi.Text = "Dnevni - clanovi";
            this.mnPrihodiDnevniClanovi.Click += new System.EventHandler(this.mnPrihodiDnevniClanovi_Click);
            // 
            // mnPrihodiPeriodicni
            // 
            this.mnPrihodiPeriodicni.Name = "mnPrihodiPeriodicni";
            this.mnPrihodiPeriodicni.Size = new System.Drawing.Size(241, 22);
            this.mnPrihodiPeriodicni.Text = "Periodicni - uplate";
            this.mnPrihodiPeriodicni.Click += new System.EventHandler(this.mnPrihodiPeriodicni_Click);
            // 
            // mnPrihodiPeriodicniClanovi
            // 
            this.mnPrihodiPeriodicniClanovi.Name = "mnPrihodiPeriodicniClanovi";
            this.mnPrihodiPeriodicniClanovi.Size = new System.Drawing.Size(241, 22);
            this.mnPrihodiPeriodicniClanovi.Text = "Periodicni - clanovi";
            this.mnPrihodiPeriodicniClanovi.Click += new System.EventHandler(this.mnPrihodiPeriodicniClanovi_Click);
            // 
            // mnPrihodiMesecni
            // 
            this.mnPrihodiMesecni.Name = "mnPrihodiMesecni";
            this.mnPrihodiMesecni.Size = new System.Drawing.Size(241, 22);
            this.mnPrihodiMesecni.Text = "Mesecni";
            this.mnPrihodiMesecni.Click += new System.EventHandler(this.mnPrihodiMesecni_Click);
            // 
            // mnIzvestajiCenovnik
            // 
            this.mnIzvestajiCenovnik.Name = "mnIzvestajiCenovnik";
            this.mnIzvestajiCenovnik.Size = new System.Drawing.Size(241, 22);
            this.mnIzvestajiCenovnik.Text = "Cenovnik";
            this.mnIzvestajiCenovnik.Click += new System.EventHandler(this.mnIzvestajiCenovnik_Click);
            // 
            // mnUplateClanova
            // 
            this.mnUplateClanova.Name = "mnUplateClanova";
            this.mnUplateClanova.Size = new System.Drawing.Size(241, 22);
            this.mnUplateClanova.Text = "Uplate clanova";
            this.mnUplateClanova.Click += new System.EventHandler(this.mnUplateClanova_Click);
            // 
            // mnAktivniClanovi
            // 
            this.mnAktivniClanovi.Name = "mnAktivniClanovi";
            this.mnAktivniClanovi.Size = new System.Drawing.Size(241, 22);
            this.mnAktivniClanovi.Text = "Aktivni clanovi";
            this.mnAktivniClanovi.Click += new System.EventHandler(this.mnAktivniClanovi_Click);
            // 
            // mnAktivniClanoviGrupe
            // 
            this.mnAktivniClanoviGrupe.Name = "mnAktivniClanoviGrupe";
            this.mnAktivniClanoviGrupe.Size = new System.Drawing.Size(241, 22);
            this.mnAktivniClanoviGrupe.Text = "Aktivni clanovi - grupe";
            this.mnAktivniClanoviGrupe.Click += new System.EventHandler(this.mnAktivniClanoviGrupe_Click);
            // 
            // mnEvidencijaPrisustvaNaTreningu
            // 
            this.mnEvidencijaPrisustvaNaTreningu.Name = "mnEvidencijaPrisustvaNaTreningu";
            this.mnEvidencijaPrisustvaNaTreningu.Size = new System.Drawing.Size(241, 22);
            this.mnEvidencijaPrisustvaNaTreningu.Text = "Evidencija prisustva na treningu";
            this.mnEvidencijaPrisustvaNaTreningu.Click += new System.EventHandler(this.mnEvidencijaPrisustvaNaTreningu_Click);
            // 
            // mnPodesavanja
            // 
            this.mnPodesavanja.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnFont,
            this.mnStampaci,
            this.mnCitacKartica});
            this.mnPodesavanja.Name = "mnPodesavanja";
            this.mnPodesavanja.Size = new System.Drawing.Size(85, 20);
            this.mnPodesavanja.Text = "Podesavanja";
            // 
            // mnFont
            // 
            this.mnFont.Name = "mnFont";
            this.mnFont.Size = new System.Drawing.Size(139, 22);
            this.mnFont.Text = "Font";
            this.mnFont.Click += new System.EventHandler(this.mnFont_Click);
            // 
            // mnStampaci
            // 
            this.mnStampaci.Name = "mnStampaci";
            this.mnStampaci.Size = new System.Drawing.Size(139, 22);
            this.mnStampaci.Text = "Stampaci";
            this.mnStampaci.Click += new System.EventHandler(this.mnStampaci_Click);
            // 
            // mnCitacKartica
            // 
            this.mnCitacKartica.Name = "mnCitacKartica";
            this.mnCitacKartica.Size = new System.Drawing.Size(139, 22);
            this.mnCitacKartica.Text = "Citac kartica";
            this.mnCitacKartica.Click += new System.EventHandler(this.mnCitacKartica_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 274);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnDatoteka;
        private System.Windows.Forms.ToolStripMenuItem mnKraj;
        private System.Windows.Forms.ToolStripMenuItem mnClanovi;
        private System.Windows.Forms.ToolStripMenuItem mnUplataClanarine;
        private System.Windows.Forms.ToolStripMenuItem mnClanoviClanovi;
        private System.Windows.Forms.ToolStripMenuItem mnCenovnik;
        private System.Windows.Forms.ToolStripMenuItem mnGrupe;
        private System.Windows.Forms.ToolStripMenuItem mnKategorije;
        private System.Windows.Forms.ToolStripMenuItem mnMesta;
        private System.Windows.Forms.ToolStripMenuItem mnInstitucije;
        private System.Windows.Forms.ToolStripMenuItem mnUplate;
        private System.Windows.Forms.ToolStripMenuItem mnIzvestaji;
        private System.Windows.Forms.ToolStripMenuItem mnPrihodiDnevniKategorije;
        private System.Windows.Forms.ToolStripMenuItem mnPrihodiDnevniGrupe;
        private System.Windows.Forms.ToolStripMenuItem mnPrihodiDnevniClanovi;
        private System.Windows.Forms.ToolStripMenuItem mnPrihodiPeriodicni;
        private System.Windows.Forms.ToolStripMenuItem mnPrihodiPeriodicniClanovi;
        private System.Windows.Forms.ToolStripMenuItem mnPrihodiMesecni;
        private System.Windows.Forms.ToolStripMenuItem mnIzvestajiCenovnik;
        private System.Windows.Forms.ToolStripMenuItem mnUplateClanova;
        private System.Windows.Forms.ToolStripMenuItem mnPodesavanja;
        private System.Windows.Forms.ToolStripMenuItem mnFont;
        private System.Windows.Forms.ToolStripMenuItem mnStampaci;
        private System.Windows.Forms.ToolStripMenuItem mnAktivniClanoviGrupe;
        private System.Windows.Forms.ToolStripMenuItem mnAktivniClanovi;
        private System.Windows.Forms.ToolStripMenuItem mnPravljenjeKartice;
        private System.Windows.Forms.ToolStripMenuItem mnCitacKartica;
        private System.Windows.Forms.ToolStripMenuItem mnEvidencijaPrisustvaNaTreningu;
        private System.Windows.Forms.ToolStripMenuItem mnPretrazivanjeClanova;
        private System.Windows.Forms.ToolStripMenuItem mnDuplikatiClanova;
    }
}

