using System;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for Kategorija.
	/// </summary>
	public class Kategorija : DomainObject, IComparable
	{
		public static readonly int NAZIV_MAX_LENGTH = 50;

		private string naziv;
		public virtual string Naziv
		{
			get { return naziv; }
			set { naziv = value; }
		}

		public Kategorija()
		{ 
        
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
					"Naziv", "Naziv kategorije je obavezan.");
			}
			else if (Naziv.Length > NAZIV_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Naziv", "Naziv kategorije moze da sadrzi maksimalno "
					+ NAZIV_MAX_LENGTH + " znakova.");
			}
		}

		#region IComparable Members

		public virtual int CompareTo(object obj)
		{
			if (!(obj is Kategorija))
				throw new ArgumentException();
			Kategorija other = obj as Kategorija;
			return this.Naziv.CompareTo(other.Naziv);
		}

		#endregion
	}
}
