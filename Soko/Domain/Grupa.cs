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

        private SifraGrupe sifra;
		public virtual SifraGrupe Sifra
		{
			get { return sifra; }
			set { sifra = value; }
		}

        private string naziv;
		public virtual string Naziv
		{
			get { return naziv; }
			set { naziv = value; }
		}

		private Kategorija kategorija;
		public virtual Kategorija Kategorija
		{
			get { return kategorija; }
			set { kategorija = value; }
		}

		private int sortOrder;
		public virtual int SortOrder
		{
			get { return sortOrder; }
			set { sortOrder = value; }
		}

        private bool imaGodisnjuClanarinu;
        public virtual bool ImaGodisnjuClanarinu
        {
            get { return imaGodisnjuClanarinu; }
            set { imaGodisnjuClanarinu = value; }
        }

		public Grupa()
		{

		}

		public Grupa(SifraGrupe sifra, string naziv, Kategorija kategorija)
		{
			this.sifra = sifra;
			this.naziv = naziv;
			this.kategorija = kategorija;
		}

		public virtual string SifraNaziv
		{
			get { return Sifra.Value + "   " + Naziv; }
		}

		public virtual string SifraCrtaNaziv
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

		public virtual int CompareTo(object obj)
		{
			if (!(obj is Grupa))
				throw new ArgumentException();

			Grupa other = (Grupa)obj;
			return this.Sifra.CompareTo(other.Sifra);
		}

		#endregion
	}
}
