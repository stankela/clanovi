using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;
using System.Collections;
using Soko.Report;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="UplataClanarineDAO"/>.
    /// </summary>
    public class UplataClanarineDAOImpl : GenericNHibernateDAO<UplataClanarine, int>, UplataClanarineDAO
    {
        #region UplataClanarineDAO Members

        public virtual bool existsUplataClan(Clan c)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from UplataClanarine u where u.Clan = :clan");
                q.SetEntity("clan", c);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual bool existsUplataGrupa(Grupa g)
        {
            try
            {
                IQuery q = Session.CreateQuery("select count(*) from UplataClanarine u where u.Grupa = :grupa");
                q.SetEntity("grupa", g);
                return (long)q.UniqueResult() > 0;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual IList<UplataClanarine> findUplate(Clan c)
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from UplataClanarine u
                                                 left join fetch u.Clan 
                                                 left join fetch u.Grupa 
                                                 where u.Clan = :clan");
                q.SetEntity("clan", c);
                return q.List<UplataClanarine>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual IList<UplataClanarine> findUplate(DateTime from, DateTime to)
        {
            from = from.Date;
            to = to.Date.AddDays(1);
            try
            {
                IQuery q = Session.CreateQuery(@"from UplataClanarine u
                                                 left join fetch u.Clan 
                                                 left join fetch u.Grupa 
                                                 where u.DatumVremeUplate >= :from and u.DatumVremeUplate <= :to");
                q.SetDateTime("from", from);
                q.SetDateTime("to", to);
                return q.List<UplataClanarine>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        // Vraca grupe za koje postoje uplate za dati datumUplate, a ne postoje
        // kategorije
        public virtual IList<Grupa> getGrupeBezKategorija(DateTime datumUplate)
        {
            datumUplate = datumUplate.Date;
            DateTime end = datumUplate.AddDays(1);
            try
            {
                IQuery q = Session.CreateQuery(@"select distinct u.Grupa
                                                 from UplataClanarine u
                                                 join u.Grupa g
                                                 where u.DatumVremeUplate >= :datum and u.DatumVremeUplate <= :end
                                                 and g.Kategorija is null");
                q.SetDateTime("datum", datumUplate);
                q.SetDateTime("end", end);
                return q.List<Grupa>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual List<object[]> getDnevniPrihodiGrupeReportItems(DateTime from, DateTime to, List<Grupa> grupe)
        {
            from = from.Date;
            to = to.Date.AddDays(1);
            try
            {
                string query = @"
select datepart(year,u.datum_vreme_uplate) god,
    datepart(month,u.datum_vreme_uplate) mes,
    datepart(day,u.datum_vreme_uplate) dan,
    g.grupa_id, g.broj_grupe, g. podgrupa, g.naziv, count(*) broj_uplata, sum(u.iznos) ukupan_iznos
from uplate u inner join grupe g
on u.grupa_id = g.grupa_id
where (u.datum_vreme_uplate between '{0}' and '{1}')
group by datepart(year,u.datum_vreme_uplate),
    datepart(month,u.datum_vreme_uplate),
    datepart(day,u.datum_vreme_uplate),
    g.grupa_id, g.broj_grupe, g. podgrupa, g.naziv
{2}
order by god desc, mes desc, dan desc, g.broj_grupe, g. podgrupa
";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                string filter = String.Empty;
                if (filterGrupe)
                    filter = " having " + getGrupeFilter(grupe, "g", "grupa_id");
                string selectItems = String.Format(query, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), filter);

                ISQLQuery q = Session.CreateSQLQuery(selectItems);
                IList<object[]> result = q.List<object[]>();
                List<object[]> result2 = new List<object[]>();
                foreach (object[] row in result)
                {
                    int god = (int)row[0];
                    int mes = (int)row[1];
                    int dan = (int)row[2];
                    int grupa_id = (int)row[3];
                    int brojGrupe = (int)row[4];
                    string podgrupa = (string)row[5];
                    string naziv = (string)row[6];
                    int brojUplata = (int)row[7];
                    decimal ukupanIznos = (decimal)row[8];
                    DateTime datumVremeUplate = new DateTime(god, mes, dan);
                    result2.Add(new object[] { brojGrupe + podgrupa, naziv, brojUplata, ukupanIznos, datumVremeUplate });
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
                    result += grupe[i].Id;
                else
                    result += "," + grupe[i].Id;
            }
            result += ")) ";
            return result;
        }

        public virtual decimal getUkupanPrihod(DateTime from, DateTime to, List<Grupa> grupe)
        {
            from = from.Date;
            to = to.Date.AddDays(1);
            try
            {
                string query = @"
                    select sum(u.Iznos) from UplataClanarine u 
                    where (u.DatumVremeUplate between :from and :to)
                    ";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                if (filterGrupe)
                    query += " and " + getGrupeFilter(grupe, "u", "Grupa.Id");

                IQuery q = Session.CreateQuery(query);
                q.SetDateTime("from", from);
                q.SetDateTime("to", to);
                object result = q.UniqueResult();
                if (result == null)
                    return 0;
                else
                    return (decimal)result;
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

        public virtual List<object[]> getDnevniPrihodiKategorijeReportItems(DateTime datum)
        {
            try
            {
                // NOTE: Probao sam u where da stavim "where u.datum_vreme_uplate between '{0}' and '{1}'"
                // ali tada dobijam cudne rezultate (ne znam zasto)
                string query = @"
SELECT k.naziv, SUM(u.iznos), COUNT(*)
FROM uplate u INNER JOIN (grupe g INNER JOIN kategorije k
	ON g.kategorija_id = k.kategorija_id)
	ON u.grupa_id = g.grupa_id
WHERE datepart(year,u.datum_vreme_uplate) = {0}
    and datepart(month,u.datum_vreme_uplate) = {1}
    and datepart(day,u.datum_vreme_uplate) = {2}
GROUP BY k.kategorija_id, k.naziv
ORDER BY k.kategorija_id
";
                query = String.Format(query, datum.Year, datum.Month, datum.Day);
                ISQLQuery q = Session.CreateSQLQuery(query);
                IList result = q.List();
                List<object[]> result2 = new List<object[]>();
                for (int i = 0; i < result.Count; i++)
                {
                    object[] row = (object[])result[i];
                    string naziv = (string)row[0];
                    decimal ukupanIznos = (decimal)row[1];
                    int brojUplata = (int)row[2];
                    result2.Add(new object[] { i+1, naziv, brojUplata, ukupanIznos });
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

        public virtual List<object[]> getDnevniPrihodiClanoviReportItems(DateTime from, DateTime to, List<Grupa> grupe,
            IDictionary<int, Mesto> mestaMap)
        {
            // NOTE: Iz sql upita je izbacen left join sa tabelom mesta zato sto se left join jako sporo izvrsava
            from = from.Date;
            to = to.Date.AddDays(1);
            try
            {
                string query = @"
SELECT c.broj, c.ime, c.prezime, c.adresa, c.mesto_id,
	u.vazi_od, u.iznos
FROM grupe g INNER JOIN (uplate u INNER JOIN clanovi c
	ON u.clan_id = c.clan_id)
	ON g.grupa_id = u.grupa_id
WHERE (u.datum_vreme_uplate BETWEEN '{0}' AND '{1}')
{2}
ORDER BY datepart(year,u.datum_vreme_uplate) DESC,
    datepart(month,u.datum_vreme_uplate) DESC,
    datepart(day,u.datum_vreme_uplate) DESC,
g.broj_grupe, g.podgrupa, c.prezime, c.ime;
";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                string filter = String.Empty;
                if (filterGrupe)
                    filter = " AND " + getGrupeFilter(grupe, "g", "grupa_id");
                query = String.Format(query, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), filter);

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<object[]> result2 = new List<object[]>();
                foreach (object[] row in result)
                {
                    int broj = (int)row[0];
                    string ime = (string)row[1];
                    string prezime = (string)row[2];
                    string adresa = (string)row[3];

                    Nullable<int> mesto_id = null;
                    string mesto = String.Empty;
                    if (row[4] != null)
                    {
                        mesto_id = (int)row[4];
                        mesto = mestaMap[mesto_id.Value].Naziv;
                    }
                    
                    DateTime vaziOd = (DateTime)row[5];
                    decimal iznos = (decimal)row[6];
                    string clan = Clan.formatPrezimeImeBrojAdresaMesto(
                        prezime, ime, broj, adresa, mesto);
                    result2.Add(new object[] { clan, vaziOd, iznos });
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

        public virtual List<ReportGrupa> getDnevniPrihodiClanoviReportGrupeDanGrupa(DateTime from, DateTime to,
            List<Grupa> grupe)
        {
            // NOTE: U select delu je dodat dan, mesec i godina da bi mogao da ih koristim u order by delu.
            from = from.Date;
            to = to.Date.AddDays(1);
            try
            {
                string query = @"
SELECT
    datepart(year,u.datum_vreme_uplate) god,
    datepart(month,u.datum_vreme_uplate) mes,
    datepart(day,u.datum_vreme_uplate) dan,
    g.broj_grupe, g.podgrupa, g.naziv, Sum(u.iznos), Count(*)
FROM grupe g INNER JOIN uplate u
	ON g.grupa_id = u.grupa_id
WHERE (u.datum_vreme_uplate BETWEEN '{0}' AND '{1}')
{2}
GROUP BY
    datepart(year,u.datum_vreme_uplate),
    datepart(month,u.datum_vreme_uplate),
    datepart(day,u.datum_vreme_uplate),
    g.broj_grupe, g.podgrupa, g.naziv
ORDER BY god DESC, mes DESC, dan DESC, g.broj_grupe, g.podgrupa
";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                string filter = String.Empty;
                if (filterGrupe)
                    filter = " AND " + getGrupeFilter(grupe, "g", "grupa_id");
                query = String.Format(query, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), filter);

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<ReportGrupa> result2 = new List<ReportGrupa>();
                foreach (object[] row in result)
                {
                    int god = (int)row[0];
                    int mes = (int)row[1];
                    int dan = (int)row[2];
                    int brojGrupe = (int)row[3];
                    string podgrupa = (string)row[4];
                    string nazivGrupe = (string)row[5];
                    decimal ukupanIznos = (decimal)row[6];
                    int brojClanova = (int)row[7];
                    string sifra = brojGrupe + podgrupa;

                    object[] data = new object[] { new SifraGrupe(sifra), nazivGrupe, ukupanIznos };
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

        public virtual List<ReportGrupa> getDnevniPrihodiClanoviReportGrupeDan(DateTime from, DateTime to, List<Grupa> grupe)
        {
            from = from.Date;
            to = to.Date.AddDays(1);
            try
            {
                string query = @"
SELECT
    datepart(year,u.datum_vreme_uplate) god,
    datepart(month,u.datum_vreme_uplate) mes,
    datepart(day,u.datum_vreme_uplate) dan,
    Sum(u.iznos), Count(*)
FROM uplate u
WHERE (u.datum_vreme_uplate BETWEEN '{0}' AND '{1}')
{2}
GROUP BY
    datepart(year,u.datum_vreme_uplate),
    datepart(month,u.datum_vreme_uplate),
    datepart(day,u.datum_vreme_uplate)
ORDER BY god DESC, mes DESC, dan DESC
";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                string filter = String.Empty;
                if (filterGrupe)
                    filter = " AND " + getGrupeFilter(grupe, "u", "grupa_id");
                query = String.Format(query, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), filter);

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<ReportGrupa> result2 = new List<ReportGrupa>();
                foreach (object[] row in result)
                {
                    int god = (int)row[0];
                    int mes = (int)row[1];
                    int dan = (int)row[2];
                    decimal ukupanIznos = (decimal)row[3];
                    int brojClanova = (int)row[4];
                    DateTime datumUplate = new DateTime(god, mes, dan);

                    object[] data = new object[] { datumUplate, ukupanIznos };
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

        public virtual List<object[]> getPeriodicniPrihodiUplateReportItems(DateTime from, DateTime to, List<Grupa> grupe,
            IDictionary<int, Mesto> mestaMap)
        {
            from = from.Date;
            to = to.Date.AddDays(1);
            try
            {
                string query = @"
SELECT
    c.broj, c.ime, c.prezime, c.adresa, c.mesto_id,
    datepart(year,u.datum_vreme_uplate) god,
    datepart(month,u.datum_vreme_uplate) mes,
    datepart(day,u.datum_vreme_uplate) dan,
    u.iznos
FROM grupe g INNER JOIN (uplate u INNER JOIN clanovi c
	ON u.clan_id = c.clan_id)
	ON g.grupa_id = u.grupa_id
WHERE
    (u.datum_vreme_uplate BETWEEN '{0}' AND '{1}')
{2}
ORDER BY
    g.broj_grupe, g.podgrupa, c.prezime, c.ime,
    god DESC, mes DESC, dan DESC
";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                string filter = String.Empty;
                if (filterGrupe)
                    filter = " AND " + getGrupeFilter(grupe, "g", "grupa_id");
                query = String.Format(query, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), filter);

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<object[]> result2 = new List<object[]>();
                foreach (object[] row in result)
                {
                    int broj = (int)row[0];
                    string ime = (string)row[1];
                    string prezime = (string)row[2];
                    string adresa = (string)row[3];

                    Nullable<int> mesto_id = null;
                    string mesto = String.Empty;
                    if (row[4] != null)
                    {
                        mesto_id = (int)row[4];
                        mesto = mestaMap[mesto_id.Value].Naziv;
                    }

                    int god = (int)row[5];
                    int mes = (int)row[6];
                    int dan = (int)row[7];
                    decimal iznos = (decimal)row[8];

                    string clan = Clan.formatPrezimeImeBrojAdresaMesto(
                            prezime, ime, broj, adresa, mesto);
                    DateTime datum = new DateTime(god, mes, dan);
                    result2.Add(new object[] { clan, datum, iznos });
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

        public virtual List<ReportGrupa> getPeriodicniPrihodiUplateReportGrupe(DateTime from, DateTime to, List<Grupa> grupe)
        {
            from = from.Date;
            to = to.Date.AddDays(1);
            try
            {
                string query = @"
SELECT u.grupa_id, g.broj_grupe, g.podgrupa, g.naziv, Sum(u.iznos), Count(*)
FROM uplate u INNER JOIN grupe g
	ON u.grupa_id = g.grupa_id
WHERE (u.datum_vreme_uplate BETWEEN '{0}' AND '{1}')
{2}
GROUP BY u.grupa_id, g.broj_grupe, g.podgrupa, g.naziv
ORDER BY g.broj_grupe, g.podgrupa
";
                bool filterGrupe = grupe != null && grupe.Count > 0;
                string filter = String.Empty;
                if (filterGrupe)
                    filter = " AND " + getGrupeFilter(grupe, "g", "grupa_id");
                query = String.Format(query, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), filter);

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<ReportGrupa> result2 = new List<ReportGrupa>();
                foreach (object[] row in result)
                {
                    int grupa_id = (int)row[0];
                    int brojGrupe = (int)row[1];
                    string podgrupa = (string)row[2];
                    string nazivGrupe = (string)row[3];
                    decimal ukupanIznos = (decimal)row[4];
                    int brojClanova = (int)row[5];
                    string sifra = brojGrupe + podgrupa;

                    object[] data = new object[] { new SifraGrupe(sifra), nazivGrupe, 
												 ukupanIznos };
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

        public virtual List<object[]> getMesecniPrihodiReportItems(DateTime from, DateTime to)
        {
            from = from.Date;
            to = to.Date.AddDays(1);
            try
            {
                string query = @"
SELECT
    datepart(year,u.datum_vreme_uplate) god,
    datepart(month,u.datum_vreme_uplate) mes,
    g.broj_grupe, g.podgrupa, g.naziv, Count(*), Sum(u.iznos)
FROM uplate u INNER JOIN grupe g
	ON u.grupa_id = g.grupa_id
WHERE (u.datum_vreme_uplate BETWEEN '{0}' AND '{1}')
GROUP BY
    datepart(year,u.datum_vreme_uplate),
    datepart(month,u.datum_vreme_uplate),
    g.broj_grupe, g.podgrupa, g.naziv
ORDER BY god DESC, mes DESC, g.broj_grupe, g.podgrupa";
                query = String.Format(query, from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"));

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<object[]> result2 = new List<object[]>();
                foreach (object[] row in result)
                {
                    short god = (short)(int)row[0];
                    short mes = (short)(int)row[1];
                    int brojGrupe = (int)row[2];
                    string podgrupa = (string)row[3];
                    string nazivGrupe = (string)row[4];
                    int brojUplata = (int)row[5];
                    decimal ukupanIznos = (decimal)row[6];

                    string sifra = brojGrupe + podgrupa;

                    result2.Add(new object[] { sifra, nazivGrupe, brojUplata, ukupanIznos, god, mes });
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

        public virtual List<object[]> getUplateClanovaReportItems(int idClana)
        {
            try
            {
                string query = @"
SELECT
    u.datum_vreme_uplate,
    datepart(year,u.datum_vreme_uplate) god,
    datepart(month,u.datum_vreme_uplate) mes,
    datepart(day,u.datum_vreme_uplate) dan,
    datepart(hour,u.datum_vreme_uplate) sat,
    datepart(minute,u.datum_vreme_uplate) minut,
    g.broj_grupe, g.podgrupa, u.vazi_od, u.iznos, 
	u.napomena, u.korisnik
FROM grupe g INNER JOIN (uplate u INNER JOIN clanovi c
	ON u.clan_id = c.clan_id)
	ON g.grupa_id = u.grupa_id
{0}
ORDER BY c.broj, god DESC, mes DESC, dan DESC, sat DESC, minut DESC
";
                string filter = String.Empty;
                if (idClana != -1)
                    filter = " WHERE u.clan_id = " + idClana.ToString();
                query = String.Format(query, filter);

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<object[]> result2 = new List<object[]>();
                foreach (object[] row in result)
                {
                    DateTime datumVremeUplate = (DateTime)row[0];
                    int god = (short)(int)row[1];
                    int mes = (short)(int)row[2];
                    int dan = (short)(int)row[3];
                    int sat = (short)(int)row[4];
                    int min = (short)(int)row[5];
                    int brojGrupe = (int)row[6];
                    string podgrupa = (string)row[7];
                    DateTime vaziOd = (DateTime)row[8];
                    decimal iznos = (decimal)row[9];
                    string napomena = (string)row[10];
                    string korisnik = (string)row[11];

                    string sifra = brojGrupe + podgrupa;

                    result2.Add(new object[] { datumVremeUplate.Date, datumVremeUplate.TimeOfDay, sifra,
					    vaziOd, iznos, napomena, korisnik });
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

        public virtual List<ReportGrupa> getUplateClanovaReportGroups(int idClana, IDictionary<int, Mesto> mestaMap)
        {
            try
            {
                string query = @"
SELECT
    c.broj, c.ime, c.prezime,
    c.datum_rodjenja,
    c.adresa, c.mesto_id, Count(*)
FROM uplate u INNER JOIN clanovi c
	ON u.clan_id = c.clan_id
{0}
GROUP BY
    c.broj, c.ime, c.prezime,
    c.datum_rodjenja,
    c.adresa, c.mesto_id
ORDER BY c.broj
";
                string filter = String.Empty;
                if (idClana != -1)
                    filter = " WHERE u.clan_id = " + idClana.ToString();
                query = String.Format(query, filter);

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<ReportGrupa> result2 = new List<ReportGrupa>();
                foreach (object[] row in result)
                {
                    int broj = (int)row[0];
                    string ime = (string)row[1];
                    string prezime = (string)row[2];
                    Nullable<DateTime> datumRodjenja = null;
                    if (row[3] != null)
                        datumRodjenja = (DateTime)row[3];
                    string adresa = (string)row[4];

                    Nullable<int> mesto_id = null;
                    string mesto = String.Empty;
                    if (row[5] != null)
                    {
                        mesto_id = (int)row[5];
                        mesto = mestaMap[mesto_id.Value].Naziv;
                    }

                    string clan = Clan.formatPrezimeImeBrojDatumRodjAdresaMesto(
                            prezime, ime, broj, datumRodjenja, adresa, mesto);

                    int brojClanova = (int)row[6];
                    object[] data = new object[] { clan };

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

        #endregion

        public override IList<UplataClanarine> FindAll()
        {
            try
            {
                IQuery q = Session.CreateQuery(@"from UplataClanarine u");
                return q.List<UplataClanarine>();
            }
            catch (HibernateException ex)
            {
                string message = String.Format(
                    "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, ex.Message);
                throw new InfrastructureException(message, ex);
            }
        }

    }
}