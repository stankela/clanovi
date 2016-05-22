using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.ApplicationServices;
using Soko.UI;
using Soko.Data;
using System.Windows.Forms;

namespace Soko
{
    class SingleInstanceApplication : WindowsFormsApplicationBase
    {
        private static SingleInstanceApplication application;
        internal static SingleInstanceApplication Application
        {
            get
            {
                if (application == null)
                    application = new SingleInstanceApplication();
                return application;
            }
        }

        // Must call base constructor to ensure correct initial 
        // WindowsFormsApplicationBase configuration
        public SingleInstanceApplication()
        {
            // This ensures the underlying single-SDI framework is employed, 
            // and OnStartupNextInstance is fired
            this.IsSingleInstance = true;
        }

        protected override void OnCreateMainForm()
        {
            // Do your initialization here

            // This creates singleton instance of NHibernateHelper and builds session factory
            NHibernateHelper nh = NHibernateHelper.Instance;
     
            // Then create the main form, the splash screen will automatically close
            this.MainForm = new Form1();
        }
    }
}
