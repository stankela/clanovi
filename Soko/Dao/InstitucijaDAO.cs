using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.Institucija"/> entity.
    /// </summary>
    public interface InstitucijaDAO : GenericDAO<Institucija, int>
    {
        bool existsInstitucijaMesto(Mesto m);
    }
}
