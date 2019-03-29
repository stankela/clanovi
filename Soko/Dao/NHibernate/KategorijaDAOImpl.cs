using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="KategorijaDAO"/>.
    /// </summary>
    public class KategorijaDAOImpl : GenericNHibernateDAO<Kategorija, int>, KategorijaDAO
    {
        #region KategorijaDAO Members

        public virtual bool existsKategorijaNaziv(string naziv)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from Kategorija k where k.Naziv like :naziv");
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

        #endregion

        public override IList<Kategorija> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Kategorija");
                return q.List<Kategorija>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public IList<Kategorija> FindAllSortById()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Kategorija k order by k.Id asc");
                return q.List<Kategorija>();
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