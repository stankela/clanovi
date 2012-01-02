using System;
using Soko.Domain;

namespace Soko
{
	/// <summary>
	/// Summary description for UplataDTO.
	/// </summary>
	public class UplataDTO
	{
		public UplataDTO(int idStavke, string clan, decimal iznos, 
			DateTime datumUplate, TimeSpan vremeUplate, string grupa, DateTime vaziOd, 
			string napomena, string korisnik)
		{
			this.idStavke = idStavke;
			this.clan = clan;
			this.iznos = iznos;
			this.datumUplate = datumUplate;
			this.vremeUplate = vremeUplate;
			this.grupa = grupa;
			this.vaziOd = vaziOd;
			this.napomena = napomena;
			this.korisnik = korisnik;
		}
	
		private int idStavke;
		public int IdStavke
		{
			get { return idStavke; }
		}

		private string clan;
		public string Clan
		{
			get { return clan; }
		}

		private decimal iznos;
		public decimal Iznos
		{
			get { return iznos; }
		}

		private DateTime datumUplate;
		public DateTime DatumUplate
		{
			get { return datumUplate; }
		}

		private TimeSpan vremeUplate;
		public TimeSpan VremeUplate
		{
			get { return vremeUplate; }
		}

		private string grupa;
		public string Grupa
		{
			get { return grupa; }
		}

		private DateTime vaziOd;
		public DateTime VaziOd
		{
			get { return vaziOd; }
		}

		private string napomena;
		public string Napomena
		{
			get { return napomena; }
		}

		private string korisnik;
		public string Korisnik
		{
			get { return korisnik; }
		}

	}
}
