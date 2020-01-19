using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;
using Soko.Report;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.DolazakNaTreningMesecni"/> entity.
    /// </summary>
    public interface DolazakNaTreningMesecniDAO : GenericDAO<DolazakNaTreningMesecni, int>
    {
        DolazakNaTreningMesecni getDolazakNaTrening(Clan c, int god, int mes);
        IList<DolazakNaTreningMesecni> getDolazakNaTrening(Clan c, int fromYear, int fromMonth, int toYear, int toMonth);
        void deleteDolasci(DateTime from, DateTime to);
        void insertDolazak(int godina, int mesec, int brojDolazaka, int clan_id);
    }
}
