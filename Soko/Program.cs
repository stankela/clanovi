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
            //new SqlCeUtilities().CreateDatabase(@"..\..\clanovi_podaci2.sdf", "sdv");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // This creates singleton instance of NHibernateHelper and builds session factory
            NHibernateHelper nh = NHibernateHelper.Instance;

            parseOptionsFile();

            Application.Run(new Form1());
            //SingleInstanceApplication.Application.Run(args);
        }

        private static void parseOptionsFile()
        {
            string fileName = "Options.txt";
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
                }
            }
            catch (FileNotFoundException)
            {
                MessageDialogs.showMessage("Ne mogu da pronadjem fajl Options.txt", "Program za clanarinu");
            }
        }
    }
}