using System;
using System.Drawing;
using Soko.Exceptions;
using Soko.Dao;
using System.Collections.Generic;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for DnevniPrihodiKategorijeIzvestaj.
	/// </summary>
	public class DnevniPrihodiKategorijeIzvestaj : Izvestaj
	{
		private DateTime datum;

		DnevniPrihodiKategorijeLista lista;

		private Font titleFont;
		private Font subTitleFont;
		private Font sokDruVojFont;
		private Font adresaFont;
		private Font telFaxMailFont;

		private Font ukupanIznosFont;

		private Font summaryFontLower;
		private Font summaryFontUpper;

		private StringFormat sokDruVojFormat;
		private StringFormat adresaFormat;
		private StringFormat telFaxMailFormat;
		private StringFormat titleFormat;
		private StringFormat subTitleFormat;

		public float summaryY;
		public int summaryPageNum;

		public DnevniPrihodiKategorijeIzvestaj(DateTime dat)
		{
			datum = dat.Date;

			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			Title = resourceManager.GetString("prihodi_izvestaj_dnevni_kategorije_title");
			SubTitle = datum.ToShortDateString();
			DocumentName = Title;
	
			Font itemFont = new Font("Times New Roman", 15);
			Font itemsHeaderFont = new Font("Times New Roman", 12);
			lista = new DnevniPrihodiKategorijeLista(datum,
				this, 1, 0f, itemFont, itemsHeaderFont);

			createFormats();
			createFonts();
		}

		protected override void doSetupContent(Graphics g)
		{
			float afterHeaderHeight = 1.0f * subTitleFont.GetHeight(g);
			lista.StartY = contentBounds.Y + afterHeaderHeight;

			bool repeat;
			do
			{
				lista.setupContent(g, contentBounds);
				repeat = lista.LastPageNum > 1 
					|| lista.EndY + getSummaryHeight(g) > contentBounds.Bottom;
				if (repeat)
				{
					Font itemFont = lista.ItemFont;
					lista.ItemFont = new Font(
						itemFont.Name, itemFont.SizeInPoints - 0.5f, itemFont.Style);
					if (lista.ItemFont.SizeInPoints < 10f)
						repeat = false;
				}
			} while (repeat);

			if (getSummaryHeight(g) > contentBounds.Height)
				throw new SmallPageSizeException();
			
			summaryPageNum = lista.LastPageNum;
			summaryY = lista.EndY;
			if (summaryY + getSummaryHeight(g) > contentBounds.Bottom)
			{
				summaryPageNum++;
				summaryY = contentBounds.Y + afterHeaderHeight;
			}
			lastPageNum = summaryPageNum;
		}

		private void createFormats()
		{
			sokDruVojFormat = new StringFormat();
			sokDruVojFormat.Alignment = StringAlignment.Center;
			sokDruVojFormat.LineAlignment = StringAlignment.Near;

			adresaFormat = new StringFormat();
			adresaFormat.Alignment = StringAlignment.Center;
			adresaFormat.LineAlignment = StringAlignment.Far;

			telFaxMailFormat = new StringFormat();
			telFaxMailFormat.Alignment = StringAlignment.Center;
			telFaxMailFormat.LineAlignment = StringAlignment.Far;

			titleFormat = new StringFormat();
			titleFormat.Alignment = StringAlignment.Center;
			titleFormat.LineAlignment = StringAlignment.Near;

			subTitleFormat = new StringFormat();
			subTitleFormat.Alignment = StringAlignment.Center;
			subTitleFormat.LineAlignment = StringAlignment.Far;

		}

		private void createFonts()
		{
			titleFont = new Font("Times New Roman", 15, FontStyle.Bold);
			subTitleFont = new Font("Times New Roman", 13, FontStyle.Bold);
			sokDruVojFont = new Font("Times New Roman", 18, FontStyle.Bold);
			adresaFont = new Font("Times New Roman", 13);
			telFaxMailFont = new Font("Times New Roman", 13);
			ukupanIznosFont = new Font("Times New Roman", 15);
			summaryFontLower = new Font("Times New Roman", 13);
			summaryFontUpper = new Font("Times New Roman", 14);
		}

		public float getSummaryHeight(Graphics g)
		{
			float result;
			float svegaHeight = summaryFontLower.GetHeight(g) * 2.0f;
			result = 2f * svegaHeight;
			result += 3.0f * svegaHeight;
			result += summaryFontUpper.GetHeight(g);
			return result;
		}

		public override float getHeaderHeight(Graphics g, RectangleF marginBounds)
		{
			float relHeight = 24.5f;
			float relHeaderHeight = 4.5f;
			return relHeaderHeight / relHeight * marginBounds.Height;
		}

		public override void drawHeader(Graphics g, int pageNum)
		{
			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.SlikeResursi", this.GetType().Assembly);
			Image sokoImage = (Image)resourceManager.GetObject("slika_soko");

			float upperHeight = headerBounds.Height / 2.1f;
			float lowerHeight = headerBounds.Height - upperHeight;
			RectangleF upperHeaderBounds = new RectangleF(headerBounds.Location, 
				new SizeF(headerBounds.Width, upperHeight));
			RectangleF lowerHeaderBounds = new RectangleF(new PointF(headerBounds.X, headerBounds.Y + upperHeight), 
				new SizeF(headerBounds.Width, lowerHeight));
			using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
			{
				g.DrawRectangle(pen, upperHeaderBounds.X, upperHeaderBounds.Y,
					upperHeaderBounds.Width, upperHeaderBounds.Height);
			}

			float pictureHeight = upperHeaderBounds.Height;
			float pictureWidth = pictureHeight * 1.2f;
			RectangleF pictureBounds = new RectangleF(headerBounds.X, headerBounds.Y, pictureWidth, pictureHeight);
			pictureBounds.Inflate(- 0.05f * pictureWidth, - 0.05f * pictureHeight);
			g.DrawImage(sokoImage, pictureBounds);

			resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);

			string sokDruVoj = resourceManager.GetString("izvestaj_header_sok_dru_voj");
			float sokDruVojHeight = sokDruVojFont.GetHeight(g);
			RectangleF sokDruVojRect = new RectangleF(headerBounds.X + pictureWidth,
				headerBounds.Y, headerBounds.Width - pictureWidth, sokDruVojHeight);
			ReportText sokDruVojReportText = new ReportText(sokDruVoj, sokDruVojFont, 
				blackBrush, sokDruVojRect, sokDruVojFormat);

			string adresa = resourceManager.GetString("izvestaj_header_adresa");
			float adresaHeight = adresaFont.GetHeight(g) * 1.2f;
			RectangleF adresaRect = new RectangleF(sokDruVojRect.X,
				sokDruVojRect.Y + sokDruVojRect.Height, sokDruVojRect.Width, adresaHeight);
			ReportText adresaReportText = new ReportText(adresa, adresaFont, 
				blackBrush, adresaRect, adresaFormat);

			string telFaxMail = resourceManager.GetString("izvestaj_header_tel_fax_mail");
			float telFaxMailHeight = telFaxMailFont.GetHeight(g) * 1.2f;
			RectangleF telFaxMailRect = new RectangleF(adresaRect.X,
				adresaRect.Y + adresaRect.Height, adresaRect.Width, telFaxMailHeight);
			ReportText telFaxMailReportText = new ReportText(telFaxMail, telFaxMailFont, 
				blackBrush, telFaxMailRect, telFaxMailFormat);

			float titleHeight = titleFont.GetHeight(g);
			if (SubTitle != String.Empty)
				titleHeight += subTitleFont.GetHeight(g) * 1.5f;
			float titleY = lowerHeaderBounds.Y + 
				(lowerHeaderBounds.Height - titleHeight);
			RectangleF titleRect = new RectangleF(lowerHeaderBounds.X, titleY, 
				lowerHeaderBounds.Width, titleHeight);
			ReportText titleReportText = new ReportText(Title, titleFont, 
				blackBrush, titleRect, titleFormat);
			ReportText subTitleReportText = null;
			if (SubTitle != String.Empty)
			{
				subTitleReportText = new ReportText(SubTitle, subTitleFont, 
					blackBrush, titleRect, subTitleFormat);
			}
				
			sokDruVojReportText.draw(g);
			adresaReportText.draw(g);
			telFaxMailReportText.draw(g);
			titleReportText.draw(g);
			if (subTitleReportText != null)
				subTitleReportText.draw(g);
		}

		public override void drawContent(Graphics g, int pageNum)
		{
			lista.drawContent(g, contentBounds, pageNum);
			drawSummary(g, contentBounds, pageNum);
		}

		private void drawSummary(Graphics g, RectangleF contentBounds, int pageNum)
		{
			if (pageNum != summaryPageNum)
				return;
			float svegaHeight = summaryFontLower.GetHeight(g) * 2.0f;
			float y = summaryY + 2f * svegaHeight;

			float lineY = y - 0.5f * svegaHeight;
			using(Pen pen = new Pen(Color.Black, 1/72f * 0.5f))
			{
				g.DrawLine(pen, new PointF(contentBounds.X + contentBounds.Width / 2, lineY),
					new PointF(contentBounds.X + contentBounds.Width, lineY));
			}
			PointF svegaLocation = new PointF(contentBounds.X + contentBounds.Width / 2, y);
			string svega = "svega:";
			g.DrawString(svega, summaryFontLower, blackBrush, svegaLocation);

			//decimal ukupanIznos = ((DnevniPrihodiKategorijeGrupa)groups[0]).UkupanIznos;
			decimal ukupanIznos = MapperRegistry.uplataClanarineDAO().getUkupanPrihod(datum, datum, null);
			float ukupanIznosX = svegaLocation.X + g.MeasureString(svega + "99", summaryFontLower).Width;
			float ukupanIznosBottom = svegaLocation.Y + g.MeasureString(svega, summaryFontLower).Height;
			PointF ukupanIznosBottomLeft = new PointF(ukupanIznosX, ukupanIznosBottom);
			string ukupanIznosStr = ukupanIznos.ToString("F2");
			StringFormat fmt = new StringFormat();
			fmt.Alignment = StringAlignment.Near;
			fmt.LineAlignment = StringAlignment.Far;
			g.DrawString(ukupanIznosStr, ukupanIznosFont, blackBrush, ukupanIznosBottomLeft, fmt);

			PointF slovimaLocation = new PointF(contentBounds.X + contentBounds.Width / 15,
				y + svegaHeight);
			g.DrawString("slovima:", summaryFontLower, blackBrush, slovimaLocation);

			PointF noviSadLocation = new PointF(contentBounds.X,
				y + 3.0f * svegaHeight);
			g.DrawString("NOVI SAD", summaryFontUpper, blackBrush, noviSadLocation);

			PointF uplatioLocation = new PointF(contentBounds.X + contentBounds.Width / 1.4f,
				noviSadLocation.Y);
			g.DrawString("UPLATIO:", summaryFontUpper, blackBrush, uplatioLocation);
		}

	}


	public class DnevniPrihodiKategorijeLista : ReportLista
	{
		DateTime datum;

		public DnevniPrihodiKategorijeLista(DateTime datum, Izvestaj izvestaj, 
			int pageNum, float y, Font itemFont, Font itemsHeaderFont) 
				: base(izvestaj, pageNum, y, itemFont, itemsHeaderFont)
		{
			this.datum = datum;
		
			fetchItems();
		}

		private void fetchItems()
		{
			items = MapperRegistry.uplataClanarineDAO()
				.getDnevniPrihodiKategorijeReportItems(datum);
		
			groups = new List<ReportGrupa>();
			groups.Add(new ReportGrupa(0, items.Count));
		}

		public void setupContent(Graphics g, RectangleF contentBounds)
		{
			createColumns(g, contentBounds);

			itemHeight = itemFont.GetHeight(g) * 1.2f;
			itemsHeaderHeight = itemsHeaderFont.GetHeight(g) * 1.2f;
			groupHeaderHeight = itemsHeaderHeight;

			createListLayout(groupHeaderHeight, itemHeight, 0f, 0f, contentBounds);
		}

		private void createColumns(Graphics g, RectangleF contentBounds)
		{
			StringFormat redBrojFormat = new StringFormat();
			redBrojFormat.Alignment = StringAlignment.Far;

			String brojUplataTitle = "Broj uplata";
			StringFormat brojUplataFormat = new StringFormat();
			brojUplataFormat.Alignment = StringAlignment.Center;
			StringFormat brojUplataHeaderFormat = new StringFormat();
			brojUplataHeaderFormat.Alignment = StringAlignment.Center;

			String iznosTitle = "Iznos";
			StringFormat iznosFormat = new StringFormat();
			iznosFormat.Alignment = StringAlignment.Far;
			StringFormat iznosHeaderFormat = new StringFormat();
			iznosHeaderFormat.Alignment = StringAlignment.Far;

			// NOTE: Kolone najpre kreiram bez x i width svojstava (tj. sa placeholder
			// vrednostima 0 i 1), zato sto su mi potrebne za izracunavanje x i width
			// vrednosti. Kada izracunam x i width, tada azuriram i kolone.
			columns.Clear();
			addColumn(0, 1).ItemRectFormat = redBrojFormat;
			addColumn(0, 1);
			addColumn(0, 1, brojUplataFormat, 
				brojUplataTitle, brojUplataHeaderFormat);
			addColumn(0, 1, "F2", iznosFormat, iznosTitle, iznosHeaderFormat);
		
			float redBrojX = contentBounds.X;
			float redBrojWidth = getColumnMaxWidth(0, g, itemFont);
			float iznosWidth = getColumnMaxWidth(3, g, itemFont) + 
				g.MeasureString("9", itemFont).Width;
			float iznosX = contentBounds.Right - iznosWidth;

			float brojUplataWidth = getColumnMaxWidth(2, g, itemFont);
			float brojUplataHeaderWidth = g.MeasureString(
				columns[2].HeaderTitle, itemsHeaderFont).Width;
			brojUplataWidth = Math.Max(brojUplataWidth, brojUplataHeaderWidth);
			float brojUplataX = iznosX - brojUplataWidth;

			float kategorijaX = redBrojX + redBrojWidth + g.MeasureString("9", itemFont).Width;
			float kategorijaWidth = brojUplataX - kategorijaX;

			columns[0].X = redBrojX;
			columns[0].Width = redBrojWidth;
			columns[1].X = kategorijaX;
			columns[1].Width = kategorijaWidth;
			columns[2].X = brojUplataX;
			columns[2].Width = brojUplataWidth;
			columns[3].X = iznosX;
			columns[3].Width = iznosWidth;

		}

		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			drawItemsHeader(g, groupHeaderRect.Y, false);
		}
		
	}
}
