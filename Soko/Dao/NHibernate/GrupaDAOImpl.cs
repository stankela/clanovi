using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;
using Soko.Misc;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="GrupaDAO"/>.
    /// </summary>
    public class GrupaDAOImpl : GenericNHibernateDAO<Grupa, int>, GrupaDAO
    {
        #region GrupaDAO Members

        public virtual bool existsGrupa(Kategorija k)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from Grupa g where g.Kategorija = :kat");
                q.SetEntity("kat", k);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual bool existsGrupa(FinansijskaCelina f)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from Grupa g where g.FinansijskaCelina = :f");
                q.SetEntity("f", f);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual bool existsGrupaSifra(SifraGrupe sifra)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"select count(*)
                                                 from Grupa g
                                                 where g.Sifra.BrojGrupe = :grupa
                                                 and g.Sifra.Podgrupa = :podgrupa");
                q.SetInt32("grupa", sifra.BrojGrupe);
                q.SetString("podgrupa", sifra.Podgrupa);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual bool existsGrupaNaziv(string naziv)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from Grupa g where g.Naziv = :naziv");
                q.SetString("naziv", naziv);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual IList<Grupa> findGodisnjaClanarina()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Grupa g");
                IList<Grupa> grupe = q.List<Grupa>();
                IList<Grupa> result = new List<Grupa>();
                foreach (Grupa g in grupe)
                {
                    if (g.ImaGodisnjuClanarinu)
                    {
                        result.Add(g);
                    }
                }
                return result;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        #endregion

        public override IList<Grupa> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Grupa g
                                                 left join fetch g.Kategorija
                                                 left join fetch g.FinansijskaCelina");
                return q.List<Grupa>();
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