using System;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for Institucija.
	/// </summary>
	public class Institucija : DomainObject
	{
		public static readonly int NAZIV_MAX_LENGTH = 50;
		public static readonly int ADRESA_MAX_LENGTH = 50;
    
		private string naziv;
		public string Naziv
		{
			get { return naziv; }
			set { naziv = value; }
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

		public string MestoNaziv
		{
			get
			{
				if (Mesto != null)
					return Mesto.Naziv;
				else
					return String.Empty;
			}
		}

		public string NazivAdresaMesto
		{
			get
			{
				string result = Naziv;
				if (!String.IsNullOrEmpty(Adresa))
				{
					result += ", " + Adresa;
				}
				if (MestoNaziv != String.Empty)
				{
					result += ", " + MestoNaziv;
				}
				return result;
			}
		}

		public Institucija()
		{

		}

		public Institucija(string naziv, string adresa, Mesto mesto)
		{
			this.naziv = naziv;
			this.adresa = adresa;
			this.mesto = mesto;
		}

		public override string ToString()
		{
			return Naziv;
		}

		public override void validate(Notification notification)
		{
			// validate Naziv
			if (String.IsNullOrEmpty(Naziv))
			{
				notification.RegisterMessage(
					"Naziv", "Naziv institucije je obavezan.");
			}
			else if (Naziv.Length > NAZIV_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Naziv", "Naziv institucije moze da sadrzi maksimalno "
					+ NAZIV_MAX_LENGTH + " znakova.");
			}
          
			// validate Adresa
			if (Adresa != null && Adresa.Length > ADRESA_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Adresa", "Adresa institucije moze da sadrzi maksimalno "
					+ ADRESA_MAX_LENGTH + " znakova.");
			}
		}
	}
}
