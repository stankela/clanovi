using System;
using System.Drawing;
using Soko.Domain;
using Soko.Exceptions;
using System.Collections.Generic;
using Bilten.Dao;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for PeriodicniClanoviIzvestaj.
	/// </summary>
	public class PeriodicniClanoviIzvestaj : Izvestaj
	{
		private DateTime fromDate;
		private DateTime toDate;

		private ReportText ukupnoClanovaBrojReportText;
		private ReportText ukupnoClanovaCaptionReportText;
		private PeriodicniClanoviLista lista;

		private Font ukupnoClanovaCaptionFont;
		private Font ukupnoClanovaBrojFont;

        public PeriodicniClanoviIzvestaj(DateTime from, DateTime to, 
            List<Grupa> grupe, FinansijskaCelina finCelina)
		{
			this.fromDate = from.Date;
			this.toDate = to.Date;
            this.finCelina = finCelina;

			Title = "Uplate po grupama";
			SubTitle = fromDate.ToShortDateString() + " - " + toDate.ToShortDateString();
			DocumentName = Title;
	
			ukupnoClanovaCaptionFont = new Font("Arial", 10);
			ukupnoClanovaBrojFont = new Font("Courier New", 10, FontStyle.Bold);
			
			Font itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = null;
			Font groupTitleFont = new Font("Courier New", 10, FontStyle.Bold);
			lista = new PeriodicniClanoviLista(fromDate, toDate, grupe, this, 1, 0f, 
				itemFont, itemsHeaderFont, groupTitleFont);
		}

		protected override void doSetupContent(Graphics g)
		{
			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);

			string ukupnoClanovaText = 
				resourceManager.GetString("per_clanovi_izv_uk_clan_caption");
			ukupnoClanovaText += " " + fromDate.ToShortDateString() + " - "
				+ toDate.ToShortDateString();

			string ukupnoClanovaBroj = "   " + lista.Items.Count;

			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Far;
			float ukupnoClanovaHeight = Math.Max(ukupnoClanovaCaptionFont.GetHeight(g),
				ukupnoClanovaBrojFont.GetHeight(g));
			RectangleF ukupnoClanovaRect = new RectangleF(contentBounds.X, 
				contentBounds.Y + ukupnoClanovaHeight, contentBounds.Width, ukupnoClanovaHeight);

			ukupnoClanovaBrojReportText = new ReportText(
				ukupnoClanovaBroj, ukupnoClanovaBrojFont, 
				blackBrush, ukupnoClanovaRect, format);

			ukupnoClanovaRect.Width -= 
				g.MeasureString(ukupnoClanovaBroj, ukupnoClanovaBrojFont).Width;
			ukupnoClanovaCaptionReportText = new ReportText(
				ukupnoClanovaText, ukupnoClanovaCaptionFont, 
				blackBrush, ukupnoClanovaRect, format);


			float listaStartOffset = 3 * ukupnoClanovaHeight;
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
				ukupnoClanovaBrojReportText.draw(g);
				ukupnoClanovaCaptionReportText.draw(g);
			}
			lista.drawContent(g, contentBounds, pageNum);
		}
	}

	public class PeriodicniClanoviLista : ReportLista
	{
		private DateTime fromDate;
		private DateTime toDate;
        private List<Grupa> grupe;

		private float relClan = 0.0f;
		private float relDatumUplate = 14.0f;
		
		private Font groupTitleFont;
		private float groupTitleHeight;

        public PeriodicniClanoviLista(DateTime from, DateTime to, List<Grupa> grupe,
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
            IDictionary<int, Mesto> mestaMap = DAOFactoryFactory.DAOFactory.GetMestoDAO().getMestaMap();
            items = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO()
				.getPeriodicniPrihodiUplateReportItems(fromDate, toDate, grupe, mestaMap);

            groups = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO()
				.getPeriodicniPrihodiUplateReportGrupe(fromDate, toDate, grupe);

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
			float clanWidth = xDatumUplate - xClan;
			float datumUplateWidth = contentBounds.Right - xDatumUplate;

			StringFormat datumUplateFormat = new StringFormat();
			datumUplateFormat.Alignment = StringAlignment.Far;

			columns.Clear();
			addColumn(xClan, clanWidth);
			addColumn(xDatumUplate, datumUplateWidth, datumUplateFormat, "dd.MM.yyyy");

		}
		
		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			ReportGrupa gr = groups[groupId];
			string sifra = ((SifraGrupe)gr.Data[0]).Value;
			string naziv = (string)gr.Data[1];
			string sifNaz = sifra + " - " + naziv;

			float xClan = columns[0].X;
			float xOffset = xClan - groupHeaderRect.X;
			groupHeaderRect.X += xOffset;
			groupHeaderRect.Width -= xOffset;
			g.DrawString(sifNaz, groupTitleFont, blackBrush, groupHeaderRect);
			
			float y = groupHeaderRect.Y + groupTitleFont.GetHeight(g);
			using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
			{
				g.DrawLine(pen, new PointF(xClan, y), new PointF(groupHeaderRect.X + groupHeaderRect.Width, y));
			}
		}
	}
}
