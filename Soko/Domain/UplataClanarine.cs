using System;
using System.Collections.Generic;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for UplataClanarine.
	/// </summary>
	public class UplataClanarine : DomainObject
	{
		public static readonly int NAPOMENA_MAX_LENGTH = 200;
		public static readonly int KORISNIK_MAX_LENGTH = 50;

		public UplataClanarine()
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

		public virtual Nullable<DateTime> DatumUplate
		{
			get
            {
                if (DatumVremeUplate == null)
                    return null;
                else
                    return DatumVremeUplate.Value.Date; 
            }
		}

		public virtual Nullable<TimeSpan> VremeUplate
		{
            get
            {
                if (DatumVremeUplate == null)
                    return null;
                else
                    return DatumVremeUplate.Value.TimeOfDay;
            }
        }

        private Nullable<DateTime> datumVremeUplate;
        public virtual Nullable<DateTime> DatumVremeUplate
        {
            get { return datumVremeUplate; }
            set { datumVremeUplate = value; }
        }

        private Nullable<DateTime> vaziOd;
		public virtual Nullable<DateTime> VaziOd
		{
			get { return vaziOd; }
			set { vaziOd = value; }
		}

		private Nullable<decimal> iznos;
		public virtual Nullable<decimal> Iznos
		{
			get { return iznos; }
			set { iznos = value; }
		}

		private string napomena;
		public virtual string Napomena
		{
			get { return napomena; }
			set { napomena = value; }
		}

		private string korisnik;
		public virtual string Korisnik
		{
			get { return korisnik; }
			set { korisnik = value; }
		}

        private List<UplataClanarine> uplate = new List<UplataClanarine>();
        public virtual List<UplataClanarine> Uplate
        {
            get { return uplate; }
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

        public virtual string IznosDin
        {
            get
            {
                if (Iznos != null)
                    return Iznos.Value.ToString("F2") + " Din";
                else
                    return String.Empty;
            }
        }

        public override void validate(Notification notification)
		{
            if (Uplate.Count == 0)
                validate(notification, this);
            else
            {
                foreach (UplataClanarine u in Uplate)
                {
                    validate(notification, u);
                }
            }
		}

        private void validate(Notification notification, UplataClanarine u)
        {
            if (u.Clan == null)
            {
                notification.RegisterMessage("Clan", "Clan je obavezan.");
            }

            if (u.Grupa == null)
            {
                notification.RegisterMessage("Grupa", "Grupa je obavezna.");
            }

            if (u.DatumUplate == null)
            {
                notification.RegisterMessage(
                    "DatumUplate", "Datum uplate clanarine je obavezan.");
            }

            if (u.VremeUplate == null)
            {
                notification.RegisterMessage(
                    "VremeUplate", "Vreme uplate clanarine je obavezno.");
            }

            if (u.VaziOd == null)
            {
                notification.RegisterMessage(
                    "VaziOd", "Datum vazenja clanarine je obavezan.");
            }

            if (u.Iznos == null)
            {
                notification.RegisterMessage(
                    "Iznos", "Iznos uplate je obavezan.");
            }
            else if (u.Iznos < 0)
            {
                notification.RegisterMessage(
                    "Iznos", "Iznos uplate mora da bude pozitivan.");
            }

            if (u.Napomena != null && u.Napomena.Length > NAPOMENA_MAX_LENGTH)
            {
                notification.RegisterMessage(
                    "Napomena", "Napomena moze da sadrzi maksimalno "
                    + NAPOMENA_MAX_LENGTH + " znakova.");
            }

            if (u.Korisnik != null && u.Korisnik.Length > KORISNIK_MAX_LENGTH)
            {
                notification.RegisterMessage(
                    "Korisnik", "Naziv korisnika moze da sadrzi maksimalno "
                    + KORISNIK_MAX_LENGTH + " znakova.");
            }
        }
    }
}
