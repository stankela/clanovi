using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;
using NHibernate.Criterion;

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

        public virtual bool existsClanInstitucija(Institucija i)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from Clan c where c.Institucija = :inst");
                q.SetEntity("inst", i);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual bool existsClanImePrezimeDatumRodjenja(Clan c)
        {
            try
            {
                ICriteria crit = Session.CreateCriteria(typeof(Clan));
                if (string.IsNullOrEmpty(c.Ime))
                    crit.Add(Expression.IsNull("Ime"));
                else
                    crit.Add(Expression.Eq("Ime", c.Ime));
                if (string.IsNullOrEmpty(c.Prezime))
                    crit.Add(Expression.IsNull("Prezime"));
                else
                    crit.Add(Expression.Eq("Prezime", c.Prezime));
                if (c.DatumRodjenja == null)
                    crit.Add(Expression.IsNull("DatumRodjenja"));
                else
                    crit.Add(Expression.Eq("DatumRodjenja", c.DatumRodjenja.Value));
                return crit.List().Count > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual int getMaxBroj()
        {
            try
            {
                IQuery q = Session.CreateQuery("select max(c.Broj) from Clan c");
                return (int)q.UniqueResult();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual Clan findForBrojKartice(int brojKartice)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Clan c where c.BrojKartice = :broj");
                q.SetInt32("broj", brojKartice);
                IList<Clan> result = q.List<Clan>();
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

        public virtual IList<Clan> findKojiNePlacajuClanarinu()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Clan c left join fetch c.Mesto
                                                 where c.NeplacaClanarinu = true");
                return q.List<Clan>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual IList<Clan> findClanoviSaKarticom()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Clan c
                                                 where c.BrojKartice is not null
                                                 and c.BrojKartice > 0");
                return q.List<Clan>();
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
                IQuery q = Session.CreateQuery(@"from Clan c left join fetch c.Mesto");
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