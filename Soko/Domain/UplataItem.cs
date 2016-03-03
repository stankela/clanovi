using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soko.Domain
{
    public class UplataItem
    {
        private DateTime vaziOd;
        public virtual DateTime VaziOd
        {
            get { return vaziOd; }
            set { vaziOd = value; }
        }

        private decimal iznos;
        public virtual decimal Iznos
        {
            get { return iznos; }
            set { iznos = value; }
        }

    }
}
