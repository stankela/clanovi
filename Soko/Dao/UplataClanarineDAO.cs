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
        IList<UplataClanarine> findUplateVaziOd(DateTime from, DateTime to);
        IList<UplataClanarine> findUplate(IList<Grupa> grupe, DateTime from, DateTime to);
        IList<Grupa> getGrupeBezKategorija(DateTime datumUplate);
        List<object[]> getDnevniPrihodiGrupeReportItems(DateTime from, DateTime to, List<Grupa> grupe);
        decimal getUkupanPrihod(DateTime from, DateTime to, List<Grupa> grupe);
        List<object[]> getDnevniPrihodiKategorijeReportItems(DateTime datum, List<Grupa> grupe);
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
        List<object[]> getAktivniClanoviReportItems(DateTime from, DateTime to, List<Grupa> grupe,
            IDictionary<int, Mesto> mestaMap);
        List<object[]> getMesecniPrihodiReportItems(DateTime from, DateTime to, List<Grupa> grupe);
        List<object[]> getUplateClanovaReportItems(int idClana);
        List<ReportGrupa> getUplateClanovaReportGroups(int idClana, IDictionary<int, Mesto> mestaMap);
        int countUplateDatumVremeUplate(DateTime from, DateTime to);
        void deleteUplateDatumVremeUplate(DateTime from, DateTime to);
        List<object[]> findUplateVaziOdPlacenoUnapred(DateTime from, DateTime to);
        void insertUplata(DateTime datum_vreme_uplate, DateTime vazi_od, decimal iznos, string napomena,
            string korisnik, int clan_id, int grupa_id);
    }
}
