using Bilten.Dao;
using NHibernate;
using NHibernate.Context;
using Soko.Data;
using Soko.Domain;
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
    public partial class BiracKategorije : Form
    {
        private List<Kategorija> kategorije;
        public Kategorija SelKategorija = null;

        public BiracKategorije()
        {
            InitializeComponent();
            this.Text = "Izaberite kategoriju";
            cmbKategorija.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    kategorije = loadKategorije();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }

            setKategorije(kategorije);
            if (kategorije.Count > 0)
                SelectedKategorija = kategorije[0];
            else
                SelectedKategorija = null;
        }

        private List<Kategorija> loadKategorije()
        {
            List<Kategorija> result = new List<Kategorija>(
                DAOFactoryFactory.DAOFactory.GetKategorijaDAO().FindAllSortById());
            return result;
        }

        private void setKategorije(List<Kategorija> kategorije)
        {
            cmbKategorija.Items.Clear();
            foreach (Kategorija k in kategorije)
            {
                cmbKategorija.Items.Add(k.Naziv);
            }
        }

        private Kategorija SelectedKategorija
        {
            get
            {
                if (cmbKategorija.SelectedIndex >= 0)
                    return kategorije[cmbKategorija.SelectedIndex];
                else
                    return null;
            }
            set
            {
                if (value == null || kategorije.IndexOf(value) == -1)
                    cmbKategorija.SelectedIndex = -1;
                else
                    cmbKategorija.SelectedIndex = kategorije.IndexOf(value);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelKategorija = SelectedKategorija;
        }
    }
}
