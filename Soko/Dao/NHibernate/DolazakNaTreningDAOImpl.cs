using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;
using System.Collections;
using Soko.Report;
using Soko.Misc;
using Iesi.Collections;
using Soko.UI;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="DolazakNaTreningDAO"/>.
    /// </summary>
    public class DolazakNaTreningDAOImpl : GenericNHibernateDAO<DolazakNaTrening, int>, DolazakNaTreningDAO
    {
        public virtual IList<DolazakNaTrening> getDolazakNaTrening(DateTime from, DateTime to)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from DolazakNaTrening d
                                                 left join fetch d.Clan 
                                                 where d.DatumVremeDolaska >= :from and d.DatumVremeDolaska <= :to
                                                 order by d.DatumVremeDolaska asc");
                q.SetDateTime("from", from);
                q.SetDateTime("to", to);
                return q.List<DolazakNaTrening>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual IList<DolazakNaTrening> getDolazakNaTrening(Clan c, DateTime from, DateTime to)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from DolazakNaTrening d
                                                 left join fetch d.Clan c
                                                 where c = :clan
                                                 and d.DatumVremeDolaska >= :from and d.DatumVremeDolaska <= :to
                                                 order by d.DatumVremeDolaska asc");
                q.SetEntity("clan", c);
                q.SetDateTime("from", from);
                q.SetDateTime("to", to);
                return q.List<DolazakNaTrening>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual List<object[]> getEvidencijaTreningaReportItems(DateTime from, DateTime to, List<Grupa> grupe)
        {
            to = to.AddMinutes(1);
            try
            {
                string query = @"
SELECT
    c.broj, c.ime, c.prezime, c.datum_rodjenja,
    d.datum_vreme_dolaska
FROM clanovi c INNER JOIN (dolazak_na_trening d LEFT OUTER JOIN grupe g
	ON d.grupa_id = g.grupa_id)
	ON c.clan_id = d.clan_id
WHERE
    (d.datum_vreme_dolaska BETWEEN '{0}' AND '{1}')
{2}
ORDER BY
    g.broj_grupe, g.podgrupa, c.prezime, c.ime
";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                string filter = String.Empty;
                if (filterGrupe)
                    filter = " AND " + Util.getGrupeFilter(grupe, "g", "grupa_id");
                query = String.Format(query, from.ToString("yyyy-MM-dd HH:mm:ss"),
                    to.ToString("yyyy-MM-dd HH:mm:ss"), filter);

                IList<object[]> dolasci = Session.CreateSQLQuery(query).List<object[]>();
                List<object[]> result = new List<object[]>();
                foreach (object[] row in dolasci)
                {
                    int broj = (int)row[0];
                    string ime = (string)row[1];
                    string prezime = (string)row[2];

                    Nullable<DateTime> datumRodjenja = null;
                    if (row[3] != null)
                        datumRodjenja = (DateTime)row[3];

                    DateTime datumVremeDolaska = (DateTime)row[4];

                    string clan = Clan.formatPrezimeImeBrojDatumRodjAdresaMesto(
                            prezime, ime, broj, datumRodjenja, String.Empty, String.Empty);
                    result.Add(new object[] { clan, datumVremeDolaska });
                }
                return result;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual List<ReportGrupa> getEvidencijaTreningaReportGrupe(DateTime from, DateTime to, List<Grupa> grupe)
        {
            to = to.AddMinutes(1);
            try
            {
                string query = @"
SELECT d.grupa_id, g.broj_grupe, g.podgrupa, g.naziv, Count(*)
FROM dolazak_na_trening d LEFT OUTER JOIN grupe g
	ON d.grupa_id = g.grupa_id
WHERE (d.datum_vreme_dolaska BETWEEN '{0}' AND '{1}')
{2}
GROUP BY d.grupa_id, g.broj_grupe, g.podgrupa, g.naziv
ORDER BY g.broj_grupe, g.podgrupa
";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                string filter = String.Empty;
                if (filterGrupe)
                    filter = " AND " + Util.getGrupeFilter(grupe, "g", "grupa_id");
                query = String.Format(query, from.ToString("yyyy-MM-dd HH:mm:ss"),
                    to.ToString("yyyy-MM-dd HH:mm:ss"), filter);

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<ReportGrupa> result2 = new List<ReportGrupa>();
                foreach (object[] row in result)
                {
                    string sifraNaziv = String.Empty;
                    if (row[0] != null)
                    {
                        int grupa_id = (int)row[0];
                        int brojGrupe = (int)row[1];
                        string podgrupa = (string)row[2];
                        string nazivGrupe = (string)row[3];
                        string sifra = brojGrupe + podgrupa;
                        sifraNaziv = sifra + " - " + nazivGrupe;
                    }
                    int brojClanova = (int)row[4];

                    object[] data = new object[] { sifraNaziv };
                    result2.Add(new ReportGrupa(data, brojClanova));
                }
                return result2;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual List<object[]> getEvidencijaTreningaReportItems(int clanId, DateTime from, DateTime to,
            List<Grupa> grupe)
        {
            to = to.AddMinutes(1);
            try
            {
                string query = @"
SELECT
    g.naziv, d.datum_vreme_dolaska
FROM dolazak_na_trening d LEFT OUTER JOIN grupe g
	ON d.grupa_id = g.grupa_id
WHERE
	(d.clan_id = {0}) AND
    (d.datum_vreme_dolaska BETWEEN '{1}' AND '{2}')
{3}
ORDER BY
    d.datum_vreme_dolaska
";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                string filter = String.Empty;
                if (filterGrupe)
                    filter = " AND " + Util.getGrupeFilter(grupe, "g", "grupa_id");
                query = String.Format(query, clanId, from.ToString("yyyy-MM-dd HH:mm:ss"),
                    to.ToString("yyyy-MM-dd HH:mm:ss"), filter);

                IList<object[]> dolasci = Session.CreateSQLQuery(query).List<object[]>();
                List<object[]> result = new List<object[]>();
                foreach (object[] row in dolasci)
                {
                    string nazivGrupe = String.Empty;
                    if (row[0] != null)
                        nazivGrupe = (string)row[0];

                    DateTime datumVremeDolaska = (DateTime)row[1];

                    result.Add(new object[] { nazivGrupe, datumVremeDolaska });
                }
                return result;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual List<object[]> getDolazakNaTreningMesecniReportItems(DateTime from, DateTime to, bool samoNedostajuceUplate)
        {
            try
            {
                string dolasciQuery = @"
SELECT DISTINCT
    datepart(year, d.datum_vreme_dolaska) god,
    datepart(month, d.datum_vreme_dolaska) mes,
    datepart(day, d.datum_vreme_dolaska) dan,
    c.clan_id, c.broj, c.ime, c.prezime, c.datum_rodjenja, c.ne_placa_clanarinu,
    g.naziv
FROM clanovi c INNER JOIN (dolazak_na_trening d LEFT OUTER JOIN grupe g
	ON d.grupa_id = g.grupa_id)
	ON c.clan_id = d.clan_id
WHERE (d.datum_vreme_dolaska BETWEEN '{0}' AND '{1}')
ORDER BY god, mes, g.naziv, c.prezime, c.ime, c.datum_rodjenja, dan";

                dolasciQuery = String.Format(dolasciQuery, from.ToString("yyyy-MM-dd HH:mm:ss"),
                    to.ToString("yyyy-MM-dd HH:mm:ss"));
                IList<object[]> dolasci = Session.CreateSQLQuery(dolasciQuery).List<object[]>();

                string uplateQuery = @"
SELECT DISTINCT
    datepart(year, u.vazi_od) god,
    datepart(month, u.vazi_od) mes,
    u.clan_id
FROM uplate u
WHERE (u.vazi_od BETWEEN '{0}' AND '{1}')";
                uplateQuery = String.Format(uplateQuery, from.ToString("yyyy-MM-dd HH:mm:ss"),
                    to.ToString("yyyy-MM-dd HH:mm:ss"));
                IList<object[]> uplate = Session.CreateSQLQuery(uplateQuery).List<object[]>();

                ISet uplateSet = new HashedSet();
                foreach (object[] row in uplate)
                {
                    int god = (int)row[0];
                    int mes = (int)row[1];
                    int id = (int)row[2];
                    uplateSet.Add(new ClanGodinaMesec(id, god, mes));
                }

                DateTime firstDateTimeInYear = new DateTime(from.Year, 1, 1, 0, 0, 0);
                DateTime lastDateTimeInYear = new DateTime(to.AddYears(1).Year, 1, 1, 0, 0, 0).AddSeconds(-1);

                List<Grupa> godisnjaClanarinaGrupe = new List<Grupa>();
                GrupaDAO grupaDAO = DAOFactoryFactory.DAOFactory.GetGrupaDAO();
                IList<Grupa> grupe = grupaDAO.FindAll();
                foreach (Grupa g in grupe)
                {
                    if (g.ImaGodisnjuClanarinu)
                    {
                        godisnjaClanarinaGrupe.Add(g);
                    }
                }
                if (godisnjaClanarinaGrupe.Count == 0)
                {
                    // TODO3: Da li je ovo potrebno?
                    MessageDialogs.showMessage("Ne mogu da pronadjem grupu za godisnju clanarinu", "Greska");
                }

                string uplateGodisnjaClanarinaQuery = @"
SELECT DISTINCT
    datepart(year, u.vazi_od) god,
    u.clan_id
FROM uplate u
WHERE (u.vazi_od BETWEEN '{0}' AND '{1}')
{2}";
                string filter = String.Empty;
                if (godisnjaClanarinaGrupe.Count > 0)
                    filter = " AND " + Util.getGrupeFilter(godisnjaClanarinaGrupe, "u", "grupa_id");

                uplateGodisnjaClanarinaQuery = String.Format(uplateGodisnjaClanarinaQuery,
                    firstDateTimeInYear.ToString("yyyy-MM-dd HH:mm:ss"),
                    lastDateTimeInYear.ToString("yyyy-MM-dd HH:mm:ss"),
                    filter);
                IList<object[]> uplateGodisnjaClanarina = Session.CreateSQLQuery(uplateGodisnjaClanarinaQuery).List<object[]>();

                ISet godisnjeUplateSet = new HashedSet();
                foreach (object[] row in uplateGodisnjaClanarina)
                {
                    int god = (int)row[0];
                    int id = (int)row[1];
                    godisnjeUplateSet.Add(new ClanGodinaMesec(id, god, 1));
                }

                List<object[]> result = new List<object[]>();
                int prev_id = -1;
                object[] prevItem = null;
                int brojDana = 0;
                foreach (object[] row in dolasci)
                {
                    int god = (int)row[0];
                    int mes = (int)row[1];
                    int id = (int)row[3];
                    bool neplacaClanarinu = (bool)row[8];

                    bool imaUplatu = uplateSet.Contains(new ClanGodinaMesec(id, god, mes));
                    if (!imaUplatu)
                        imaUplatu = godisnjeUplateSet.Contains(new ClanGodinaMesec(id, god, 1));
                    if (!imaUplatu)
                        imaUplatu = neplacaClanarinu;

                    if (samoNedostajuceUplate && imaUplatu)
                    {
                        continue;
                    }

                    int broj = (int)row[4];
                    string ime = (string)row[5];
                    string prezime = (string)row[6];

                    Nullable<DateTime> datumRodjenja = null;
                    if (row[7] != null)
                        datumRodjenja = (DateTime)row[7];

                    string nazivGrupe = String.Empty;
                    if (row[9] != null)
                        nazivGrupe = (string)row[9];

                    string imaUplatuStr = "NE";
                    if (imaUplatu)
                        imaUplatuStr = "";

                    string clan = Clan.formatPrezimeImeBrojDatumRodjAdresaMesto(
                        prezime, ime, broj, datumRodjenja, String.Empty, String.Empty);
                    object[] item = new object[] { clan, nazivGrupe, imaUplatuStr, god, mes };

                    if (prevItem != null && god == (int)prevItem[3] && mes == (int)prevItem[4]
                        && id == prev_id && nazivGrupe == (string)prevItem[1])
                    {
                        ++brojDana;
                    }
                    else
                    {
                        if (prevItem != null)
                        {
                            if ((string)prevItem[2] == "NE")
                            {
                                prevItem[2] = formatBrojTreninga(brojDana, samoNedostajuceUplate);
                            }
                            result.Add(prevItem);
                        }
                        brojDana = 1;
                        prev_id = id;
                        prevItem = item;
                    }
                }
                // Add last item
                if (prevItem != null)
                {
                    if ((string)prevItem[2] == "NE")
                    {
                        prevItem[2] = formatBrojTreninga(brojDana, samoNedostajuceUplate);
                    }
                    result.Add(prevItem);
                }

                return result;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        private string formatBrojTreninga(int brojDana, bool samoNedostajuceUplate)
        {
            string result;
            if (samoNedostajuceUplate)
            {
                result = brojDana.ToString();
            }
            else
            {
                result = "NE (" + brojDana.ToString() + ")";
            }
            return result;
        }

        public virtual List<object[]> getNeplacenDolazakNaTrening(DateTime from, DateTime to)
        {
            try
            {
                string dolasciQuery = @"
SELECT DISTINCT
    d.datum_vreme_dolaska,
    c.clan_id, c.ne_placa_clanarinu,
    g.grupa_id
FROM clanovi c INNER JOIN (dolazak_na_trening d LEFT OUTER JOIN grupe g
	ON d.grupa_id = g.grupa_id)
	ON c.clan_id = d.clan_id
WHERE (d.datum_vreme_dolaska BETWEEN '{0}' AND '{1}')";

                dolasciQuery = String.Format(dolasciQuery, from.ToString("yyyy-MM-dd HH:mm:ss"),
                    to.ToString("yyyy-MM-dd HH:mm:ss"));
                IList<object[]> dolasci = Session.CreateSQLQuery(dolasciQuery).List<object[]>();

                string uplateQuery = @"
SELECT DISTINCT
    datepart(year, u.vazi_od) god,
    datepart(month, u.vazi_od) mes,
    u.clan_id
FROM uplate u
WHERE (u.vazi_od BETWEEN '{0}' AND '{1}')";
                uplateQuery = String.Format(uplateQuery, from.ToString("yyyy-MM-dd HH:mm:ss"),
                    to.ToString("yyyy-MM-dd HH:mm:ss"));
                IList<object[]> uplate = Session.CreateSQLQuery(uplateQuery).List<object[]>();

                ISet uplateSet = new HashedSet();
                foreach (object[] row in uplate)
                {
                    int god = (int)row[0];
                    int mes = (int)row[1];
                    int id = (int)row[2];
                    uplateSet.Add(new ClanGodinaMesec(id, god, mes));
                }

                DateTime firstDateTimeInYear = new DateTime(from.Year, 1, 1, 0, 0, 0);
                DateTime lastDateTimeInYear = new DateTime(to.AddYears(1).Year, 1, 1, 0, 0, 0).AddSeconds(-1);

                List<Grupa> godisnjaClanarinaGrupe = new List<Grupa>();
                GrupaDAO grupaDAO = DAOFactoryFactory.DAOFactory.GetGrupaDAO();
                IList<Grupa> grupe = grupaDAO.FindAll();
                foreach (Grupa g in grupe)
                {
                    if (g.ImaGodisnjuClanarinu)
                    {
                        godisnjaClanarinaGrupe.Add(g);
                    }
                }
                if (godisnjaClanarinaGrupe.Count == 0)
                {
                    // TODO3: Da li je ovo potrebno?
                    MessageDialogs.showMessage("Ne mogu da pronadjem grupu za godisnju clanarinu", "Greska");
                }

                string uplateGodisnjaClanarinaQuery = @"
SELECT DISTINCT
    datepart(year, u.vazi_od) god,
    u.clan_id
FROM uplate u
WHERE (u.vazi_od BETWEEN '{0}' AND '{1}')
{2}";
                string filter = String.Empty;
                if (godisnjaClanarinaGrupe.Count > 0)
                    filter = " AND " + Util.getGrupeFilter(godisnjaClanarinaGrupe, "u", "grupa_id");

                uplateGodisnjaClanarinaQuery = String.Format(uplateGodisnjaClanarinaQuery,
                    firstDateTimeInYear.ToString("yyyy-MM-dd HH:mm:ss"),
                    lastDateTimeInYear.ToString("yyyy-MM-dd HH:mm:ss"),
                    filter);
                IList<object[]> uplateGodisnjaClanarina = Session.CreateSQLQuery(uplateGodisnjaClanarinaQuery).List<object[]>();

                ISet godisnjeUplateSet = new HashedSet();
                foreach (object[] row in uplateGodisnjaClanarina)
                {
                    int god = (int)row[0];
                    int id = (int)row[1];
                    godisnjeUplateSet.Add(new ClanGodinaMesec(id, god, 1));
                }

                List<object[]> result = new List<object[]>();
                foreach (object[] row in dolasci)
                {
                    DateTime datum_vreme_dolaska = (DateTime)row[0];
                    int clan_id = (int)row[1];
                    bool neplacaClanarinu = (bool)row[2];
                    object grupa_id = row[3];

                    bool imaUplatu = uplateSet.Contains(
                        new ClanGodinaMesec(clan_id, datum_vreme_dolaska.Year, datum_vreme_dolaska.Month));
                    if (!imaUplatu)
                        imaUplatu = godisnjeUplateSet.Contains(new ClanGodinaMesec(clan_id, datum_vreme_dolaska.Year, 1));
                    if (!imaUplatu)
                        imaUplatu = neplacaClanarinu;

                    if (imaUplatu)
                    {
                        continue;
                    }
                    object[] item = new object[] { datum_vreme_dolaska, clan_id, grupa_id };
                    result.Add(item);
                }
                return result;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual void deleteDolasci(DateTime from, DateTime to)
        {
            try
            {
                string query = @"
DELETE FROM dolazak_na_trening WHERE datum_vreme_dolaska BETWEEN '{0}' AND '{1}'";
                query = String.Format(query, from.ToString("yyyy-MM-dd HH:mm:ss"), to.ToString("yyyy-MM-dd HH:mm:ss"));
                Session.CreateSQLQuery(query).UniqueResult();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual void insertDolazak(DateTime datum_vreme_dolaska, int clan_id, int grupa_id)
        { 
            try
            {
                string query1 = @"
INSERT INTO dolazak_na_trening (datum_vreme_dolaska, clan_id, grupa_id) VALUES ('{0}', {1}, {2})";
                string query2 = @"
INSERT INTO dolazak_na_trening (datum_vreme_dolaska, clan_id) VALUES ('{0}', {1})";
                string query;
                if (grupa_id != -1)
                {
                    query = String.Format(query1, datum_vreme_dolaska.ToString("yyyy-MM-dd HH:mm:ss"), clan_id, grupa_id);
                }
                else
                {
                    query = String.Format(query2, datum_vreme_dolaska.ToString("yyyy-MM-dd HH:mm:ss"), clan_id);
                }
                Session.CreateSQLQuery(query).UniqueResult();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }
    }

    // TODO2: Prebaci ovu klasu na drugo mesto.
    public class ClanGodinaMesec
    {
        public int clan_id;
        public int godina;
        public int mesec;

        public ClanGodinaMesec(int clan_id, int godina, int mesec)
        {
            this.clan_id = clan_id;
            this.godina = godina;
            this.mesec = mesec;
        }

        public override bool Equals(object other)
        {
            if (object.ReferenceEquals(this, other))
                return true;
            if (!(other is ClanGodinaMesec))
                return false;
            ClanGodinaMesec that = (ClanGodinaMesec)other;
            return this.clan_id == that.clan_id && this.godina == that.godina && this.mesec == that.mesec;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = clan_id;
                result = 29 * result + godina.GetHashCode() + mesec.GetHashCode();
                return result;
            }
        }
    }
}