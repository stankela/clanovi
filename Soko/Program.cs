using System;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.IO;
using Soko.UI;

namespace Soko
{
    static class Program
    {
        public static int VERZIJA_PROGRAMA = 4;

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

            parseOptionsFile();
            SingleInstanceApplication.Application.Run(args);
        }

        private static void parseOptionsFile()
        {
            string fileName = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Options.txt");
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                for (int i = 0; i < lines.Length; ++i)
                {
                    if (lines[i].ToUpper().Contains("PokreniCitacKartica".ToUpper()))
                    {
                        Options.Instance.PokreniCitacKartica = bool.Parse(lines[i].Split(' ')[1].Trim());
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