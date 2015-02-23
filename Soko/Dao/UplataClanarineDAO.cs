using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;
using Soko.Report;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.UplataClanarine"/> entity.
    /// </summary>
    public interface UplataClanarineDAO : GenericDAO<UplataClanarine, int>
    {
        bool existsUplataClan(Clan c);
        bool existsUplataGrupa(Grupa g);
        IList<UplataClanarine> findUplate(Clan c);
        IList<UplataClanarine> findUplate(DateTime from, DateTime to);
        IList<Grupa> getGrupeBezKategorija(DateTime datumUplate);
        List<object[]> getDnevniPrihodiGrupeReportItems(DateTime from, DateTime to, List<Grupa> grupe);
        decimal getUkupanPrihod(DateTime from, DateTime to, List<Grupa> grupe);
        List<object[]> getDnevniPrihodiKategorijeReportItems(DateTime datum);
        List<object[]> getDnevniPrihodiClanoviReportItems(DateTime from, DateTime to, List<Grupa> grupe,
            IDictionary<int, Mesto> mestaMap);
        List<ReportGrupa> getDnevniPrihodiClanoviReportGrupeDanGrupa(DateTime from, DateTime to, List<Grupa> grupe);
        List<ReportGrupa> getDnevniPrihodiClanoviReportGrupeDan(DateTime from, DateTime to, List<Grupa> grupe);
        List<object[]> getPeriodicniPrihodiUplateReportItems(DateTime from, DateTime to, List<Grupa> grupe,
            IDictionary<int, Mesto> mestaMap);
        List<ReportGrupa> getPeriodicniPrihodiUplateReportGrupe(DateTime from, DateTime to, List<Grupa> grupe);
        List<object[]> getAktivniClanoviPoGrupamaReportItems(DateTime from, DateTime to, List<Grupa> grupe,
            IDictionary<int, Mesto> mestaMap, IDictionary<SifraGrupe, int> duplikati);
        List<ReportGrupa> getAktivniClanoviPoGrupamaReportGrupe(DateTime from, DateTime to, List<Grupa> grupe,
            IDictionary<SifraGrupe, int> duplikati);
        List<object[]> getAktivniClanoviReportItems(DateTime from, DateTime to, IDictionary<int, Mesto> mestaMap);
        List<object[]> getMesecniPrihodiReportItems(DateTime from, DateTime to);
        List<object[]> getUplateClanovaReportItems(int idClana);
        List<ReportGrupa> getUplateClanovaReportGroups(int idClana, IDictionary<int, Mesto> mestaMap);
    }
}
