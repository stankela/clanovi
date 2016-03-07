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

        public Form1()
        {
            InitializeComponent();
            this.Text = "Uplata clanarine";

            refreshAdminModeUI(Options.Instance.AdminMode);

            //LocalizeUI();

            loadOptions();
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
      
            regkey.Close();
        }

        private void mnKraj_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            karticaTimer.Enabled = false;
            saveOptions();
            NHibernateHelper.Instance.SessionFactory.Close();
        }

        private void mnUplataClanarine_Click(object sender, EventArgs e)
        {
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

        private void mnClanoviClanovi_Click(object sender, EventArgs e)
        {
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
            FontChooserDialog dlg = new FontChooserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Options.Instance.Font = dlg.SelectedFont;
            }
        }

        private void mnStampaci_Click(object sender, EventArgs e)
        {
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

        public void PokreniCitacKartica()
        {
            if (!CitacKarticaEnabled)
            {
                CitacKarticaForm citacKarticaForm = new CitacKarticaForm();
                citacKarticaForm.Show();
                CitacKarticaEnabled = true;
            }
        }

        public void ZaustaviCitacKartica()
        {
            if (CitacKarticaEnabled)
            {
                CitacKarticaEnabled = false;
                SingleInstanceApplication.GlavniProzor.CitacKarticaForm.Close();
            }
        }

        private void mnCitacKartica_Click(object sender, EventArgs e)
        {
            CitacKarticaDialog form = new CitacKarticaDialog();
            if (form.ShowDialog() == DialogResult.OK)
            {
                CitacKarticaForm citacKarticaForm = this.CitacKarticaForm;
                if (citacKarticaForm != null)
                {
                    citacKarticaForm.PodesiVelicinu();
                }
            }
        }

        private System.Timers.Timer karticaTimer;
        private int numTimerEvents = 0;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            PokreniCitacKartica();

            // Normally, the timer is declared at the class level, 
            // so that it stays in scope as long as it is needed. 
            // If the timer is declared in a long-running method,   
            // KeepAlive must be used to prevent the JIT compiler  
            // from allowing aggressive garbage collection to occur  
            // before the method ends. You can experiment with this 
            // by commenting out the class-level declaration and  
            // uncommenting the declaration below; then uncomment 
            // the GC.KeepAlive(aTimer) at the end of the method. 
            //System.Timers.Timer aTimer; 

            karticaTimer = new System.Timers.Timer();
            karticaTimer.Elapsed += new System.Timers.ElapsedEventHandler(karticaTimer_Elapsed);
            initKarticaTimer();

            // If the timer is declared in a long-running method, use 
            // KeepAlive to prevent garbage collection from occurring 
            // before the method ends. 
            //GC.KeepAlive(aTimer);        
        }

        public void initKarticaTimer()
        {
            // Set the Interval (in milliseconds).
            karticaTimer.Interval = Options.Instance.CitacKarticaTimerInterval;
            karticaTimer.Enabled = true;
        }

        private void karticaTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            ++numTimerEvents;
            if (numTimerEvents % 2 == 0)
            {
                // citac kartica
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
                        Graphics g = SingleInstanceApplication.GlavniProzor.CitacKarticaForm.CreateGraphics();
                        g.Clear(Options.Instance.PozadinaCitacaKartica);
                        g.Dispose();
                        repaint = false;
                    }
                    long elapsedMs;
                    lastRead = CitacKartica.getCitacKartica().TryReadDolazakNaTrening(out elapsedMs);
                }
            }
            else
            {
                // pisac kartica
                if (!PisacKarticaEnabled)
                    return;

                PravljenjeKarticeForm pkf = PravljenjeKarticeForm;
                if (pkf != null && pkf.PendingWrite)
                {
                    CitacKarticaEnabled = false;
                    PisacKarticaEnabled = false;
                    string msg = String.Empty;
                    int brojPokusaja = Options.Instance.BrojPokusajaCitacKartica;
                    while (brojPokusaja > 0)
                    {
                        try
                        {
                            string okMsg;
                            pkf.WriteKartica(out okMsg);
                            brojPokusaja = 0;
                            msg = okMsg;
                        }
                        catch (WriteCardException ex)
                        {
                            --brojPokusaja;
                            if (brojPokusaja > 0)
                            {
                                pkf.PendingWrite = true;
                            }
                            else
                            {
                                msg = ex.Message;
                            }
                        }
                        catch (Exception ex)
                        {
                            brojPokusaja = 0;
                            msg = ex.Message;
                        }
                    }
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
                        string msg = String.Empty;
                        int brojPokusaja = Options.Instance.BrojPokusajaCitacKartica;
                        while (brojPokusaja > 0)
                        {
                            try
                            {
                                dlg.ReadKartica();
                                brojPokusaja = 0;
                            }
                            catch (ReadCardException ex)
                            {
                                --brojPokusaja;
                                if (brojPokusaja > 0)
                                {
                                    dlg.PendingRead = true;
                                }
                                else
                                {
                                    msg = ex.Message;
                                }
                            }
                            catch (Exception ex)
                            {
                                brojPokusaja = 0;
                                msg = ex.Message;
                            }
                        }
                        CitacKarticaEnabled = true;
                        if (msg != String.Empty)
                        {
                            MessageDialogs.showMessage(msg, "Ocitavanje kartice");
                        }
                        PisacKarticaEnabled = true;
                    }
                }
            }
        }

        private void mnEvidencijaPrisustvaNaTreningu_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Evidencija prisustva na treningu", true, true, false);
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
                LozinkaForm f = new LozinkaForm();
                if (f.ShowDialog() == DialogResult.OK && f.Lozinka == "Lozinka")
                {
                    Options.Instance.AdminMode = true;
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
                    p.setIzvestaj(new NedostajuceUplateIzvestaj(dlg.OdDatum, dlg.DoDatum));
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
    }
}
