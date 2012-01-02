using System;
using System.Data;
using System.Data.OleDb;
using Soko.Domain;
using Soko.Exceptions;
using System.Collections.Generic;

namespace Soko.Dao
{
	/// <summary>
	/// Summary description for DAO.
	/// </summary>
	public abstract class DAO<T> where T : DomainObject
	{
		protected IDictionary<Key, T> loadedMap = new Dictionary<Key, T>();

		public DAO()
		{

		}

	//	private static int numCalls;

		protected OleDbDataReader executeReader(OleDbCommand cmd)
		{
//#if DEBUG
//			string s = string.Format("{0} {1}   {2}", ++numCalls, this.GetType().Name, 
//				cmd.CommandText);
//			Console.WriteLine(s);
//#endif

			OleDbConnection conn = new OleDbConnection(ConfigurationParameters.ConnectionString);
			cmd.Connection = conn;
			try
			{
				conn.Open();
				// CommandBehavior.CloseConnection znaci da kada se DataReader zatvori
				// zatvara se i konekcija
				return cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (OleDbException e)
			{
				// in Open()
				conn.Close();
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			catch (InvalidOperationException e)
			{
				// in ExecuteReader()
				conn.Close();
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
		}
	
		protected OleDbDataReader executeReader(OleDbCommand cmd, OleDbTransaction tr)
		{
			try
			{
				cmd.Connection = tr.Connection;
				cmd.Transaction = tr;
				return cmd.ExecuteReader();
			}
			catch (OleDbException e)
			{
				// in Open()
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			catch (InvalidOperationException e)
			{
				// in ExecuteReader()
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
		}
	
		protected object executeScalar(OleDbCommand cmd)
		{
			OleDbConnection conn = new OleDbConnection(ConfigurationParameters.ConnectionString);
			cmd.Connection = conn;
			try
			{
				conn.Open();
				object result = cmd.ExecuteScalar();
				if (!Convert.IsDBNull(result))
					return result;
				else
					return null;
			}
			catch (OleDbException e)
			{
				// in Open()
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			catch (InvalidOperationException e)
			{
				// in ExecuteScalar()
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			finally
			{
				conn.Close();
			}
		}
		
		protected object executeScalar(OleDbCommand cmd, OleDbTransaction tr)
		{
			try
			{
				cmd.Connection = tr.Connection;
				cmd.Transaction = tr;
				return cmd.ExecuteScalar();
			}
			catch (OleDbException e)
			{
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			catch (InvalidOperationException e)
			{
				// in ExecuteScalar(), Connection
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
		}
		
		protected static int executeNonQuery(OleDbCommand cmd)
		{
			OleDbConnection conn = new OleDbConnection(ConfigurationParameters.ConnectionString);
			cmd.Connection = conn;
			try
			{
				conn.Open();
				return cmd.ExecuteNonQuery();
			}
			catch (OleDbException e)
			{
				// in Open()
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			catch (InvalidOperationException e)
			{
				// in ExecuteNonQuery()
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			finally
			{
				conn.Close();
			}
		}

		protected int executeNonQuery(OleDbCommand cmd, OleDbTransaction tr)
		{
			try
			{
				cmd.Connection = tr.Connection;
				cmd.Transaction = tr;
				return cmd.ExecuteNonQuery();
			}
			catch (InvalidOperationException e)
			{
				// in ExecuteNonQuery(), Connection
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
		}

		// can throw InfrastructureException
		public T getById(Key key)
		{
			if (loadedMap.ContainsKey(key))
				return loadedMap[key];
			
			OleDbCommand cmd = new OleDbCommand(getByIdSQL());
			addIdParameter(key, cmd);
			OleDbDataReader rdr = executeReader(cmd);
			T result = null;
			if (rdr.Read())
				result = load(rdr);
			rdr.Close();
			return result;
		}

		// NOTE: Parametar za primarni kljuc treba da ima naziv "@Id"
		protected virtual void addIdParameter(Key key, OleDbCommand cmd)
		{
			cmd.Parameters.Add("@Id", OleDbType.Integer);
			cmd.Parameters["@Id"].Value = key.intValue();
		}

		protected T load(OleDbDataReader rdr)
		{
			Key id = loadKey(rdr);
			if (loadedMap.ContainsKey(id))
				return loadedMap[id];

			T result = createEntity();
			result.Key = id;
			loadedMap.Add(id, result);
			loadData(result, rdr);
			return result;
		}

		// NOTE: Kada je primarni kljuc integer, podrazumeva se da je prva kolona u
		// result setu
		protected virtual Key loadKey(OleDbDataReader rdr)
		{
			return new Key(rdr.GetInt32(0));
		}

		protected abstract string getByIdSQL();
		protected abstract T createEntity();
		protected abstract void loadData(T entity, OleDbDataReader rdr);
		protected abstract string selectAllSQL();
		protected abstract string insertSQL();
		protected abstract void addInsertParameters(T entity, OleDbCommand cmd);
		protected abstract string updateSQL();
		protected abstract void addUpdateParameters(T entity, OleDbCommand cmd);
		protected abstract string deleteSQL();

		// can throw InfrastructureException
		public List<T> getAll()
		{
			OleDbCommand cmd = new OleDbCommand(selectAllSQL());
			OleDbDataReader rdr = executeReader(cmd);
            List<T> result = loadAll(rdr);
			rdr.Close(); // istovremeno zatvara i konekciju otvorenu u executeReader
			return result;
		}

        protected List<T> getAll(OleDbTransaction tr)
		{
			OleDbCommand cmd = new OleDbCommand(selectAllSQL());
			OleDbDataReader rdr = executeReader(cmd, tr);
            List<T> result = loadAll(rdr);
			rdr.Close();
			return result;
		}

        protected List<T> loadAll(OleDbDataReader rdr)
		{
            List<T> result = new List<T>();
			while (rdr.Read())
			{
				result.Add(load(rdr));
			}
			return result;
		}

		public bool insert(T entity)
		{
			OleDbCommand cmd = new OleDbCommand(insertSQL());
			addInsertParameters(entity, cmd);

			OleDbConnection conn = new OleDbConnection(ConfigurationParameters.ConnectionString);
			OleDbTransaction tr = null;
			try
			{
				conn.Open(); // can throw
				tr = conn.BeginTransaction();
				if (executeNonQuery(cmd, tr) != 1) // can throw
					return false;

				if (entity.Key == null)
				{
					string selectIdentitySQL = "SELECT @@IDENTITY";
					OleDbCommand cmd2 = new OleDbCommand(selectIdentitySQL);
					entity.Key = new Key(executeScalar(cmd2, tr)); // can throw
				}

				if (!onEntityInserted(entity, tr))
					return false;

				tr.Commit();
				if (!loadedMap.ContainsKey(entity.Key))
				{
					// NOTE: Proverava se da li je entitet vec u mapi zato sto je
					// mogao da dospe tamo u metodu onEntityInserted
					// (npr. ako je onEntityInserted ucitao sve entitete)
					loadedMap.Add(entity.Key, entity);
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

		// hook metod koji omogucava da se obave dodatne radnje nakon sto je objekt
		// dodat u bazu. Vraca true ako su dodatne radnje uspesno obavljene,
		// u suprotnom false.
		protected virtual bool onEntityInserted(T entity, OleDbTransaction tr)
		{
			return true;
		}

		// can throw InfrastructureException
		public bool update(T entity)
		{				
			OleDbCommand cmd = new OleDbCommand(updateSQL());
			addUpdateParameters(entity, cmd);
			int recordsAffected = executeNonQuery(cmd);
			return recordsAffected == 1;
		}

		// can throw InfrastructureException
		public bool delete(T entity)
		{
			OleDbCommand cmd = new OleDbCommand(deleteSQL());
			addDeleteParameters(entity, cmd);
			int recordsAffected = executeNonQuery(cmd);

			if (recordsAffected == 1)
			{
				if (loadedMap.ContainsKey(entity.Key))
					loadedMap.Remove(entity.Key);
				return true;
			}
			else
				return false;
		}

		protected virtual void addDeleteParameters(T entity, OleDbCommand cmd)
		{
			cmd.Parameters.Add("@Id", OleDbType.Integer).Value = entity.Key.intValue();
		}


		// Data Definition

		public static bool createTable(string table, string fields)
		{
			if (!tableExists(table))
			{
				string sql = "CREATE TABLE [" + table + "] " + fields;
				OleDbCommand cmd = new OleDbCommand(sql);
				int r = executeNonQuery(cmd);
				return true;
			}
			return false;
		}

		public static bool tableExists(string table)
		{
			// Restrictions for OleDbSchemaGuid.Tables
			// TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE
			object[] restrictions = {null, null, null, "TABLE"};
			DataTable tbl = getOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);

			bool result = false;
			foreach(DataRow row in tbl.Rows)
				if (row["TABLE_NAME"].ToString() == table)
				{
					result = true;
					break;
				}
			return result;
		}

		public static bool addColumn(string table, string column, string typeSizeNull)
		{
			if (!columnExists(table, column))
			{
				string sql = "ALTER TABLE [" + table + "] ADD COLUMN [" + column + "] " + 
					typeSizeNull;
				OleDbCommand cmd = new OleDbCommand(sql);
				executeNonQuery(cmd);
				return true;
			}
			return false;
		}

		public static bool columnExists(string table, string column)
		{
			// Restrictions for OleDbSchemaGuid.Columns
			// TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME
			object[] restrictions = {null, null, table, null};
			DataTable tbl = getOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions);

			bool result = false;
			foreach(DataRow row in tbl.Rows)
				if (row["COLUMN_NAME"].ToString() == column)
				{
					result = true;
					break;
				}
			return result;
		}

		protected static DataTable getOleDbSchemaTable(Guid schema, 
			object[] restrictions)
		{
			OleDbConnection conn = new OleDbConnection(ConfigurationParameters.ConnectionString);
			try
			{
				conn.Open();
				return conn.GetOleDbSchemaTable(schema, restrictions);
			}
			catch (OleDbException e)
			{
				// in Open, GetOleDbSchemaTable
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			catch (InvalidOperationException e)
			{
				// in GetOleDbSchemaTable
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			catch (ArgumentException e)
			{
				// in GetOleDbSchemaTable
				throw new InfrastructureException(
					Strings.getFullDatabaseAccessExceptionMessage(e.Message), e);
			}
			finally
			{
				conn.Close();
			}
		}

	}
}
