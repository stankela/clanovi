using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Bilten.Dao;
using Soko.Domain;
using NHibernate;
using Soko.Data;
using NHibernate.Context;

namespace Soko.UI
{
    public partial class CitacKarticaForm : Form
    {
        private const string FONT_NAME = "Arial";
        string maxGrupa;

        public CitacKarticaForm()
        {
            InitializeComponent();
            Text = "Citac kartica";
            this.ControlBox = false;
            this.ShowInTaskbar = false;
        }

        private void CitacKarticaForm_Load(object sender, EventArgs e)
        {
            maxGrupa = getMaxGrupa();
            PodesiVelicinu();
        }

        public void PodesiVelicinu()
        {
            string msg = CitacKartica.getCitacKartica().FormatMessage(1234, maxGrupa);
            Graphics g = this.CreateGraphics();
            Font font = new Font(FONT_NAME, Options.Instance.VelicinaSlovaZaCitacKartica, FontStyle.Bold);
            SizeF size = g.MeasureString(msg, font);
            font.Dispose();
            g.Dispose();

            this.ClientSize = size.ToSize();
            Rectangle screenRect = Screen.FromControl(this).Bounds;
            this.Location = new Point(screenRect.Width - this.Width, 0);
        }

        private string getMaxGrupa()
        {
            string result = "Grupa";
            try
            {
                using (ISession session = NHibernateHelper.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    GrupaDAO grupaDAO = DAOFactoryFactory.DAOFactory.GetGrupaDAO();
                    IList<Grupa> grupe = grupaDAO.FindAll();
                    foreach (Grupa g in grupe)
                    {
                        if (g.Naziv.Length > result.Length)
                        {
                            result = g.Naziv;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.SessionFactory);
            }
            return result;
        }

        private void CitacKarticaForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Yellow);
        }

        public void PrikaziOcitavanje(string msg, Color color)
        {
            Graphics g = this.CreateGraphics();
            g.Clear(color);
            Font font = new Font(FONT_NAME, Options.Instance.VelicinaSlovaZaCitacKartica, FontStyle.Bold);
            g.DrawString(msg, font, Brushes.Black, 0, 0);
            font.Dispose();
            g.Dispose();
        }
    }
}
