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
    public partial class DuplikatiClanovaForm : Form
    {
        private List<Clan> clanovi;
        private List<Clan> duplikati;
        Dictionary<int, List<UplataClanarine>> dictUplate;

        public DuplikatiClanovaForm()
        {
            InitializeComponent();

            listView1.FullRowSelect = true;
            listView1.View = View.Details;
            //listView1.HeaderStyle = ColumnHeaderStyle.None;

            listView1.Columns.Add("");
            listView1.Columns.Add("Id");
            listView1.Columns.Add("Broj");
            listView1.Columns.Add("Prezime");
            listView1.Columns.Add("Ime");
            listView1.Columns.Add("Datum rodjenja");
            listView1.Columns.Add("Pristupnica");
            listView1.Columns.Add("Kartica");
            listView1.Columns.Add("Adresa");
            listView1.Columns.Add("Mesto");
            listView1.Columns.Add("Tel. 1");
            listView1.Columns.Add("Tel. 2");
            listView1.Columns.Add("Uplate");
            listView1.Columns.Add("Poslednja uplata");

            listView1.Columns[0].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[1].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[2].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[3].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[4].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[5].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[6].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[7].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[8].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[9].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[10].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[11].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[12].TextAlign = HorizontalAlignment.Left;
            listView1.Columns[13].TextAlign = HorizontalAlignment.Left;

            loadDuplikati();
        }

        private void loadDuplikati()
        {
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);

                    clanovi = loadClanovi();
                    duplikati = findDuplikati(clanovi);
                    loadUplate(duplikati);


                    int broj = 1;
                    ListViewItem[] items = new ListViewItem[duplikati.Count];
                    for (int i = 0; i < duplikati.Count; ++i)
                    {
                        Clan c = duplikati[i];
                        if (c != null)
                        {
                            string datumRodjenja = String.Empty;
                            string imaKarticu = "NE";
                            if (c.ImaKarticu)
                                imaKarticu = "DA";
                            string imaPristupnicu = "NE";
                            if (c.ImaPristupnicu)
                                imaPristupnicu = "DA";

                            string mesto = String.Empty;
                            if (c.Mesto != null)
                                mesto = c.Mesto.Naziv;

                            string imaUplate = "NE";
                            string poslednjaUplata = String.Empty;
                            if (dictUplate.ContainsKey(c.Id))
                            {
                                List<UplataClanarine> uplate = dictUplate[c.Id];
                                if (uplate.Count > 0)
                                {
                                    imaUplate = "DA";
                                    poslednjaUplata = uplate[0].VaziOd.Value.ToString("dd.MM.yyyy");
                                }
                            }

                            if (c.DatumRodjenja != null)
                                datumRodjenja = c.DatumRodjenja.Value.ToString("dd.MM.yyyy");
                            items[i] = new ListViewItem(new string[] { broj.ToString(), c.Id.ToString(), c.Broj.ToString(),
                                c.Prezime, c.Ime,
                                datumRodjenja, imaPristupnicu, imaKarticu, c.Adresa, mesto, c.Telefon1, c.Telefon2,
                                imaUplate, poslednjaUplata });
                        }
                        else
                        {
                            ++broj;
                            items[i] = new ListViewItem(new string[] { "", "", "" });
                        }
                    }
                    listView1.Items.Clear();
                    listView1.Items.AddRange(items);
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(ex.Message, "Citac kartica");
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }
        }

        private void loadUplate(List<Clan> duplikati)
        {
            dictUplate = new Dictionary<int, List<UplataClanarine>>();
            for (int i = 0; i < duplikati.Count; ++i)
            {
                Clan c = duplikati[i];
                if (c == null)
                    continue;
                if (!dictUplate.ContainsKey(c.Id))
                {
                    UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
                    List<UplataClanarine> uplate = new List<UplataClanarine>(uplataClanarineDAO.findUplate(c));
                    if (uplate == null)
                        uplate = new List<UplataClanarine>();
                    Util.sortByVaziOdDesc(uplate);
                    dictUplate.Add(c.Id, uplate);
                }
                else
                {
                    throw new Exception("Greska");
                }
            }
            return;
        }

        private List<Clan> loadClanovi()
        {
            List<Clan> result = new List<Clan>(DAOFactoryFactory.DAOFactory.GetClanDAO().FindAll());

            PropertyDescriptor propDescPrez = TypeDescriptor.GetProperties(typeof(Clan))["Prezime"];
            PropertyDescriptor propDescIme = TypeDescriptor.GetProperties(typeof(Clan))["Ime"];
            PropertyDescriptor propDescDatumRodj = TypeDescriptor.GetProperties(typeof(Clan))["DatumRodjenja"];
            PropertyDescriptor propDescBroj = TypeDescriptor.GetProperties(typeof(Clan))["Broj"];
            PropertyDescriptor[] propDesc = new PropertyDescriptor[4] { propDescPrez, propDescIme, propDescDatumRodj,
                propDescBroj };
            ListSortDirection[] direction = new ListSortDirection[4] { ListSortDirection.Ascending,
                ListSortDirection.Ascending, ListSortDirection.Ascending, ListSortDirection.Ascending };

            result.Sort(new SortComparer<Clan>(propDesc, direction));

            return result;
        }

        private List<Clan> findDuplikati(List<Clan> clanovi)
        {
            Clan prevClan = null;
            bool prevIsDuplikat = false;
            List<Clan> duplikati = new List<Clan>();
            for (int i = 0; i < clanovi.Count; ++i)
            {
                Clan c = clanovi[i];
                if (compare(c, prevClan))
                {
                    if (duplikati.Count == 0 || !compare(prevClan, duplikati[duplikati.Count - 1]))
                        duplikati.Add(prevClan);
                    duplikati.Add(c);
                    prevIsDuplikat = true;
                }
                else if (prevIsDuplikat)
                {
                    prevIsDuplikat = false;
                    duplikati.Add(null);
                }
                prevClan = c;
            }
            return duplikati;
        }

        bool compare(Clan c1, Clan c2)
        {
            if (c1 == null || c2 == null)
                return false;
            return c1.Ime == c2.Ime && c1.Prezime == c2.Prezime && c1.DatumRodjenja == c2.DatumRodjenja;
        }

        private void btnMergeSelected_Click(object sender, EventArgs e)
        {
            List<ListViewItem> selectedItems = new List<ListViewItem>();
            for (int i = 0; i < listView1.Items.Count; ++i)
            {
                ListViewItem item = listView1.Items[i];
                if (item.Selected)
                {
                    selectedItems.Add(item);
                }
            }
            string id1 = String.Empty;
            string id2 = String.Empty;
            if (selectedItems.Count > 0)
            {
                id1 = selectedItems[0].SubItems[1].Text;
            }
            if (selectedItems.Count > 1)
            {
                id2 = selectedItems[1].SubItems[1].Text;
            }

            if (id1 != String.Empty && id2 != String.Empty)
            {
                MergeClanoviForm form = new MergeClanoviForm(int.Parse(id1), int.Parse(id2));
                form.ShowDialog();
            }
        }
    }
}
