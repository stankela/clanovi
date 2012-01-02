using System;
using System.Data.OleDb;
using Soko.Domain;

namespace Soko.Dao
{
	/// <summary>
	/// Summary description for InstitucijaDAO.
	/// </summary>
    public class InstitucijaDAO : DAO<Institucija>
	{
		private readonly string NAZIV = "@Naziv";
		private readonly string ADRESA = "@Adresa";
		private readonly string MESTO = "@Mesto";
		private readonly string ID_INSTITUCIJE = "@ID_Institucije";
		
		public InstitucijaDAO()
		{

		}

		private readonly string COLUMNS = " [ID institucije], Naziv, Adresa, Mesto ";

		protected override string getByIdSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Institucije " +
				" WHERE [ID institucije] = ?";
		}
		
		public Institucija getById(int id)
		{
			return getById(new Key(id));
		}

        protected override Institucija createEntity()
		{
			return new Institucija();
		}

		protected override string selectAllSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Institucije";
		}

		protected override string insertSQL()
		{
			return "INSERT INTO Institucije(Naziv, Adresa, Mesto) VALUES (?, ?, ?)";
		}

        protected override void addInsertParameters(Institucija entity, OleDbCommand cmd)
		{
            addDataParameters(entity, cmd);
		}

		private void addDataParameters(Institucija inst, OleDbCommand cmd)
		{
			cmd.Parameters.Add(NAZIV, OleDbType.VarWChar, Institucija.NAZIV_MAX_LENGTH);
			cmd.Parameters.Add(ADRESA, OleDbType.VarWChar, Institucija.ADRESA_MAX_LENGTH);
			cmd.Parameters.Add(MESTO, OleDbType.VarWChar, Mesto.ZIP_MAX_LENGTH);
			
			if (inst.Naziv == null)
				cmd.Parameters[NAZIV].Value = DBNull.Value;
			else
				cmd.Parameters[NAZIV].Value = inst.Naziv;
			
			// NOTE: U Access bazi je specifikovano da Address nije obavezno, ali da
			// ako je zadato duzina mora da mu bude veca od nula. Zbog toga se u
			// slucaju kada je inst.Adresa == String.Empty u bazu upisuje null
			if (String.IsNullOrEmpty(inst.Adresa))
				cmd.Parameters[ADRESA].Value = DBNull.Value;
			else
				cmd.Parameters[ADRESA].Value = inst.Adresa;

			if (inst.Mesto == null)
				cmd.Parameters[MESTO].Value = DBNull.Value;
			else
				cmd.Parameters[MESTO].Value = inst.Mesto.Zip;
		}

		protected override string updateSQL()
		{
			return "UPDATE Institucije SET Naziv = ?, Adresa = ?, Mesto = ? " +
				"WHERE [ID institucije] = ?";
		}

        protected override void addUpdateParameters(Institucija entity, OleDbCommand cmd)
		{
            addDataParameters(entity, cmd);
            addKeyParameters(entity, cmd);
		}
		
		private void addKeyParameters(Institucija inst, OleDbCommand cmd)
		{			
			cmd.Parameters.Add(ID_INSTITUCIJE, OleDbType.Integer);
			cmd.Parameters[ID_INSTITUCIJE].Value = inst.Key.intValue();		
		}
		
		protected override string deleteSQL()
		{
			return "DELETE FROM Institucije WHERE [ID institucije] = ?";
		}

        protected override void loadData(Institucija entity, OleDbDataReader rdr)
		{
			string naziv = null;
			if (!Convert.IsDBNull(rdr["Naziv"]))
				naziv = (string)rdr["Naziv"];
			
			string adresa = null;
			if (!Convert.IsDBNull(rdr["Adresa"]))
				adresa = (string)rdr["Adresa"];
			
			Mesto mesto = null;
			if (!Convert.IsDBNull(rdr["Mesto"]))
			{
				string zip = (string)rdr["Mesto"];
				mesto = MapperRegistry.mestoDAO().getById(zip);
			}

            entity.Naziv = naziv;
            entity.Adresa = adresa;
            entity.Mesto = mesto;
		}

		// can throw InfrastructureException
		public bool existsInstitucijaZip(string zip)
		{
			string selectCountByMesto = "SELECT Count(*) " +
				"FROM Institucije " +
				"WHERE Mesto = ? ";

			OleDbCommand cmd = new OleDbCommand(selectCountByMesto);
			cmd.Parameters.Add(MESTO, OleDbType.VarWChar, Mesto.ZIP_MAX_LENGTH)
				.Value = zip;
			return (int)executeScalar(cmd) > 0;
		}

		public bool existsInstitucijaNaziv(string naziv)
		{
			string selectCountByNaziv = "SELECT Count(*) " +
				"FROM Institucije " +
				"WHERE Naziv = ?";

			OleDbCommand cmd = new OleDbCommand(selectCountByNaziv);
			cmd.Parameters.Add(NAZIV, OleDbType.VarWChar, Institucija.NAZIV_MAX_LENGTH)
				.Value = naziv;
			return (int)executeScalar(cmd) > 0;
		}
	}
}
