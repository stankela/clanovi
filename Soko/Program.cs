using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.IO;
using Soko.UI;
using Soko.Exceptions;

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

            string database = ConfigurationParameters.DatabaseName;
            FileInfo fi = new FileInfo(database);
            if (!fi.Exists)
            {
                string msg = "Ne mogu da pronadjem datoteku '{0}' u tekucem direktorijumu.";
                MessageDialogs.showError(String.Format(msg, database), "Greska");
                return;
            }

            List<string> messages = new List<string>();
            try
            {
                new DatabaseUpdater().updateDatabase(messages);
                if (messages.Count > 0)
                {
                    string msg = messages[0];
                    for (int i = 1; i < messages.Count; i++)
                        msg += '\n' + messages[i];
                    MessageDialogs.showMessage(msg, "Info");
                }
            }
            catch (InfrastructureException ex)
            {
                MessageDialogs.showError(ex.Message, "Greska");
            }

            new SqlCeUtilities().CreateDatabase(@"..\..\clanovi_podaci2.sdf", "sdv");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            //Application.Run(new Form1());
            SingleInstanceApplication.Application.Run(args);
        }
    }
}