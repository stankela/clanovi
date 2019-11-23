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
using System.Threading;
using System.IO.Pipes;
using System.IO;
using System.Diagnostics;

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
        const string PoslednjiMesecZaGodisnjeClanarineRegKey = "PoslednjiMesecZaGodisnjeClanarine";
        const string VelicinaSlovaNaDisplejuRegKey = "VelicinaSlovaNaDispleju";
        const string PrikaziBojeKodOcitavanjaRegKey = "PrikaziBojeKodOcitavanja";
        const string PrikaziImeClanaKodOcitavanjaRegKey = "PrikaziImeClanaKodOcitavanja";
        const string PrikaziDisplejPrekoCelogEkranaRegKey = "PrikaziDisplejPrekoCelogEkrana";
        const string SirinaDisplejaRegKey = "SirinaDispleja";
        const string VisinaDisplejaRegKey = "VisinaDispleja";
        const string UvekPitajZaLozinkuRegKey = "UvekPitajZaLozinku";
        const string LozinkaTimerMinutiRegKey = "LozinkaTimerMinuti";
        const string LogToFileRegKey = "LogToFile";
        const string TraziLozinkuPreOtvaranjaProzoraRegKey = "TraziLozinkuPreOtvaranjaProzora";
        const string CitacKarticaThreadIntervalRegKey = "CitacKarticaThreadInterval";
        const string CitacKarticaThreadSkipCountRegKey = "CitacKarticaThreadSkipCount";
        const string CitacKarticaThreadVisibleCountRegKey = "CitacKarticaThreadVisibleCount";
        const string CitacKarticaThreadPauzaZaBrisanjeRegKey = "CitacKarticaThreadPauzaZaBrisanje";
        const string UseWaitAndReadLoopRegKey = "UseWaitAndReadLoop";
        const string NumSecondsWaitAndReadRegKey = "NumSecondsWaitAndRead";
        const string NedostajuceUplateStartDateRegKey = "NedostajuceUplateStartDate";

        private FormWindowState lastWindowState = FormWindowState.Normal;
        private System.Timers.Timer lozinkaTimer;
        private System.Timers.Timer citacKarticaDictionaryTimer;
        private bool passwordExpired;
        public static Form1 Instance;
        public AnonymousPipeServerStream pipeServer;
        private Process clientProcess;
        public StreamWriter pipeServerStreamWriter;
        private bool clientStarted;

        public Form1()
        {
            InitializeComponent();
            Instance = this;
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
                if (regkey.GetValue(PoslednjiMesecZaGodisnjeClanarineRegKey) != null)
                    Options.Instance.PoslednjiMesecZaGodisnjeClanarine = int.Parse((string)regkey.GetValue(PoslednjiMesecZaGodisnjeClanarineRegKey));
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
                if (regkey.GetValue(TraziLozinkuPreOtvaranjaProzoraRegKey) != null)
                    Options.Instance.TraziLozinkuPreOtvaranjaProzora = bool.Parse((string)regkey.GetValue(TraziLozinkuPreOtvaranjaProzoraRegKey));
                if (regkey.GetValue(CitacKarticaThreadIntervalRegKey) != null)
                    Options.Instance.CitacKarticaThreadInterval = int.Parse((string)regkey.GetValue(CitacKarticaThreadIntervalRegKey));
                if (regkey.GetValue(CitacKarticaThreadSkipCountRegKey) != null)
                    Options.Instance.CitacKarticaThreadSkipCount = int.Parse((string)regkey.GetValue(CitacKarticaThreadSkipCountRegKey));
                if (regkey.GetValue(CitacKarticaThreadVisibleCountRegKey) != null)
                    Options.Instance.CitacKarticaThreadVisibleCount = int.Parse((string)regkey.GetValue(CitacKarticaThreadVisibleCountRegKey));
                if (regkey.GetValue(CitacKarticaThreadPauzaZaBrisanjeRegKey) != null)
                    Options.Instance.CitacKarticaThreadPauzaZaBrisanje = int.Parse((string)regkey.GetValue(CitacKarticaThreadPauzaZaBrisanjeRegKey));
                if (regkey.GetValue(UseWaitAndReadLoopRegKey) != null)
                    Options.Instance.UseWaitAndReadLoop = bool.Parse((string)regkey.GetValue(UseWaitAndReadLoopRegKey));
                if (regkey.GetValue(NumSecondsWaitAndReadRegKey) != null)
                    Options.Instance.NumSecondsWaitAndRead = int.Parse((string)regkey.GetValue(NumSecondsWaitAndReadRegKey));
                if (regkey.GetValue(NedostajuceUplateStartDateRegKey) != null)
                    Options.Instance.NedostajuceUplateStartDate = DateTime.Parse((string)regkey.GetValue(NedostajuceUplateStartDateRegKey));
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
            regkey.SetValue(PoslednjiMesecZaGodisnjeClanarineRegKey, Options.Instance.PoslednjiMesecZaGodisnjeClanarine.ToString());
            regkey.SetValue(VelicinaSlovaNaDisplejuRegKey, Options.Instance.VelicinaSlovaZaCitacKartica.ToString());
            regkey.SetValue(PrikaziBojeKodOcitavanjaRegKey, Options.Instance.PrikaziBojeKodOcitavanja.ToString());
            regkey.SetValue(PrikaziImeClanaKodOcitavanjaRegKey, Options.Instance.PrikaziImeClanaKodOcitavanjaKartice.ToString());
            regkey.SetValue(PrikaziDisplejPrekoCelogEkranaRegKey, Options.Instance.PrikaziDisplejPrekoCelogEkrana.ToString());
            regkey.SetValue(SirinaDisplejaRegKey, Options.Instance.SirinaDispleja.ToString());
            regkey.SetValue(VisinaDisplejaRegKey, Options.Instance.VisinaDispleja.ToString());
            regkey.SetValue(UvekPitajZaLozinkuRegKey, Options.Instance.UvekPitajZaLozinku.ToString());
            regkey.SetValue(LozinkaTimerMinutiRegKey, Options.Instance.LozinkaTimerMinuti.ToString());
            regkey.SetValue(LogToFileRegKey, Options.Instance.LogToFile.ToString());
            regkey.SetValue(TraziLozinkuPreOtvaranjaProzoraRegKey, Options.Instance.TraziLozinkuPreOtvaranjaProzora.ToString());
            regkey.SetValue(CitacKarticaThreadIntervalRegKey, Options.Instance.CitacKarticaThreadInterval.ToString());
            regkey.SetValue(CitacKarticaThreadSkipCountRegKey, Options.Instance.CitacKarticaThreadSkipCount.ToString());
            regkey.SetValue(CitacKarticaThreadVisibleCountRegKey, Options.Instance.CitacKarticaThreadVisibleCount.ToString());
            regkey.SetValue(CitacKarticaThreadPauzaZaBrisanjeRegKey, Options.Instance.CitacKarticaThreadPauzaZaBrisanje.ToString());
            regkey.SetValue(UseWaitAndReadLoopRegKey, Options.Instance.UseWaitAndReadLoop.ToString());
            regkey.SetValue(NumSecondsWaitAndReadRegKey, Options.Instance.NumSecondsWaitAndRead.ToString());
            regkey.SetValue(NedostajuceUplateStartDateRegKey, Options.Instance.NedostajuceUplateStartDate.ToString("dd-MM-yyyy"));
      
            regkey.Close();
        }

        private void mnKraj_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*if (Options.Instance.TraziLozinkuPreOtvaranjaProzora)
                return;

            if (this.WindowState == FormWindowState.Minimized)
            {
                e.Cancel = true;
            }*/
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Options.Instance.JedinstvenProgram)
            {
                zaustaviCitacKartica();
                lozinkaTimer.Stop();
                Sesija.Instance.EndSession();
                saveOptions();
            }
            else if (Options.Instance.IsProgramZaClanarinu)
            {
                lozinkaTimer.Stop();
                Sesija.Instance.EndSession();
                saveOptions();

                if (Options.Instance.UseWaitAndReadLoop)
                {
                    // NOTE: Moram da pozivam Kill jer ako se client program regularno zatvori (slanjem "Exit" poruke)
                    // WaitAndReadDataCard je i dalje aktivan dok ne istekne interval, a samim tim i proces je i dalje
                    // aktivan, i nije moguce ponovo restartovanje programa (ili je moguce ali imamo istovremeno dva
                    // procesa).
                    if (clientStarted)
                        clientProcess.Kill();
                }
                else
                {
                    sendToPipeClient("Exit");
                }

                clientProcess.Close();
                if (pipeServerStreamWriter != null)
                    ((IDisposable)pipeServerStreamWriter).Dispose();
                if (pipeServer != null)
                    ((IDisposable)pipeServer).Dispose();
            }
            else
            {
                // Stavljeno u ApplicationExit zato sto se iz nekog razloga FormClosed ne poziva za ovaj slucaj
            }

            NHibernateHelper.Instance.SessionFactory.Close();
        }

        public void zaustaviCitacKartica()
        {
            CitacKartica.Instance.RequestStop();
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
            if (!Options.Instance.TraziLozinkuPreOtvaranjaProzora)
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
            BiracFinansijskeCeline dlg2;
            try
            {
                dlg2 = new BiracFinansijskeCeline();
                dlg2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            if (dlg2.DialogResult != DialogResult.OK)
                return;
            
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
                    p.setIzvestaj(new DnevniPrihodiKategorijeIzvestaj(dlg.Datum.Date,
                        getGrupeForFinCelina(dlg2.SelFinCelina), dlg2.SelFinCelina));
                    p.ShowDialog();
                }
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
            BiracFinansijskeCeline dlg2;
            try
            {
                dlg2 = new BiracFinansijskeCeline();
                dlg2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            if (dlg2.DialogResult != DialogResult.OK)
                return;
            
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o dnevnim prihodima", true, false, false, dlg2.SelFinCelina);
                dlg.ShowDialog();
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
                        dlg.DoDatum.Date, dlg.Grupe, dlg2.SelFinCelina));
                    p.ShowDialog();
                }
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
            BiracFinansijskeCeline dlg2;
            try
            {
                dlg2 = new BiracFinansijskeCeline();
                dlg2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            if (dlg2.DialogResult != DialogResult.OK)
                return;

            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o dnevnim prihodima", true, false, false, dlg2.SelFinCelina);
                dlg.ShowDialog();
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
                        dlg.DoDatum.Date, dlg.Grupe, dlg2.SelFinCelina));
                    p.ShowDialog();
                }
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
            BiracFinansijskeCeline dlg2;
            try
            {
                dlg2 = new BiracFinansijskeCeline();
                dlg2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            if (dlg2.DialogResult != DialogResult.OK)
                return;

            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o prihodima", true, false, false, dlg2.SelFinCelina);
                dlg.ShowDialog();
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
                        dlg.DoDatum.Date, dlg.Grupe, dlg2.SelFinCelina));
                    p.ShowDialog();
                }
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
            BiracFinansijskeCeline dlg2;
            try
            {
                dlg2 = new BiracFinansijskeCeline();
                dlg2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            if (dlg2.DialogResult != DialogResult.OK)
                return;

            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o prihodima", true, false, false, dlg2.SelFinCelina);
                dlg.ShowDialog();
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
                        dlg.DoDatum.Date, dlg.Grupe, dlg2.SelFinCelina));
                    p.ShowDialog();
                }
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
            BiracFinansijskeCeline dlg2;
            try
            {
                dlg2 = new BiracFinansijskeCeline();
                dlg2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            if (dlg2.DialogResult != DialogResult.OK)
                return;
            
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o mesecnim prihodima", false, false, true, null);
                dlg.ShowDialog();
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
                    p.setIzvestaj(new MesecniPrihodiIzvestaj(dlg.OdDatum, dlg.DoDatum,
                        getGrupeForFinCelina(dlg2.SelFinCelina), dlg2.SelFinCelina));
                    p.ShowDialog();
                }
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

        private List<Grupa> getGrupeForFinCelina(FinansijskaCelina finCelina)
        {
            List<Grupa> result;
            using (ISession session = NHibernateHelper.Instance.OpenSession())
            using (session.BeginTransaction())
            {
                if (finCelina != null)
                {
                    result = new List<Grupa>(DAOFactoryFactory.DAOFactory.GetGrupaDAO()
                        .findForFinansijskaCelina(finCelina));
                }
                else
                {
                    result = null;
                    //result = new List<Grupa>(DAOFactoryFactory.DAOFactory.GetGrupaDAO().FindAll());
                }
            }
            return result;
        }

        private void mnAktivniClanoviGrupe_Click(object sender, EventArgs e)
        {
            BiracFinansijskeCeline dlg2;
            try
            {
                dlg2 = new BiracFinansijskeCeline();
                dlg2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            if (dlg2.DialogResult != DialogResult.OK)
                return;

            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Aktivni clanovi - grupe", true, false, false, dlg2.SelFinCelina);
                dlg.ShowDialog();
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
                        dlg.DoDatum.Date, dlg.Grupe, dlg2.SelFinCelina));
                    p.ShowDialog();
                }
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
            BiracFinansijskeCeline dlg2;
            try
            {
                dlg2 = new BiracFinansijskeCeline();
                dlg2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }
            if (dlg2.DialogResult != DialogResult.OK)
                return;

            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Aktivni clanovi", false, false, false, null);
                dlg.ShowDialog();
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
                        dlg.DoDatum.Date, getGrupeForFinCelina(dlg2.SelFinCelina), dlg2.SelFinCelina));
                    p.ShowDialog();
                }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
        }

        private void init()
        {
            // This creates singleton instance of NHibernateHelper and builds session factory
            NHibernateHelper nh = NHibernateHelper.Instance;

            loadOptions();

            if (Options.Instance.JedinstvenProgram)
            {
                this.Text = "Uplata clanarine";
                refreshAdminModeUI(Options.Instance.AdminMode);
                //LocalizeUI();

                Sesija.Instance.InitSession();

                initlozinkaTimer();

                initCitacKarticaDictionary();
                pokreniCitacKartica();
            }
            else if (Options.Instance.IsProgramZaClanarinu)
            {
                this.Text = "Uplata clanarine";
                refreshAdminModeUI(Options.Instance.AdminMode);
                //LocalizeUI();

                Sesija.Instance.InitSession();

                initlozinkaTimer();

                pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
                clientProcess = new Process();
                clientProcess.StartInfo.FileName = Options.Instance.ClientPath;
                // Pass the client process a handle to the server.
                clientProcess.StartInfo.Arguments = pipeServer.GetClientHandleAsString();
                clientProcess.StartInfo.UseShellExecute = false;

                clientStarted = true;
                try
                {
                    clientProcess.Start();
                }
                catch (Exception)
                {
                    clientStarted = false;
                    MessageDialogs.showMessage("GRESKA: Ne mogu da pokrenem citac kartica '" + Options.Instance.ClientPath + "'",
                        Form1.Instance.Text);
                }
                pipeServer.DisposeLocalCopyOfClientHandle();

                if (clientStarted)
                {
                    pipeServerStreamWriter = new StreamWriter(pipeServer);
                }
            }
            else
            {
                initCitacKarticaDictionary();
                pokreniCitacKartica();
                pokreniPipeClientThread();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!Options.Instance.JedinstvenProgram && !Options.Instance.IsProgramZaClanarinu)
            {
                // TODO3: Probaj da nadjes bolji nacin za sakrivanje Form1, zato sto se u ovom slucaju (tj. kada se
                // Visible postavi na false) ipak Form1 prikaze za trenutak pre nego sto se sakrije.
                this.Visible = false;
            }
        }

        private void initlozinkaTimer()
        {
            lozinkaTimer = new System.Timers.Timer();
            lozinkaTimer.Elapsed += lozinkaTimer_Elapsed;
            lozinkaTimer.Interval = Options.Instance.LozinkaTimerMinuti * 60 * 1000;
            passwordExpired = true;
        }

        private void initCitacKarticaDictionary()
        {
            CitacKarticaDictionary.Instance.Init();
            // Na svakih 8 sati proveravaj da li smo usli u novi dan, i ako jesmo reinicijalizuj CitacKarticaDictionary
            citacKarticaDictionaryTimer = new System.Timers.Timer();
            citacKarticaDictionaryTimer.Elapsed += citacKarticaDictionaryTimer_Elapsed;
            citacKarticaDictionaryTimer.Interval = 8 * 60 * 60 * 1000;
            citacKarticaDictionaryTimer.Start();
        }

        void citacKarticaDictionaryTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (CitacKarticaDictionary.Instance.InitDate.Month != DateTime.Now.Month
                || CitacKarticaDictionary.Instance.InitDate.Year != DateTime.Now.Year)
            {
                CitacKarticaDictionary.Instance.Init();
            }
        }

        public void pokreniCitacKartica()
        {
            CitacKarticaForm citacKarticaForm = new CitacKarticaForm();
            citacKarticaForm.Show();

            Thread citacKarticaThread;
            if (Options.Instance.UseWaitAndReadLoop)
            {
                citacKarticaThread = new Thread(new ThreadStart(CitacKartica.Instance.WaitAndReadLoop));
            }
            else
            {
                citacKarticaThread = new Thread(new ThreadStart(CitacKartica.Instance.ReadLoop));
            }
            citacKarticaThread.Start();
        }

        private void pokreniPipeClientThread()
        {
            Thread pipeClientThread = new Thread(new ThreadStart(Form1.Instance.pipeClientWorker));
            pipeClientThread.Start();
        }

        private void pipeClientWorker()
        {
            using (PipeStream pipeClient = new AnonymousPipeClientStream(PipeDirection.In, Options.Instance.PipeHandle))
            {
                using (StreamReader sr = new StreamReader(pipeClient))
                {
                    string temp;

                    // Read the server data.
                    while ((temp = sr.ReadLine()) != null)
                    {
                        if (temp.ToUpper().StartsWith(CitacKarticaDictionary.DODAJ_CLANA.ToUpper()))
                        {
                            try
                            {
                                using (ISession session = NHibernateHelper.Instance.OpenSession())
                                using (session.BeginTransaction())
                                {
                                    CurrentSessionContext.Bind(session);

                                    string[] parts = temp.Split(' ');
                                    ClanDAO clanDAO = DAOFactoryFactory.DAOFactory.GetClanDAO();
                                    Clan clan = clanDAO.FindById(int.Parse(parts[1]));
                                    
                                    CitacKarticaDictionary.Instance.DodajClanaSaKarticom(clan);
                                }
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
                        else if (temp.ToUpper().StartsWith(CitacKarticaDictionary.DODAJ_UPLATE.ToUpper()))
                        {
                            try
                            {
                                using (ISession session = NHibernateHelper.Instance.OpenSession())
                                using (session.BeginTransaction())
                                {
                                    CurrentSessionContext.Bind(session);

                                    List<UplataClanarine> uplate = new List<UplataClanarine>();
                                    string[] parts = temp.Split(' ');
                                    UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
                                    for (int i = 1; i < parts.Length; ++i)
                                    {
                                        uplate.Add(uplataClanarineDAO.FindById(int.Parse(parts[i])));
                                    }
                                    CitacKarticaDictionary.Instance.DodajUplate(uplate);
                                }
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
                        else if (temp.ToUpper().StartsWith(CitacKarticaDictionary.UPDATE_NEPLACA_CLANARINU.ToUpper()))
                        {
                            string[] parts = temp.Split(' ');
                            CitacKarticaDictionary.Instance.UpdateNeplacaClanarinu(int.Parse(parts[1]), bool.Parse(parts[2]));
                        }
                        else if (temp.ToUpper().StartsWith("CitacKarticaOpcije".ToUpper()))
                        {
                            string COMPortReader = String.Empty;
                            string COMPortWriter = String.Empty;
                            string PoslednjiDanZaUplate = String.Empty;
                            string PoslednjiMesecZaGodisnjeClanarine = String.Empty;
                            string VelicinaSlovaZaCitacKartica = String.Empty;
                            string PrikaziBojeKodOcitavanja = String.Empty;
                            string PrikaziImeClanaKodOcitavanjaKartice = String.Empty;
                            string PrikaziDisplejPrekoCelogEkrana = String.Empty;
                            string SirinaDispleja = String.Empty;
                            string VisinaDispleja = String.Empty;

                            string[] parts = temp.Split(' ');
                            for (int i = 1; i < parts.Length; i = i + 2)
                            {
                                if (parts[i].ToUpper() == "COMPortReader".ToUpper())
                                {
                                    COMPortReader = parts[i + 1];
                                }
                                else if (parts[i].ToUpper() == "COMPortWriter".ToUpper())
                                {
                                    COMPortWriter = parts[i + 1];
                                }
                                else if (parts[i].ToUpper() == "PoslednjiDanZaUplate".ToUpper())
                                {
                                    PoslednjiDanZaUplate = parts[i + 1];
                                }
                                else if (parts[i].ToUpper() == "PoslednjiMesecZaGodisnjeClanarine".ToUpper())
                                {
                                    PoslednjiMesecZaGodisnjeClanarine = parts[i + 1];
                                }
                                else if (parts[i].ToUpper() == "VelicinaSlovaZaCitacKartica".ToUpper())
                                {
                                    VelicinaSlovaZaCitacKartica = parts[i + 1];
                                }
                                else if (parts[i].ToUpper() == "PrikaziBojeKodOcitavanja".ToUpper())
                                {
                                    PrikaziBojeKodOcitavanja = parts[i + 1];
                                }
                                else if (parts[i].ToUpper() == "PrikaziImeClanaKodOcitavanjaKartice".ToUpper())
                                {
                                    PrikaziImeClanaKodOcitavanjaKartice = parts[i + 1];
                                }
                                else if (parts[i].ToUpper() == "PrikaziDisplejPrekoCelogEkrana".ToUpper())
                                {
                                    PrikaziDisplejPrekoCelogEkrana = parts[i + 1];
                                }
                                else if (parts[i].ToUpper() == "SirinaDispleja".ToUpper())
                                {
                                    SirinaDispleja = parts[i + 1];
                                }
                                else if (parts[i].ToUpper() == "VisinaDispleja".ToUpper())
                                {
                                    VisinaDispleja = parts[i + 1];
                                }
                            }

                            Options.Instance.COMPortReader = int.Parse(COMPortReader);
                            Options.Instance.COMPortWriter = int.Parse(COMPortWriter);
                            Options.Instance.PoslednjiDanZaUplate = int.Parse(PoslednjiDanZaUplate);
                            Options.Instance.PoslednjiMesecZaGodisnjeClanarine = int.Parse(PoslednjiMesecZaGodisnjeClanarine);
                            Options.Instance.VelicinaSlovaZaCitacKartica = int.Parse(VelicinaSlovaZaCitacKartica);
                            Options.Instance.PrikaziBojeKodOcitavanja = bool.Parse(PrikaziBojeKodOcitavanja);
                            Options.Instance.PrikaziImeClanaKodOcitavanjaKartice = bool.Parse(PrikaziImeClanaKodOcitavanjaKartice);
                            Options.Instance.PrikaziDisplejPrekoCelogEkrana = bool.Parse(PrikaziDisplejPrekoCelogEkrana);
                            Options.Instance.SirinaDispleja = int.Parse(SirinaDispleja);
                            Options.Instance.VisinaDispleja = int.Parse(VisinaDispleja);
                        }
                        else if (temp.ToUpper().StartsWith("EXIT"))
                        {
                            Application.Exit();
                        }
                    }
                }
            }
        }

        // TODO3: Razmisli da li treba biranje fin. celine i za 3 izvestaja vezana za evidenciju na treningu.

        private void mnEvidencijaPrisustvaNaTreningu_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Dolazak na trening", true, true, false, null);
                dlg.DateTimePickerFrom.CustomFormat = "dd.MM.yyyy HH:mm";
                dlg.DateTimePickerTo.CustomFormat = "dd.MM.yyyy HH:mm";
                dlg.ShowDialog();
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
            if (!Options.Instance.JedinstvenProgram)
            {
                MessageBox.Show("Simulator citaca kartica radi samo kada citac nije poseban program.\n" +
                    "Postavite JedinstvenProgram na true u fajlu Options.txt");
                return;
            }
            SimulatorCitacaKarticaForm f = new SimulatorCitacaKarticaForm();
            f.ShowDialog();
        }

        private void mnNedostajuceUplate_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Nedostajuce uplate", false, false, true, null);
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            PrintOrExportForm form = new PrintOrExportForm();
            if (form.ShowDialog() != DialogResult.OK)
                return;
            bool exportToFile = form.Eksportuj;
            String fileName = "";
            if (exportToFile)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Snimi izvestaj";
                saveFileDialog1.Filter = "Text Documents (*.txt)|*.txt";
                saveFileDialog1.FileName = "Nedostajuce uplate.txt";
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                    return;
                fileName = saveFileDialog1.FileName;
            }

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    if (exportToFile)
                    {
                        StreamWriter streamWriter = File.CreateText(fileName);
                        streamWriter.WriteLine("NEDOSTAJU\u0106E UPLATE");
                        streamWriter.WriteLine("");
                        List<object[]> items = new DolazakNaTreningMesecniLista(dlg.OdDatum, dlg.DoDatum, true).getItems();
                        foreach (object[] item in items)
                        {
                            streamWriter.WriteLine(item[0].ToString() + '\t' + item[1].ToString() + '\t' + item[2].ToString());
                        }
                        streamWriter.Close();
                    }
                    else
                    {
                        PreviewDialog p = new PreviewDialog();
                        p.setIzvestaj(new DolazakNaTreningMesecniIzvestaj(dlg.OdDatum, dlg.DoDatum, true));
                        p.ShowDialog();
                    }
                }
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
                dlg = new BiracIntervala("Dolazak na trening - mesecni", false, false, true, null);
                dlg.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
                return;
            }

            if (dlg.DialogResult != DialogResult.OK)
                return;

            PrintOrExportForm form = new PrintOrExportForm();
            if (form.ShowDialog() != DialogResult.OK)
                return;
            bool exportToFile = form.Eksportuj;
            String fileName = "";
            if (exportToFile)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Title = "Snimi izvestaj";
                saveFileDialog1.Filter = "Text Documents (*.txt)|*.txt";
                saveFileDialog1.FileName = "Dolazak na trening i uplate.txt";
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                    return;
                fileName = saveFileDialog1.FileName;
            }

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    if (exportToFile)
                    {
                        StreamWriter streamWriter = File.CreateText(fileName);
                        streamWriter.WriteLine("DOLAZAK NA TRENING I UPLATE");
                        streamWriter.WriteLine("");
                        List<object[]> items = new DolazakNaTreningMesecniLista(dlg.OdDatum, dlg.DoDatum, false).getItems();
                        foreach (object[] item in items)
                        {
                            streamWriter.WriteLine(item[0].ToString() + '\t' + item[1].ToString() + '\t' + item[2].ToString());
                        }
                        streamWriter.Close();
                    }
                    else
                    {
                        PreviewDialog p = new PreviewDialog();
                        p.setIzvestaj(new DolazakNaTreningMesecniIzvestaj(dlg.OdDatum, dlg.DoDatum, false));
                        p.ShowDialog();
                    }
                }
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
            if (Options.Instance.TraziLozinkuPreOtvaranjaProzora)
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

        private void mnClanoviKojiNePlacajuClanarinu_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            try
            {
                ClanoviKojiNePlacajuForm f = new ClanoviKojiNePlacajuForm();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }

        private void mnOpcije_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            OptionsForm form = new OptionsForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (!Options.Instance.UvekPitajZaLozinku)
                {
                    lozinkaTimer.Interval = Options.Instance.LozinkaTimerMinuti * 60 * 1000;
                }
            }
        }

        public void sendToPipeClient(string msg)
        {
            try
            {
                if (pipeServerStreamWriter != null)
                {
                    pipeServerStreamWriter.AutoFlush = true;
                    pipeServerStreamWriter.WriteLine(msg);
                }
            }
            catch (Exception ex)
            {
                // Exception is raised if the pipe is broken  or disconnected.
                MessageDialogs.showMessage(ex.Message, Form1.Instance.Text);
            }
        }

        private void mnDolazakNaTrening_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();

            try
            {
                DolazakNaTreningForm d = new DolazakNaTreningForm();
                d.ShowDialog();
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

        private void mnFinansijskeCeline_Click(object sender, EventArgs e)
        {
            if (!dozvoliOtvaranjeProzora())
                return;

            try
            {
                FinansijskeCelineForm f = new FinansijskeCelineForm();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
        }
    }
}
