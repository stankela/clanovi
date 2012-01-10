using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.MesecnaClanarina"/> entity.
    /// </summary>
    public interface MesecnaClanarinaDAO : GenericDAO<MesecnaClanarina, int>
    {
        bool existsClanarinaGrupa(Grupa g);
        IList<MesecnaClanarina> getCenovnik();
        IList<MesecnaClanarina> findForGrupa(Grupa g);
        MesecnaClanarina getVazecaClanarinaForGrupa(Grupa g);
    }
}
