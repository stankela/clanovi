using System;
using System.Data.OleDb;
using Soko.Domain;

namespace Soko.Dao
{
	/// <summary>
	/// Summary description for KategorijaDAO.
	/// </summary>
	public class KategorijaDAO : DAO<Kategorija>
	{
		private readonly string NAZIV = "@Naziv";
		private readonly string ID_KATEGORIJE = "@ID_Kategorije";
	
		public KategorijaDAO()
		{

		}

		private readonly string COLUMNS = " [ID Kategorije], Naziv ";
		
		protected override string getByIdSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Kategorije " +
				" WHERE [ID Kategorije] = ?";
		}
		
		public Kategorija getById(int katId)
		{
			return getById(new Key(katId));
		}

        protected override Kategorija createEntity()
		{
			return new Kategorija();
		}

		protected override string selectAllSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Kategorije";
		}

		protected override string insertSQL()
		{
			return "INSERT INTO Kategorije(Naziv) VALUES (?)";
		}

        protected override void addInsertParameters(Kategorija entity, OleDbCommand cmd)
		{
            addDataParameters(entity, cmd);
		}

		private void addDataParameters(Kategorija kat, OleDbCommand cmd)
		{
			cmd.Parameters.Add(NAZIV, OleDbType.VarWChar, Kategorija.NAZIV_MAX_LENGTH);
			
			if (kat.Naziv == null)
				cmd.Parameters[NAZIV].Value = DBNull.Value;
			else
				cmd.Parameters[NAZIV].Value = kat.Naziv;
		}

		protected override string updateSQL()
		{
			return "UPDATE Kategorije SET Naziv = ? WHERE [ID Kategorije] = ?";
		}

        protected override void addUpdateParameters(Kategorija entity, OleDbCommand cmd)
		{
            addDataParameters(entity, cmd);
            addKeyParameters(entity, cmd);
		}
		
		private void addKeyParameters(Kategorija kat, OleDbCommand cmd)
		{			
			cmd.Parameters.Add(ID_KATEGORIJE, OleDbType.Integer);
			cmd.Parameters[ID_KATEGORIJE].Value = kat.Key.intValue();		
		}
		
		protected override string deleteSQL()
		{
			return "DELETE FROM Kategorije WHERE [ID Kategorije] = ?";
		}

        protected override void loadData(Kategorija entity, OleDbDataReader rdr)
		{
			// TODO: Promeni da se za indeksiranje ridera umesto stringa koristi
			// integer (promeni za sve DAO)
			string naziv = null;
			if (!Convert.IsDBNull(rdr["Naziv"]))
				naziv = (string)rdr["Naziv"];

            entity.Naziv = naziv;
		}

		public bool existsKategorija(string naziv)
		{
			string selectCountByNaziv = "SELECT Count(*) " +
				"FROM Kategorije " +
				"WHERE Naziv = ?";

			OleDbCommand cmd = new OleDbCommand(selectCountByNaziv);
			cmd.Parameters.Add(NAZIV, OleDbType.VarWChar, Kategorija.NAZIV_MAX_LENGTH)
				.Value = naziv;
			return (int)executeScalar(cmd) > 0;
		}
	}
}
