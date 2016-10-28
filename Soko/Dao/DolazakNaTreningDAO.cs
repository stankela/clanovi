using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;
using Soko.Report;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.DolazakNaTrening"/> entity.
    /// </summary>
    public interface DolazakNaTreningDAO : GenericDAO<DolazakNaTrening, int>
    {
        IList<DolazakNaTrening> getDolazakNaTrening(DateTime from, DateTime to);
        IList<DolazakNaTrening> getDolazakNaTrening(Clan c, DateTime from, DateTime to);
        List<object[]> getEvidencijaTreningaReportItems(DateTime from, DateTime to, List<Grupa> grupe);
        List<ReportGrupa> getEvidencijaTreningaReportGrupe(DateTime from, DateTime to, List<Grupa> grupe);
        List<object[]> getEvidencijaTreningaReportItems(int clanId, DateTime from, DateTime to, List<Grupa> grupe);
        List<object[]> getDolazakNaTreningMesecniReportItems(DateTime from, DateTime to, bool samoNedostajuceUplate);
  }
}
