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
            this.mnCenovnik = new System.Windows.Forms.ToolStripMenuItem();
            this.mnGrupe = new System.Windows.Forms.ToolStripMenuItem();
            this.mnKategorije = new System.Windows.Forms.ToolStripMenuItem();
            this.mnMesta = new System.Windows.Forms.ToolStripMenuItem();
            this.mnInstitucije = new System.Windows.Forms.ToolStripMenuItem();
            this.mnUplate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnIzvestaji = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiDnevniKategorije = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiDnevniGrupe = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiDnevniClanovi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiPeriodicni = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiPeriodicniClanovi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPrihodiMesecni = new System.Windows.Forms.ToolStripMenuItem();
            this.mnIzvestajiCenovnik = new System.Windows.Forms.ToolStripMenuItem();
            this.mnUplateClanova = new System.Windows.Forms.ToolStripMenuItem();
            this.mnPodesavanja = new System.Windows.Forms.ToolStripMenuItem();
            this.mnFont = new System.Windows.Forms.ToolStripMenuItem();
            this.mnStampaci = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mestaPrimaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.institucijeMestoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clanMestoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kategorijaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grupaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cenovnikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clanarinaGrupaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnDatoteka,
            this.mnClanovi,
            this.mnIzvestaji,
            this.mnPodesavanja,
            this.convertToolStripMenuItem});
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
            this.mnDatoteka.Size = new System.Drawing.Size(63, 20);
            this.mnDatoteka.Text = "Datoteka";
            // 
            // mnKraj
            // 
            this.mnKraj.Name = "mnKraj";
            this.mnKraj.Size = new System.Drawing.Size(104, 22);
            this.mnKraj.Text = "Kraj";
            this.mnKraj.Click += new System.EventHandler(this.mnKraj_Click);
            // 
            // mnClanovi
            // 
            this.mnClanovi.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnUplataClanarine,
            this.mnClanoviClanovi,
            this.mnCenovnik,
            this.mnGrupe,
            this.mnKategorije,
            this.mnMesta,
            this.mnInstitucije,
            this.mnUplate});
            this.mnClanovi.Name = "mnClanovi";
            this.mnClanovi.Size = new System.Drawing.Size(54, 20);
            this.mnClanovi.Text = "Clanovi";
            // 
            // mnUplataClanarine
            // 
            this.mnUplataClanarine.Name = "mnUplataClanarine";
            this.mnUplataClanarine.Size = new System.Drawing.Size(162, 22);
            this.mnUplataClanarine.Text = "Uplata clanarine";
            this.mnUplataClanarine.Click += new System.EventHandler(this.mnUplataClanarine_Click);
            // 
            // mnClanoviClanovi
            // 
            this.mnClanoviClanovi.Name = "mnClanoviClanovi";
            this.mnClanoviClanovi.Size = new System.Drawing.Size(162, 22);
            this.mnClanoviClanovi.Text = "Clanovi";
            this.mnClanoviClanovi.Click += new System.EventHandler(this.mnClanoviClanovi_Click);
            // 
            // mnCenovnik
            // 
            this.mnCenovnik.Name = "mnCenovnik";
            this.mnCenovnik.Size = new System.Drawing.Size(162, 22);
            this.mnCenovnik.Text = "Cenovnik";
            this.mnCenovnik.Click += new System.EventHandler(this.mnCenovnik_Click);
            // 
            // mnGrupe
            // 
            this.mnGrupe.Name = "mnGrupe";
            this.mnGrupe.Size = new System.Drawing.Size(162, 22);
            this.mnGrupe.Text = "Grupe";
            this.mnGrupe.Click += new System.EventHandler(this.mnGrupe_Click);
            // 
            // mnKategorije
            // 
            this.mnKategorije.Name = "mnKategorije";
            this.mnKategorije.Size = new System.Drawing.Size(162, 22);
            this.mnKategorije.Text = "Kategorije";
            this.mnKategorije.Click += new System.EventHandler(this.mnKategorije_Click);
            // 
            // mnMesta
            // 
            this.mnMesta.Name = "mnMesta";
            this.mnMesta.Size = new System.Drawing.Size(162, 22);
            this.mnMesta.Text = "Mesta";
            this.mnMesta.Click += new System.EventHandler(this.mnMesta_Click);
            // 
            // mnInstitucije
            // 
            this.mnInstitucije.Name = "mnInstitucije";
            this.mnInstitucije.Size = new System.Drawing.Size(162, 22);
            this.mnInstitucije.Text = "Institucije";
            this.mnInstitucije.Click += new System.EventHandler(this.mnInstitucije_Click);
            // 
            // mnUplate
            // 
            this.mnUplate.Name = "mnUplate";
            this.mnUplate.Size = new System.Drawing.Size(162, 22);
            this.mnUplate.Text = "Uplate";
            this.mnUplate.Click += new System.EventHandler(this.mnUplate_Click);
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
            this.mnUplateClanova});
            this.mnIzvestaji.Name = "mnIzvestaji";
            this.mnIzvestaji.Size = new System.Drawing.Size(60, 20);
            this.mnIzvestaji.Text = "Izvestaji";
            // 
            // mnPrihodiDnevniKategorije
            // 
            this.mnPrihodiDnevniKategorije.Name = "mnPrihodiDnevniKategorije";
            this.mnPrihodiDnevniKategorije.Size = new System.Drawing.Size(176, 22);
            this.mnPrihodiDnevniKategorije.Text = "Dnevni - kategorije";
            this.mnPrihodiDnevniKategorije.Click += new System.EventHandler(this.mnPrihodiDnevniKategorije_Click);
            // 
            // mnPrihodiDnevniGrupe
            // 
            this.mnPrihodiDnevniGrupe.Name = "mnPrihodiDnevniGrupe";
            this.mnPrihodiDnevniGrupe.Size = new System.Drawing.Size(176, 22);
            this.mnPrihodiDnevniGrupe.Text = "Dnevni - grupe";
            this.mnPrihodiDnevniGrupe.Click += new System.EventHandler(this.mnPrihodiDnevniGrupe_Click);
            // 
            // mnPrihodiDnevniClanovi
            // 
            this.mnPrihodiDnevniClanovi.Name = "mnPrihodiDnevniClanovi";
            this.mnPrihodiDnevniClanovi.Size = new System.Drawing.Size(176, 22);
            this.mnPrihodiDnevniClanovi.Text = "Dnevni - clanovi";
            this.mnPrihodiDnevniClanovi.Click += new System.EventHandler(this.mnPrihodiDnevniClanovi_Click);
            // 
            // mnPrihodiPeriodicni
            // 
            this.mnPrihodiPeriodicni.Name = "mnPrihodiPeriodicni";
            this.mnPrihodiPeriodicni.Size = new System.Drawing.Size(176, 22);
            this.mnPrihodiPeriodicni.Text = "Periodicni - uplate";
            this.mnPrihodiPeriodicni.Click += new System.EventHandler(this.mnPrihodiPeriodicni_Click);
            // 
            // mnPrihodiPeriodicniClanovi
            // 
            this.mnPrihodiPeriodicniClanovi.Name = "mnPrihodiPeriodicniClanovi";
            this.mnPrihodiPeriodicniClanovi.Size = new System.Drawing.Size(176, 22);
            this.mnPrihodiPeriodicniClanovi.Text = "Periodicni - clanovi";
            this.mnPrihodiPeriodicniClanovi.Click += new System.EventHandler(this.mnPrihodiPeriodicniClanovi_Click);
            // 
            // mnPrihodiMesecni
            // 
            this.mnPrihodiMesecni.Name = "mnPrihodiMesecni";
            this.mnPrihodiMesecni.Size = new System.Drawing.Size(176, 22);
            this.mnPrihodiMesecni.Text = "Mesecni";
            this.mnPrihodiMesecni.Click += new System.EventHandler(this.mnPrihodiMesecni_Click);
            // 
            // mnIzvestajiCenovnik
            // 
            this.mnIzvestajiCenovnik.Name = "mnIzvestajiCenovnik";
            this.mnIzvestajiCenovnik.Size = new System.Drawing.Size(176, 22);
            this.mnIzvestajiCenovnik.Text = "Cenovnik";
            this.mnIzvestajiCenovnik.Click += new System.EventHandler(this.mnIzvestajiCenovnik_Click);
            // 
            // mnUplateClanova
            // 
            this.mnUplateClanova.Name = "mnUplateClanova";
            this.mnUplateClanova.Size = new System.Drawing.Size(176, 22);
            this.mnUplateClanova.Text = "Uplate clanova";
            this.mnUplateClanova.Click += new System.EventHandler(this.mnUplateClanova_Click);
            // 
            // mnPodesavanja
            // 
            this.mnPodesavanja.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnFont,
            this.mnStampaci});
            this.mnPodesavanja.Name = "mnPodesavanja";
            this.mnPodesavanja.Size = new System.Drawing.Size(81, 20);
            this.mnPodesavanja.Text = "Podesavanja";
            // 
            // mnFont
            // 
            this.mnFont.Name = "mnFont";
            this.mnFont.Size = new System.Drawing.Size(128, 22);
            this.mnFont.Text = "Font";
            this.mnFont.Click += new System.EventHandler(this.mnFont_Click);
            // 
            // mnStampaci
            // 
            this.mnStampaci.Name = "mnStampaci";
            this.mnStampaci.Size = new System.Drawing.Size(128, 22);
            this.mnStampaci.Text = "Stampaci";
            this.mnStampaci.Click += new System.EventHandler(this.mnStampaci_Click);
            // 
            // convertToolStripMenuItem
            // 
            this.convertToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addColumnsToolStripMenuItem,
            this.mestaPrimaryToolStripMenuItem,
            this.institucijeMestoToolStripMenuItem,
            this.clanMestoToolStripMenuItem,
            this.kategorijaToolStripMenuItem,
            this.grupaToolStripMenuItem,
            this.cenovnikToolStripMenuItem,
            this.clanarinaGrupaToolStripMenuItem,
            this.convertAllToolStripMenuItem});
            this.convertToolStripMenuItem.Name = "convertToolStripMenuItem";
            this.convertToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.convertToolStripMenuItem.Text = "Convert";
            // 
            // addColumnsToolStripMenuItem
            // 
            this.addColumnsToolStripMenuItem.Name = "addColumnsToolStripMenuItem";
            this.addColumnsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addColumnsToolStripMenuItem.Text = "Add columns";
            this.addColumnsToolStripMenuItem.Click += new System.EventHandler(this.addColumnsToolStripMenuItem_Click);
            // 
            // mestaPrimaryToolStripMenuItem
            // 
            this.mestaPrimaryToolStripMenuItem.Name = "mestaPrimaryToolStripMenuItem";
            this.mestaPrimaryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.mestaPrimaryToolStripMenuItem.Text = "Mesto";
            this.mestaPrimaryToolStripMenuItem.Click += new System.EventHandler(this.mestaPrimaryToolStripMenuItem_Click);
            // 
            // institucijeMestoToolStripMenuItem
            // 
            this.institucijeMestoToolStripMenuItem.Name = "institucijeMestoToolStripMenuItem";
            this.institucijeMestoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.institucijeMestoToolStripMenuItem.Text = "Institucija ";
            this.institucijeMestoToolStripMenuItem.Click += new System.EventHandler(this.institucijeMestoToolStripMenuItem_Click);
            // 
            // clanMestoToolStripMenuItem
            // 
            this.clanMestoToolStripMenuItem.Name = "clanMestoToolStripMenuItem";
            this.clanMestoToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clanMestoToolStripMenuItem.Text = "Clan";
            this.clanMestoToolStripMenuItem.Click += new System.EventHandler(this.clanMestoToolStripMenuItem_Click);
            // 
            // kategorijaToolStripMenuItem
            // 
            this.kategorijaToolStripMenuItem.Name = "kategorijaToolStripMenuItem";
            this.kategorijaToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.kategorijaToolStripMenuItem.Text = "Kategorija";
            this.kategorijaToolStripMenuItem.Click += new System.EventHandler(this.kategorijaToolStripMenuItem_Click);
            // 
            // grupaToolStripMenuItem
            // 
            this.grupaToolStripMenuItem.Name = "grupaToolStripMenuItem";
            this.grupaToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.grupaToolStripMenuItem.Text = "Grupa";
            this.grupaToolStripMenuItem.Click += new System.EventHandler(this.grupaToolStripMenuItem_Click);
            // 
            // cenovnikToolStripMenuItem
            // 
            this.cenovnikToolStripMenuItem.Name = "cenovnikToolStripMenuItem";
            this.cenovnikToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cenovnikToolStripMenuItem.Text = "Clanarina";
            this.cenovnikToolStripMenuItem.Click += new System.EventHandler(this.cenovnikToolStripMenuItem_Click);
            // 
            // clanarinaGrupaToolStripMenuItem
            // 
            this.clanarinaGrupaToolStripMenuItem.Name = "clanarinaGrupaToolStripMenuItem";
            this.clanarinaGrupaToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.clanarinaGrupaToolStripMenuItem.Text = "Uplata";
            this.clanarinaGrupaToolStripMenuItem.Click += new System.EventHandler(this.clanarinaGrupaToolStripMenuItem_Click);
            // 
            // convertAllToolStripMenuItem
            // 
            this.convertAllToolStripMenuItem.Name = "convertAllToolStripMenuItem";
            this.convertAllToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.convertAllToolStripMenuItem.Text = "Convert all";
            this.convertAllToolStripMenuItem.Click += new System.EventHandler(this.convertAllToolStripMenuItem_Click);
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
        private System.Windows.Forms.ToolStripMenuItem convertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mestaPrimaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem institucijeMestoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clanMestoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grupaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clanarinaGrupaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cenovnikToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kategorijaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertAllToolStripMenuItem;
    }
}

