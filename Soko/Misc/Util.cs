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

    }
}
