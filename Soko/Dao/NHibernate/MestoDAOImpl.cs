using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="MestoDAO"/>.
    /// </summary>
    public class MestoDAOImpl : GenericNHibernateDAO<Mesto, int>, MestoDAO
    {
        #region MestoDAO Members

        public virtual IList<Mesto> FindMestoByNaziv(string naziv)
        {
            try
            {
                IQuery q = Session.CreateQuery("from Mesto m where m.Naziv like :naziv");
                q.SetString("naziv", naziv);
                return q.List<Mesto>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        #endregion

        public override IList<Mesto> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from Mesto");
                return q.List<Mesto>();
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