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

        public virtual IList<MesecnaClanarina> getCenovnik()
        {
            // NOTE: Correlated subqueries don't work in SQL Server Compact
            try
            {
                IQuery q = Session.CreateQuery(@"from MesecnaClanarina mc
                                                 left join fetch mc.Grupa
                                                 order by mc.Grupa.Id asc, mc.VaziOd desc");
                IList<MesecnaClanarina> result = q.List<MesecnaClanarina>();
                List<MesecnaClanarina> result2 = new List<MesecnaClanarina>();
                int prevId = -1;
                foreach (MesecnaClanarina mc in result)
                {
                    if (mc.Grupa.Id != prevId)
                    {
                        result2.Add(mc);
                        prevId = mc.Grupa.Id;
                    }
                }
                return result2;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual IList<MesecnaClanarina> findForGrupa(Grupa g)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from MesecnaClanarina mc left join fetch mc.Grupa g where g = :grupa");
                q.SetEntity("grupa", g);
                return q.List<MesecnaClanarina>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual MesecnaClanarina getVazecaClanarinaForGrupa(Grupa g)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from MesecnaClanarina mc
                                                 left join fetch mc.Grupa g
                                                 where g = :grupa
                                                 order by mc.VaziOd desc");
                q.SetEntity("grupa", g);
                IList<MesecnaClanarina> result = q.List<MesecnaClanarina>();
                if (result.Count > 0)
                    return result[0];
                else
                    return null;
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