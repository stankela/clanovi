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
			set { grupa = value; }
		}

        private DateTime vaziOd = VAZI_OD_PLACEHOLDER;
		public virtual DateTime VaziOd
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

		public MesecnaClanarina(Grupa grupa, DateTime vaziOd, decimal iznos)
		{
			this.grupa = grupa;
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
