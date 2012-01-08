using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="ClanDAO"/>.
    /// </summary>
    public class ClanDAOImpl : GenericNHibernateDAO<Clan, int>, ClanDAO
    {
        #region ClanDAO Members

        public virtual bool existsClanMesto(Mesto m)
        {
            try
            {

                IQuery q = Session.CreateQuery("select count(*) from Clan c where c.Mesto = :mesto");
                q.SetEntity("mesto", m);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        #endregion

        public override IList<Clan> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Clan");
                return q.List<Clan>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

    }
}