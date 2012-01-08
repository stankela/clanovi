using System;
using System.Data.OleDb;
using Soko.Domain;

namespace Soko.Dao
{
	/// <summary>
	/// Summary description for MestoDAO.
	/// </summary>
	public class MestoDAO : DAO<Mesto>
	{
		private readonly string ZIP = "@ZIP";
		private readonly string NAZIV = "@Naziv";
		private readonly string OLD_ZIP = "@Old_ZIP";
		
		public MestoDAO()
		{

		}

		private readonly string COLUMNS = " ZIP, Naziv ";

		protected override string getByIdSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Mesta " +
				" WHERE ZIP = ?";
		}
		
		// can throw InfrastructureException
		public Mesto getById(string zip)
		{
			return getById(new Key(zip));
		}

		protected override void addIdParameter(Key key, OleDbCommand cmd)
		{
			cmd.Parameters.Add("@Id", OleDbType.VarWChar, Mesto.ZIP_MAX_LENGTH)
				.Value = key.stringValue();
		}

		protected override Key loadKey(OleDbDataReader rdr)
		{
			return new Key((string)rdr["ZIP"]);
		}

        protected override Mesto createEntity()
		{
			return new Mesto();
		}

        protected override void loadData(Mesto entity, OleDbDataReader rdr)
		{
			string naziv = null;
			if (!Convert.IsDBNull(rdr["Naziv"]))
				naziv = (string)rdr["Naziv"];

            entity.Naziv = naziv;
		}

		protected override string selectAllSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Mesta ";
		}
		
		protected override string insertSQL()
		{
			return "INSERT INTO Mesta(ZIP, Naziv) VALUES (?, ?)";
		}

        protected override void addInsertParameters(Mesto entity, OleDbCommand cmd)
		{
            addKeyParameters(entity, cmd);
            addDataParameters(entity, cmd);
		}

		private void addKeyParameters(Mesto m, OleDbCommand cmd)
		{
			cmd.Parameters.Add(ZIP, OleDbType.VarWChar, Mesto.ZIP_MAX_LENGTH);
			cmd.Parameters[ZIP].Value = m.Zip;
		}

		private void addDataParameters(Mesto m, OleDbCommand cmd)
		{
			cmd.Parameters.Add(NAZIV, OleDbType.VarWChar, Mesto.NAZIV_MAX_LENGTH);
			
			if (m.Naziv == null)
				cmd.Parameters[NAZIV].Value = DBNull.Value;
			else
				cmd.Parameters[NAZIV].Value = m.Naziv;
		}

		protected override string updateSQL()
		{
			return "UPDATE Mesta SET Naziv = ? WHERE ZIP = ?";
		}

        protected override void addUpdateParameters(Mesto entity, OleDbCommand cmd)
		{
            addDataParameters(entity, cmd);
            addKeyParameters(entity, cmd);
		}

		// update kada je promenjen kljuc
		public bool update(Mesto m, string oldZip)
		{
			if (String.IsNullOrEmpty(oldZip))
				throw new ArgumentException();

			// NOTE: Izgleda da je u Access bazi za ON UPDATE postavljeno CASCADE
			// jer kada se promeni Zip (primary key) za mesto, automatski se menjaju
			// odgovarajuce kolone u tabelama Clanovi i Institucije. Tako da ne mora
			// da se to radi u kodu.

			string updateMesto = "UPDATE Mesta SET ZIP = ?, Naziv = ? " +
				"WHERE ZIP = ?";
				
			OleDbCommand cmd = new OleDbCommand(updateMesto);
			addKeyParameters(m, cmd);
			addDataParameters(m, cmd);

			cmd.Parameters.Add(OLD_ZIP, OleDbType.VarWChar, Mesto.ZIP_MAX_LENGTH)
				.Value = oldZip;

			int recordsAffected = executeNonQuery(cmd);
			bool success = recordsAffected == 1;
			if (success)
			{
				Key oldKey = new Key(oldZip);
				if (loadedMap.ContainsKey(oldKey))
					loadedMap.Remove(oldKey);
				loadedMap.Add(m.Key, m);
			}
			return success;
		}

		protected override string deleteSQL()
		{
			return "DELETE FROM Mesta WHERE ZIP = ?";
		}

		protected override void addDeleteParameters(Mesto entity, OleDbCommand cmd)
		{
			addKeyParameters(entity, cmd);
		}

		// can throw InfrastructureException
		public bool existsMestoNaziv(string naziv)
		{
			string selectCountByNaziv = "SELECT Count(*) " +
				"FROM Mesta " +
				"WHERE Naziv = ?";

			OleDbCommand cmd = new OleDbCommand(selectCountByNaziv);
			cmd.Parameters.Add(NAZIV, OleDbType.VarWChar, Mesto.NAZIV_MAX_LENGTH)
				.Value = naziv;
			return (int)executeScalar(cmd) > 0;
		}
	}
}
