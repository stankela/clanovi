using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.IO;
using Soko.UI;
using Soko.Exceptions;
using Soko.Data;

namespace Soko
{
    static class Program
    {
        public static int VERZIJA_PROGRAMA = 2;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Language.SetKeyboardLanguage(Language.acKeyboardLanguage.hklSerbianLatin);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("sr-SP-Latn");
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            // Kreiranje prazne baze
            //new SqlCeUtilities().CreateDatabase(ConfigurationParameters.DatabaseFile, ConfigurationParameters.Password);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                // Ako postoje argumenti, u pitanju je klient process (prvi argument je pipe handle,
                // prosledjen od servera).
                Options.Instance.PipeHandle = args[0];
                Options.Instance.JedinstvenProgram = false;
                Options.Instance.IsProgramZaClanarinu = false;
            }
            else
            {
                parseOptionsFile();
            }

            Application.ApplicationExit += Application_ApplicationExit;

            if (Options.Instance.JedinstvenProgram || Options.Instance.IsProgramZaClanarinu)
            {
                SingleInstanceApplication.Application.Run(args);
            }
            else
            {
                Application.Run(new Form1());
            }
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (Options.Instance.JedinstvenProgram)
            { 
            
            }
            else if (Options.Instance.IsProgramZaClanarinu)
            {

            }
            else
            {
                // NOTE: Ovaj kod se nece izvrsiti ako je UseWaitAndReadLoop = true zato sto se tada program zatvara
                // pomocu Kill, i ApplicationExit ne generise.
                Form1.Instance.zaustaviCitacKartica();
                NHibernateHelper.Instance.SessionFactory.Close();
            }
        }

        private static void parseOptionsFile()
        {
            string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Options.txt");
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                for (int i = 0; i < lines.Length; ++i)
                {
                    if (lines[i].ToUpper().Contains("JedinstvenProgram".ToUpper()))
                    {
                        Options.Instance.JedinstvenProgram = bool.Parse(lines[i].Split(' ')[1].Trim());
                    }
                    else if (lines[i].ToUpper().Contains("IsProgramZaClanarinu".ToUpper()))
                    {
                        Options.Instance.IsProgramZaClanarinu = bool.Parse(lines[i].Split(' ')[1].Trim());
                    }
                    else if (lines[i].ToUpper().Contains("ClientPath".ToUpper()))
                    {
                        Options.Instance.ClientPath = lines[i].Split(' ')[1].Trim();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                MessageDialogs.showMessage("Ne mogu da pronadjem fajl Options.txt", "Program za clanarinu");
            }
        }
    }
}