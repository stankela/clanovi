using System;

namespace Soko.Domain
{
    public class DolazakNaTreningMesecni : DomainObject
    {
        public DolazakNaTreningMesecni()
        {

        }

        private Clan clan;
        public virtual Clan Clan
        {
            get { return clan; }
            set { clan = value; }
        }

        private int godina;
        public virtual int Godina
        {
            get { return godina; }
            set { godina = value; }
        }

        private int mesec;
        public virtual int Mesec
        {
            get { return mesec; }
            set { mesec = value; }
        }

        private int brojDolazaka;
        public virtual int BrojDolazaka
        {
            get { return brojDolazaka; }
            set { brojDolazaka = value; }
        }
    }
}
