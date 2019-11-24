using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;
using System.Collections;
using Soko.Report;
using Soko.Misc;
using Iesi.Collections;
using Soko.UI;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="DolazakNaTreningDAO"/>.
    /// </summary>
    public class DolazakNaTreningMesecniDAOImpl : GenericNHibernateDAO<DolazakNaTreningMesecni, int>,
        DolazakNaTreningMesecniDAO
    {
        public virtual DolazakNaTreningMesecni getDolazakNaTrening(Clan c, int god, int mes)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"
                    from DolazakNaTreningMesecni d
                    where d.Clan = :clan
                    and d.Godina = :god
                    and d.Mesec = :mes");
                q.SetEntity("clan", c);
                q.SetInt32("god", god);
                q.SetInt32("mes", mes);
                IList<DolazakNaTreningMesecni> result = q.List<DolazakNaTreningMesecni>();
                if (result == null || result.Count == 0)
                    return null;
                else
                    return result[0];
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual IList<DolazakNaTreningMesecni> getDolazakNaTrening(Clan c, int fromYear, int fromMonth,
            int toYear, int toMonth)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"
                    from DolazakNaTreningMesecni d
                    where d.Clan = :clan
                    and (d.Godina > :fromYear or (d.Godina = :fromYear and d.Mesec >= :fromMonth))
                    and (d.Godina < :toYear or (d.Godina = :toYear and d.Mesec <= :toMonth))");
                q.SetEntity("clan", c);
                q.SetInt32("fromYear", fromYear);
                q.SetInt32("fromMonth", fromMonth);
                q.SetInt32("toYear", toYear);
                q.SetInt32("toMonth", toMonth);
                return q.List<DolazakNaTreningMesecni>();
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