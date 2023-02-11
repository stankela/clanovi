using System.Data.SqlServerCe;
using System;
using System.Data;
using Soko.Exceptions;
using System.Reflection;
using System.IO;
using Soko;

public class SqlCeUtilities
{
    public void CreateDatabase(string fileName, string password)
    {
        string connectionString;
        if (System.IO.File.Exists(fileName))
            return;
            //System.IO.File.Delete(fileName);

        connectionString = ConfigurationParameters.GetConnectionString(fileName, password);

        // we'll use the SqlServerCe connection object to get the database file path
        using (SqlCeConnection localConnection = new SqlCeConnection(connectionString))
        {
            // The SqlCeConnection.Database property contains the file parth portion
            // of the database from the full connectionstring
            if (!System.IO.File.Exists(localConnection.Database))
            {
                using (SqlCeEngine sqlCeEngine = new SqlCeEngine(connectionString))
                {
                    sqlCeEngine.CreateDatabase();
                    CreateInitialDatabaseObjects(connectionString);
                }
            }
        }
    }

    private void CreateInitialDatabaseObjects(string connectionString)
    {
        using (SqlCeConnection connection = new SqlCeConnection(connectionString))
        {
            // To simplify editing and testing TSQL statements,
            // the commands are placed in a managed resource of the dll.
            string script;            
            string resourceName = "Soko.create_all_objects.sqlce";
            System.Reflection.Assembly assem = this.GetType().Assembly;
            using (System.IO.Stream stream = assem.GetManifestResourceStream(resourceName))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    script = reader.ReadToEnd();
                }
            }

            // Using the SQL Management Studio convention of using GO to identify individual commands
            // Create a list of commands to execute
            string[] commands = script.Split(
                new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            SqlCeCommand cmd = new SqlCeCommand();
            cmd.Connection = connection;
            connection.Open();
            foreach (string command in commands)
            {
                string command2 = command.Trim();
                if (!String.IsNullOrEmpty(command2))
                {
                    cmd.CommandText = command2;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }

    public static void ExecuteScript(string fileName, string password, string scriptFile, bool embeddedResource)
    {
        string connectionString = ConfigurationParameters.GetConnectionString(fileName, password);
        using (SqlCeConnection connection = new SqlCeConnection(connectionString))
        {
            string script = String.Empty;
            if (embeddedResource)
            {
                Assembly assem = Assembly.GetExecutingAssembly();
                using (Stream stream = assem.GetManifestResourceStream(scriptFile))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        script = reader.ReadToEnd();
                    }
                }
            }
            else
            {
                using (StreamReader reader = new StreamReader(scriptFile))
                {
                    script = reader.ReadToEnd();
                }
            }

            // Using the SQL Management Studio convention of using GO to identify individual commands
            // Create a list of commands to execute
            string[] commands = script.Split(
                new string[] { "GO" }, StringSplitOptions.RemoveEmptyEntries);

            SqlCeCommand cmd = new SqlCeCommand();
            cmd.Connection = connection;
            connection.Open();
            foreach (string command in commands)
            {
                string command2 = command.Trim();
                if (!String.IsNullOrEmpty(command2))
                {
                    cmd.CommandText = command2;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public static SqlCeDataReader executeReader(SqlCeCommand cmd, string readErrorMsg, string connectionString = null)
    {
        if (connectionString == null)
            connectionString = ConfigurationParameters.ConnectionString;
        SqlCeConnection conn = new SqlCeConnection(connectionString);
        cmd.Connection = conn;
        try
        {
            conn.Open();
            // CommandBehavior.CloseConnection znaci da kada se DataReader zatvori
            // zatvara se i konekcija
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        catch (SqlCeException e)
        {
            // in Open()
            conn.Close();
            throw new InfrastructureException(readErrorMsg, e);
        }
        catch (InvalidOperationException e)
        {
            // in ExecuteReader()
            conn.Close();
            throw new InfrastructureException(readErrorMsg, e);
        }
    }

    public static bool tableExists(string tableName)
    {
        string sql = @"SELECT * FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_NAME = @TableName";
        SqlCeCommand cmd = new SqlCeCommand(sql);
        cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = tableName;

        string errMsg = "Greska prilikom citanja podataka iz baze.";
        SqlCeDataReader rdr = SqlCeUtilities.executeReader(cmd, errMsg);
        bool result = false;
        if (rdr.Read())
            result = true;
        rdr.Close();
        return result;
    }

    public static int getDatabaseVersionNumber()
    {
        string tableName = "verzija_baze";
        if (!SqlCeUtilities.tableExists(tableName))
        {
            return -1;
        }

        string sql = "SELECT broj_verzije FROM " + tableName +
            " WHERE verzija_id = 1";
        SqlCeCommand cmd = new SqlCeCommand(sql);

        string errMsg = "Greska prilikom citanja podataka iz baze.";
        SqlCeDataReader rdr = SqlCeUtilities.executeReader(cmd, errMsg);
        int result;
        if (rdr.Read())
            result = rdr.GetInt32(0);
        else
            result = 0;  // Prazna baza
        rdr.Close();
        return result;
    }

    public static void updateDatabaseVersionNumber(int newVersion)
    {
        string tableName = "verzija_baze";
        string sql =
            String.Format("UPDATE {0} SET broj_verzije = {1} WHERE verzija_id = 1", tableName, newVersion);
        SqlCeCommand cmd = new SqlCeCommand(sql);

        SqlCeConnection conn = new SqlCeConnection(ConfigurationParameters.ConnectionString);
        string errMsg = "Neuspesna promena baze.";
        SqlCeTransaction tr = null;

        // TODO: Ovaj kod se ponavlja na vise mesta u fajlu.
        try
        {
            conn.Open();
            tr = conn.BeginTransaction();

            cmd.Connection = conn;
            cmd.Transaction = tr;
            int result = cmd.ExecuteNonQuery();

            tr.Commit();
        }
        catch (SqlCeException e)
        {
            // in Open()
            if (tr != null)
                tr.Rollback(); // TODO: this can throw Exception and InvalidOperationException
            throw new InfrastructureException(errMsg, e);
        }
        catch (InvalidOperationException e)
        {
            // in ExecuteNonQuery(), ExecureScalar()
            if (tr != null)
                tr.Rollback();
            throw new InfrastructureException(errMsg, e);
        }
        // za svaki slucaj
        catch (Exception)
        {
            if (tr != null)
                tr.Rollback();
            throw;
        }
        finally
        {
            conn.Close();
        }
    }

    public static void dropReferentialConstraint(string table, string referencedTable)
    {
        string findConstraintSQL = @"
                SELECT CONSTRAINT_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS
                WHERE CONSTRAINT_TABLE_NAME = @Table AND UNIQUE_CONSTRAINT_TABLE_NAME = @ReferencedTable";
        SqlCeCommand cmd = new SqlCeCommand(findConstraintSQL);
        cmd.Parameters.Add("@Table", SqlDbType.NVarChar).Value = table;
        cmd.Parameters.Add("@ReferencedTable", SqlDbType.NVarChar).Value = referencedTable;

        string errMsg = "Greska prilikom citanja podataka iz baze.";
        SqlCeDataReader rdr = SqlCeUtilities.executeReader(cmd, errMsg);

        if (!rdr.Read())
            throw new Exception("Constraint does not exist.");

        string constraintName = (string)rdr["CONSTRAINT_NAME"];

        // NOTE: Izgleda da dodavanje parametara (pomocu @parameterName) radi samo kada je parametar sa desne
        // strane znaka jednakosti (kao u findConstraintSQL). Zato ovde koristim spajanje stringova.
        string dropConstraintSQL = "ALTER TABLE " + table + " DROP CONSTRAINT " + constraintName;

        SqlCeCommand cmd2 = new SqlCeCommand(dropConstraintSQL);

        SqlCeConnection conn = new SqlCeConnection(ConfigurationParameters.ConnectionString);
        errMsg = "Neuspesna promena baze.";
        SqlCeTransaction tr = null;
        try
        {
            conn.Open();
            tr = conn.BeginTransaction();

            cmd2.Connection = conn;
            cmd2.Transaction = tr;
            int result = cmd2.ExecuteNonQuery();

            tr.Commit();
        }
        catch (SqlCeException e)
        {
            // in Open()
            if (tr != null)
                tr.Rollback(); // TODO: this can throw Exception and InvalidOperationException
            throw new InfrastructureException(errMsg, e);
        }
        catch (InvalidOperationException e)
        {
            // in ExecuteNonQuery(), ExecureScalar()
            if (tr != null)
                tr.Rollback();
            throw new InfrastructureException(errMsg, e);
        }
        // za svaki slucaj
        catch (Exception)
        {
            if (tr != null)
                tr.Rollback();
            throw;
        }
        finally
        {
            conn.Close();
        }
    }
}