using System;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for Grupa.
	/// </summary>
	public class Grupa : DomainObject, IComparable
	{
		public static readonly int SIFRA_MAX_LENGTH = 6;
		public static readonly int NAZIV_MAX_LENGTH = 50;
		
		public SifraGrupe Sifra
		{
			get
			{
				if (Key != null)
					return (SifraGrupe)Key.Value();
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

		private Kategorija kategorija;
		public Kategorija Kategorija
		{
			get { return kategorija; }
			set { kategorija = value; }
		}

		private int sortOrder;
		public int SortOrder
		{
			get { return sortOrder; }
			set { sortOrder = value; }
		}

		public Grupa()
		{

		}

		public Grupa(SifraGrupe sifra, string naziv, Kategorija kategorija)
		{
			Sifra = sifra;
			this.naziv = naziv;
			this.kategorija = kategorija;
		}

		public string SifraNaziv
		{
			get { return Sifra.Value + "   " + Naziv; }
		}

		public string SifraCrtaNaziv
		{
			// TODO: Umesto sto kreiras nova svojstva za razlicite formate ispisa,
			// mozda bi bilo bolje da implementiras ToString koji prima format
			// parametre
			get { return formatSifraCrtaNaziv(Sifra.Value, Naziv); }
		}

		public static string formatSifraCrtaNaziv(string sifra, string naziv)
		{
			return sifra + " - " + naziv;
		}

		public override string ToString()
		{
			return SifraCrtaNaziv;
		}

		public override void validate(Notification notification)
		{
			// validate Sifra
			if (Sifra == null)
			{
				notification.RegisterMessage(
					"Sifra", "Sifra grupe je obavezna.");
			}
			else if (Sifra.Value.Length > SIFRA_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Sifra", "Sifra grupe moze da sadrzi maksimalno "
					+ SIFRA_MAX_LENGTH + " znakova.");
			}
			
			// validate Naziv
			if (String.IsNullOrEmpty(Naziv))
			{
				notification.RegisterMessage(
					"Naziv", "Naziv grupe je obavezan.");
			}
			else if (Naziv.Length > NAZIV_MAX_LENGTH)
			{
				notification.RegisterMessage(
					"Naziv", "Naziv grupe moze da sadrzi maksimalno "
					+ NAZIV_MAX_LENGTH + " znakova.");
			}
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (!(obj is Grupa))
				throw new ArgumentException();

			Grupa other = (Grupa)obj;
			return this.Sifra.CompareTo(other.Sifra);
		}

		#endregion
	}
}
