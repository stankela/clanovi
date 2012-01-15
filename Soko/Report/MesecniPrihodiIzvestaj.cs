using System;
using System.Drawing;
using Soko.Exceptions;
using System.Collections.Generic;
using Bilten.Dao;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for MesecniPrihodiIzvestaj.
	/// </summary>
	public class MesecniPrihodiIzvestaj : Izvestaj
	{
		private ReportText ukupanPrihodIznosReportText;
		private ReportText ukupanPrihodCaptionReportText;
		private MesecniPrihodiLista lista;
		
		private Font ukupanPrihodCaptionFont;
		private Font ukupanPrihodIznosFont;

		private DateTime fromDate;
		private DateTime toDate;

		public MesecniPrihodiIzvestaj(DateTime from, DateTime to)
		{
			fromDate = from.Date;
			toDate = to.Date;
			convertDatesToMonthBoundaries();

			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			Title = resourceManager.GetString("prihodi_izvestaj_mesecni_title");
			SubTitle = fromDate.ToShortDateString() + " - " + 
				toDate.ToShortDateString();
			DocumentName = Title;
		
			ukupanPrihodCaptionFont = new Font("Arial", 10);
			ukupanPrihodIznosFont = new Font("Courier New", 10, FontStyle.Bold);
			
			Font itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = new Font("Courier New", 9);
			Font groupTitleFont = new Font("Courier New", 10, FontStyle.Bold);
			lista = new MesecniPrihodiLista(fromDate, toDate, this, 1, 0f, 
				itemFont, itemsHeaderFont, groupTitleFont);
		}

		private void convertDatesToMonthBoundaries()
		{
			fromDate = new DateTime(fromDate.Year, fromDate.Month, 1).Date;
			toDate = toDate.AddMonths(1);
			toDate = new DateTime(toDate.Year, toDate.Month, 1);
			toDate = toDate.AddDays(-1);
		}

		protected override void doSetupContent(Graphics g)
		{
			string ukupanPrihodText = "Ukupan prihod za period ";
			ukupanPrihodText += fromDate.ToShortDateString() + " - "
				+ toDate.ToShortDateString();

            decimal ukupanPrihod = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO().getUkupanPrihod(fromDate, toDate, null);
			string ukupanPrihodIznos = "   " + ukupanPrihod.ToString("F2");

			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Far;
			float ukupanPrihodHeight = Math.Max(ukupanPrihodCaptionFont.GetHeight(g),
				ukupanPrihodIznosFont.GetHeight(g));
			RectangleF ukupanPrihodRect = new RectangleF(contentBounds.X, 
				contentBounds.Y + ukupanPrihodHeight, contentBounds.Width, ukupanPrihodHeight);

			ukupanPrihodIznosReportText = new ReportText(
				ukupanPrihodIznos, ukupanPrihodIznosFont, 
				blackBrush, ukupanPrihodRect, format);

			ukupanPrihodRect.Width -= 
				g.MeasureString(ukupanPrihodIznos, ukupanPrihodIznosFont).Width;
			ukupanPrihodCaptionReportText = new ReportText(
				ukupanPrihodText, ukupanPrihodCaptionFont, 
				blackBrush, ukupanPrihodRect, format);


			float listaStartOffset = 3 * ukupanPrihodHeight;
			if (listaStartOffset > contentBounds.Height)
				throw new SmallPageSizeException();

			lista.StartY = contentBounds.Y + listaStartOffset;
			lista.setupContent(g, contentBounds);
			lastPageNum = lista.LastPageNum;
		}

		public override void drawContent(Graphics g, int pageNum)
		{
			if (pageNum == 1)
			{
				ukupanPrihodIznosReportText.draw(g);
				ukupanPrihodCaptionReportText.draw(g);
			}
			lista.drawContent(g, contentBounds, pageNum);
		}
	}


	public class MesecniPrihodiLista : GrupniPrihodiLista
	{
		public MesecniPrihodiLista(DateTime from, DateTime to,
			Izvestaj izvestaj, int pageNum, float y,
			Font itemFont, Font itemsHeaderFont, Font groupTitleFont) 
			: base(from, to, null, izvestaj, pageNum, y, itemFont, itemsHeaderFont,
			groupTitleFont)
		{
			fetchItems();
		}

		private void fetchItems()
		{
            items = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO()
				.getMesecniPrihodiReportItems(fromDate, toDate);
			createGroups();
		}

		private void createGroups()
		{
			groups = new List<ReportGrupa>();
			int groupId = 0;
			int groupStart = 0;
			int i = 0;
			while (i < items.Count)
			{
				object[] itemsRow = items[i];
				short year, currYear;
				short month, currMonth;
				year = currYear = (short)itemsRow[4];
				month = currMonth = (short)itemsRow[5];
				decimal ukupanIznos = 0;
				int numGroupItems = 0;
				while (currMonth == month && currYear == year)
				{
					ukupanIznos += (decimal)itemsRow[3];
					numGroupItems++;
					if (++i < items.Count)
					{
						itemsRow = items[i];
						currYear = (short)itemsRow[4];
						currMonth = (short)itemsRow[5];
					}
					else
						break;
				}
				object[] data = new object[] { year, month, ukupanIznos };
				ReportGrupa g = new ReportGrupa(data, groupStart, numGroupItems);
				groups.Add(g);
				groupId++;
				groupStart += numGroupItems;
			}
		}

		protected override void drawGroupTitle(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			ReportGrupa gr = groups[groupId];
			int godina = (short)gr.Data[0];
			int mesec = (short)gr.Data[1];
			decimal ukupanIznos = (decimal)gr.Data[2];

			string godMes = new DateTime(godina, mesec, 1).ToString("MMM yyyy");
			g.DrawString(godMes, groupTitleFont, blackBrush, groupHeaderRect);
			StringFormat fmt = new StringFormat();
			fmt.Alignment = StringAlignment.Far;
			fmt.LineAlignment = StringAlignment.Near;
			g.DrawString(ukupanIznos.ToString("F2"), groupTitleFont, blackBrush, groupHeaderRect, fmt);
			
			float y = groupHeaderRect.Y + groupTitleFont.GetHeight(g);
			using(Pen pen = new Pen(Color.Black, 1/72f * 1f))
			{
				g.DrawLine(pen, new PointF(groupHeaderRect.X, y), 
					new PointF(groupHeaderRect.Right, y));
			}
		}
	}
}
