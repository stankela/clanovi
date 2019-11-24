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

        public override UplataClanarineDAO GetUplataClanarineDAO()
        {
            return new UplataClanarineDAOImpl();
        }

        public override KategorijaDAO GetKategorijaDAO()
        {
            return new KategorijaDAOImpl();
        }

        public override FinansijskaCelinaDAO GetFinansijskaCelinaDAO()
        {
            return new FinansijskaCelinaDAOImpl();
        }

        public override GrupaDAO GetGrupaDAO()
        {
            return new GrupaDAOImpl();
        }

        public override MesecnaClanarinaDAO GetMesecnaClanarinaDAO()
        {
            return new MesecnaClanarinaDAOImpl();
        }

        public override DolazakNaTreningDAO GetDolazakNaTreningDAO()
        {
            return new DolazakNaTreningDAOImpl();
        }

        public override DolazakNaTreningMesecniDAO GetDolazakNaTreningMesecniDAO()
        {
            return new DolazakNaTreningMesecniDAOImpl();
        }
    }
}