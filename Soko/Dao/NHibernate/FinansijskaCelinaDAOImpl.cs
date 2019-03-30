using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="FinansijskaCelinaDAO"/>.
    /// </summary>
    public class FinansijskaCelinaDAOImpl : GenericNHibernateDAO<FinansijskaCelina, int>, FinansijskaCelinaDAO
    {
        #region FinansijskaCelinaDAO Members

        public virtual bool existsFinansijskaCelinaNaziv(string naziv)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from FinansijskaCelina f where f.Naziv like :naziv");
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

        public IList<FinansijskaCelina> FindAllSortById()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from FinansijskaCelina f order by f.Id asc");
                return q.List<FinansijskaCelina>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        #endregion

        public override IList<FinansijskaCelina> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from FinansijskaCelina");
                return q.List<FinansijskaCelina>();
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