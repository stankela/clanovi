using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="InstitucijaDAO"/>.
    /// </summary>
    public class InstitucijaDAOImpl : GenericNHibernateDAO<Institucija, int>, InstitucijaDAO
    {
        #region InstitucijaDAO Members

        public virtual bool existsInstitucijaMesto(Mesto m)
        {
            try
            {

                IQuery q = Session.CreateQuery("select count(*) from Institucija i where i.Mesto = :mesto");
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

        public override IList<Institucija> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Institucija");
                return q.List<Institucija>();
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