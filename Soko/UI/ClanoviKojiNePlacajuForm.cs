using Bilten.Dao;
using NHibernate;
using NHibernate.Context;
using Soko.Data;
using Soko.Domain;
using Soko.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Soko.UI
{
    public partial class ClanoviKojiNePlacajuForm : EntityListForm
    {
        private const string BROJ = "Broj";
        private const string PREZIME = "Prezime";
        private const string IME = "Ime";
        private const string DATUM_RODJENJA = "DatumRodjenja";
        private const string ADRESA = "Adresa";
        private const string MESTO = "NazivMesta";

        public ClanoviKojiNePlacajuForm()
        {
            InitializeComponent();
            initialize(typeof(Clan));
            sortByPrezimeIme();
            updateBrojClanovaLabel();
        }

        private void updateBrojClanovaLabel()
        {
            lblBrojClanova.Text = entities.Count.ToString() + " clanova";
        }

        private void sortByPrezimeIme()
        {
            PropertyDescriptor propDescPrez = TypeDescriptor.GetProperties(typeof(Clan))["Prezime"];
            PropertyDescriptor propDescIme = TypeDescriptor.GetProperties(typeof(Clan))["Ime"];
            PropertyDescriptor propDescDatumRodj = TypeDescriptor.GetProperties(typeof(Clan))["DatumRodjenja"];
            PropertyDescriptor[] propDesc = new PropertyDescriptor[3] { propDescPrez, propDescIme, propDescDatumRodj };
            ListSortDirection[] direction = new ListSortDirection[3] { ListSortDirection.Ascending,
                ListSortDirection.Ascending, ListSortDirection.Ascending };
			
            entities.Sort(new SortComparer<object>(propDesc, direction));
        }

        protected override DataGridView getDataGridView()
        {
            return dataGridView1;
        }

        protected override void initUI()
        {
            base.initUI();
            this.Size = new Size(Size.Width, 450);
            this.Text = "Clanovi koji ne placaju clanarinu";
        }

        protected override void addGridColumns()
        {
            AddColumn("Broj", BROJ, 70, DataGridViewContentAlignment.MiddleCenter);
            AddColumn("Prezime", PREZIME, 120);
            AddColumn("Ime", IME, 90);
            AddColumn("Datum rodjenja", DATUM_RODJENJA, 90, "{0:dd.MM.yyyy}");
            AddColumn("Adresa", ADRESA, 170);
            AddColumn("Mesto", MESTO, 110);
        }

        protected override List<object> loadEntities()
        {
            ClanDAO clanDAO = DAOFactoryFactory.DAOFactory.GetClanDAO();
            return new List<Clan>(clanDAO.findKojiNePlacajuClanarinu()).ConvertAll<object>(
                delegate(Clan c)
                {
                    return c;
                });
        }

        protected override EntityDetailForm createEntityDetailForm(Nullable<int> entityId)
        {
            return new ClanDialog(entityId);
        }

        private void btnDodaj_Click(object sender, System.EventArgs e)
        {
            BiracClanaDialog form;
            try
            {
                form = new BiracClanaDialog();
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

            if (form.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    Clan clan = form.Clan;
                    clan.NeplacaClanarinu = true;
                    DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent(clan);
                    session.Transaction.Commit();
                    onEntityAdded(clan);
                    updateBrojClanovaLabel();
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

        private void btnBrisi_Click(object sender, System.EventArgs e)
        {
            Clan clan = (Clan)getSelectedEntity();
            if (clan == null)
                return;
            if (!MessageDialogs.queryConfirmation(deleteConfirmationMessage(clan), this.Text))
                return;

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    clan.NeplacaClanarinu = false;
                    DAOFactoryFactory.DAOFactory.GetClanDAO().MakePersistent(clan);
                    session.Transaction.Commit();
                    onEntityDeleted(clan);
                    updateBrojClanovaLabel();
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

        protected override string deleteConfirmationMessage(DomainObject entity)
        {
            return String.Format("Da li zelite da izbrisete clana \"{0}\"?", entity);
        }

        private void btnZatvori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void txtClan_TextChanged(object sender, EventArgs e)
        {
            string text = txtClan.Text;
            Clan clan = null;
            int broj;
            if (int.TryParse(text, out broj))
            {
                clan = findClan(broj);
            }
            else if (text != String.Empty)
            {
                clan = searchForClan(text);
            }
            setSelectedClan(clan);
        }

        private void setSelectedClan(Clan clan)
        {
            // NOTE: Da bi radilo ispravno, Clan mora da implementira Equals i GetHashCode
            List<object> items = (List<object>)dataGridView1.DataSource;
            int index = items.IndexOf(clan);
            if (index >= 0)
                getCurrencyManager().Position = index;
        }

        private Clan findClan(int broj)
        {
            foreach (Clan c in entities)
            {
                if (c.Broj == broj)
                    return c;
            }
            return null;
        }

        private Clan searchForClan(string text)
        {
            foreach (Clan c in entities)
            {
                if (c.PrezimeIme.StartsWith(text, StringComparison.OrdinalIgnoreCase))
                    return c;
            }
            return null;
        }

        private void ClanoviKojiNePlacajuForm_Shown(object sender, EventArgs e)
        {
            txtClan.Focus();
        }
    }
}
