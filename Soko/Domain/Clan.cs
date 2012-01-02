using System;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for Clan.
	/// </summary>
	public class Clan : DomainObject
	{
		private readonly static int IME_MAX_LENGTH = 20;
		private readonly static int PREZIME_MAX_LENGTH = 30;
		private readonly static int ADRESA_MAX_LENGTH = 70;
		private readonly static int TELEFON_MAX_LENGTH = 20;
		private readonly static int NAPOMENA_MAX_LENGTH = 255;
	
		public Clan()
		{

		}

		private Nullable<int> broj;
        public Nullable<int> Broj
		{
			get { return broj; }
			set { broj = value; }
		}

		private string ime;
		public string Ime
		{
			get { return ime; }
			set { ime = value; }
		}

		private string prezime;
		public string Prezime
		{
			get { return prezime; }
			set { prezime = value; }
		}

		private Nullable<DateTime> datumRodjenja;
        public Nullable<DateTime> DatumRodjenja
		{
			get { return datumRodjenja; }
			set { datumRodjenja = value; }
		}

		private string adresa;
		public string Adresa
		{
			get { return adresa; }
			set { adresa = value; }
		}

		private Mesto mesto;
		public Mesto Mesto
		{
			get { return mesto; }
			set { mesto = value; }
		}

		private string telefon1;
		public string Telefon1
		{
			get { return telefon1; }
			set { telefon1 = value; }
		}

		private string telefon2;
		public string Telefon2
		{
			get { return telefon2; }
			set { telefon2 = value; }
		}

		private Institucija institucija;
		public Institucija Institucija
		{
			get { return institucija; }
			set { institucija = value; }
		}

		private string napomena;
		public string Napomena
		{
			get { return napomena; }
			set { napomena = value; }
		}

		public string BrojPrezimeImeDatumRodjenja
		{
			get
			{
				string result = String.Empty;
				if (Broj != null)
					result += Broj.ToString();
				if (!String.IsNullOrEmpty(Prezime))
				{
					if (result != String.Empty)
						result += new String(' ', 3);
					result += Prezime;
				}
				if (!String.IsNullOrEmpty(Ime))
				{
					if (result != String.Empty)
						result += new String(' ', 1);
					result += Ime;
				}
				if (DatumRodjenja != null)
				{
					if (result != String.Empty)
						result += new String(' ', 2);
					result += DatumRodjenja.Value.ToShortDateString();
				}
				return result;
			}
		}

		public string BrojImePrezime
		{
			get
			{
				string result = String.Empty;
				if (Broj != null)
					result += Broj.ToString();
				if (!String.IsNullOrEmpty(Ime))
				{
					if (result != String.Empty)
						result += new String(' ', 1);
					result += Ime;
				}
				if (!String.IsNullOrEmpty(Prezime))
				{
					if (result != String.Empty)
						result += new String(' ', 1);
					result += Prezime;
				}
				return result;
			}
		}

		public override string ToString()
		{
			return BrojImePrezime;
		}

		public string NazivMesta
		{
			get
			{
				if (mesto != null)
					return mesto.Naziv;
				else
					return String.Empty;
			}
		}

		public string PrezimeImeBrojDatumRodjAdresaMesto
		{
			get
			{
				string p = String.Empty;
				if (Prezime != null)
					p = Prezime;

				string i = String.Empty;
				if (Ime != null)
					i = Ime;

				string a = String.Empty;
				if (Adresa != null)
					a = Adresa;

				string m = String.Empty;
				if (Mesto != null)
					m = Mesto.Naziv;

                return formatPrezimeImeBrojDatumRodjAdresaMesto(
                    p, i, Broj, DatumRodjenja, a, m);
			}
		}

		public static string formatPrezimeImeBrojDatumRodjAdresaMesto(string prezime,
			string ime, Nullable<int> broj, Nullable<DateTime> datumRodjenja, 
            string adresa, string mesto)
		{
			string result = String.Empty;
			if (prezime != String.Empty)
			{
				result += prezime;
			}
			if (ime != String.Empty)
			{
				if (result != String.Empty)
					result += " ";
				result += ime;
			}
			if (broj != null)
			{
				if (result != String.Empty)
					result += " ";
				result += "[" + broj.Value + "]";
			}
			if (datumRodjenja != null)
			{
				if (result != String.Empty)
					result += ", ";
				result += datumRodjenja.Value.ToShortDateString();
			}
			if (adresa != String.Empty)
			{
				if (result != String.Empty)
					result += ", ";
				result += adresa;
			}
			if (mesto != String.Empty)
			{
				if (result != String.Empty)
					result += ", ";
				result += mesto;
			}
			return result;
		}

		public static string formatPrezimeImeBrojAdresaMesto(string prezime,
			string ime, Nullable<int> broj, string adresa, string mesto)
		{
			return formatPrezimeImeBrojDatumRodjAdresaMesto(prezime, ime, broj,
				null, adresa, mesto);
		}

		public override void validate(Notification notification)
		{
			if (Broj == null)
			{
				notification.RegisterMessage("Broj", "Broj clana je obavezan.");
			}
			else if (Broj <= 0)
			{
				notification.RegisterMessage("Broj", "Broj clana mora da bude pozitivan.");
			}

			if (Ime != null && Ime.Length > IME_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Ime", "Ime clana moze da sadrzi maksimalno "
					+ IME_MAX_LENGTH + " znakova.");
			}

			if (Prezime != null && Prezime.Length > PREZIME_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Prezime", "Prezime clana moze da sadrzi maksimalno "
					+ PREZIME_MAX_LENGTH + " znakova.");
			}

			if (String.IsNullOrEmpty(Ime) && String.IsNullOrEmpty(Prezime))
			{
				notification.RegisterMessage(
					"Ime", "Obavezno je ime ili prezime.");
			}
	
			if (Adresa != null && Adresa.Length > ADRESA_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Adresa", "Adresa moze da sadrzi maksimalno "
					+ ADRESA_MAX_LENGTH + " znakova.");
			}
			
			if (Telefon1 != null && Telefon1.Length > TELEFON_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Telefon1", "Telefon moze da sadrzi maksimalno "
					+ TELEFON_MAX_LENGTH + " znakova.");
			}
			
			if (Telefon2 != null && Telefon2.Length > TELEFON_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Telefon2", "Telefon moze da sadrzi maksimalno "
					+ TELEFON_MAX_LENGTH + " znakova.");
			}
					
			if (Napomena != null && Napomena.Length > NAPOMENA_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Napomena", "Napomena moze da sadrzi maksimalno "
					+ NAPOMENA_MAX_LENGTH + " znakova.");
			}
		}
	}
}
