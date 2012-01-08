using System;

namespace Soko.Dao
{
	/// <summary>
	/// Summary description for MapperRegistry.
	/// </summary>
	public class MapperRegistry
	{
        private MapperRegistry()
		{

		}

		private static MapperRegistry instance = new MapperRegistry();
		
		protected MestoDAO mestoDao = new MestoDAO();
		protected ClanDAO clanDao = new ClanDAO();
		protected InstitucijaDAO institucijaDao = new InstitucijaDAO();
		protected KategorijaDAO kategorijaDao = new KategorijaDAO();
		protected GrupaDAO grupaDao = new GrupaDAO();
		protected MesecnaClanarinaDAO mesecnaClanarinaDao = new MesecnaClanarinaDAO();
		protected UplataClanarineDAO uplataClanarineDao = new UplataClanarineDAO();

		// Everything that's stored on the registry is stored on the instance.
		// To make access easier, public methods are made static.

		public static MestoDAO mestoDAO() { return instance.mestoDao; }	
		public static ClanDAO clanDAO() { return instance.clanDao; }	
		public static InstitucijaDAO institucijaDAO() { return instance.institucijaDao; }
		public static KategorijaDAO kategorijaDAO() { return instance.kategorijaDao; }
		public static GrupaDAO grupaDAO() { return instance.grupaDao; }
		public static MesecnaClanarinaDAO mesecnaClanarinaDAO() { return instance.mesecnaClanarinaDao; }
		public static UplataClanarineDAO uplataClanarineDAO() { return instance.uplataClanarineDao; }

		// Registry can be reinitialized simply by creating a new instance.
		public static void initialize() 
		{
			instance = new MapperRegistry();
		}
	}
}
