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
using RawInput_dll;

namespace Soko.UI
{
    public partial class CitacKarticaForm : Form
    {
        private const string FONT_NAME = "Arial";
        private string maxGrupa;
        private Font font;
        private string msg;
        private Color color;

        private RawInput _rawinput;
        public RawInput GetRawInput()
        {
            return _rawinput;
        }

        // Vrednost false obezbedjuje da ce se generisati keyboard ulaz sa RFID citaca cak i ako Form1 nije u
        // foregroundu.
        private const bool CaptureOnlyInForeground = false;

        private void podesiKeyboardRawInput()
        {
            // Resenje preuzeto sa
            // www.codeproject.com/articles/Using-Raw-Input-from-C-to-handle-multiple-keyboard

            _rawinput = new RawInput(Handle, CaptureOnlyInForeground);

            // Adding a message filter will cause keypresses to be handled
            // Ovo znaci da nakon sto se izvrsi OnKeyPressed, nece se generisati dodatni key eventi. Ako se izostavi
            // ovaj poziv, i ako je npr TextBox u fokusu kada ocitam karticu na RFID citacu koji generise keyboard
            // ulaz, tekst ce se ispisati u TextBoxu.
            _rawinput.AddMessageFilter();

            //Win32.DeviceAudit();            // Writes a file DeviceAudit.txt to the current directory
        }

        public CitacKarticaForm()
        {
            InitializeComponent();
            Text = "Citac kartica";
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;

            PodesiFont();
            msg = String.Empty;
        }

        private void CitacKarticaForm_Load(object sender, EventArgs e)
        {
            podesiKeyboardRawInput();
            maxGrupa = getMaxGrupa();
            PodesiVelicinu();
        }

        public void PodesiVelicinu()
        {
            Screen[] screens = Screen.AllScreens;
            if (screens.Length == 1)
            {
                string msg = CitacKartica.FormatMessage(12345, null, maxGrupa);
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
                if (Options.Instance.PrikaziDisplejPrekoCelogEkrana)
                {
                    this.Size = new Size(screens[1].Bounds.Width, screens[1].Bounds.Height);
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.ClientSize = new Size(Options.Instance.SirinaDispleja, Options.Instance.VisinaDispleja);                
                }
            }
        }

        private string getMaxGrupa()
        {
            string result = "";
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
            if (result == "")
                result = "GrupaGrupaGrupaGrupa";
            return result;
        }

        private void CitacKarticaForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (this.msg != String.Empty)
            {
                g.Clear(color);
                g.DrawString(msg, font, Brushes.Black, 0, 0);
            }
            else
            {
                g.Clear(Options.Instance.PozadinaCitacaKartica);
            }
        }

        public void Prikazi(string msg, Color color)
        {
            this.msg = msg;
            this.color = color;
            this.Invalidate();
            this.Update();
        }

        public void PodesiFont()
        {
            font = new Font(FONT_NAME, Options.Instance.VelicinaSlovaZaCitacKartica, FontStyle.Bold);
        }

        public void Clear()
        {
            this.msg = String.Empty;
            this.Invalidate();
            this.Update();
        }
    }
}
