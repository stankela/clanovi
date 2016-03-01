using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;

// TODO2: Izbaci Bilten svugde gde se pojavljuje

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
        int getMaxBrojKartice();
        Clan findForBrojKartice(int brojKartice);
    }
}
