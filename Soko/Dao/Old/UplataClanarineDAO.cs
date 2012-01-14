using System;
using System.Data.OleDb;
using Soko.Domain;
using Soko.Report;
using System.Collections.Generic;

namespace Soko.Dao
{
	/// <summary>
	/// Summary description for UplataClanarineDAO.
	/// </summary>
    public class UplataClanarineDAO : DAO<UplataClanarine>
	{
		private readonly string ID_CLANA = "@Id_clana";
		private readonly string GRUPA = "@Grupa";
		private readonly string DATUM_UPLATE = "@Datum_uplate";
		private readonly string VREME_UPLATE = "@Vreme_uplate";
		private readonly string DATUM_CLANARINE = "@Datum_clanarine";
		private readonly string IZNOS = "@Iznos";
		private readonly string NAPOMENA = "@Napomena";
		private readonly string KORISNIK = "@Korisnik";
		
		public UplataClanarineDAO()
		{

		}

		private readonly string COLUMNS = " [Id stavke], [Id clana], Grupa, " +
			"[Datum uplate], [Vreme uplate], [Datum clanarine], Iznos, Napomena, Korisnik ";
		
		protected override string getByIdSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Clanarina " +
				" WHERE [Id stavke] = ? ";
		}

		public UplataClanarine getById(int idUplate)
		{
			return getById(new Key(idUplate));
		}

        protected override UplataClanarine createEntity()
		{
			return new UplataClanarine();
		}

        protected override void loadData(UplataClanarine entity, OleDbDataReader rdr)
		{
			Clan clan = null;
			if (!Convert.IsDBNull(rdr["Id clana"]))
			{
				int id = (int)rdr["Id clana"];
				clan = MapperRegistry.clanDAO().getById(id);
			}
			
			Grupa grupa = null;
			if (!Convert.IsDBNull(rdr["Grupa"]))
			{
				SifraGrupe sifraGrupe = new SifraGrupe((string)rdr["Grupa"]);
				grupa = MapperRegistry.grupaDAO().getById(sifraGrupe);
			}
			
			Nullable<DateTime> datumUplate = null;
			if (!Convert.IsDBNull(rdr["Datum uplate"]))
				datumUplate = (DateTime)rdr["Datum uplate"];

			Nullable<TimeSpan> vremeUplate = null;
			if (!Convert.IsDBNull(rdr["Vreme uplate"]))
				vremeUplate = ((DateTime)rdr["Vreme uplate"]).TimeOfDay;

			Nullable<DateTime> vaziOd = null;
			if (!Convert.IsDBNull(rdr["Datum clanarine"]))
				vaziOd = (DateTime)rdr["Datum clanarine"];

			Nullable<decimal> iznos = null;
			if (!Convert.IsDBNull(rdr["Iznos"]))
				iznos = (decimal)rdr["Iznos"];

			string napomena = null;
			if (!Convert.IsDBNull(rdr["Napomena"]))
				napomena = (string)rdr["Napomena"];

			string korisnik = null;
			if (!Convert.IsDBNull(rdr["Korisnik"]))
				korisnik = (string)rdr["Korisnik"];

            entity.Clan = clan;
            entity.Grupa = grupa;
            entity.DatumVremeUplate = new DateTime(
                datumUplate.Value.Year, datumUplate.Value.Month, datumUplate.Value.Day,
                vremeUplate.Value.Hours, vremeUplate.Value.Minutes, vremeUplate.Value.Seconds);
            entity.VaziOd = vaziOd;
            entity.Iznos = iznos;
            entity.Napomena = napomena;
            entity.Korisnik = korisnik;
		}

		protected override string selectAllSQL()
		{
			return "SELECT " + COLUMNS + 
				" FROM Clanarina ";
		}
		
		protected override string insertSQL()
		{
			return "INSERT INTO Clanarina([Id clana], Grupa, [Datum uplate], " +
				"[Vreme uplate], [Datum clanarine], Iznos, Napomena, Korisnik) " +
				" VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
		}

        protected override void addInsertParameters(UplataClanarine entity, OleDbCommand cmd)
		{
            addDataParameters(entity, cmd);
        }

		private void addDataParameters(UplataClanarine uc, OleDbCommand cmd)
		{
			cmd.Parameters.Add(ID_CLANA, OleDbType.Integer);
			cmd.Parameters.Add(GRUPA, OleDbType.VarWChar, 6);
			cmd.Parameters.Add(DATUM_UPLATE, OleDbType.DBDate);
			cmd.Parameters.Add(VREME_UPLATE, OleDbType.DBTime);
			cmd.Parameters.Add(DATUM_CLANARINE, OleDbType.DBDate);
			cmd.Parameters.Add(IZNOS, OleDbType.Currency);
			cmd.Parameters.Add(NAPOMENA, OleDbType.VarWChar, 200);
			cmd.Parameters.Add(KORISNIK, OleDbType.VarWChar, 50);

			if (uc.Clan == null)
				cmd.Parameters[ID_CLANA].Value = DBNull.Value;
			else
				cmd.Parameters[ID_CLANA].Value = uc.Clan.Key.intValue();

			if (uc.Grupa == null)
				cmd.Parameters[GRUPA].Value = DBNull.Value;
			else
				cmd.Parameters[GRUPA].Value = uc.Grupa.Sifra.Value;

			if (uc.DatumUplate == null)
				cmd.Parameters[DATUM_UPLATE].Value = DBNull.Value;
			else
				cmd.Parameters[DATUM_UPLATE].Value = uc.DatumUplate.Value;

			if (uc.VremeUplate == null)
				cmd.Parameters[VREME_UPLATE].Value = DBNull.Value;
			else
				cmd.Parameters[VREME_UPLATE].Value = uc.VremeUplate.Value;

			if (uc.VaziOd == null)
				cmd.Parameters[DATUM_CLANARINE].Value = DBNull.Value;
			else
				cmd.Parameters[DATUM_CLANARINE].Value = uc.VaziOd.Value;

			if (uc.Iznos == null)
				cmd.Parameters[IZNOS].Value = DBNull.Value;
			else
				cmd.Parameters[IZNOS].Value = uc.Iznos.Value;

			if (uc.Napomena == null)
				cmd.Parameters[NAPOMENA].Value = DBNull.Value;
			else
				cmd.Parameters[NAPOMENA].Value = uc.Napomena;

			if (uc.Korisnik == null)
				cmd.Parameters[KORISNIK].Value = DBNull.Value;
			else
				cmd.Parameters[KORISNIK].Value = uc.Korisnik;

		}

		protected override string updateSQL()
		{
			throw new NotSupportedException();
		}

		protected override void addUpdateParameters(UplataClanarine entity, OleDbCommand cmd)
		{
			throw new NotSupportedException();
		}

		protected override string deleteSQL()
		{
			throw new NotSupportedException();
		}

		public bool existsUplataGrupa(string sifraGrupe)
		{
			string selectCountByGrupa = "SELECT Count(*) " +
				"FROM Clanarina " +
				"WHERE Grupa = ? ";

			OleDbCommand cmd = new OleDbCommand(selectCountByGrupa);
			cmd.Parameters.Add(GRUPA, OleDbType.VarWChar, Grupa.SIFRA_MAX_LENGTH)
				.Value = sifraGrupe;
			return (int)executeScalar(cmd) > 0;
		}
	
		public bool existsUplataClan(int idClana)
		{
			string selectCountByClan = "SELECT Count(*) " +
				"FROM Clanarina " +
				"WHERE [ID clana] = ? ";

			OleDbCommand cmd = new OleDbCommand(selectCountByClan);
			cmd.Parameters.Add(ID_CLANA, OleDbType.Integer).Value = idClana;
			return (int)executeScalar(cmd) > 0;
		}

		// Vraca grupe za koje postoje uplate za dati datumUplate, a ne postoje
		// kategorije
        public List<Grupa> getGrupeBezKategorija(DateTime datumUplate)
		{
			string selectGroups = "SELECT DISTINCT G.Sifra " +
				"FROM Clanarina C INNER JOIN Grupe G " +
				"  ON C.Grupa = G.Sifra " +
				"WHERE C.[Datum uplate] = ? AND G.Kategorija IS NULL";

			OleDbCommand cmd = new OleDbCommand(selectGroups);
			cmd.Parameters.Add("@DatumUplate", OleDbType.DBDate).Value = datumUplate;

			OleDbDataReader rdr = executeReader(cmd);
            List<Grupa> result = new List<Grupa>();
			while (rdr.Read())
			{
				SifraGrupe sifraGrupe = new SifraGrupe((string)rdr["Sifra"]);
				result.Add(MapperRegistry.grupaDAO().getById(sifraGrupe));
			}
			rdr.Close();
			return result;
		}
	
		public DateTime getMinDatumUplate()
		{
			string selectMinDatumUplate = "SELECT Min([Datum uplate]) FROM Clanarina";
			OleDbCommand cmd = new OleDbCommand(selectMinDatumUplate);
			object result = executeScalar(cmd);
			if (result != null)
				return ((DateTime)result).Date;
			else
				return DateTime.Now.Date;
		}

        private List<object[]> loadClanVaziOdIznos(OleDbDataReader rdr, string datumColumnName)
		{
			int brojOrd = rdr.GetOrdinal("Broj");
			int imeOrd = rdr.GetOrdinal("Ime");
			int preOrd = rdr.GetOrdinal("Prezime");
			int adrOrd = rdr.GetOrdinal("Adresa");
			int mesOrd = rdr.GetOrdinal("NazivMesta");
			int vazOrd = rdr.GetOrdinal(datumColumnName);
			int iznOrd = rdr.GetOrdinal("Iznos");

            List<object[]> result = new List<object[]>();
			while (rdr.Read())
			{
				int broj = rdr.GetInt32(brojOrd);

				string ime = String.Empty;
				if (!Convert.IsDBNull(rdr[imeOrd]))
					ime = rdr.GetString(imeOrd);

				string prezime = String.Empty;
				if (!Convert.IsDBNull(rdr[preOrd]))
					prezime = rdr.GetString(preOrd);

				string adresa = String.Empty;
				if (!Convert.IsDBNull(rdr[adrOrd]))
					adresa = rdr.GetString(adrOrd);

				string mesto = String.Empty;
				if (!Convert.IsDBNull(rdr[mesOrd]))
					mesto = rdr.GetString(mesOrd);

				string clan = Clan.formatPrezimeImeBrojAdresaMesto(
						prezime, ime, broj, adresa, mesto);
				
				DateTime vaziOd = rdr.GetDateTime(vazOrd);
				decimal iznos = rdr.GetDecimal(iznOrd);

				result.Add(new object[] { clan, vaziOd, iznos} );
			}
			return result;
		}

        public List<object[]> getDnevniPrihodiClanoviReportItems(DateTime from, 
			DateTime to, List<Grupa> grupe)
		{
			// placeholder {0} predstavlja filter grupa
			string selectDnevniPrihodiClanoviReportItems =
@"SELECT C1.Broj, C1.Ime, C1.Prezime, C1.Adresa, M.Naziv AS NazivMesta,
	C2.[Datum clanarine], C2.Iznos
FROM Grupe G INNER JOIN (Clanarina C2 INNER JOIN (Clanovi C1 LEFT JOIN Mesta M
	ON C1.Mesto = M.ZIP)
	ON C2.[ID clana] = C1.[ID clana])
	ON G.Sifra = C2.Grupa
WHERE (C2.[Datum uplate] BETWEEN ? AND ?)
{0}
ORDER BY C2.[Datum uplate] DESC, G.[Sort order], C1.Prezime, C1.Ime";

			bool filterGrupe = grupe != null && grupe.Count > 0;
			string filter = String.Empty;
			if (filterGrupe)
				filter = " AND " + getGrupeFilter(grupe, "G", "Sifra");
			string selectItems = String.Format(
				selectDnevniPrihodiClanoviReportItems, filter);
			
			OleDbCommand cmd = new OleDbCommand(selectItems);
			addIntervalParams(cmd, from, to);
			if (filterGrupe)
				addGrupeFilterParams(cmd, grupe);

			OleDbDataReader rdr = executeReader(cmd);
            List<object[]> result = loadClanVaziOdIznos(rdr, "Datum clanarine");
			rdr.Close();
			return result;
		}

		private void addGrupeFilterParams(OleDbCommand cmd, List<Grupa> grupe)
		{
			for (int i = 0; i < grupe.Count; i++)
			{
				cmd.Parameters.Add("@Grupa" + i.ToString(), OleDbType.VarWChar, 
					Grupa.SIFRA_MAX_LENGTH).Value = grupe[i].Sifra.Value;
			}
		}

		private void addIntervalParams(OleDbCommand cmd, DateTime from, DateTime to)
		{
			cmd.Parameters.Add("@From", OleDbType.DBDate).Value = from;
			cmd.Parameters.Add("@To", OleDbType.DBDate).Value = to;
		}

		private string getGrupeFilter(List<Grupa> grupe, string table, string grupaColumn)
		{
			string result;
			if (table != String.Empty)
				result = String.Format("({0}.{1} IN (", table, grupaColumn);
			else
				result = String.Format("({0} IN (", grupaColumn);

			for (int i = 0; i < grupe.Count; i++)
			{
				if (i == 0)
					result += "?";
				else
					result += ",?";
			}
			result += ")) ";
			return result;
		}

        public List<ReportGrupa> getDnevniPrihodiClanoviReportGrupeDanGrupa(DateTime from,
			DateTime to, List<Grupa> grupe)
		{
			string selectDnevniPrihodiClanoviReportGrupeDanGrupa =
@"SELECT C.Grupa, G.Naziv, Sum(C.Iznos) AS [Ukupan iznos], Count(*) AS [Broj clanova]
FROM Grupe G INNER JOIN Clanarina C
	ON G.Sifra = C.Grupa
GROUP BY C.[Datum uplate], C.Grupa, G.Naziv, G.[Sort order]
HAVING (C.[Datum uplate] BETWEEN ? AND ?)
{0}
ORDER BY C.[Datum uplate] DESC, G.[Sort order]";
		
			bool filterGrupe = grupe != null && grupe.Count > 0;
			string filter = String.Empty;
			if (filterGrupe)
				filter = " AND " + getGrupeFilter(grupe, "C", "Grupa");
			string selectGroups = String.Format(
				selectDnevniPrihodiClanoviReportGrupeDanGrupa, filter);

			OleDbCommand cmd = new OleDbCommand(selectGroups);
			addIntervalParams(cmd, from, to);
			if (filterGrupe)
				addGrupeFilterParams(cmd, grupe);
			OleDbDataReader rdr = executeReader(cmd);
            List<ReportGrupa> result = loadDnevniPrihodiClanoviReportGrupeDanGrupa(rdr);
			rdr.Close();
			return result;
		}

        private List<ReportGrupa> loadDnevniPrihodiClanoviReportGrupeDanGrupa(OleDbDataReader rdr)
		{
			int gruOrd = rdr.GetOrdinal("Grupa");
			int nazOrd = rdr.GetOrdinal("Naziv");
			int ukuOrd = rdr.GetOrdinal("Ukupan iznos");
			int broOrd = rdr.GetOrdinal("Broj Clanova");

            List<ReportGrupa> result = new List<ReportGrupa>();
			while (rdr.Read())
			{
				string sifra = rdr.GetString(gruOrd);
				string naziv = rdr.GetString(nazOrd);
				decimal ukupanIznos = rdr.GetDecimal(ukuOrd);
				int brojClanova = rdr.GetInt32(broOrd);				

				object[] data = new object[] { new SifraGrupe(sifra), naziv, ukupanIznos };
				result.Add(new ReportGrupa(data, brojClanova));
			}
			return result;
		}

        public List<ReportGrupa> getDnevniPrihodiClanoviReportGrupeDan(DateTime from,
            DateTime to, List<Grupa> grupe)
		{
			string selectDnevniPrihodiClanoviReportGrupeDan =
@"SELECT C.[Datum uplate], Sum(C.Iznos) AS [Ukupan iznos], Count(*) AS [Broj clanova]
FROM Clanarina C
{0}
GROUP BY C.[Datum uplate]
HAVING C.[Datum uplate] BETWEEN ? AND ?
ORDER BY C.[Datum uplate] DESC";
	
			bool filterGrupe = grupe != null && grupe.Count > 0;
			string filter = String.Empty;
			if (filterGrupe)
				filter = " WHERE " + getGrupeFilter(grupe, "C", "Grupa");
			string selectGroups = String.Format(
				selectDnevniPrihodiClanoviReportGrupeDan, filter);

			OleDbCommand cmd = new OleDbCommand(selectGroups);
			if (filterGrupe)
				addGrupeFilterParams(cmd, grupe);
			addIntervalParams(cmd, from, to);
			OleDbDataReader rdr = executeReader(cmd);
            List<ReportGrupa> result = loadDnevniPrihodiClanoviReportGrupeDan(rdr);
			rdr.Close();
			return result;
		}

        private List<ReportGrupa> loadDnevniPrihodiClanoviReportGrupeDan(OleDbDataReader rdr)
		{
			int datOrd = rdr.GetOrdinal("Datum uplate");
			int ukuOrd = rdr.GetOrdinal("Ukupan iznos");
			int broOrd = rdr.GetOrdinal("Broj Clanova");

            List<ReportGrupa> result = new List<ReportGrupa>();
			while (rdr.Read())
			{
				DateTime datumUplate = rdr.GetDateTime(datOrd);
				decimal ukupanIznos = rdr.GetDecimal(ukuOrd);
				int brojClanova = rdr.GetInt32(broOrd);				

				object[] data = new object[] { datumUplate, ukupanIznos };
				result.Add(new ReportGrupa(data, brojClanova));
			}
			return result;
		}

		public decimal getUkupanPrihod(DateTime from, DateTime to, 
            List<Grupa> grupe)
		{
			string selectUkupanPrihod = "SELECT Sum(Iznos) FROM Clanarina " +
				"WHERE ([Datum uplate] BETWEEN ? AND ?) ";
			bool filterGrupe = grupe != null && grupe.Count > 0;
			if (filterGrupe)
				selectUkupanPrihod += " AND " + getGrupeFilter(grupe, String.Empty, "Grupa");

			OleDbCommand cmd = new OleDbCommand(selectUkupanPrihod);
			addIntervalParams(cmd, from, to);
			if (filterGrupe)
				addGrupeFilterParams(cmd, grupe);

			object result = executeScalar(cmd);
			if (result != null)
				return (decimal)result;
			else
				return 0;
		}

        public List<object[]> getDnevniPrihodiGrupeReportItems(DateTime from,
            DateTime to, List<Grupa> grupe)
		{
			// Datum uplate kolona u rezultatu je potreban da bi po njemu mogli rucno da
			// se grupisu itemi
			string selectDnevniPrihodiGrupeReportItems =
@"SELECT C.[Datum uplate], C.Grupa, G.Naziv, Count(*) AS [Broj uplata], 
	Sum(C.Iznos) AS [Ukupan iznos]
FROM Grupe G INNER JOIN Clanarina C
	ON G.Sifra = C.Grupa
GROUP BY C.[Datum uplate], C.Grupa, G.Naziv, G.[Sort order]
HAVING (C.[Datum uplate] BETWEEN ? AND ?)
{0}
ORDER BY C.[Datum uplate] DESC, G.[Sort order]";

			bool filterGrupe = grupe != null && grupe.Count > 0;
			string filter = String.Empty;
			if (filterGrupe)
				filter = " AND " + getGrupeFilter(grupe, "C", "Grupa");
			string selectItems = String.Format(
				selectDnevniPrihodiGrupeReportItems, filter);
	
			OleDbCommand cmd = new OleDbCommand(selectItems);
			addIntervalParams(cmd, from, to);
			if (filterGrupe)
				addGrupeFilterParams(cmd, grupe);
			OleDbDataReader rdr = executeReader(cmd);
            List<object[]> result = loadDnevniPrihodiGrupeReportItems(rdr);
			rdr.Close();
			return result;
		}

        private List<object[]> loadDnevniPrihodiGrupeReportItems(OleDbDataReader rdr)
		{
			int datOrd = rdr.GetOrdinal("Datum uplate");
			int gruOrd = rdr.GetOrdinal("Grupa");
			int nazOrd = rdr.GetOrdinal("Naziv");
			int broOrd = rdr.GetOrdinal("Broj uplata");
			int ukuOrd = rdr.GetOrdinal("Ukupan iznos");

            List<object[]> result = new List<object[]>();
			while (rdr.Read())
			{
				DateTime datumUplate = rdr.GetDateTime(datOrd);
				string sifra = rdr.GetString(gruOrd);
				string naziv = rdr.GetString(nazOrd);
				int brojUplata = rdr.GetInt32(broOrd);				
				decimal ukupanIznos = rdr.GetDecimal(ukuOrd);

				result.Add(new object[] { sifra, naziv, brojUplata, 
											ukupanIznos, datumUplate } );
			}
			return result;
		}

        public List<object[]> getMesecniPrihodiReportItems(DateTime from, DateTime to)
		{
			// Year i Month kolone u rezultatu su potrebne da bi po njima mogli rucno da
			// se grupisu itemi.
			// Placeholder je za filter datuma
			string selectMesecniPrihodiReportItems =
@"SELECT Year(C.[Datum uplate]) AS Godina, Month(C.[Datum uplate]) AS Mesec, C.Grupa,
	G.Naziv, Count(*) AS [Broj uplata], Sum(C.Iznos) AS [Ukupan iznos]
FROM Grupe G INNER JOIN Clanarina C
	ON G.Sifra = C.Grupa
WHERE (C.[Datum uplate] BETWEEN ? AND ?)
GROUP BY Year(C.[Datum uplate]), Month(C.[Datum uplate]), C.Grupa, G.Naziv, G.[Sort order]
ORDER BY Year(C.[Datum uplate]) DESC, Month(C.[Datum uplate]) DESC, G.[Sort order]";

			OleDbCommand cmd = new OleDbCommand(selectMesecniPrihodiReportItems);
			addIntervalParams(cmd, from, to);
			OleDbDataReader rdr = executeReader(cmd);
            List<object[]> result = loadMesecniPrihodiReportItems(rdr);
			rdr.Close();
			return result;
		}

        private List<object[]> loadMesecniPrihodiReportItems(OleDbDataReader rdr)
		{
			int godOrd = rdr.GetOrdinal("Godina");
			int mesOrd = rdr.GetOrdinal("Mesec");
			int gruOrd = rdr.GetOrdinal("Grupa");
			int nazOrd = rdr.GetOrdinal("Naziv");
			int broOrd = rdr.GetOrdinal("Broj uplata");
			int ukuOrd = rdr.GetOrdinal("Ukupan iznos");

            List<object[]> result = new List<object[]>();
			while (rdr.Read())
			{
				short godina = rdr.GetInt16(godOrd);
				short mesec = rdr.GetInt16(mesOrd);
				string sifra = rdr.GetString(gruOrd);
				string naziv = rdr.GetString(nazOrd);
				int brojUplata = rdr.GetInt32(broOrd);				
				decimal ukupanIznos = rdr.GetDecimal(ukuOrd);

				result.Add(new object[] { sifra, naziv, brojUplata, 
											ukupanIznos, godina, mesec } );
			}
			return result;
		}

        public List<object[]> getPeriodicniPrihodiUplateReportItems(DateTime from,
            DateTime to, List<Grupa> grupe)
		{
			string selectPeriodicniPrihodiUplateReportItems =
@"SELECT C1.Broj, C1.Ime, C1.Prezime, C1.Adresa, M.Naziv AS NazivMesta,
	C2.[Datum uplate], C2.Iznos
FROM Grupe G INNER JOIN (Clanarina C2 INNER JOIN (Clanovi C1 LEFT JOIN Mesta M
	ON C1.Mesto = M.ZIP)
	ON C2.[ID clana] = C1.[ID clana])
	ON G.Sifra = C2.Grupa
WHERE (C2.[Datum uplate] BETWEEN ? AND ?)
{0}
ORDER BY G.[Sort order], C1.Prezime, C1.Ime, C2.[Datum uplate] DESC";

			bool filterGrupe = grupe != null && grupe.Count > 0;
			string filter = String.Empty;
			if (filterGrupe)
				filter = " AND " + getGrupeFilter(grupe, "G", "Sifra");
			string selectItems = String.Format(
				selectPeriodicniPrihodiUplateReportItems, filter);
	
			OleDbCommand cmd = new OleDbCommand(selectItems);
			addIntervalParams(cmd, from, to);
			if (filterGrupe)
				addGrupeFilterParams(cmd, grupe);
			OleDbDataReader rdr = executeReader(cmd);
            List<object[]> result = loadClanVaziOdIznos(rdr, "Datum uplate");
			rdr.Close();
			return result;
		}

        public List<ReportGrupa> getPeriodicniPrihodiUplateReportGrupe(DateTime from,
            DateTime to, List<Grupa> grupe)
		{
			string selectPeriodicniPrihodiUplateReportGrupe =
@"SELECT C.Grupa, G.Naziv, Sum(C.Iznos) AS [Ukupan iznos], Count(*) AS [Broj clanova]
FROM Grupe G INNER JOIN Clanarina C
	ON G.Sifra = C.Grupa
WHERE (C.[Datum uplate] BETWEEN ? AND ?)
GROUP BY C.Grupa, G.Naziv, G.[Sort order]
{0}
ORDER BY G.[Sort order]";

			bool filterGrupe = grupe != null && grupe.Count > 0;
			string filter = String.Empty;
			if (filterGrupe)
				filter = " HAVING " + getGrupeFilter(grupe, "C", "Grupa");
			string selectGroups = String.Format(
				selectPeriodicniPrihodiUplateReportGrupe, filter);

			OleDbCommand cmd = new OleDbCommand(selectGroups);
			addIntervalParams(cmd, from, to);
			if (filterGrupe)
				addGrupeFilterParams(cmd, grupe);
			OleDbDataReader rdr = executeReader(cmd);
            List<ReportGrupa> result = loadPeriodicniPrihodiUplateReportGrupe(rdr);
			rdr.Close();
			return result;
		}

        private List<ReportGrupa> loadPeriodicniPrihodiUplateReportGrupe(OleDbDataReader rdr)
		{
			int gruOrd = rdr.GetOrdinal("Grupa");
			int nazOrd = rdr.GetOrdinal("Naziv");
			int ukuOrd = rdr.GetOrdinal("Ukupan iznos");
			int broOrd = rdr.GetOrdinal("Broj Clanova");

            List<ReportGrupa> result = new List<ReportGrupa>();
			while (rdr.Read())
			{
				string sifra = rdr.GetString(gruOrd);
				string naziv = rdr.GetString(nazOrd);
				decimal ukupanIznos = rdr.GetDecimal(ukuOrd);
				int brojClanova = rdr.GetInt32(broOrd);				

				object[] data = new object[] { new SifraGrupe(sifra), naziv, 
												 ukupanIznos }; 
				result.Add(new ReportGrupa(data, brojClanova));
			}
			return result;
		}

        public List<object[]> getUplateClanovaReportItems(int idClana)
		{
			// placeholder {0} je filter za clana 
			string selectUplateClanovaReportItems =
@"SELECT [Datum uplate], [Vreme uplate], Grupa, [Datum clanarine], Iznos, 
	C2.Napomena, Korisnik
FROM Clanovi C1 INNER JOIN Clanarina C2
	ON C1.[ID clana] = C2.[ID clana]
{0}
ORDER BY Broj, [Datum uplate] DESC, [Vreme uplate] DESC";

			string filter = String.Empty;
			if (idClana != -1)
				filter = " WHERE C2.[ID clana] = ? ";
			string selectItems = String.Format(selectUplateClanovaReportItems, filter);

			OleDbCommand cmd = new OleDbCommand(selectItems);
			if (idClana != -1)
				cmd.Parameters.Add("@IdClana", OleDbType.Integer).Value = idClana;
			OleDbDataReader rdr = executeReader(cmd);
            List<object[]> result = loadUplateClanovaReportItems(rdr);
			rdr.Close();
			return result;
		}

        private List<object[]> loadUplateClanovaReportItems(OleDbDataReader rdr)
		{
			int datUplOrd = rdr.GetOrdinal("Datum uplate");
			int vreOrd = rdr.GetOrdinal("Vreme uplate");
			int gruOrd = rdr.GetOrdinal("Grupa");
			int datClaOrd = rdr.GetOrdinal("Datum clanarine");
			int iznOrd = rdr.GetOrdinal("Iznos");
			int napOrd = rdr.GetOrdinal("Napomena");
			int korOrd = rdr.GetOrdinal("Korisnik");

            List<object[]> result = new List<object[]>();
			while (rdr.Read())
			{
				DateTime datumUplate = rdr.GetDateTime(datUplOrd);
				TimeSpan vremeUplate = rdr.GetDateTime(vreOrd).TimeOfDay;
				string grupa = rdr.GetString(gruOrd);
				DateTime datumClanarine = rdr.GetDateTime(datClaOrd);
				decimal iznos = rdr.GetDecimal(iznOrd);

				string napomena = String.Empty;
				object nap = rdr[napOrd];
				if (!Convert.IsDBNull(nap))
					napomena = (string)nap;

				string korisnik = String.Empty;
				object kor = rdr[korOrd];
				if (!Convert.IsDBNull(kor))
					korisnik = (string)kor;

				result.Add(new object[] { datumUplate, vremeUplate, grupa,
					datumClanarine, iznos, napomena, korisnik });
			}
			return result;
		}

        public List<ReportGrupa> getUplateClanovaReportGroups(int idClana)
		{
			string selectUplateClanovaReportGroups =
@"SELECT C1.Broj, C1.Ime, C1.Prezime, C1.[Datum rodjenja], C1.Adresa,
	M.Naziv AS NazivMesta, Count(*) AS [Broj clanova]
FROM Clanarina C2 INNER JOIN (Clanovi C1 LEFT JOIN Mesta M
	ON C1.Mesto = M.ZIP)
	ON C2.[ID clana] = C1.[ID clana]
{0}
GROUP BY Broj, Ime, Prezime, [Datum rodjenja], Adresa, M.Naziv
ORDER BY Broj";

			string filter = String.Empty;
			if (idClana != -1)
				filter = " WHERE C2.[ID clana] = ? ";
			string selectGroups = String.Format(selectUplateClanovaReportGroups, filter);

			OleDbCommand cmd = new OleDbCommand(selectGroups);
			if (idClana != -1)
				cmd.Parameters.Add("@IdClana", OleDbType.Integer).Value = idClana;
			OleDbDataReader rdr = executeReader(cmd);
            List<ReportGrupa> result = loadUplateClanovaReportGroups(rdr);
			rdr.Close();
			return result;
		}

        private List<ReportGrupa> loadUplateClanovaReportGroups(OleDbDataReader rdr)
		{
			int brojOrd = rdr.GetOrdinal("Broj");
			int imeOrd = rdr.GetOrdinal("Ime");
			int preOrd = rdr.GetOrdinal("Prezime");
			int datOrd = rdr.GetOrdinal("Datum rodjenja");
			int adrOrd = rdr.GetOrdinal("Adresa");
			int mesOrd = rdr.GetOrdinal("NazivMesta");
			int broClaOrd = rdr.GetOrdinal("Broj clanova");

            List<ReportGrupa> result = new List<ReportGrupa>();
			while (rdr.Read())
			{
				int broj = rdr.GetInt32(brojOrd);

				string ime = String.Empty;
				if (!Convert.IsDBNull(rdr[imeOrd]))
					ime = rdr.GetString(imeOrd);

				string prezime = String.Empty;
				if (!Convert.IsDBNull(rdr[preOrd]))
					prezime = rdr.GetString(preOrd);

				Nullable<DateTime> datumRodjenja = null;
				if (!Convert.IsDBNull(rdr[datOrd]))
					datumRodjenja = rdr.GetDateTime(datOrd);
		
				string adresa = String.Empty;
				if (!Convert.IsDBNull(rdr[adrOrd]))
					adresa = rdr.GetString(adrOrd);

				string mesto = String.Empty;
				if (!Convert.IsDBNull(rdr[mesOrd]))
					mesto = rdr.GetString(mesOrd);

				string clan = Clan.formatPrezimeImeBrojDatumRodjAdresaMesto(
						prezime, ime, broj, datumRodjenja, adresa, mesto);
				
				int brojClanova = rdr.GetInt32(broClaOrd);
				object[] data = new object[] { clan };

				result.Add(new ReportGrupa(data, brojClanova));
			}
			return result;
		}

        public List<object[]> getDnevniPrihodiKategorijeReportItems(DateTime datum)
		{
			string selectDnevniPrihodiKategorijeReportItems =
@"SELECT K.Naziv, Sum(C2.Iznos) AS [Ukupan iznos], Count(*) AS [Broj uplata]
FROM Clanarina C2 INNER JOIN (Grupe G INNER JOIN Kategorije K
	ON G.Kategorija = K.[ID Kategorije])
	ON C2.Grupa = G.Sifra
WHERE C2.[Datum uplate] = ?
GROUP BY K.[ID Kategorije], K.Naziv
ORDER BY K.[ID Kategorije]";

			string selectItems = String.Format(
				selectDnevniPrihodiKategorijeReportItems, datum);
			OleDbCommand cmd = new OleDbCommand(selectItems);
			cmd.Parameters.Add("@DatumUplate", OleDbType.DBDate).Value = datum;
			OleDbDataReader rdr = executeReader(cmd);
            List<object[]> result = loadDnevniPrihodiKategorijeReportItems(rdr);
			rdr.Close();
			return result;
		}

        private List<object[]> loadDnevniPrihodiKategorijeReportItems(OleDbDataReader rdr)
		{
			int nazOrd = rdr.GetOrdinal("Naziv");
			int ukuOrd = rdr.GetOrdinal("Ukupan iznos");
			int broOrd = rdr.GetOrdinal("Broj uplata");

            List<object[]> result = new List<object[]>();
			int redBroj = 1;
			while (rdr.Read())
			{
				string naziv = rdr.GetString(nazOrd);
				decimal ukupanIznos = rdr.GetDecimal(ukuOrd);
				int brojUplata = rdr.GetInt32(broOrd);

				result.Add(new object[] { redBroj++, naziv, brojUplata, ukupanIznos } );
			}
			return result;
		}

		// placeholder {0} je filter za clana ili interval
		private readonly string selectUplate =
@"SELECT C1.Broj, C1.Ime, C1.Prezime, C1.[Datum rodjenja], 
	C2.[ID stavke], C2.[Datum uplate], C2.[Vreme uplate], C2.Grupa, C2.[Datum clanarine], 
	C2.Iznos, C2.Napomena, C2.Korisnik, G.Naziv
FROM Grupe G INNER JOIN (Clanarina C2 INNER JOIN Clanovi C1
	ON C2.[ID clana] = C1.[ID clana])
	ON G.Sifra = C2.Grupa
{0}
ORDER BY C2.[Datum uplate] DESC, C2.[Vreme uplate] DESC";

        public List<UplataDTO> getUplate(int idClana)
		{
			string filter = " WHERE C2.[ID clana] = ? ";
			string selectItems = String.Format(selectUplate, filter);

			OleDbCommand cmd = new OleDbCommand(selectItems);
			cmd.Parameters.Add("@IdClana", OleDbType.Integer).Value = idClana;
			OleDbDataReader rdr = executeReader(cmd);
            List<UplataDTO> result = loadUplate(rdr);
			rdr.Close();
			return result;
		}

        public List<UplataDTO> getUplate(DateTime from, DateTime to)
		{
			string filter = " WHERE C2.[Datum uplate] BETWEEN ? AND ? ";
			string selectItems = String.Format(selectUplate, filter);

			OleDbCommand cmd = new OleDbCommand(selectItems);
			addIntervalParams(cmd, from, to);
			OleDbDataReader rdr = executeReader(cmd);
            List<UplataDTO> result = loadUplate(rdr);
			rdr.Close();
			return result;
		}

        private List<UplataDTO> loadUplate(OleDbDataReader rdr)
		{
			int brojOrd = rdr.GetOrdinal("Broj");
			int imeOrd = rdr.GetOrdinal("Ime");
			int preOrd = rdr.GetOrdinal("Prezime");
			int datOrd = rdr.GetOrdinal("Datum rodjenja");
			
			int idOrd = rdr.GetOrdinal("ID stavke");
			int datUplOrd = rdr.GetOrdinal("Datum uplate");
			int vreOrd = rdr.GetOrdinal("Vreme uplate");
			int gruOrd = rdr.GetOrdinal("Grupa");
			int nazOrd = rdr.GetOrdinal("Naziv");
			int datClaOrd = rdr.GetOrdinal("Datum clanarine");
			int iznOrd = rdr.GetOrdinal("Iznos");
			int napOrd = rdr.GetOrdinal("Napomena");
			int korOrd = rdr.GetOrdinal("Korisnik");

            List<UplataDTO> result = new List<UplataDTO>();
			while (rdr.Read())
			{
				int broj = rdr.GetInt32(brojOrd);

				string ime = String.Empty;
				if (!Convert.IsDBNull(rdr[imeOrd]))
					ime = rdr.GetString(imeOrd);

				string prezime = String.Empty;
				if (!Convert.IsDBNull(rdr[preOrd]))
					prezime = rdr.GetString(preOrd);

				Nullable<DateTime> datumRodjenja = null;
				if (!Convert.IsDBNull(rdr[datOrd]))
					datumRodjenja = rdr.GetDateTime(datOrd);

				int idStavke = rdr.GetInt32(idOrd);
				DateTime datumUplate = rdr.GetDateTime(datUplOrd);
				TimeSpan vremeUplate = rdr.GetDateTime(vreOrd).TimeOfDay;
				string grupa = rdr.GetString(gruOrd);
				string naziv = rdr.GetString(nazOrd);
				DateTime datumClanarine = rdr.GetDateTime(datClaOrd);
				decimal iznos = rdr.GetDecimal(iznOrd);

				string napomena = String.Empty;
				object nap = rdr[napOrd];
				if (!Convert.IsDBNull(nap))
					napomena = (string)nap;

				string korisnik = String.Empty;
				object kor = rdr[korOrd];
				if (!Convert.IsDBNull(kor))
					korisnik = (string)kor;

				string clan = Clan.formatPrezimeImeBrojDatumRodjAdresaMesto(prezime,
					ime, broj, datumRodjenja, String.Empty, String.Empty);
				string sifraGrupeNaziv = Grupa.formatSifraCrtaNaziv(grupa, naziv);

				result.Add(new UplataDTO(idStavke, clan, iznos, datumUplate, 
					vremeUplate, sifraGrupeNaziv, datumClanarine, napomena, korisnik));
			}
			return result;
		}
	}
}
