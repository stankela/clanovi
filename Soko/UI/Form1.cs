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
using Soko.Dao;
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

        public Form1()
        {
            InitializeComponent();

            //LocalizeUI();

            loadOptions();
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
            string stampacPotvrda = null;
            string stampacIzvestaj = null;
            if (regkey != null)
            {
                if (regkey.GetValue(FontSizeRegKey) != null)
                    fontSize = float.Parse((string)regkey.GetValue(FontSizeRegKey));
                if (regkey.GetValue(StampacPotvrdaRegKey) != null)
                    stampacPotvrda = (string)regkey.GetValue(StampacPotvrdaRegKey);
                if (regkey.GetValue(StampacIzvestajRegKey) != null)
                    stampacIzvestaj = (string)regkey.GetValue(StampacIzvestajRegKey);
                regkey.Close();
            }
            Options.Instance.Font = new Font(Font.FontFamily, fontSize);
            Options.Instance.PrinterNamePotvrda = stampacPotvrda;
            Options.Instance.PrinterNameIzvestaj = stampacIzvestaj;
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

            regkey.Close();
        }

        private void mnKraj_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            saveOptions();
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

            int idStavke = dlg.Entity.Id;
            string naslov = "Uplata clanarine";
            string pitanje = "Da li zelite da stampate potvrdu o uplati?";
            PotvrdaDialog dlg2 = new PotvrdaDialog(naslov, pitanje);
            if (dlg2.ShowDialog() != DialogResult.Yes)
                return;

            try
            {
                PreviewDialog p = new PreviewDialog();
                p.printWithoutPreview(new PotvrdaIzvestaj(idStavke));
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
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
                using (ISession session = NHibernateHelper.OpenSession())
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
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
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
                using (ISession session = NHibernateHelper.OpenSession())
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
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        // TODO: Probaj da u sokolskom drustvu izbrises tabele Clanovi Status, 
        // Roditelj i SSS (prvo proveri da li su prazne). Koristi za to posebnu 
        // verziju programa samo za tu namenu.   
        
        private void mnPrihodiDnevniGrupe_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o dnevnim prihodima", true);
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
                using (ISession session = NHibernateHelper.OpenSession())
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
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiDnevniClanovi_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o dnevnim prihodima", true);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
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
                PreviewDialog p = new PreviewDialog();
                p.setIzvestaj(new DnevniPrihodiIzvestaj(dlg.OdDatum.Date,
                    dlg.DoDatum.Date, dlg.Grupe));
                p.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiPeriodicni_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o prihodima", true);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
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
                PreviewDialog p = new PreviewDialog();
                p.setIzvestaj(new PeriodicniPrihodiIzvestaj(dlg.OdDatum.Date,
                    dlg.DoDatum.Date, dlg.Grupe));
                p.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiPeriodicniClanovi_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o prihodima", true);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
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
                PreviewDialog p = new PreviewDialog();
                p.setIzvestaj(new PeriodicniClanoviIzvestaj(dlg.OdDatum.Date,
                    dlg.DoDatum.Date, dlg.Grupe));
                p.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void mnPrihodiMesecni_Click(object sender, EventArgs e)
        {
            BiracIntervala dlg;
            try
            {
                dlg = new BiracIntervala("Izvestaj o mesecnim prihodima", false);
                dlg.ShowDialog();
            }
            catch (InfrastructureException ex)
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
                PreviewDialog p = new PreviewDialog();
                p.setIzvestaj(new MesecniPrihodiIzvestaj(dlg.OdDatum.Date,
                    dlg.DoDatum.Date));
                p.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
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
                using (ISession session = NHibernateHelper.OpenSession())
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
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
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
                PreviewDialog p = new PreviewDialog();
                p.setIzvestaj(new UplateClanovaIzvestaj(ceoIzvestaj, idClana));
                p.ShowDialog();
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
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

        private void convertAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SqlCeUtilities().CreateDatabase(@"..\..\clanovi_podaci2.sdf", "sdv");

            addMesta();
            addInstitucije();
            addClanovi();
            addKategorije();
            addGrupe();
            addClanarine();
            addUplate();
        }

        private void addMesta()
        {
            List<Mesto> mesta = MapperRegistry.mestoDAO().getAll();

            IDataContext dataContext = null;
            try
            {
                DataAccessProviderFactory factory = new DataAccessProviderFactory();
                dataContext = factory.GetDataContext();
                dataContext.BeginTransaction();

                foreach (Mesto m in mesta)
                {
                    dataContext.Add(m);
                }

                dataContext.Commit();
            }
            catch (Exception ex)
            {
                if (dataContext != null && dataContext.IsInTransaction)
                    dataContext.Rollback();
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                if (dataContext != null)
                    dataContext.Dispose();
                dataContext = null;
            }
        }

        private void addInstitucije()
        {
            List<Institucija> institucije = MapperRegistry.institucijaDAO().getAll();

            IDataContext dataContext = null;
            try
            {
                DataAccessProviderFactory factory = new DataAccessProviderFactory();
                dataContext = factory.GetDataContext();
                dataContext.BeginTransaction();

                IList<Mesto> mesta = loadMesta(dataContext);

                foreach (Institucija i in institucije)
                {
                    if (i.Mesto != null)
                    {
                        Mesto m = findMesto(mesta, i.Mesto.Zip);
                        if (m == null)
                            throw new Exception("greska");
                        i.Mesto = m;
                    }
                    dataContext.Add(i);
                }

                dataContext.Commit();
            }
            catch (Exception ex)
            {
                if (dataContext != null && dataContext.IsInTransaction)
                    dataContext.Rollback();
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                if (dataContext != null)
                    dataContext.Dispose();
                dataContext = null;
            }
        }

        private void addClanovi()
        {
            List<Clan> clanovi = MapperRegistry.clanDAO().getAll();

            IDataContext dataContext = null;
            try
            {
                DataAccessProviderFactory factory = new DataAccessProviderFactory();
                dataContext = factory.GetDataContext();
                dataContext.BeginTransaction();

                IList<Mesto> mesta = loadMesta(dataContext);
                IList<Institucija> institucije = loadInstitucije(dataContext);

                foreach (Clan c in clanovi)
                {
                    if (c.Broj == 751)
                    {
                        c.DatumRodjenja = new DateTime(1994, 1, 10);
                    }
                    if (c.Mesto != null)
                    {
                        Mesto m = findMesto(mesta, c.Mesto.Zip);
                        if (m == null)
                            throw new Exception("greska");
                        c.Mesto = m;
                    }
                    if (c.Institucija != null)
                    {
                        Institucija i = findInstitucija(institucije, c.Institucija.Naziv);
                        if (i == null)
                            throw new Exception("greska");
                        c.Institucija = i;
                    }
                    dataContext.Add(c);
                }

                dataContext.Commit();
            }
            catch (Exception ex)
            {
                if (dataContext != null && dataContext.IsInTransaction)
                    dataContext.Rollback();
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                if (dataContext != null)
                    dataContext.Dispose();
                dataContext = null;
            }
        }

        private void addKategorije()
        {
            List<Kategorija> kategorije = MapperRegistry.kategorijaDAO().getAll();

            IDataContext dataContext = null;
            try
            {
                DataAccessProviderFactory factory = new DataAccessProviderFactory();
                dataContext = factory.GetDataContext();
                dataContext.BeginTransaction();

                foreach (Kategorija k in kategorije)
                {
                    dataContext.Add(k);
                }

                dataContext.Commit();
            }
            catch (Exception ex)
            {
                if (dataContext != null && dataContext.IsInTransaction)
                    dataContext.Rollback();
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                if (dataContext != null)
                    dataContext.Dispose();
                dataContext = null;
            }
        }

        private void addGrupe()
        {
            List<Grupa> grupe = MapperRegistry.grupaDAO().getAll();

            IDataContext dataContext = null;
            try
            {
                DataAccessProviderFactory factory = new DataAccessProviderFactory();
                dataContext = factory.GetDataContext();
                dataContext.BeginTransaction();

                IList<Kategorija> kategorije = loadKategorije(dataContext);

                foreach (Grupa g in grupe)
                {
                    if (g.Kategorija != null)
                    {
                        Kategorija k = findKategorija(kategorije, g.Kategorija.Naziv);
                        if (k == null)
                            throw new Exception("greska");
                        g.Kategorija = k;
                    }
                    dataContext.Add(g);
                }

                dataContext.Commit();
            }
            catch (Exception ex)
            {
                if (dataContext != null && dataContext.IsInTransaction)
                    dataContext.Rollback();
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                if (dataContext != null)
                    dataContext.Dispose();
                dataContext = null;
            }
        }

        private void addClanarine()
        {
            List<MesecnaClanarina> clanarine = MapperRegistry.mesecnaClanarinaDAO().getAll();

            IDataContext dataContext = null;
            try
            {
                DataAccessProviderFactory factory = new DataAccessProviderFactory();
                dataContext = factory.GetDataContext();
                dataContext.BeginTransaction();

                IList<Grupa> grupe = loadGrupe(dataContext);

                foreach (MesecnaClanarina c in clanarine)
                {
                    if (c.Grupa != null)
                    {
                        Grupa g = findGrupa(grupe, c.Grupa);
                        if (g == null)
                            throw new Exception("greska");
                        c.Grupa = g;
                    }
                    dataContext.Add(c);
                }

                dataContext.Commit();
            }
            catch (Exception ex)
            {
                if (dataContext != null && dataContext.IsInTransaction)
                    dataContext.Rollback();
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                if (dataContext != null)
                    dataContext.Dispose();
                dataContext = null;
            }
        }

        private void addUplate()
        {
            List<UplataClanarine> uplate = MapperRegistry.uplataClanarineDAO().getAll();

            IDataContext dataContext = null;
            try
            {
                DataAccessProviderFactory factory = new DataAccessProviderFactory();
                dataContext = factory.GetDataContext();
                dataContext.BeginTransaction();

                IList<Clan> clanovi = loadClanovi(dataContext);
                IList<Grupa> grupe = loadGrupe(dataContext);


                IDictionary<int, Clan> clanoviMap = new Dictionary<int, Clan>();
                foreach (Clan c in clanovi)
                {
                    clanoviMap.Add(c.Broj.Value, c);
                }


                foreach (UplataClanarine u in uplate)
                {
                    if (u.Key.intValue() == 398)
                    {
                        u.VaziOd = new DateTime(2003, u.VaziOd.Value.Month, u.VaziOd.Value.Day);
                    }
                    if (u.Clan != null)
                    {
                        u.Clan = clanoviMap[u.Clan.Broj.Value];
                        if (u.Clan == null)
                            throw new Exception("greska");
                    }
                    if (u.Grupa != null)
                    {
                        Grupa g = findGrupa(grupe, u.Grupa);
                        if (g == null)
                            throw new Exception("greska");
                        u.Grupa = g;
                    }
                    dataContext.Add(u);
                }

                dataContext.Commit();
            }
            catch (Exception ex)
            {
                if (dataContext != null && dataContext.IsInTransaction)
                    dataContext.Rollback();
                MessageDialogs.showError(ex.Message, this.Text);
            }
            finally
            {
                if (dataContext != null)
                    dataContext.Dispose();
                dataContext = null;
            }
        }

        private IList<Mesto> loadMesta(IDataContext dataContext)
        {
            string query = @"from Mesto";
            IList<Mesto> result = dataContext.
                ExecuteQuery<Mesto>(QueryLanguageType.HQL, query,
                        new string[] { }, new object[] { });
            return result;
        }

        private Mesto findMesto(IList<Mesto> mesta, string zip)
        {
            foreach (Mesto m in mesta)
            {
                if (m.Zip.ToUpper() == zip.ToUpper())
                    return m;
            }
            return null;
        }

        private IList<Institucija> loadInstitucije(IDataContext dataContext)
        {
            string query = @"from Institucija";
            IList<Institucija> result = dataContext.
                ExecuteQuery<Institucija>(QueryLanguageType.HQL, query,
                        new string[] { }, new object[] { });
            return result;
        }

        private Institucija findInstitucija(IList<Institucija> institucije, string naziv)
        {
            foreach (Institucija i in institucije)
            {
                if (i.Naziv.ToUpper() == naziv.ToUpper())
                    return i;
            }
            return null;
        }

        private IList<Kategorija> loadKategorije(IDataContext dataContext)
        {
            string query = @"from Kategorija";
            IList<Kategorija> result = dataContext.
                ExecuteQuery<Kategorija>(QueryLanguageType.HQL, query,
                        new string[] { }, new object[] { });
            return result;
        }

        private Kategorija findKategorija(IList<Kategorija> kategorije, string naziv)
        {
            foreach (Kategorija k in kategorije)
            {
                if (k.Naziv.ToUpper() == naziv.ToUpper())
                    return k;
            }
            return null;
        }

        private IList<Grupa> loadGrupe(IDataContext dataContext)
        {
            string query = @"from Grupa";
            IList<Grupa> result = dataContext.
                ExecuteQuery<Grupa>(QueryLanguageType.HQL, query,
                        new string[] { }, new object[] { });
            return result;
        }

        private Grupa findGrupa(IList<Grupa> grupe, Grupa grupa)
        {
            foreach (Grupa g in grupe)
            {
                if (g.Sifra == grupa.Sifra)
                    return g;
            }
            return null;
        }

        private IList<Clan> loadClanovi(IDataContext dataContext)
        {
            string query = @"from Clan";
            IList<Clan> result = dataContext.
                ExecuteQuery<Clan>(QueryLanguageType.HQL, query,
                        new string[] { }, new object[] { });
            return result;
        }

        private Clan findClan(IList<Clan> clanovi, Clan clan)
        {
            foreach (Clan c in clanovi)
            {
                if (c.Broj == clan.Broj)
                    return c;
            }
            return null;
        }

    }
}
