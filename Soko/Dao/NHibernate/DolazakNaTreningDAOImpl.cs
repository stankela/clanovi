using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;
using System.Collections;
using Soko.Report;
using Soko.Misc;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="DolazakNaTreningDAO"/>.
    /// </summary>
    public class DolazakNaTreningDAOImpl : GenericNHibernateDAO<DolazakNaTrening, int>, DolazakNaTreningDAO
    {
        public virtual List<object[]> getEvidencijaTreningaReportItems(DateTime from, DateTime to, List<Grupa> grupe)
        {
            to = to.AddMinutes(1);
            try
            {
                string query = @"
SELECT
    c.broj, c.ime, c.prezime, c.datum_rodjenja,
    d.datum_vreme_dolaska, d.datum_poslednje_uplate
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

                ISQLQuery q = Session.CreateSQLQuery(query);
                IList<object[]> result = q.List<object[]>();
                List<object[]> result2 = new List<object[]>();
                foreach (object[] row in result)
                {
                    int broj = (int)row[0];
                    string ime = (string)row[1];
                    string prezime = (string)row[2];

                    Nullable<DateTime> datumRodjenja = null;
                    if (row[3] != null)
                        datumRodjenja = (DateTime)row[3];

                    DateTime datumVremeDolaska = (DateTime)row[4];

                    Nullable<DateTime> datumPoslednjeUplate = null;
                    if (row[5] != null)
                        datumPoslednjeUplate = (DateTime)row[5];

                    string clan = Clan.formatPrezimeImeBrojDatumRodjAdresaMesto(
                            prezime, ime, broj, datumRodjenja, String.Empty, String.Empty);
                    result2.Add(new object[] { clan, datumVremeDolaska, datumPoslednjeUplate });
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
                    string sifraNaziv = "Gimnasticari koji nemaju nijednu uplatu";
                    if (row[0] != null)
                    {
                        int grupa_id = (int)row[0];
                        int brojGrupe = (int)row[1];
                        string podgrupa = (string)row[2];
                        string nazivGrupe = (string)row[3];
                        string sifra = brojGrupe + podgrupa;
                        sifraNaziv = sifra + " - " + nazivGrupe;
                    }
                    decimal ukupanIznos = (decimal)0;
                    int brojClanova = (int)row[4];

                    object[] data = new object[] { sifraNaziv, ukupanIznos };
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

    }
}