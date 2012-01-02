using System;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for Mesto.
	/// </summary>
	public class Mesto : DomainObject
	{
		public static readonly int ZIP_MAX_LENGTH = 5;
		public static readonly int NAZIV_MAX_LENGTH = 30;
		
		public Mesto()
		{

		}

		public string Zip
		{
			get
			{
				if (Key != null)
					return Key.stringValue();
				return null;
			}
			set
			{
				Key = new Key(value);
			}
		}

		private string naziv;
		public string Naziv
		{
			get { return naziv; }
			set { naziv = value; }
		}
	
		public Mesto(string zip, string naziv)
		{
			this.Key = new Key(zip);
			this.naziv = naziv;
		}

		public override string ToString()
		{
			return ZipNaziv;
		}

		public string ZipNaziv
		{
			get { return Zip + " " + Naziv; }
		}

		public override void validate(Notification notification)
		{
			// validate Zip
			if (String.IsNullOrEmpty(Zip))
			{
				notification.RegisterMessage(
					"Zip", "Postanski broj je obavezan.");
			}
			else if (Zip.Length > ZIP_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Zip", "Postanski broj moze da sadrzi maksimalno "
					+ ZIP_MAX_LENGTH + " znakova.");
			}
			
			// validate Naziv
			if (String.IsNullOrEmpty(Naziv))
			{
				notification.RegisterMessage(
					"Naziv", "Naziv mesta je obavezan.");
			}
			else if (Naziv.Length > NAZIV_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Naziv", "Naziv mesta moze da sadrzi maksimalno "
					+ NAZIV_MAX_LENGTH + " znakova.");
			}
		}

	}
}
