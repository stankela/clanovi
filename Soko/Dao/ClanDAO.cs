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
        bool existsClanImePrezimeDatumRodjenja(Clan g);
        int getMaxBroj();
        Clan findForBrojKartice(int brojKartice);
        Clan findForSerijskiBrojKartice(Int64 brojKartice);
        IList<Clan> findKojiNePlacajuClanarinu();
        IList<Clan> findClanoviSaKarticom();
    }
}
