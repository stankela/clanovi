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

            // Kreiranje prazne baze
            //new SqlCeUtilities().CreateDatabase(@"..\..\clanovi_podaci2.sdf", "sdv");


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new Form1());
            SingleInstanceApplication.Application.Run(args);
        }
    }
}