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
        public static void sortByDatumVremeUplateDesc(List<UplataClanarine> uplate)
        {
            PropertyDescriptor propDesc =
                TypeDescriptor.GetProperties(typeof(UplataClanarine))["DatumVremeUplate"];
            uplate.Sort(new SortComparer<UplataClanarine>(propDesc, ListSortDirection.Descending));
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
