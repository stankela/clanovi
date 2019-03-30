using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.FinansijskaCelina"/> entity.
    /// </summary>
    public interface FinansijskaCelinaDAO : GenericDAO<FinansijskaCelina, int>
    {
        bool existsFinansijskaCelinaNaziv(string naziv);
        IList<FinansijskaCelina> FindAllSortById();
    }
}
