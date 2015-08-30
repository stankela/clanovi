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
        public static CitacKartica workerObject;
        public static Thread workerThread;

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


            // Create the thread object. This does not start the thread.
            workerObject = new CitacKartica();
            workerThread = new Thread(workerObject.DoWork);

            // Start the worker thread.
            workerThread.Start();

            // Loop until worker thread activates. 
            while (!workerThread.IsAlive);


            //Application.Run(new Form1());
            SingleInstanceApplication.Application.Run(args);

            // NOTE: Sledeci kod se nalazi na dva mesta: ovde i u metodu CitacKarticaForm_FormClosed.
            // Kod u metodu CitacKarticaForm_FormClosed se poziva kada se CitacKarticaForm zatvori pritiskom
            // na "X". Ako se Form1 zatvori sa pritiskom na "X" tada se ne poziva metod
            // CitacKarticaForm_FormClosed (zato sto nisam podesio da je Form1 owner za CitacKarticaForm, a
            // to nisam uradio da se ne bi minimizovanjem Form1 minimizovao i CitacKarticaForm),
            // i zato sam morao da ponovim kod i ovde.

            // Request that the worker thread stop itself:
            workerObject.RequestStop();

            // Use the Join method to block the current thread  
            // until the object's thread terminates.
            workerThread.Join();

            //workerThread.Abort();
        }
    }
}