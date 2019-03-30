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
    public partial class BiracFinansijskeCeline : Form
    {
        private List<FinansijskaCelina> finansijskeCeline;
        public FinansijskaCelina SelFinCelina = null;

        public BiracFinansijskeCeline()
        {
            InitializeComponent();
            this.Text = "Izaberite finansijsku celinu";
            cmbFinCelina.DropDownStyle = ComboBoxStyle.DropDownList;

            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    finansijskeCeline = loadFinCeline();
                }
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }

            setFinCeline(finansijskeCeline);
            if (finansijskeCeline.Count > 0)
                SelectedFinCelina = finansijskeCeline[0];
            else
                SelectedFinCelina = null;
        }

        private List<FinansijskaCelina> loadFinCeline()
        {
            List<FinansijskaCelina> result = new List<FinansijskaCelina>(
                DAOFactoryFactory.DAOFactory.GetFinansijskaCelinaDAO().FindAll/*SortById*/());
            return result;
        }

        private void setFinCeline(List<FinansijskaCelina> finCeline)
        {
            cmbFinCelina.Items.Clear();
            foreach (FinansijskaCelina f in finCeline)
            {
                cmbFinCelina.Items.Add(f.Naziv);
            }
        }

        private FinansijskaCelina SelectedFinCelina
        {
            get
            {
                if (cmbFinCelina.SelectedIndex >= 0)
                    return finansijskeCeline[cmbFinCelina.SelectedIndex];
                else
                    return null;
            }
            set
            {
                if (value == null || finansijskeCeline.IndexOf(value) == -1)
                    cmbFinCelina.SelectedIndex = -1;
                else
                    cmbFinCelina.SelectedIndex = finansijskeCeline.IndexOf(value);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelFinCelina = SelectedFinCelina;
        }
    }
}
