using System;
using System.Data.OleDb;
using Soko.Domain;

namespace Soko.Dao
{
	/// <summary>
	/// Summary description for ClanDAO.
	/// </summary>
	public class ClanDAO : DAO<Clan>
	{
		private readonly string ID_CLANA = "@ID_clana";
		private readonly string BROJ = "@Broj";
		private readonly string IME = "@Ime";
		private readonly string PREZIME = "@Prezime";
		private readonly string DATUM_RODJ = "@Datum_rodjenja";
		private readonly string ADRESA = "@Adresa";
		private readonly string MESTO = "@Mesto";
		private readonly string TELEFON1 = "@Telefon1";
		private readonly string TELEFON2 = "@Telefon2";
		private readonly string ID_INSTITUCIJE = "@ID_institucije";
		private readonly string NAPOMENA = "@Napomena";

		public ClanDAO()
		{

		}

		private readonly string COLUMNS = " [ID clana], Broj, Ime, Prezime, " +
			"[Datum rodjenja], Adresa, Mesto, Telefon1, Telefon2, [ID institucije], " +
			"Napomena ";

		protected override string getByIdSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Clanovi " +
				" WHERE [ID clana] = ?";
		}
		
		public Clan getById(int id)
		{
			return getById(new Key(id));
		}

        protected override Clan createEntity()
		{
			return new Clan();
		}

        protected override void loadData(Clan entity, OleDbDataReader rdr)
		{
			Nullable<int> broj = null;
			if (!Convert.IsDBNull(rdr["Broj"]))
				broj = (int)rdr["Broj"];

			string ime = null;
			if (!Convert.IsDBNull(rdr["Ime"]))
				ime = (string)rdr["Ime"];

			string prezime = null;
			if (!Convert.IsDBNull(rdr["Prezime"]))
				prezime = (string)rdr["Prezime"];

			Nullable<DateTime> datumRodj = null;
			if (!Convert.IsDBNull(rdr["Datum rodjenja"]))
				datumRodj = (DateTime)rdr["Datum rodjenja"];

			string adresa = null;
			if (!Convert.IsDBNull(rdr["Adresa"]))
				adresa = (string)rdr["Adresa"];

			Mesto mesto = null;
			if (!Convert.IsDBNull(rdr["Mesto"]))
			{
				string zip = (string)rdr["Mesto"];
				mesto = MapperRegistry.mestoDAO().getById(zip);
			}
			
			string telefon1 = null;
			if (!Convert.IsDBNull(rdr["Telefon1"]))
				telefon1 = (string)rdr["Telefon1"];

			string telefon2 = null;
			if (!Convert.IsDBNull(rdr["Telefon2"]))
				telefon2 = (string)rdr["Telefon2"];

			Institucija inst = null;
			if (!Convert.IsDBNull(rdr["ID institucije"]))
			{
				int id = (int)rdr["ID institucije"];
				inst = MapperRegistry.institucijaDAO().getById(id);
			}
			
			string napomena = null;
			if (!Convert.IsDBNull(rdr["Napomena"]))
				napomena = (string)rdr["Napomena"];

            entity.Broj = broj;
            entity.Ime = ime;
            entity.Prezime = prezime;
            entity.DatumRodjenja = datumRodj;
            entity.Adresa = adresa;
            entity.Mesto = mesto;
            entity.Telefon1 = telefon1;
            entity.Telefon2 = telefon2;
            entity.Institucija = inst;
            entity.Napomena = napomena;
		}

		protected override string selectAllSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Clanovi";
		}

		protected override string insertSQL()
		{
			return "INSERT INTO Clanovi(Broj, Ime, Prezime, [Datum rodjenja], Adresa, Mesto, Telefon1" +
				", Telefon2, [ID institucije], Napomena) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
		}

        protected override void addInsertParameters(Clan entity, OleDbCommand cmd)
		{
            addDataParameters(entity, cmd);
		}

		private void addDataParameters(Clan c, OleDbCommand cmd)
		{
			cmd.Parameters.Add(BROJ, OleDbType.Integer);
			cmd.Parameters.Add(IME, OleDbType.VarWChar, 20);
			cmd.Parameters.Add(PREZIME, OleDbType.VarWChar, 30);
			cmd.Parameters.Add(DATUM_RODJ, OleDbType.DBDate);
			cmd.Parameters.Add(ADRESA, OleDbType.VarWChar, 70);
			cmd.Parameters.Add(MESTO, OleDbType.VarWChar, 5);
			cmd.Parameters.Add(TELEFON1, OleDbType.VarWChar, 20);
			cmd.Parameters.Add(TELEFON2, OleDbType.VarWChar, 20);
			cmd.Parameters.Add(ID_INSTITUCIJE, OleDbType.Integer);
			cmd.Parameters.Add(NAPOMENA, OleDbType.VarWChar, 255);

			if (c.Broj == null)
				cmd.Parameters[BROJ].Value = DBNull.Value;
			else
				cmd.Parameters[BROJ].Value = c.Broj.Value;

			if (c.Ime == null)
				cmd.Parameters[IME].Value = DBNull.Value;
			else
				cmd.Parameters[IME].Value = c.Ime;

			if (c.Prezime == null)
				cmd.Parameters[PREZIME].Value = DBNull.Value;
			else
				cmd.Parameters[PREZIME].Value = c.Prezime;

			if (c.DatumRodjenja == null)
				cmd.Parameters[DATUM_RODJ].Value = DBNull.Value;
			else
				cmd.Parameters[DATUM_RODJ].Value = c.DatumRodjenja.Value;

			if (c.Adresa == null)
				cmd.Parameters[ADRESA].Value = DBNull.Value;
			else
				cmd.Parameters[ADRESA].Value = c.Adresa;

			if (c.Mesto == null)
				cmd.Parameters[MESTO].Value = DBNull.Value;
			else
				cmd.Parameters[MESTO].Value = c.Mesto.Zip;

			if (c.Telefon1 == null)
				cmd.Parameters[TELEFON1].Value = DBNull.Value;
			else
				cmd.Parameters[TELEFON1].Value = c.Telefon1;

			if (c.Telefon2 == null)
				cmd.Parameters[TELEFON2].Value = DBNull.Value;
			else
				cmd.Parameters[TELEFON2].Value = c.Telefon2;

			if (c.Institucija == null)
				cmd.Parameters[ID_INSTITUCIJE].Value = DBNull.Value;
			else
				cmd.Parameters[ID_INSTITUCIJE].Value = c.Institucija.Key.intValue();

			if (c.Napomena == null)
				cmd.Parameters[NAPOMENA].Value = DBNull.Value;
			else
				cmd.Parameters[NAPOMENA].Value = c.Napomena;

		}

		protected override string updateSQL()
		{
			return "UPDATE Clanovi SET Broj = ?, Ime = ?, Prezime = ?, " +
				"[Datum rodjenja] = ?, Adresa = ?, Mesto = ?, Telefon1 = ?, " +
				"Telefon2 = ?, [ID institucije] = ?, Napomena = ? WHERE [ID clana] = ?";
		}

        protected override void addUpdateParameters(Clan entity, OleDbCommand cmd)
		{
            addDataParameters(entity, cmd);
            addKeyParameters(entity, cmd);
		}

		private void addKeyParameters(Clan c, OleDbCommand cmd)
		{			
			cmd.Parameters.Add(ID_CLANA, OleDbType.Integer);
			cmd.Parameters[ID_CLANA].Value = c.Key.intValue();		
		}
		
		protected override string deleteSQL()
		{
			return "DELETE FROM Clanovi WHERE [ID clana] = ?";
		}

		// can throw InfrastructureException
		public bool existsClanMesto(string zip)
		{
			string selectCountByMesto = "SELECT Count(*) " +
				"FROM Clanovi " +
				"WHERE Mesto = ? ";

			OleDbCommand cmd = new OleDbCommand(selectCountByMesto);
			cmd.Parameters.Add(MESTO, OleDbType.VarWChar, Mesto.ZIP_MAX_LENGTH).Value = zip;
			return (int)executeScalar(cmd) > 0;
		}
	
		public bool existsClanInstitucija(int idInstitucije)
		{
			string selectCountByInstitucija = "SELECT Count(*) " +
				"FROM Clanovi " +
				"WHERE [ID institucije] = ? ";

			OleDbCommand cmd = new OleDbCommand(selectCountByInstitucija);
			cmd.Parameters.Add(ID_INSTITUCIJE, OleDbType.Integer).Value = idInstitucije;
			return (int)executeScalar(cmd) > 0;
		}

		public int getMaxBroj()
		{
			string selectMaxBroj = "SELECT Max(Broj) FROM Clanovi";
			OleDbCommand cmd = new OleDbCommand(selectMaxBroj);
			object result = executeScalar(cmd);
			if (result != null)
				return (int)result;
			else
				return 0;
		}
	}
}
