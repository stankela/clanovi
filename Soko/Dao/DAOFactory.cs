using Bilten.Dao.NHibernate;

namespace Bilten.Dao
{
    public abstract class DAOFactory
    {
        public abstract MestoDAO GetMestoDAO();
        public abstract ClanDAO GetClanDAO();
        public abstract InstitucijaDAO GetInstitucijaDAO();
        public abstract UplataClanarineDAO GetUplataClanarineDAO();
        public abstract KategorijaDAO GetKategorijaDAO();
        public abstract GrupaDAO GetGrupaDAO();
        public abstract MesecnaClanarinaDAO GetMesecnaClanarinaDAO();
        public abstract DolazakNaTreningDAO GetDolazakNaTreningDAO();
    }
}