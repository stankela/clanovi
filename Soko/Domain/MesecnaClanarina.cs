using System;

namespace Soko.Domain
{
	/// <summary>
	/// Summary description for MesecnaClanarina.
	/// </summary>
	public class MesecnaClanarina : DomainObject
	{
		private readonly static DateTime VAZI_OD_PLACEHOLDER = DateTime.MinValue;

		public MesecnaClanarina()
		{

		}

		private Grupa grupa;
		public virtual Grupa Grupa
		{
			get { return grupa; }
			set
			{ 
				if (value == null)
					throw new ArgumentException("Grupa ne sme sa bude null.");
				grupa = value; 
				if (Key != null)
					Key = new Key(grupa.Sifra, Key.Value(1));
				else
					Key = new Key(grupa.Sifra, VAZI_OD_PLACEHOLDER);
			}
		}

		public virtual DateTime VaziOd
		{
			get
			{
				if (Key != null)
					return (DateTime)Key.Value(1);
				else
					return VAZI_OD_PLACEHOLDER;
			}
			set
			{
				if (Key != null)
					Key = new Key(Key.Value(0), value);
				else
					Key = new Key(new SifraGrupe("0"), value);
			}
		}

		private Nullable<decimal> iznos;
        public virtual Nullable<decimal> Iznos
		{
			get { return iznos; }
			set { iznos = value; }
		}

		public MesecnaClanarina(Grupa grupa, DateTime vaziOd, decimal iznos)
		{
			// bitno je da se koristi svojstvo Grupa (a ne polje grupa) da bi se kljuc
			// azurirao
			this.Grupa = grupa;
			this.VaziOd = vaziOd;
			this.iznos = iznos;
		}

		public override void validate(Notification notification)
		{
			if (Grupa == null)
			{
				notification.RegisterMessage(
					"Grupa", "Grupa je obavezna.");
			}
		
			if (VaziOd == VAZI_OD_PLACEHOLDER)
			{
				notification.RegisterMessage(
					"VaziOd", "Datum vazenje clanarine je obavezan.");
			}

			if (Iznos == null)
			{
				notification.RegisterMessage(
					"Iznos", "Iznos clanarine je obavezan.");
			}
			else if (Iznos < 0)
			{
				notification.RegisterMessage(
					"Iznos", "Iznos clanarine ne sme da bude negativan.");
			}
		}

	}
}
