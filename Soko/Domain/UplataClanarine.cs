using System;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for UplataClanarine.
	/// </summary>
	public class UplataClanarine : DomainObject
	{
		public readonly int NAPOMENA_MAX_LENGTH = 200;
		public readonly int KORISNIK_MAX_LENGTH = 50;

		public UplataClanarine()
		{

		}

		private Clan clan;
		public Clan Clan
		{
			get { return clan; }
			set { clan = value; }
		}

		private Grupa grupa;
		public Grupa Grupa
		{
			get { return grupa; }
			set { grupa = value; }
		}

		private Nullable<DateTime> datumUplate;
		public Nullable<DateTime> DatumUplate
		{
			get { return datumUplate; }
			set { datumUplate = value; }
		}

		private Nullable<TimeSpan> vremeUplate;
		public Nullable<TimeSpan> VremeUplate
		{
			get { return vremeUplate; }
			set { vremeUplate = value; } 
		}

		private Nullable<DateTime> vaziOd;
		public Nullable<DateTime> VaziOd
		{
			get { return vaziOd; }
			set { vaziOd = value; }
		}

		private Nullable<decimal> iznos;
		public Nullable<decimal> Iznos
		{
			get { return iznos; }
			set { iznos = value; }
		}

		private string napomena;
		public string Napomena
		{
			get { return napomena; }
			set { napomena = value; }
		}

		private string korisnik;
		public string Korisnik
		{
			get { return korisnik; }
			set { korisnik = value; }
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
			else if (Iznos <= 0)
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
