using System;
using System.Data.OleDb;
using Soko.Domain;
using System.Collections.Generic;

namespace Soko.Dao
{
	/// <summary>
	/// Summary description for MesecnaClanarinaDAO.
	/// </summary>
    public class MesecnaClanarinaDAO : DAO<MesecnaClanarina>
	{
		private readonly string GRUPA = "@Grupa";
		private readonly string DATUM = "@Datum";
		private readonly string CENA = "@Cena";
		
		public MesecnaClanarinaDAO()
		{

		}

		private readonly string COLUMNS = " Grupa, Datum, Cena ";

		protected override string getByIdSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Cenovnik " +
				" WHERE Grupa = ? AND Datum = ? ";
		}
		
		public MesecnaClanarina getById(SifraGrupe sifraGrupe, DateTime vaziOd)
		{
			return getById(new Key(sifraGrupe, vaziOd));
		}

		protected override void addIdParameter(Key key, OleDbCommand cmd)
		{
			cmd.Parameters.Add(GRUPA, OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH)
				.Value = ((SifraGrupe)key.Value(0)).Value;
			cmd.Parameters.Add(DATUM, OleDbType.DBDate)
				.Value = (DateTime)key.Value(1);
		}

		public List<MesecnaClanarina> getCenovnik()
		{
			string selectCenovnik = "SELECT C1.Grupa, C1.Datum, C1.Cena " +
			"FROM Cenovnik C1 " +
			"WHERE C1.Datum = " +
			"   (SELECT Max(Datum) " +
			"   FROM Cenovnik C2 " +
			"   WHERE C2.Grupa = C1.Grupa)";

			OleDbCommand cmd = new OleDbCommand(selectCenovnik);
			OleDbDataReader rdr = executeReader(cmd);
			List<MesecnaClanarina> result = loadAll(rdr);
			rdr.Close(); // obavezno, da bi se zatvorila konekcija otvorena u executeReader
			return result;
		}

		public List<MesecnaClanarina> getAllForGrupa(string sifraGrupe)
		{
			string selectCenovnikByGrupa = "SELECT Grupa, Datum, Cena " +
				"FROM Cenovnik " +
				"WHERE Grupa = ? " +
				"ORDER BY Datum DESC";

			OleDbCommand cmd = new OleDbCommand(selectCenovnikByGrupa);
			cmd.Parameters.Add("Grupa", OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH)
				.Value = sifraGrupe;

			OleDbDataReader rdr = executeReader(cmd);
			List<MesecnaClanarina> result = loadAll(rdr);
			rdr.Close();
			return result;
		}

		public MesecnaClanarina getVazeciForGrupa(string sifraGrupe)
		{
			string selectForGrupa = "SELECT C1.Grupa, C1.Datum, C1.Cena " +
				"FROM Cenovnik C1 " +
				"WHERE C1.Grupa = ? AND C1.Datum = " +
				"   (SELECT Max(Datum) " +
				"   FROM Cenovnik C2 " +
				"   WHERE C2.Grupa = C1.Grupa)";

			OleDbCommand cmd = new OleDbCommand(selectForGrupa);
			cmd.Parameters.Add("Grupa", OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH)
				.Value = sifraGrupe;

			OleDbDataReader rdr = executeReader(cmd);
			MesecnaClanarina result = null;
			if (rdr.Read())
				result = (MesecnaClanarina)load(rdr);
			rdr.Close();
			return result;
		}

		protected override Key loadKey(OleDbDataReader rdr)
		{
			SifraGrupe sifraGrupe = new SifraGrupe((string)rdr["Grupa"]);
			DateTime datum = (DateTime)rdr["Datum"];
			return new Key(sifraGrupe, datum);
		}

        protected override MesecnaClanarina createEntity()
		{
			return new MesecnaClanarina();
		}

        protected override void loadData(MesecnaClanarina entity, OleDbDataReader rdr)
		{
			Grupa grupa = null;
			if (!Convert.IsDBNull(rdr["Grupa"]))
			{
				SifraGrupe sifraGrupe = new SifraGrupe((string)rdr["Grupa"]);
				grupa = MapperRegistry.grupaDAO().getById(sifraGrupe);
			}
			
			Nullable<decimal> iznos = null;
			if (!Convert.IsDBNull(rdr["Cena"]))
				iznos = (decimal)rdr["Cena"];

            entity.Grupa = grupa;
            entity.Iznos = iznos;
        }

		protected override string selectAllSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Cenovnik ";
		}
		
		protected override string insertSQL()
		{
			return "INSERT INTO Cenovnik(Grupa, Datum, Cena) VALUES (?, ?, ?)";
		}

        protected override void addInsertParameters(MesecnaClanarina entity, OleDbCommand cmd)
		{
            addKeyParameters(entity, cmd);
            addDataParameters(entity, cmd);
		}

		private void addKeyParameters(MesecnaClanarina mc, OleDbCommand cmd)
		{
			cmd.Parameters.Add(GRUPA, OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH)
				.Value = mc.Grupa.Sifra.Value;
			cmd.Parameters.Add(DATUM, OleDbType.DBDate)
				.Value = mc.VaziOd;
		}

		private void addDataParameters(MesecnaClanarina mc, OleDbCommand cmd)
		{
			cmd.Parameters.Add(CENA, OleDbType.Currency);
			
			if (mc.Iznos != null)
				cmd.Parameters[CENA].Value = mc.Iznos.Value;
			else
				cmd.Parameters[CENA].Value = DBNull.Value;
		}

		protected override string updateSQL()
		{
			throw new NotSupportedException();
		}

		protected override void addUpdateParameters(MesecnaClanarina entity, OleDbCommand cmd)
		{
			throw new NotSupportedException();
		}

		protected override string deleteSQL()
		{
			return "DELETE FROM Cenovnik WHERE (Grupa = ?) AND (Datum = ?)";
		}

		protected override void addDeleteParameters(MesecnaClanarina entity, OleDbCommand cmd)
		{
			addKeyParameters(entity, cmd);
		}

		public bool existsClanarinaGrupa(string sifraGrupe)
		{
			string selectCountByGrupa = "SELECT Count(*) " +
				"FROM Cenovnik " +
				"WHERE Grupa = ? ";

			OleDbCommand cmd = new OleDbCommand(selectCountByGrupa);
			cmd.Parameters.Add("@Grupa", OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH)
				.Value = sifraGrupe;
			return (int)executeScalar(cmd) > 0;
		}

		private readonly string selectCenovnikReportItems = 
@"SELECT C1.Grupa, G.Naziv, C1.Cena, C1.Datum
FROM Cenovnik C1 INNER JOIN Grupe G
	ON C1.Grupa = G.Sifra
WHERE C1.Datum =
	(SELECT Max(Datum)
	FROM Cenovnik C2
	WHERE C2.Grupa = C1.Grupa)
ORDER BY G.[Sort order]";

        public List<object[]> getCenovnikReportItems()
		{
			OleDbCommand cmd = new OleDbCommand(selectCenovnikReportItems);
			OleDbDataReader rdr = executeReader(cmd);
            List<object[]> result = loadGrupaNazivDatumCena(rdr);
			rdr.Close();
			return result;
		}

        private List<object[]> loadGrupaNazivDatumCena(OleDbDataReader rdr)
		{
			int gruOrd = rdr.GetOrdinal("Grupa");
			int nazOrd = rdr.GetOrdinal("Naziv");
			int cenOrd = rdr.GetOrdinal("Cena");
			int datOrd = rdr.GetOrdinal("Datum");

            List<object[]> result = new List<object[]>();
			while (rdr.Read())
			{
				string grupa = rdr.GetString(gruOrd);
				string naziv = rdr.GetString(nazOrd);
				decimal cena = rdr.GetDecimal(cenOrd);
				DateTime datum = rdr.GetDateTime(datOrd);

				result.Add(new object[] { grupa, naziv, cena, datum });
			}
			return result;
		}
	}
}
