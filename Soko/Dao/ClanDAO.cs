using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.Clan"/> entity.
    /// </summary>
    public interface ClanDAO : GenericDAO<Clan, int>
    {
        bool existsClanMesto(Mesto m);
        bool existsClanInstitucija(Institucija i);
        int getMaxBroj();
    }
}
