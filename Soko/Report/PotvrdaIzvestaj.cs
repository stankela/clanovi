using System;
using System.Drawing;
using Soko.Dao;
using Soko.Domain;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for PotvrdaIzvestaj.
	/// </summary>
	public class PotvrdaIzvestaj : Izvestaj
	{
		int idStavke;
		int broj;
		string ime;
		string prezime;
		string mesto;
		DateTime datumUplate;
		decimal iznos;
		string sifraGrupe;
		string nazivGrupe;
		DateTime datumClanarine;

		Font arial8Font;
		Font arial9Font;
		Font arial9BoldFont;
		Font arial10Font;
		Font arial14BoldFont;

		public PotvrdaIzvestaj(int id)
		{
			idStavke = id;
			DocumentName = "Potvrda";
			createFonts();

			A4 = false;
            PrinterName = Options.Instance.PrinterNamePotvrda;
		}

		private void createFonts()
		{
			arial8Font = new Font("Arial", 8);
			arial9Font = new Font("Arial", 9);
			arial9BoldFont = new Font("Arial", 9, FontStyle.Bold);
			arial10Font = new Font("Arial", 10);
			arial14BoldFont = new Font("Arial", 14, FontStyle.Bold);
		}

		private void getData()
		{
			UplataClanarine uplata = MapperRegistry.uplataClanarineDAO().getById(idStavke);
			broj = uplata.Clan.Broj.Value;
			ime = String.Empty;
			if (uplata.Clan.Ime != null)
				ime = uplata.Clan.Ime;
			prezime = String.Empty;
			if (uplata.Clan.Prezime != null)
				prezime = uplata.Clan.Prezime;
			mesto = uplata.Clan.NazivMesta;
			datumUplate = uplata.DatumUplate.Value;
			iznos = uplata.Iznos.Value;
			sifraGrupe = uplata.Grupa.Sifra.Value;
			nazivGrupe = uplata.Grupa.Naziv;
			datumClanarine = uplata.VaziOd.Value;
		}

		protected override void doSetupContent(Graphics g)
		{
			getData();
			lastPageNum = 1;
		}

		public override void drawHeader(Graphics g, int pageNum)
		{
			RectangleF contentBounds = new RectangleF(conv(5), conv(1),
				conv(85), conv(85));
			using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
			{
				//			g.DrawRectangle(pen, contentBounds.X, contentBounds.Y, contentBounds.Width,
				//				contentBounds.Height);


				System.Resources.ResourceManager resourceManager = new 
					System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);

				// sokDruVoj
				string sokDruVoj = resourceManager.GetString("potvrda_izvestaj_sok_dru_voj");
				string ns = "Novi Sad";
				float x = contentBounds.X;
				float y = contentBounds.Y + conv(0.5f);
				StringFormat fmt = new StringFormat();
				fmt.Alignment = StringAlignment.Near;
				fmt.LineAlignment = StringAlignment.Near;
				g.DrawString(sokDruVoj, arial10Font, blackBrush, 
					new PointF(x, y), fmt);
				float dy = arial10Font.GetHeight(g) * 1.2f;
				y += dy;
				g.DrawString(ns, arial10Font, blackBrush, 
					new PointF(x, y), fmt);

				// vreme
				x = contentBounds.X + contentBounds.Width;
				y = contentBounds.Y + conv(0.5f);
				StringFormat fmt2 = new StringFormat();
				fmt2.Alignment = StringAlignment.Far;
				fmt2.LineAlignment = StringAlignment.Near;
				g.DrawString(TimeOfPrint.ToShortDateString() + " " + 
					TimeOfPrint.ToShortTimeString(), arial8Font, blackBrush, 
					new PointF(x, y), fmt2);

				
				// priznanica
				string priznanica = "PRIZNANICA";
				float xPrizCenter = contentBounds.Width / 2;
				float yPrizCenter = conv(15);
				StringFormat prizFormat = new StringFormat();
				prizFormat.Alignment = StringAlignment.Center;
				prizFormat.LineAlignment = StringAlignment.Center;
				g.DrawString(priznanica, arial14BoldFont, blackBrush, 
					new PointF(contentBounds.X + xPrizCenter, 
					contentBounds.Y + yPrizCenter), prizFormat);

				// prva grupa
				string clanskiBroj = resourceManager.GetString("potvrda_izvestaj_clanski_broj");
				string imePrez = "Ime i prezime";
				string mes = "Mesto";
				SizeF imePrezSize = g.MeasureString(imePrez + " ", arial9Font);
				x = contentBounds.X + imePrezSize.Width;
				float x2 = x + conv(0.5f);
				y = contentBounds.Y + conv(25);
				dy = imePrezSize.Height * 1.2f;
				StringFormat f1 = new StringFormat();
				f1.Alignment = StringAlignment.Far;
				f1.LineAlignment = StringAlignment.Near;
				StringFormat f2 = new StringFormat();
				f2.Alignment = StringAlignment.Near;
				f2.LineAlignment = StringAlignment.Near;
				g.DrawString(clanskiBroj, arial9Font, blackBrush, 
					new PointF(x, y), f1);
				g.DrawString(broj.ToString(), arial9BoldFont, blackBrush, 
					new PointF(x2, y), f2);
				y += dy;
				g.DrawString(imePrez, arial9Font, blackBrush, 
					new PointF(x, y), f1);
				g.DrawString(FormatImePrez(ime, prezime), arial9BoldFont, blackBrush, 
					new PointF(x2, y), f2);
				y += dy;
				g.DrawString(mes, arial9Font, blackBrush, 
					new PointF(x, y), f1);
				g.DrawString(mesto, arial9BoldFont, blackBrush, 
					new PointF(x2, y), f2);
				
				// druga grupa
				string uplatioJe = "Uplatio-la je dana";
				string iznosOd = "iznos od";
				string clanZaGrupu = resourceManager.GetString("potvrda_izvestaj_clan_za_grupu");
				string kojaVazi = resourceManager.GetString("potvrda_izvestaj_koja_vazi");
				SizeF clanZaGrupuSize = g.MeasureString(clanZaGrupu + " ", arial9Font);
				x = contentBounds.X + clanZaGrupuSize.Width;
				x2 = x + conv(0.5f);
				y = contentBounds.Y + conv(45);
				dy = clanZaGrupuSize.Height * 1.2f;
				g.DrawString(uplatioJe, arial9Font, blackBrush, 
					new PointF(x, y), f1);
				g.DrawString(datumUplate.ToShortDateString(), arial9BoldFont, blackBrush, 
					new PointF(x2, y), f2);
				y += dy;
				g.DrawString(iznosOd, arial9Font, blackBrush, 
					new PointF(x, y), f1);
				g.DrawString(FormatIznos(iznos), arial9BoldFont, blackBrush, 
					new PointF(x2, y), f2);
				y += dy;
				g.DrawString(clanZaGrupu, arial9Font, blackBrush, 
					new PointF(x, y), f1);
				g.DrawString(FormatGrupa(sifraGrupe, nazivGrupe), arial9BoldFont, blackBrush, 
					new PointF(x2, y), f2);
				y += dy;
				g.DrawString(kojaVazi, arial9Font, blackBrush, 
					new PointF(x, y), f1);
				g.DrawString(datumClanarine.ToShortDateString(), 
					arial9BoldFont, blackBrush, 
					new PointF(x2, y), f2);

				// mp
				x = contentBounds.X + conv(18f);
				y = contentBounds.Y + contentBounds.Height - conv(5f);
				g.DrawString("m.p", arial9Font, blackBrush, 
					new PointF(x, y));
				x += conv(30f);
				g.DrawString("potvrdu izdao", arial9Font, blackBrush, 
					new PointF(x, y));
				g.DrawLine(pen, new PointF(x - conv(10), y), new PointF(x + conv(32), y));

			}
		}

		private float conv(float x)
		{
			return x/25.4f;
		}
		
		private string FormatImePrez(string ime, string prez)	
		{
			string result = ime;
			if (result != "")
				result += " ";
			return result + prezime;
		}

		private string FormatIznos(decimal iznos)
		{
			return iznos.ToString("F2") + " Din.";
		}

		private string FormatGrupa(string sifraGrupe, string nazivGrupe)
		{
			return sifraGrupe + " - " + nazivGrupe;
		}
	}
}
