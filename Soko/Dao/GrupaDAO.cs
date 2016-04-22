using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.Grupa"/> entity.
    /// </summary>
    public interface GrupaDAO : GenericDAO<Grupa, int>
    {
        bool existsGrupa(Kategorija k);
        bool existsGrupaSifra(SifraGrupe sifra);
        bool existsGrupaNaziv(string naziv);
        Grupa findGodisnjaClanarina();
    }
}
