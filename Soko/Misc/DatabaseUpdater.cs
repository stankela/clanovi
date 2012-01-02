using System;
using Soko.Dao;
using Soko.Domain;
using System.Collections.Generic;

namespace Soko
{
	/// <summary>
	/// Summary description for DatabaseUpdater.
	/// </summary>
	public class DatabaseUpdater
	{
		public DatabaseUpdater()
		{

		}

        public void updateDatabase(List<string> messages)
		{
			string katTable = "Kategorije";
            // NOTE: Ovde sam DAO parametrizovao sa proizvoljnim tipom (DomainObject u 
            // ovom slucaju) da bih mogao da koristim staticke metode u klasi DAO
			if (DAO<DomainObject>.createTable(katTable, "([ID Kategorije] IDENTITY (1, 1) CONSTRAINT pkKategorije PRIMARY KEY, Naziv TEXT(50) NOT NULL CONSTRAINT unNaziv UNIQUE)"))
			{
				string msg = "Created table '" + katTable + "'.";
				messages.Add(msg);
			}

			string grupeTable = "Grupe";
			string sortOrderColumn = "Sort order";
            if (DAO<DomainObject>.addColumn(grupeTable, sortOrderColumn, "INTEGER"))
			{
				string msg = "Added column '" + grupeTable + "." + sortOrderColumn + "'.";
				messages.Add(msg);
			}

			string katColumn = "Kategorija";
            if (DAO<DomainObject>.addColumn(grupeTable, katColumn, "INTEGER"))
			{
				string msg = "Added column '" + grupeTable + "." + katColumn + "'.";
				messages.Add(msg);
			}
		}

	}
}
