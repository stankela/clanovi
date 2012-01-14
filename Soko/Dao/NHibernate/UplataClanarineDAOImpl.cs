using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;

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
                return (decimal)q.UniqueResult();
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