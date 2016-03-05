using System;

namespace Soko.Domain
{
    public class DolazakNaTrening : DomainObject
    {
        public DolazakNaTrening()
        {

        }

        private Clan clan;
        public virtual Clan Clan
        {
            get { return clan; }
            set { clan = value; }
        }

        private Grupa grupa;
        public virtual Grupa Grupa
        {
            get { return grupa; }
            set { grupa = value; }
        }

        public virtual Nullable<DateTime> DatumDolaska
        {
            get
            {
                if (DatumVremeDolaska == null)
                    return null;
                else
                    return DatumVremeDolaska.Value.Date;
            }
        }

        public virtual Nullable<TimeSpan> VremeDolaska
        {
            get
            {
                if (DatumVremeDolaska == null)
                    return null;
                else
                    return DatumVremeDolaska.Value.TimeOfDay;
            }
        }

        private Nullable<DateTime> datumVremeDolaska;
        public virtual Nullable<DateTime> DatumVremeDolaska
        {
            get { return datumVremeDolaska; }
            set { datumVremeDolaska = value; }
        }

        public virtual string PrezimeImeBrojDatumRodj
        {
            get
            {
                if (Clan != null)
                    return Clan.PrezimeImeBrojDatumRodj;
                else
                    return String.Empty;
            }
        }

        public virtual string SifraGrupeCrtaNazivGrupe
        {
            get
            {
                if (Grupa != null)
                    return Grupa.SifraCrtaNaziv;
                else
                    return String.Empty;
            }
        }

        public override void validate(Notification notification)
        {
            if (Clan == null)
            {
                notification.RegisterMessage("Clan", "Clan je obavezan.");
            }

            if (Grupa == null)
            {
                notification.RegisterMessage("Grupa", "Grupa je obavezna.");
            }

            if (DatumDolaska == null)
            {
                notification.RegisterMessage(
                    "DatumDolaska", "Datum dolaska je obavezan.");
            }

            if (VremeDolaska == null)
            {
                notification.RegisterMessage(
                    "VremeDolaska", "Vreme dolaska je obavezno.");
            }
        }

    }
}
