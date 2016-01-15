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
        private EvidencijaTreningaLista lista;
        private Font itemFont;

        public EvidencijaTreningaIzvestaj(DateTime from, DateTime to, List<Grupa> grupe)
		{
			Title = "Evidencija prisustva na treningu";
            string subtitle;
            if (from.Date == to.Date)
            {
                subtitle = from.ToLongDateString();
                subtitle += "   " + from.ToShortTimeString() + " - " + to.ToShortTimeString();
            }
            else
            {
                subtitle = from.ToShortDateString() + " " + from.ToShortTimeString();
                subtitle += " - " + to.ToShortDateString() + " " + to.ToShortTimeString();
            }
            SubTitle = subtitle;
			DocumentName = Title;
	
			itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = null;
			Font groupTitleFont = new Font("Courier New", 10, FontStyle.Bold);
            lista = new EvidencijaTreningaLista(from, to, grupe, this, 1, 0f, 
				itemFont, itemsHeaderFont, groupTitleFont);
		}

		protected override void doSetupContent(Graphics g)
		{
            float itemHeight = g.MeasureString("0", itemFont).Height;
            float listaStartOffset = 1 * itemHeight;
			if (listaStartOffset > contentBounds.Height)
				throw new SmallPageSizeException();

			lista.StartY = contentBounds.Y + listaStartOffset;
			lista.setupContent(g, contentBounds);
			lastPageNum = lista.LastPageNum;
		}

		public override void drawContent(Graphics g, int pageNum)
		{
			lista.drawContent(g, contentBounds, pageNum);
		}
	}

	public class EvidencijaTreningaLista : ReportLista
	{
		private DateTime from;
		private DateTime to;
        private List<Grupa> grupe;

		private float relClan = 0.0f;
		private float relVremeDolaska = 9.0f;
		private float relDatumUplate = 14f;
		
		private Font groupTitleFont;
		private float groupTitleHeight;

        public EvidencijaTreningaLista(DateTime from, DateTime to, List<Grupa> grupe,
			Izvestaj izvestaj, int pageNum, float y,
			Font itemFont, Font itemsHeaderFont, Font groupTitleFont) 
			: base(izvestaj, pageNum, y, itemFont, itemsHeaderFont)
		{
			this.from = from;
			this.to = to;
			this.grupe = grupe;
			this.groupTitleFont = groupTitleFont;

			fetchItems();
		}

		private void fetchItems()
		{
            items = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO()
				.getEvidencijaTreningaReportItems(from, to, grupe);

            groups = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO()
                .getEvidencijaTreningaReportGrupe(from, to, grupe);

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
			float xVremeDolaska = contentBounds.X + relVremeDolaska / relWidth * contentBounds.Width;
			float xDatumUplate = contentBounds.X + relDatumUplate / relWidth * contentBounds.Width;
			float clanWidth = xVremeDolaska - xClan;
			float vremeDolaskaWidth = xDatumUplate - xVremeDolaska;
			float datumUplateWidth = contentBounds.Right - xDatumUplate;

			StringFormat vremeDolaskaFormat = new StringFormat();
			vremeDolaskaFormat.Alignment = StringAlignment.Far;

			StringFormat datumUplateFormat = new StringFormat();
			datumUplateFormat.Alignment = StringAlignment.Far;
			
			columns.Clear();
			addColumn(xClan, clanWidth);
			addColumn(xVremeDolaska, vremeDolaskaWidth, vremeDolaskaFormat, "dd.MM.yyyy HH:mm:ss");
            addColumn(xDatumUplate, datumUplateWidth, datumUplateFormat, "dd.MM.yyyy");

		}
		
		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			ReportGrupa gr = groups[groupId];
            string sifNaz = (string)gr.Data[0];

			float xClan = columns[0].X;
			float xOffset = xClan - groupHeaderRect.X;
			groupHeaderRect.X += xOffset;
			groupHeaderRect.Width -= xOffset;
			g.DrawString(sifNaz, groupTitleFont, blackBrush, groupHeaderRect);

            RectangleF dolazakHeaderRect = new RectangleF(columns[1].X, groupHeaderRect.Y, columns[1].Width,
                groupHeaderRect.Height);
            StringFormat dolazakFormat = columns[1].ItemRectFormat;
            g.DrawString("Vreme dolaska", itemFont, blackBrush, dolazakHeaderRect, dolazakFormat);

            RectangleF uplataHeaderRect = new RectangleF(columns[2].X, groupHeaderRect.Y, columns[2].Width,
                groupHeaderRect.Height);
            StringFormat uplataFormat = columns[2].ItemRectFormat;
            g.DrawString("Uplata", itemFont, blackBrush, uplataHeaderRect, uplataFormat);
			
			float y = groupHeaderRect.Y + groupTitleFont.GetHeight(g);
			using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
			{
				g.DrawLine(pen, new PointF(xClan, y), new PointF(groupHeaderRect.X + groupHeaderRect.Width, y));
			}
		}
	}
}
