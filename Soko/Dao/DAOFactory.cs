using Bilten.Dao.NHibernate;

namespace Bilten.Dao
{
    public abstract class DAOFactory
    {
        public abstract MestoDAO GetMestoDAO();
        public abstract ClanDAO GetClanDAO();
        public abstract InstitucijaDAO GetInstitucijaDAO();
    }
}