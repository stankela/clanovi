using System;
using System.Data.OleDb;
using Soko.Domain;
using Soko.Exceptions;
using System.Collections.Generic;

namespace Soko.Dao
{
	/// <summary>
	/// Summary description for GrupaDAO.
	/// </summary>
	public class GrupaDAO : DAO<Grupa>
	{
		private readonly string SIFRA = "@Sifra";
		private readonly string NAZIV = "@Naziv";
		private readonly string KATEGORIJA_ID = "@Kategorija";
		private readonly string SORT_ORDER = "@Sort_Order";
		private readonly string OLD_SIFRA = "@Old_Sifra";

		public GrupaDAO()
		{

		}

		private readonly string COLUMNS = " Sifra, Naziv, [Sort order], Kategorija ";
		
		protected override string getByIdSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Grupe " +
				" WHERE Sifra = ?";
		}
		
		public Grupa getById(SifraGrupe sifraGrupe)
		{
			return getById(new Key(sifraGrupe));
		}

		protected override void addIdParameter(Key key, OleDbCommand cmd)
		{
			cmd.Parameters.Add("@Id", OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH)
				.Value = ((SifraGrupe)key.Value()).Value;
		}

		protected override Key loadKey(OleDbDataReader rdr)
		{
			return new Key(new SifraGrupe((string)rdr["Sifra"]));
		}

        protected override Grupa createEntity()
		{
			return new Grupa();
		}

        protected override void loadData(Grupa entity, OleDbDataReader rdr)
		{
			string naziv = null;
			if (!Convert.IsDBNull(rdr["Naziv"]))
				naziv = (string)rdr["Naziv"];

			Kategorija kategorija = null;
			if (!Convert.IsDBNull(rdr["Kategorija"]))
			{
				int katId = (int)rdr["Kategorija"];
				kategorija = MapperRegistry.kategorijaDAO().getById(katId);
			}
			
			int sortOrder = 0;
			if (!Convert.IsDBNull(rdr["Sort order"]))
				sortOrder = (int)rdr["Sort order"];

            entity.Naziv = naziv;
            entity.Kategorija = kategorija;
            entity.SortOrder = sortOrder;
		}

		protected override string selectAllSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Grupe ";
		}
		
		protected override string insertSQL()
		{
			return "INSERT INTO Grupe(Sifra, Naziv, [Sort order], Kategorija) VALUES (?, ?, ?, ?)";
		}

        protected override void addInsertParameters(Grupa entity, OleDbCommand cmd)
		{
            addKeyParameters(entity, cmd);
            addDataParameters(entity, cmd);
		}

		private void addKeyParameters(Grupa g, OleDbCommand cmd)
		{
			cmd.Parameters.Add(SIFRA, OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH);
			cmd.Parameters[SIFRA].Value = g.Sifra.Value;
		}

		private void addDataParameters(Grupa g, OleDbCommand cmd)
		{
			cmd.Parameters.Add(NAZIV, OleDbType.VarWChar, Grupa.NAZIV_MAX_LENGTH);
			cmd.Parameters.Add(SORT_ORDER, OleDbType.Integer);
			cmd.Parameters.Add(KATEGORIJA_ID, OleDbType.Integer);
		
			if (g.Naziv == null)
				cmd.Parameters[NAZIV].Value = DBNull.Value;
			else
				cmd.Parameters[NAZIV].Value = g.Naziv;

			cmd.Parameters[SORT_ORDER].Value = g.SortOrder;
	
			if (g.Kategorija == null)
				cmd.Parameters[KATEGORIJA_ID].Value = DBNull.Value;
			else
				cmd.Parameters[KATEGORIJA_ID].Value = g.Kategorija.Key.intValue();
		}

		protected override string updateSQL()
		{
			return "UPDATE Grupe SET Naziv = ?, [Sort order] = ?, Kategorija = ? " +
				"WHERE Sifra = ?";
		}

        protected override void addUpdateParameters(Grupa entity, OleDbCommand cmd)
		{
            addDataParameters(entity, cmd);
            addKeyParameters(entity, cmd);
		}

		protected override string deleteSQL()
		{
			return "DELETE FROM Grupe WHERE Sifra = ?";
		}

		protected override void addDeleteParameters(Grupa entity, OleDbCommand cmd)
		{
			addKeyParameters(entity, cmd);
		}

		public bool existsGrupa(int kategorijaId)
		{
			string selectCountByKategorija = "SELECT Count(*) " +
				"FROM Grupe " +
				"WHERE Kategorija = ?";

			OleDbCommand cmd = new OleDbCommand(selectCountByKategorija);
			cmd.Parameters.Add(KATEGORIJA_ID, OleDbType.Integer)
				.Value = kategorijaId;
			return (int)executeScalar(cmd) > 0;
		}

		public bool existsGrupaNaziv(string naziv)
		{
			string selectCountByNaziv = "SELECT Count(*) " +
				"FROM Grupe " +
				"WHERE Naziv = ?";

			OleDbCommand cmd = new OleDbCommand(selectCountByNaziv);
			cmd.Parameters.Add(NAZIV, OleDbType.VarWChar, Grupa.NAZIV_MAX_LENGTH)
				.Value = naziv;
			return (int)executeScalar(cmd) > 0;
		}

		// update kada je promenjen kljuc
		public bool update(Grupa g, SifraGrupe oldSifra)
		{
			if (oldSifra == null)
				throw new ArgumentException();

			string updateGrupa = "UPDATE Grupe SET Sifra = ?, Naziv = ?, " +
				"[Sort order] = ?, Kategorija = ? " +
				"WHERE Sifra = ?";
				
			OleDbCommand cmd = new OleDbCommand(updateGrupa);
			addKeyParameters(g, cmd);
			addDataParameters(g, cmd);

			cmd.Parameters.Add(OLD_SIFRA, OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH)
				.Value = oldSifra.Value;

			OleDbConnection conn = new OleDbConnection(ConfigurationParameters.ConnectionString);
			OleDbTransaction tr = null;
			try
			{
				conn.Open(); // can throw
				tr = conn.BeginTransaction();

				if (executeNonQuery(cmd, tr) != 1) // can throw
					return false;
				if (!updateSortOrder(tr))
					return false;

				tr.Commit();
		
				Key oldKey = new Key(oldSifra);
                if (loadedMap.ContainsKey(oldKey))
                    loadedMap.Remove(oldKey);
                if (!loadedMap.ContainsKey(g.Key))
				{
					// NOTE: Proverava se da li je entitet vec u mapi zato sto je
					// mogao da dospe tamo u metodu updateSortOrder
					loadedMap.Add(g.Key, g);
				}
				return true;
			}
			catch (OleDbException e)
			{
				// in Open()
				if (tr != null)
					tr.Rollback(); // TODO: this can throw Exception and InvalidOperationException
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			catch (InvalidOperationException e)
			{
				// in Commit
				if (tr != null)
					tr.Rollback();
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			catch (InfrastructureException)
			{
				// in executeNonQuery, executeScalar
				if (tr != null)
					tr.Rollback();
				throw;
			}
			catch (Exception e)
			{
				// in Commit
				if (tr != null)
					tr.Rollback();
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			finally
			{
				conn.Close();
			}
		}

	
		protected override bool onEntityInserted(Grupa entity, OleDbTransaction tr)
		{
			return updateSortOrder(tr);
		}

		private bool updateSortOrder(OleDbTransaction tr)
		{
			string updateSortOrder = "UPDATE Grupe SET [Sort order] = ? " +
				"WHERE Sifra = ?";
			
			List<Grupa> grupe = getAll(tr);
			grupe.Sort();
			for (int i = 0; i < grupe.Count; i++)
                grupe[i].SortOrder = i;

			foreach (Grupa g in grupe)
			{
				OleDbCommand cmd = new OleDbCommand(updateSortOrder);
				cmd.Parameters.Add(SORT_ORDER, OleDbType.Integer).Value = g.SortOrder;
				cmd.Parameters.Add(SIFRA, OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH)
					.Value = g.Sifra.Value;
				if (executeNonQuery(cmd, tr) != 1) // can throw
					return false;
			}
			return true;
		}
	}
}
