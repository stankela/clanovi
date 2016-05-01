using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Soko.Exceptions;
using Soko.Report;
using Soko.Domain;
using Soko.Data;
using NHibernate;
using NHibernate.Context;
using Bilten.Dao;
using Soko.Misc;

namespace Soko.UI
{
    public partial class Form1 : Form
    {
        const string RegKey = @"Software\SasaStankovic\Program";
        const string FontSizeRegKey = "FontSize";
        const string StampacPotvrdaRegKey = "StampacPotvrda";
        const string StampacIzvestajRegKey = "StampacIzvestaj";
        const string COMPortReaderRegKey = "COMPortReader";
        const string COMPortWriterRegKey = "COMPortWriter";
        const string PoslednjiDanZaUplateRegKey = "PoslednjiDanZaUplate";
        const string VelicinaSlovaNaDisplejuRegKey = "VelicinaSlovaNaDispleju";
        const string PrikaziBojeKodOcitavanjaRegKey = "PrikaziBojeKodOcitavanja";
        const string PrikaziImeClanaKodOcitavanjaRegKey = "PrikaziImeClanaKodOcitavanja";
        const string PrikaziDisplejPrekoCelogEkranaRegKey = "PrikaziDisplejPrekoCelogEkrana";
        const string SirinaDisplejaRegKey = "SirinaDispleja";
        const string VisinaDisplejaRegKey = "VisinaDispleja";
        const string UvekPitajZaLozinkuRegKey = "UvekPitajZaLozinku";
        const string LozinkaTimerMinutiRegKey = "LozinkaTimerMinuti";
        const string LogToFileRegKey = "LogToFile";
        const string CitacKarticaTimerIntervalRegKey = "CitacKarticaTimerInterval";
        const string TraziLozinkuPreOtvatanjaProzoraRegKey = "TraziLozinkuPreOtvatanjaProzora";

        private FormWindowState lastWindowState = FormWindowState.Normal;
        private System.Timers.Timer lozinkaTimer;
        private bool passwordExpired;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Uplata clanarine";

            refreshAdminModeUI(Options.Instance.AdminMode);

            //LocalizeUI();

            loadOptions();
            Sesija.Instance.InitSession();

            passwordExpired = true;
        }

        private void refreshAdminModeUI(bool adminMode)
        {
            mnAdmin.Visible = adminMode;
            mnAdmin.Enabled = adminMode;
        }

        private void LocalizeUI()
        {
            // TODO
            /*
            System.Resources.ResourceManager resourceManager = new
                System.Resources.ResourceManager("Soko.Resources.UIResursi", this.GetType().Assembly);
            mnClanovi.Text = resourceManager.GetString("form1_mn_clanovi_text");
            mnUplataClanarine.Text = resourceManager.GetString("form1_mn_uplata_clanarine_text");
            mnClanoviClanovi.Text = resourceManager.GetString("form1_mn_clanovi_clanovi_text");
            mnIzvestaji.Text = resourceManager.GetString("form1_mn_izvestaji_text");
            mnPrihodiMesecni.Text = resourceManager.GetString("form1_mn_prihodi_mesecni_text");
            mnUplateClanova.Text = resourceManager.GetString("form1_mn_uplate_clanova_text");
            */
        }

        private void loadOptions()
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(RegKey);
            float fontSize = Font.SizeInPoints;
            if (regkey != null)
            {
                if (regkey.GetValue(FontSizeRegKey) != null)
                    fontSize = float.Parse((string)regkey.GetValue(FontSizeRegKey));
                if (regkey.GetValue(StampacPotvrdaRegKey) != null)
                    Options.Instance.PrinterNamePotvrda = (string)regkey.GetValue(StampacPotvrdaRegKey);
                if (regkey.GetValue(StampacIzvestajRegKey) != null)
                    Options.Instance.PrinterNameIzvestaj = (string)regkey.GetValue(StampacIzvestajRegKey);
                if (regkey.GetValue(COMPortReaderRegKey) != null)
                    Options.Instance.COMPortReader = int.Parse((string)regkey.GetValue(COMPortReaderRegKey));
                if (regkey.GetValue(COMPortWriterRegKey) != null)
                    Options.Instance.COMPortWriter = int.Parse((string)regkey.GetValue(COMPortWriterRegKey));
                if (regkey.GetValue(PoslednjiDanZaUplateRegKey) != null)
                    Options.Instance.PoslednjiDanZaUplate = int.Parse((string)regkey.GetValue(PoslednjiDanZaUplateRegKey));
                if (regkey.GetValue(VelicinaSlovaNaDisplejuRegKey) != null)
                    Options.Instance.VelicinaSlovaZaCitacKartica = int.Parse((string)regkey.GetValue(VelicinaSlovaNaDisplejuRegKey));
                if (regkey.GetValue(PrikaziBojeKodOcitavanjaRegKey) != null)
                    Options.Instance.PrikaziBojeKodOcitavanja = bool.Parse((string)regkey.GetValue(PrikaziBojeKodOcitavanjaRegKey));
                if (regkey.GetValue(PrikaziImeClanaKodOcitavanjaRegKey) != null)
                    Options.Instance.PrikaziImeClanaKodOcitavanjaKartice = bool.Parse((string)regkey.GetValue(PrikaziImeClanaKodOcitavanjaRegKey));
                if (regkey.GetValue(PrikaziDisplejPrekoCelogEkranaRegKey) != null)
                    Options.Instance.PrikaziDisplejPrekoCelogEkrana = bool.Parse((string)regkey.GetValue(PrikaziDisplejPrekoCelogEkranaRegKey));
                if (regkey.GetValue(SirinaDisplejaRegKey) != null)
                    Options.Instance.SirinaDispleja = int.Parse((string)regkey.GetValue(SirinaDisplejaRegKey));
                if (regkey.GetValue(VisinaDisplejaRegKey) != null)
                    Options.Instance.VisinaDispleja = int.Parse((string)regkey.GetValue(VisinaDisplejaRegKey));
                if (regkey.GetValue(UvekPitajZaLozinkuRegKey) != null)
                    Options.Instance.UvekPitajZaLozinku = bool.Parse((string)regkey.GetValue(UvekPitajZaLozinkuRegKey));
                if (regkey.GetValue(LozinkaTimerMinutiRegKey) != null)
                    Options.Instance.LozinkaTimerMinuti = int.Parse((string)regkey.GetValue(LozinkaTimerMinutiRegKey));
                if (regkey.GetValue(LogToFileRegKey) != null)
                    Options.Instance.LogToFile = bool.Parse((string)regkey.GetValue(LogToFileRegKey));
                if (regkey.GetValue(CitacKarticaTimerIntervalRegKey) != null)
                    Options.Instance.CitacKarticaTimerInterval = int.Parse((string)regkey.GetValue(CitacKarticaTimerIntervalRegKey));
                if (regkey.GetValue(TraziLozinkuPreOtvatanjaProzoraRegKey) != null)
                    Options.Instance.TraziLozinkuPreOtvatanjaProzora = bool.Parse((string)regkey.GetValue(TraziLozinkuPreOtvatanjaProzoraRegKey));
                regkey.Close();
            }
            Options.Instance.Font = new Font(Font.FontFamily, fontSize);
        }

        private void saveOptions()
        {
            RegistryKey regkey = Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (regkey == null)
                regkey = Registry.CurrentUser.CreateSubKey(RegKey);

            regkey.SetValue(FontSizeRegKey, Options.Instance.Font.SizeInPoints.ToString());

            string stampacPotvrda = String.Empty;
            if (Options.Instance.PrinterNamePotvrda != null)
                stampacPotvrda = Options.Instance.PrinterNamePotvrda;
            regkey.SetValue(StampacPotvrdaRegKey, stampacPotvrda);

            string stampacIzvestaj = String.Empty;
            if (Options.Instance.PrinterNameIzvestaj != null)
                stampacIzvestaj = Options.Instance.PrinterNameIzvestaj;
            regkey.SetValue(StampacIzvestajRegKey, stampacIzvestaj);

            regkey.SetValue(COMPortReaderRegKey, Options.Instance.COMPortReader.ToString());
            regkey.SetValue(COMPortWriterRegKey, Options.Instance.COMPortWriter.ToString());
            regkey.SetValue(PoslednjiDanZaUplateRegKey, Options.Instance.PoslednjiDanZaUplate.ToString());
            regkey.SetValue(VelicinaSlovaNaDisplejuRegKey, Options.Instance.VelicinaSlovaZaCitacKartica.ToString());
            regkey.SetValue(PrikaziBojeKodOcitavanjaRegKey, Options.Instance.PrikaziBojeKodOcitavanja.ToString());
            regkey.SetValue(PrikaziImeClanaKodOcitavanjaRegKey, Options.Instance.PrikaziImeClanaKodOcitavanjaKartice.ToString());
            regkey.SetValue(PrikaziDisplejPrekoCelogEkranaRegKey, Options.Instance.PrikaziDisplejPrekoCelogEkrana.ToString());
            regkey.SetValue(SirinaDisplejaRegKey, Options.Instance.SirinaDispleja.ToString());
            regkey.SetValue(VisinaDisplejaRegKey, Options.Instance.VisinaDispleja.ToString());
            regkey.SetValue(UvekPitajZaLozinkuRegKey, Options.Instance.UvekPitajZaLozinku.ToString());
            regkey.SetValue(LozinkaTimerMinutiRegKey, Options.Instance.LozinkaTimerMinuti.ToString());
            regkey.SetValue(LogToFileRegKey, Options.Instance.LogToFile.ToString());
            regkey.SetValue(CitacKarticaTimerIntervalRegKey, Options.Instance.CitacKarticaTimerInterval.ToString());
            regkey.SetValue(TraziLozinkuPreOtvatanjaProzoraRegKey, Options.Instance.TraziLozinkuPreOtvatanjaProzora.ToString());
      
            regkey.Close();
        }

        private void mnKraj_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            karticaTimer.Enabled = false;
            lozinkaTimer.Stop();
            Sesija.Instance.EndSession();
            saveOptions();
            NHibernateHelper.Instance.SessionFactory.Close();
        }

        private void mnUplataClanarine_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            UplataClanarineDialog dlg;
            try
            {
                dlg = new UplataClanarineDialog(null);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            CitacKarticaDictionary.Instance.DodajUplate(dlg.Uplate);

            string naslov = "Uplata clanarine";
            string pitanje = "Da li zelite da stampate potvrdu o uplati?";
            PotvrdaDialog dlg2 = new PotvrdaDialog(naslov, pitanje);
            if (dlg2.ShowDialog() != DialogResult.Yes)
                return;

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();

                    List<int> idList = new List<int>();
                    foreach (UplataClanarine u in dlg.Uplate)
                    {
                        idList.Add(u.Id);
                    }

                    p.printWithoutPreview(new PotvrdaIzvestaj(idList));
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }
        }

        private bool dozvoliOtvaranjeProzora()
        {
            if (!Options.Instance.TraziLozinkuPreOtvatanjaProzora)
                return true;
            if (!passwordExpired)
            {
                // restartuj tajmer svaki put kada korisnik otvori prozor
                lozinkaTimer.Stop();
                lozinkaTimer.Start();
                return true;
            }

            LozinkaForm f = new LozinkaForm(Options.Instance.AdminLozinka, true, false);
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (!Options.Instance.UvekPitajZaLozinku)
                {
                    passwordExpired = false;
                    lozinkaTimer.Start();
                }
                return true;
            }
            return false;
        }

        private void mnClanoviClanovi_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            try
            {
                ClanoviForm f = new ClanoviForm();
                f.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnCenovnik_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            try
            {
                MesecneClanarineForm d = new MesecneClanarineForm();
                d.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnGrupe_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            try
            {
                GrupeForm d = new GrupeForm();
                d.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnKategorije_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            try
            {
                KategorijeForm f = new KategorijeForm();
                f.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnMesta_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            try
            {
                MestaForm form = new MestaForm();
                form.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnInstitucije_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            try
            {
                InstitucijeForm d = new InstitucijeForm();
                d.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnUplate_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            try
            {
                UplateClanarineForm d = new UplateClanarineForm();
                d.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiDnevniKategorije_Click(object sender, EventArgs e)
        {
            BiracDana dlg = new BiracDana("Izvestaj o prihodima");
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            List<Grupa> grupeBezKategorija;
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    grupeBezKategorija = new List<Grupa>(DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO().
                        getGrupeBezKategorija(dlg.Datum.Date));
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }

            if (grupeBezKategorija.Count > 0)
            {
                GrupeBezKategorijaDialog d = new GrupeBezKategorijaDialog(grupeBezKategorija);
                d.ShowDialog();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new DnevniPrihodiKategorijeIzvestaj(dlg.Datum.Date));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiDnevniGrupe_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o dnevnim prihodima", true, false, false);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new DnevniPrihodiGrupeIzvestaj(dlg.OdDatum.Date,
                        dlg.DoDatum.Date, dlg.Grupe));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiDnevniClanovi_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o dnevnim prihodima", true, false, false);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new DnevniPrihodiIzvestaj(dlg.OdDatum.Date,
                        dlg.DoDatum.Date, dlg.Grupe));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiPeriodicni_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o prihodima", true, false, false);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new PeriodicniPrihodiIzvestaj(dlg.OdDatum.Date,
                        dlg.DoDatum.Date, dlg.Grupe));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiPeriodicniClanovi_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o prihodima", true, false, false);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new PeriodicniClanoviIzvestaj(dlg.OdDatum.Date,
                        dlg.DoDatum.Date, dlg.Grupe));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiMesecni_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o mesecnim prihodima", false, false, true);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new MesecniPrihodiIzvestaj(dlg.OdDatum, dlg.DoDatum));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnIzvestajiCenovnik_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new CenovnikIzvestaj());
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnUplateClanova_Click(object sender, EventArgs e)
        {
            BiracClana dlg;
            try
            {
                dlg = new BiracClana("Izvestaj o uplatama clanova");
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            bool ceoIzvestaj = dlg.CeoIzvestaj;
            int idClana = dlg.IdClana;
            if (idClana < 0)
                ceoIzvestaj = true;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new UplateClanovaIzvestaj(ceoIzvestaj, idClana));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnFont_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            FontChooserDialog dlg = new FontChooserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Options.Instance.Font = dlg.SelectedFont;
            }
        }

        private void mnStampaci_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            PrinterSelectionForm form = new PrinterSelectionForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                Options.Instance.PrinterNamePotvrda = form.PrinterNamePotvrda;
                Options.Instance.PrinterNameIzvestaj = form.PrinterNameIzvestaj;
            }
        }

        private void mnAktivniClanoviGrupe_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Aktivni clanovi - grupe", true, false, false);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new AktivniClanoviGrupeIzvestaj(dlg.OdDatum.Date,
                        dlg.DoDatum.Date, dlg.Grupe));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnAktivniClanovi_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Aktivni clanovi", false, false, false);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new AktivniClanoviIzvestaj(dlg.OdDatum.Date,
                        dlg.DoDatum.Date));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPravljenjeKartice_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            PravljenjeKarticeForm dlg;
            try
            {
                dlg = new PravljenjeKarticeForm();
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }                                   
        }

        public CitacKarticaForm CitacKarticaForm
        {
            get
            {
                foreach (Form f in Application.OpenForms)
                {
                    CitacKarticaForm form = f as CitacKarticaForm;
                    if (form != null)
                    {
                        return form;
                    }
                }
                return null;
            }
        }

        public PravljenjeKarticeForm PravljenjeKarticeForm
        {
            get
            {
                foreach (Form f in Application.OpenForms)
                {
                    PravljenjeKarticeForm form = f as PravljenjeKarticeForm;
                    if (form != null)
                    {
                        return form;
                    }
                }
                return null;
            }
        }

        public UplataClanarineDialog UplataClanarineDialog
        {
            get
            {
                foreach (Form f in Application.OpenForms)
                {
                    UplataClanarineDialog form = f as UplataClanarineDialog;
                    if (form != null)
                    {
                        return form;
                    }
                }
                return null;
            }
        }

        public AdminForm AdminForm
        {
            get
            {
                foreach (Form f in Application.OpenForms)
                {
                    AdminForm form = f as AdminForm;
                    if (form != null)
                    {
                        return form;
                    }
                }
                return null;
            }
        }

        private void mnCitacKartica_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            CitacKarticaDialog form = new CitacKarticaDialog();
            if (form.ShowDialog() == DialogResult.OK)
            {
                CitacKarticaForm citacKarticaForm = this.CitacKarticaForm;
                if (citacKarticaForm != null)
                {
                    citacKarticaForm.PodesiVelicinu();
                    citacKarticaForm.PodesiFont();
                }
            }
        }

        private System.Timers.Timer karticaTimer;
        private bool citacKarticaJeNaRedu = false;
        private bool lastRead = false;
        private bool repaint = true;

        private bool citacKarticaEnabled = false;
        public bool CitacKarticaEnabled
        {
            get { return citacKarticaEnabled; }
            set { citacKarticaEnabled = value; }
        }

        private bool pisacKarticaEnabled = true;
        public bool PisacKarticaEnabled
        {
            get { return pisacKarticaEnabled; }
            set { pisacKarticaEnabled = value; }
        }

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                // Kada se zakljuca ekran
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                // Kada se otkljca ekran
                CitacKarticaForm f = SingleInstanceApplication.GlavniProzor.CitacKarticaForm;
                if (f != null)
                {
                    f.Show();
                    f.BringToFront();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CitacKarticaDictionary.Instance.Init();

            // Dogadjaj kada se ekran otkljuca/zakljuca.
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);

            lozinkaTimer = new System.Timers.Timer();
            lozinkaTimer.Elapsed += lozinkaTimer_Elapsed;
            lozinkaTimer.Interval = Options.Instance.LozinkaTimerMinuti * 60 * 1000;
            
            // Pokreni citac kartica
            CitacKarticaForm citacKarticaForm = new CitacKarticaForm();
            citacKarticaForm.Show();
            CitacKarticaEnabled = true;

            karticaTimer = new System.Timers.Timer();
            karticaTimer.Elapsed += new System.Timers.ElapsedEventHandler(karticaTimer_Elapsed);
            startKarticaTimer();
        }

        public void startKarticaTimer()
        {
            if (!Options.Instance.CitacKarticeNaPosebnomThreadu)
            {
                // Set the Interval (in milliseconds).
                karticaTimer.Interval = Options.Instance.CitacKarticaTimerInterval;
                karticaTimer.Enabled = true;
            }
        }

        private int waitingCount = 0;
        private int waitingMax;

        private void karticaTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            if (waitingCount > 0)
            {
                CitacKarticaForm citacKarticaForm = this.CitacKarticaForm;
                if (citacKarticaForm != null)
                {
                    if (waitingCount == waitingMax)
                    {
                        citacKarticaForm.Prikazi("Sa\u010Dekajte ...", Options.Instance.PozadinaCitacaKartica);
                    }
                    else if (waitingCount == 1)
                    {
                        citacKarticaForm.Clear();
                    }
                }
                --waitingCount;
                return;
            }
            else if (CitacKarticaDictionary.Instance.CreationDate.Month != DateTime.Now.Month
                || CitacKarticaDictionary.Instance.CreationDate.Year != DateTime.Now.Year)
            {
                CitacKarticaDictionary.Instance.Init();

                // sacekaj 5 sekundi
                waitingMax = Convert.ToInt32(5000.0 / Options.Instance.CitacKarticaTimerInterval);
                waitingCount = waitingMax;
                return;
            }

            citacKarticaJeNaRedu = !citacKarticaJeNaRedu;
            if (citacKarticaJeNaRedu)
            {
                handleCitacKartica();
            }
            else
            {
                handlePisacKartica();
            }
        }

        private void handleCitacKartica()
        {
            if (!CitacKarticaEnabled)
                return;

            if (lastRead)
            {
                lastRead = false;
                repaint = true;
            }
            else
            {
                if (repaint)
                {
                    CitacKarticaForm citacKarticaForm = this.CitacKarticaForm;
                    if (citacKarticaForm != null)
                    {
                        citacKarticaForm.Clear();
                        repaint = false;
                    }
                }
                try
                {
                    lastRead = CitacKartica.Instance.TryReadDolazakNaTrening();
                }
                catch (Exception ex)
                {
                    // sacekaj 5 sekundi
                    waitingMax = Convert.ToInt32(5000.0 / Options.Instance.CitacKarticaTimerInterval);
                    waitingCount = waitingMax;

                    // Uvek loguj ovaj izuzetak
                    Sesija.Instance.Log("CITAC EXCEPTION", true);
                    if (ex.Message != null)
                        Sesija.Instance.Log(ex.Message);
                }
            }
        }

        private void handlePisacKartica()
        {
            Sesija.Instance.Log("P entered");

            if (!PisacKarticaEnabled)
                return;

            try
            {
                doHandlePisacKartica();
            }
            finally
            {
                if (!CitacKarticaEnabled || !PisacKarticaEnabled)
                {
                    // Uvek loguj ovaj izuzetak
                    Sesija.Instance.Log("UNMANAGED EXCEPTION", true);
                }
                CitacKarticaEnabled = true;
                PisacKarticaEnabled = true;
            }
        }

        private void doHandlePisacKartica()
        {
            PravljenjeKarticeForm pkf = PravljenjeKarticeForm;
            if (pkf != null && pkf.PendingWrite)
            {
                CitacKarticaEnabled = false;
                PisacKarticaEnabled = false;

                string msg;
                pkf.handlePisacKarticaWrite(out msg);

                CitacKarticaEnabled = true;
                MessageDialogs.showMessage(msg, "Pravljenje kartice");
                PisacKarticaEnabled = true;
            }
            else
            {
                UplataClanarineDialog dlg = UplataClanarineDialog;
                if (dlg != null && dlg.PendingRead)
                {
                    CitacKarticaEnabled = false;
                    PisacKarticaEnabled = false;

                    string msg;
                    dlg.handlePisacKarticaRead(out msg);

                    CitacKarticaEnabled = true;
                    if (msg != String.Empty)
                    {
                        MessageDialogs.showMessage(msg, "Ocitavanje kartice");
                    }
                    PisacKarticaEnabled = true;
                }
            }
        }

        private void mnEvidencijaPrisustvaNaTreningu_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Dolazak na trening", true, true, false);
                dlg.DateTimePickerFrom.CustomFormat = "dd.MM.yyyy HH:mm";
                dlg.DateTimePickerTo.CustomFormat = "dd.MM.yyyy HH:mm";
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new EvidencijaTreningaIzvestaj(dlg.ClanId, dlg.OdDatumVreme,
                        dlg.DoDatumVreme, dlg.Grupe));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnDuplikatiClanova_Click(object sender, EventArgs e)
        {
            try
            {
                DuplikatiClanovaForm f = new DuplikatiClanovaForm();
                f.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                LozinkaForm f = new LozinkaForm("Lozinka", false, true);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Options.Instance.AdminMode = f.AdminMode;
                    refreshAdminModeUI(Options.Instance.AdminMode);
                }
            }
        }

        private void mnSimulatorCitacaKartica_Click(object sender, EventArgs e)
        {
            SimulatorCitacaKarticaForm f = new SimulatorCitacaKarticaForm();
            f.ShowDialog();
        }

        private void mnNedostajuceUplate_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Nedostajuce uplate", false, false, true);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new DolazakNaTreningMesecniIzvestaj(dlg.OdDatum, dlg.DoDatum, true));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnAdminOpcije_Click(object sender, EventArgs e)
        {
            AdminForm f = new AdminForm();
            f.ShowDialog();
        }

        private void mnDolazakNaTreningMesecni_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Dolazak na trening - mesecni", false, false, true);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    PreviewDialog p = new PreviewDialog();
                    p.setIzvestaj(new DolazakNaTreningMesecniIzvestaj(dlg.OdDatum, dlg.DoDatum, false));
                    p.ShowDialog();
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        void lozinkaTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            passwordExpired = true;
            lozinkaTimer.Stop();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (Options.Instance.TraziLozinkuPreOtvatanjaProzora)
                return;

            // Check if window state changes
            if (WindowState != lastWindowState)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    if (Options.Instance.UvekPitajZaLozinku)
                    {
                        passwordExpired = true;
                    }
                    else
                    {
                        passwordExpired = false;
                        lozinkaTimer.Start();
                    }
                }
                else if (WindowState == FormWindowState.Normal && lastWindowState == FormWindowState.Minimized)
                {
                    // Restored
                    if (passwordExpired)
                    {
                        LozinkaForm f = new LozinkaForm(Options.Instance.AdminLozinka, true, false);
                        if (f.ShowDialog() != DialogResult.OK)
                        {
                            this.WindowState = FormWindowState.Minimized;
                        }
                    }
                    else
                    {
                        lozinkaTimer.Stop();
                    }
                }
                lastWindowState = WindowState;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Options.Instance.TraziLozinkuPreOtvatanjaProzora)
                return;

            if (this.WindowState == FormWindowState.Minimized)
            {
                e.Cancel = true;
            }
        }

        private void mnClanoviKojiNePlacajuClanarinu_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            try
            {
                ClanoviKojiNePlacajuForm f = new ClanoviKojiNePlacajuForm();
                f.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnLozinka_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            LozinkaOptionsForm form = new LozinkaOptionsForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (!Options.Instance.UvekPitajZaLozinku)
                {
                    lozinkaTimer.Interval = Options.Instance.LozinkaTimerMinuti * 60 * 1000;
                }
            }
        }
    }
}
