using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.Kategorija"/> entity.
    /// </summary>
    public interface KategorijaDAO : GenericDAO<Kategorija, int>
    {
        bool existsKategorijaNaziv(string naziv);
        IList<Kategorija> FindAllSortById();
    }
}
