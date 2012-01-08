using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.Mesto"/> entity.
    /// </summary>
    public interface MestoDAO : GenericDAO<Mesto, int>
    {
        IList<Mesto> FindMestoByNaziv(string naziv);
    }
}
