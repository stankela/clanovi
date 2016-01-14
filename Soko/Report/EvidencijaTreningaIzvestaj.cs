using System;
using System.Drawing;
using Soko.Domain;
using Soko.Exceptions;
using System.Collections.Generic;
using Bilten.Dao;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for EvidencijaTreningaIzvestaj.
	/// </summary>
	public class EvidencijaTreningaIzvestaj : Izvestaj
	{
		private DateTime fromDate;
		private DateTime toDate;
        private List<Grupa> grupe;

		private ReportText ukupanPrihodIznosReportText;
		private ReportText ukupanPrihodCaptionReportText;
        private EvidencijaTreningaLista lista;

		private Font ukupanPrihodCaptionFont;
		private Font ukupanPrihodIznosFont;

        public EvidencijaTreningaIzvestaj(DateTime from, DateTime to, List<Grupa> grupe)
		{
			this.fromDate = from;
			this.toDate = to;
			this.grupe = grupe;

			Title = "Evidencija prisustva na treningu";
            string subtitle;
            if (fromDate.Date == toDate.Date)
            {
                subtitle = fromDate.ToLongDateString();
                subtitle += "   " + fromDate.ToShortTimeString() + " - " + toDate.ToShortTimeString();
            }
            else
            {
                subtitle = fromDate.ToShortDateString() + " " + fromDate.ToShortTimeString();
                subtitle += " - " + toDate.ToShortDateString() + " " + toDate.ToShortTimeString();
            }
            SubTitle = subtitle;
			DocumentName = Title;
	
			ukupanPrihodCaptionFont = new Font("Arial", 10);
			ukupanPrihodIznosFont = new Font("Courier New", 10, FontStyle.Bold);
			
			Font itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = null;
			Font groupTitleFont = new Font("Courier New", 10, FontStyle.Bold);
            lista = new EvidencijaTreningaLista(fromDate, toDate, grupe, this, 1, 0f, 
				itemFont, itemsHeaderFont, groupTitleFont);
		}

		protected override void doSetupContent(Graphics g)
		{
			string ukupanPrihodText = "Ukupan prihod za period ";
			ukupanPrihodText += fromDate.ToShortDateString() + " - "
				+ toDate.ToShortDateString();

            decimal ukupanPrihod = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO().getUkupanPrihod(fromDate, toDate, grupe);
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

	public class EvidencijaTreningaLista : ReportLista
	{
		private DateTime fromDate;
		private DateTime toDate;
        private List<Grupa> grupe;

		private float relClan = 0.0f;
		private float relDatumUplate = 9.0f;
		private float relIznos = 14f;
		
		private Font groupTitleFont;
		private float groupTitleHeight;

        public EvidencijaTreningaLista(DateTime from, DateTime to, List<Grupa> grupe,
			Izvestaj izvestaj, int pageNum, float y,
			Font itemFont, Font itemsHeaderFont, Font groupTitleFont) 
			: base(izvestaj, pageNum, y, itemFont, itemsHeaderFont)
		{
			this.fromDate = from;
			this.toDate = to;
			this.grupe = grupe;
			this.groupTitleFont = groupTitleFont;

			fetchItems();
		}

		private void fetchItems()
		{
            items = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO()
				.getEvidencijaTreningaReportItems(fromDate, toDate, grupe);

            groups = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO()
                .getEvidencijaTreningaReportGrupe(fromDate, toDate, grupe);

			int start = 0;
			for (int i = 0; i < groups.Count; i++)
			{
				ReportGrupa g = groups[i];
				g.Start = start;
				start += g.Count;
			}
	
			addRedBrojClana();
		}

		private void addRedBrojClana()
		{
			for (int i = 0; i < groups.Count; i++)
			{
				ReportGrupa g = groups[i];
				for (int j = 0; j < g.Count; j++)
				{
					object[] item = items[g.Start + j];
					item[0] = String.Format("{0} {1}", j + 1, item[0]);
				}
			}
		}

		public void setupContent(Graphics g, RectangleF contentBounds)
		{
			createColumns(g, contentBounds);

			itemHeight = itemFont.GetHeight(g) * 1.2f;
			groupTitleHeight = groupTitleFont.GetHeight(g) * 1.2f;
			groupHeaderHeight = groupTitleHeight;
			float afterGroupHeight = itemHeight * 0.75f;

			createListLayout(groupHeaderHeight, itemHeight, afterGroupHeight, 0f,
				contentBounds);
		}

		private void createColumns(Graphics g, RectangleF contentBounds)
		{
			float relWidth = Izvestaj.relWidth;
			float xClan = contentBounds.X + relClan / relWidth * contentBounds.Width;
			float xDatumUplate = contentBounds.X + relDatumUplate / relWidth * contentBounds.Width;
			float xIznos = contentBounds.X + relIznos / relWidth * contentBounds.Width;
			float clanWidth = xDatumUplate - xClan;
			float datumUplateWidth = xIznos - xDatumUplate;
			float iznosWidth = contentBounds.Right - xIznos;

			StringFormat datumUplateFormat = new StringFormat();
			datumUplateFormat.Alignment = StringAlignment.Center;

			StringFormat iznosFormat = new StringFormat();
			iznosFormat.Alignment = StringAlignment.Far;
			
			columns.Clear();
			addColumn(xClan, clanWidth);
			addColumn(xDatumUplate, datumUplateWidth, datumUplateFormat, "dd.MM.yyyy hh:mm:ss");
            addColumn(xIznos, iznosWidth, iznosFormat, "dd.MM.yyyy");

		}
		
		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			ReportGrupa gr = groups[groupId];
            string sifNaz = (string)gr.Data[0];
			decimal ukupanIznos = (decimal)gr.Data[1];

			float xClan = columns[0].X;
			float xOffset = xClan - groupHeaderRect.X;
			groupHeaderRect.X += xOffset;
			groupHeaderRect.Width -= xOffset;
			g.DrawString(sifNaz, groupTitleFont, blackBrush, groupHeaderRect);
			StringFormat iznosFormat = columns[2].ItemRectFormat;
			g.DrawString(ukupanIznos.ToString("F2"), groupTitleFont, blackBrush, groupHeaderRect, iznosFormat);
			
			float y = groupHeaderRect.Y + groupTitleFont.GetHeight(g);
			using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
			{
				g.DrawLine(pen, new PointF(xClan, y), new PointF(groupHeaderRect.X + groupHeaderRect.Width, y));
			}
		}
	}
}
