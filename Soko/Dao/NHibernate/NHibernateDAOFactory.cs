using System;
using Soko.Exceptions;

namespace Bilten.Dao.NHibernate
{
    /**
	 * Returns NHibernate-specific instances of DAOs.
	 */

    public class NHibernateDAOFactory : DAOFactory
    {
        public override MestoDAO GetMestoDAO()
        {
            return new MestoDAOImpl();
        }

        public override ClanDAO GetClanDAO()
        {
            return new ClanDAOImpl();
        }

        public override InstitucijaDAO GetInstitucijaDAO()
        {
            return new InstitucijaDAOImpl();
        }
    }
}