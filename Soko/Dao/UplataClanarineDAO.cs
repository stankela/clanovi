using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;

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
    }
}
