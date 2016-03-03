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
            if (Clan == null)
            {
                notification.RegisterMessage("Clan", "Clan je obavezan.");
            }

            if (Grupa == null)
            {
                notification.RegisterMessage("Grupa", "Grupa je obavezna.");
            }

            if (DatumUplate == null)
            {
                notification.RegisterMessage(
                    "DatumUplate", "Datum uplate clanarine je obavezan.");
            }

            if (VremeUplate == null)
            {
                notification.RegisterMessage(
                    "VremeUplate", "Vreme uplate clanarine je obavezno.");
            }

            if (VaziOd == null)
            {
                notification.RegisterMessage(
                    "VaziOd", "Datum vazenja clanarine je obavezan.");
            }

            if (Iznos == null)
            {
                notification.RegisterMessage(
                    "Iznos", "Iznos uplate je obavezan.");
            }
            else if (Iznos < 0)
            {
                notification.RegisterMessage(
                    "Iznos", "Iznos uplate mora da bude pozitivan.");
            }

            if (Napomena != null && Napomena.Length > NAPOMENA_MAX_LENGTH)
            {
                notification.RegisterMessage(
                    "Napomena", "Napomena moze da sadrzi maksimalno "
                    + NAPOMENA_MAX_LENGTH + " znakova.");
            }

            if (Korisnik != null && Korisnik.Length > KORISNIK_MAX_LENGTH)
            {
                notification.RegisterMessage(
                    "Korisnik", "Naziv korisnika moze da sadrzi maksimalno "
                    + KORISNIK_MAX_LENGTH + " znakova.");
            }
        }
    }
}
