using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Soko.Domain;
using System.ComponentModel;

namespace Soko.Misc
{
    class Util
    {
        public static void sortByVaziOdDesc(List<UplataClanarine> uplate)
        {
            PropertyDescriptor propDesc =
                TypeDescriptor.GetProperties(typeof(UplataClanarine))["VaziOd"];
            uplate.Sort(new SortComparer<UplataClanarine>(propDesc, ListSortDirection.Descending));
        }

        public static void sortByPrezimeImeDatumRodjenja(List<Clan> clanovi)
        {
            PropertyDescriptor propDescPrez = TypeDescriptor.GetProperties(typeof(Clan))["Prezime"];
            PropertyDescriptor propDescIme = TypeDescriptor.GetProperties(typeof(Clan))["Ime"];
            PropertyDescriptor propDescDatumRodj = TypeDescriptor.GetProperties(typeof(Clan))["DatumRodjenja"];
            PropertyDescriptor[] propDesc = new PropertyDescriptor[3] { propDescPrez, propDescIme, propDescDatumRodj };
            ListSortDirection[] direction = new ListSortDirection[3] { 
                ListSortDirection.Ascending, ListSortDirection.Ascending, ListSortDirection.Ascending };

            clanovi.Sort(new SortComparer<Clan>(propDesc, direction));
        }

        // Pretpostavlja se da lista sadrzi samo dolaske za jednog clana
        public static void sortByDatumDolaskaDesc(List<DolazakNaTrening> dolasci)
        {
            PropertyDescriptor propDesc =
                TypeDescriptor.GetProperties(typeof(DolazakNaTrening))["DatumDolaska"];
            dolasci.Sort(new SortComparer<DolazakNaTrening>(propDesc, ListSortDirection.Descending));
        }

        public static string getGrupeFilter(List<Grupa> grupe, string table, string grupaColumn)
        {
            string result;
            if (table != String.Empty)
                result = String.Format("({0}.{1} IN (", table, grupaColumn);
            else
                result = String.Format("({0} IN (", grupaColumn);

            for (int i = 0; i < grupe.Count; i++)
            {
                if (i == 0)
                    result += grupe[i].Id;
                else
                    result += "," + grupe[i].Id;
            }
            result += ")) ";
            return result;
        }
    }
}
