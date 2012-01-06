using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Soko.Domain;
using NHibernate;
using NHibernate.Cfg;

// TODO: Ovo treba da bude Soko.Data.NHibernate namespace
namespace Soko.Data
{
    public class NHibernateHelper
    {
        public static readonly ISessionFactory SessionFactory;

        static NHibernateHelper()
        {
            try
            {
      //          Configuration cfg = new Configuration();
        //        cfg.Configure();
          //      cfg.AddAssembly(typeof(Klub).Assembly);
                Configuration cfg = new PersistentConfigurationBuilder().GetConfiguration();
                SessionFactory = cfg.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                // TODO: Use your own logging mechanism rather than Console.Error
                Console.Error.WriteLine(ex);
                throw new Exception("NHibernate initialization failed", ex);
            }
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            return SessionFactory.GetCurrentSession();
        }
    }
}
