using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="MesecnaClanarinaDAO"/>.
    /// </summary>
    public class MesecnaClanarinaDAOImpl : GenericNHibernateDAO<MesecnaClanarina, int>, MesecnaClanarinaDAO
    {
        #region MesecnaClanarinaDAO Members

        public virtual bool existsClanarinaGrupa(Grupa g)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from MesecnaClanarina mc where mc.Grupa = :grupa");
                q.SetEntity("grupa", g);
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

        public override IList<MesecnaClanarina> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from MesecnaClanarina");
                return q.List<MesecnaClanarina>();
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