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
        public readonly ISessionFactory SessionFactory;

        protected NHibernateHelper()
        {
            SessionFactory = createSessionFactory();
        }

        private ISessionFactory createSessionFactory()
        {
            try
            {
                Configuration cfg = new PersistentConfigurationBuilder().GetConfiguration();

                // Configuration cfg = new Configuration();
                // cfg.Configure();
                // cfg.AddAssembly(typeof(Klub).Assembly);

                /*string configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\hibernate.cfg.xml");
                cfg.Configure(configurationPath);
                string employeeConfigurationFile = HttpContext.Current.Server.MapPath(@"~\Models\Nhibernate\Gimnasticar.hbm.xml");
                cfg.AddFile(employeeConfigurationFile);*/

                return cfg.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                // TODO: Use your own logging mechanism rather than Console.Error
                Console.Error.WriteLine(ex);
                throw new Exception("NHibernate initialization failed", ex);
            }
        }

        private static NHibernateHelper instance;
        public static NHibernateHelper Instance
        {
            get
            {
                if (instance == null)
                    instance = new NHibernateHelper();
                return instance;
            }
        }

        public ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public ISession GetCurrentSession()
        {
            return SessionFactory.GetCurrentSession();
        }
    }
}
