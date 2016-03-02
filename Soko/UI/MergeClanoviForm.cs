using Bilten.Dao;
using NHibernate;
using NHibernate.Context;
using Soko.Data;
using Soko.Domain;
using Soko.Misc;
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
    public partial class MergeClanoviForm : Form
    {
        private int id1;
        private int id2;
        Nullable<int> brojClanova;
        int brojUplata;
        decimal total;


        public MergeClanoviForm(int id1, int id2)
        {
            InitializeComponent();
            this.id1 = id1;
            this.id2 = id2;
            initUI();
            updateUI(false);
        }

        private void initUI()
        {
            txtId1.ReadOnly = true;
            txtId2.ReadOnly = true;
            initUplateListView(listView1);
            initUplateListView(listView2);
        }

        private void updateUI(bool afterMerge)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    ClanDAO clanDAO = DAOFactoryFactory.DAOFactory.GetClanDAO();
                    Clan clan1 = clanDAO.GetById(id1);
                    Clan clan2 = clanDAO.GetById(id2);

                    List<UplataClanarine> uplate1 = loadUplate(clan1);
                    List<UplataClanarine> uplate2 = loadUplate(clan2);

                    updateClanUI(clan1, txtId1, txtBroj1, txtIme1, txtPrezime1, txtDatumRodjenja1, txtAdresa1, txtMesto1,
                        txtTelefon1_1, txtTelefon2_1, ckbPristupnica1, ckbKartica1, uplate1);
                    updateClanUI(clan2, txtId2, txtBroj2, txtIme2, txtPrezime2, txtDatumRodjenja2, txtAdresa2, txtMesto2,
                        txtTelefon1_2, txtTelefon2_2, ckbPristupnica2, ckbKartica2, uplate2);

                    updateUplateListView(listView1, uplate1);
                    updateUplateListView(listView2, uplate2);

                    lblBrojUplata1.Text = uplate1.Count.ToString() + " uplata";
                    lblBrojUplata2.Text = uplate2.Count.ToString() + " uplata";

                    updateStatistics(afterMerge);
                }
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(ex.Message, "Citac kartica");
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
                Cursor.Hide();
                Cursor.Current = Cursors.Arrow;
            }
        }

        private void updateStatistics(bool afterMerge)
        {
            ClanDAO clanDAO = DAOFactoryFactory.DAOFactory.GetClanDAO();
            int noviBrojClanova = clanDAO.FindAll().Count;

            UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
            IList<UplataClanarine> sveUplate = uplataClanarineDAO.FindAll();

            int noviBrojUplata = sveUplate.Count;

            decimal noviTotal = 0;
            for (int i = 0; i < sveUplate.Count; ++i)
            {
                noviTotal += sveUplate[i].Iznos.Value;
            }

            if (brojClanova != null)
            {
                bool greska = false;

                if (afterMerge)
                {
                    if (noviBrojClanova != brojClanova - 1)
                    {
                        MessageDialogs.showError("Broj clanova se ne poklapa", this.Text);
                        greska = true;
                    }
                }
                else
                {
                    if (noviBrojClanova != brojClanova)
                    {
                        MessageDialogs.showError("Broj clanova se ne poklapa", this.Text);
                        greska = true;
                    }
                }

                if (noviBrojUplata != brojUplata)
                {
                    MessageDialogs.showError("Broj uplata se ne poklapa", this.Text);
                    greska = true;
                }
                if (noviTotal != total)
                {
                    MessageDialogs.showError("Total se ne poklapa", this.Text);
                    greska = true;
                }
                if (!greska)
                {
                    MessageDialogs.showMessage("OK", this.Text);
                }
            }
            brojClanova = noviBrojClanova;
            brojUplata = noviBrojUplata;
            total = noviTotal;

            lblBrojClanova.Text = brojClanova.ToString() + " clanova";
            lblBrojUplata.Text = brojUplata.ToString() + " uplata";
            lblTotal.Text = total.ToString() + " Din";
        }

        private void updateUplateListView(ListView listView, List<UplataClanarine> uplate)
        {
            if (uplate == null || uplate.Count == 0)
            {
                listView.Items.Clear();
                return;
            }

            ListViewItem[] items = new ListViewItem[uplate.Count];
            for (int i = 0; i < uplate.Count; ++i)
            {
                UplataClanarine u = uplate[i];
                items[i] = new ListViewItem(new string[] {
                            u.VaziOd.Value.ToString("MMM"), u.VaziOd.Value.ToString("yyyy"),
                            u.IznosDin, u.Grupa.Naziv });
            }
            listView.Items.Clear();
            listView.Items.AddRange(items);
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void initUplateListView(ListView listView)
        {
            listView.View = View.Details;
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.Columns.Add("Mesec");
            listView.Columns.Add("Godina");
            listView.Columns.Add("Iznos");
            listView.Columns.Add("Grupa");
            listView.Columns[0].TextAlign = HorizontalAlignment.Right;
            listView.Columns[1].TextAlign = HorizontalAlignment.Right;
            listView.Columns[2].TextAlign = HorizontalAlignment.Right;
            listView.Columns[3].TextAlign = HorizontalAlignment.Left;
        }

        private void updateClanUI(Clan clan, TextBox txtId, TextBox txtBroj, TextBox txtIme, TextBox txtPrezime,
            TextBox txtDatumRodjenja, TextBox txtAdresa, TextBox txtMesto, TextBox txtTelefon1,
            TextBox txtTelefon2, CheckBox ckbPristupnica, CheckBox ckbKartica, List<UplataClanarine> uplate)
        {
            if (clan == null)
            {
                txtBroj.Text = String.Empty;
                txtIme.Text = String.Empty;
                txtPrezime.Text = String.Empty;
                txtDatumRodjenja.Text = String.Empty;
                txtAdresa.Text = String.Empty;
                txtMesto.Text = String.Empty;
                txtTelefon1.Text = String.Empty;
                txtTelefon2.Text = String.Empty;
                ckbPristupnica.Checked = false;
                ckbKartica.Checked = false;
                return;
            }

            txtId.Text = clan.Id.ToString();
            txtBroj.Text = clan.Broj.ToString();
            txtIme.Text = clan.Ime;
            txtPrezime.Text = clan.Prezime;

            txtDatumRodjenja.Text = String.Empty;
            if (clan.DatumRodjenja != null)
                txtDatumRodjenja.Text = clan.DatumRodjenja.Value.ToString("dd.MM.yyyy");

            txtAdresa.Text = clan.Adresa;

            txtMesto.Text = String.Empty;
            if (clan.Mesto != null)
                txtMesto.Text = clan.Mesto.Naziv;

            txtTelefon1.Text = clan.Telefon1;
            txtTelefon2.Text = clan.Telefon2;

            ckbPristupnica.Checked = clan.ImaPristupnicu;
            ckbKartica.Checked = clan.ImaKarticu;
        }

        private List<UplataClanarine> loadUplate(Clan c)
        {
            if (c == null)
                return new List<UplataClanarine>();

            UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
            List<UplataClanarine> result = new List<UplataClanarine>(uplataClanarineDAO.findUplate(c));
            if (result == null)
                result = new List<UplataClanarine>();
            Util.sortByVaziOdDesc(result);
            return result;
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MergeClanoviForm_Shown(object sender, EventArgs e)
        {
            btnZatvori.Focus();
        }

        private void btnUpdateUI_Click(object sender, EventArgs e)
        {
            updateUI(false);
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            if (!radioButton1.Checked && !radioButton2.Checked)
            {
                MessageDialogs.showMessage("Izaberite koji clan ostaje nakon merdzovanja", this.Text);
                return;
            }
            if (radioButton1.Checked)
            {
                merge(id1, id2, 1);
            }
            else
            {
                merge(id2, id1, 2);
            }
            updateUI(true);
        }

        private void merge(int clanId1, int zaBrisanje, int number)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Show();
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    ClanDAO clanDAO = DAOFactoryFactory.DAOFactory.GetClanDAO();
                    Clan clan1 = clanDAO.FindById(clanId1);
                    Clan clan2 = clanDAO.FindById(zaBrisanje);

                    List<UplataClanarine> uplate2 = loadUplate(clan2);
                    for (int i = 0; i < uplate2.Count; ++i)
                    {
                        uplate2[i].Clan = clan1;
                    }

                    clanDAO.MakeTransient(clan2);

                    if (number == 1)
                    {
                        clan1.Adresa = txtAdresa1.Text;
                        clan1.Telefon1 = txtTelefon1_1.Text;
                        clan1.Telefon2 = txtTelefon2_1.Text;
                    }
                    else if (number == 2)
                    {
                        clan1.Adresa = txtAdresa2.Text;
                        clan1.Telefon1 = txtTelefon1_2.Text;
                        clan1.Telefon2 = txtTelefon2_2.Text;
                    }

                    session.Transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(ex.Message, "Citac kartica");
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
