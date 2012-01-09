using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="UplataClanarineDAO"/>.
    /// </summary>
    public class UplataClanarineDAOImpl : GenericNHibernateDAO<UplataClanarine, int>, UplataClanarineDAO
    {
        #region UplataClanarineDAO Members

        public virtual bool existsUplataClan(Clan c)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from UplataClanarine u where u.Clan = :clan");
                q.SetEntity("clan", c);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual bool existsUplataGrupa(Grupa g)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from UplataClanarine u where u.Grupa = :grupa");
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

        public virtual IList<UplataClanarine> findUplate(Clan c)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from UplataClanarine u
                                                 left join fetch u.Clan 
                                                 left join fetch u.Grupa 
                                                 where u.Clan = :clan");
                q.SetEntity("clan", c);
                return q.List<UplataClanarine>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual IList<UplataClanarine> findUplate(DateTime from, DateTime to)
        {
            to = to.AddDays(1);
            try
            {
                IQuery q = Session.CreateQuery(@"from UplataClanarine u
                                                 left join fetch u.Clan 
                                                 left join fetch u.Grupa 
                                                 where u.DatumVremeUplate >= :from and u.DatumVremeUplate <= :to");
                q.SetDateTime("from", from);
                q.SetDateTime("to", to);
                return q.List<UplataClanarine>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        #endregion

        public override IList<UplataClanarine> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from UplataClanarine u");
                return q.List<UplataClanarine>();
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