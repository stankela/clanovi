using System;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for FinansijskaCelina.
	/// </summary>
	public class FinansijskaCelina : DomainObject, IComparable
	{
		public static readonly int NAZIV_MAX_LENGTH = 50;

		private string naziv;
		public virtual string Naziv
		{
			get { return naziv; }
			set { naziv = value; }
		}

        public FinansijskaCelina()
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
					"Naziv", "Naziv je obavezan.");
			}
			else if (Naziv.Length > NAZIV_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Naziv", "Naziv moze da sadrzi maksimalno "
					+ NAZIV_MAX_LENGTH + " znakova.");
			}
		}

		#region IComparable Members

		public virtual int CompareTo(object obj)
		{
            if (!(obj is FinansijskaCelina))
				throw new ArgumentException();
            FinansijskaCelina other = obj as FinansijskaCelina;
			return this.Naziv.CompareTo(other.Naziv);
		}

		#endregion
	}
}
