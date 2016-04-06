﻿using System;
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
            this.StartPosition = FormStartPosition.Manual;
        }

        private void CitacKarticaForm_Load(object sender, EventArgs e)
        {
            maxGrupa = getMaxGrupa();
            PodesiVelicinu();
        }

        public void PodesiVelicinu()
        {
            Screen[] screens = Screen.AllScreens;
            if (screens.Length == 1)
            {
                string msg = CitacKartica.getCitacKartica().FormatMessage(12345, maxGrupa);
                Graphics g = this.CreateGraphics();
                Font font = new Font(FONT_NAME, 28, FontStyle.Bold);
                SizeF size = g.MeasureString(msg, font);
                this.ClientSize = size.ToSize();
                this.Location = new Point(screens[0].Bounds.Width - this.Width, 0);
                font.Dispose();
                g.Dispose();
            }
            else
            {
                this.Location = new Point(screens[0].Bounds.Width, 0);
                this.Size = new Size(screens[1].Bounds.Width, screens[1].Bounds.Height);
            }
        }

        private string getMaxGrupa()
        {
            string result = "Grupa";
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
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
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }
            return result;
        }

        private void CitacKarticaForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Options.Instance.PozadinaCitacaKartica);
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
