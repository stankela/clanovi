using System;
using System.Drawing;
using Soko.Exceptions;
using System.Collections.Generic;
using Soko.Domain;
using Bilten.Dao;
using NHibernate;
using NHibernate.Context;
using Soko.Data;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for DnevniPrihodiGrupeIzvestaj.
	/// </summary>
	public class DnevniPrihodiGrupeIzvestaj : Izvestaj
	{
		private ReportText ukupanPrihodIznosReportText;
		private ReportText ukupanPrihodCaptionReportText;
		private DnevniPrihodiGrupeLista lista;

		private Font ukupanPrihodCaptionFont;
		private Font ukupanPrihodIznosFont;

		private DateTime fromDate;
		private DateTime toDate;
		private List<Grupa> grupe;

        public DnevniPrihodiGrupeIzvestaj(DateTime from, DateTime to, 
            List<Grupa> grupe, FinansijskaCelina finCelina)
		{
			this.fromDate = from.Date;
			this.toDate = to.Date;
			this.grupe = grupe;
            this.finCelina = finCelina;

			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			Title = resourceManager.GetString("prihodi_izvestaj_dnevni_title");
			SubTitle = fromDate.ToShortDateString() + " - " + 
				toDate.ToShortDateString();
			DocumentName = Title;

			ukupanPrihodCaptionFont = new Font("Arial", 10);
			ukupanPrihodIznosFont = new Font("Courier New", 10, FontStyle.Bold);
			
			Font itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = new Font("Courier New", 9);
			Font groupTitleFont = new Font("Courier New", 10, FontStyle.Bold);
			lista = new DnevniPrihodiGrupeLista(fromDate, toDate, grupe, this, 1, 0f, 
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

	public class GrupniPrihodiLista : ReportLista
	{
		protected DateTime fromDate;
		protected DateTime toDate;
		protected List<Grupa> grupe;
		
		private float relGrupa = 0.5f;
		private float relNazivGrupe = 2.5f;
		private float relBrojUplata = 12.5f;
		private float relIznos = 15f;

		protected Font groupTitleFont;
		protected float groupTitleHeight;

        public GrupniPrihodiLista(DateTime from, DateTime to, List<Grupa> grupe,
			Izvestaj izvestaj, int pageNum, float y,
			Font itemFont, Font itemsHeaderFont, Font groupTitleFont) 
			: base(izvestaj, pageNum, y, itemFont, itemsHeaderFont)
		{
			this.fromDate = from;
			this.toDate = to;
			this.grupe = grupe;
			this.groupTitleFont = groupTitleFont;
		}

		public void setupContent(Graphics g, RectangleF contentBounds)
		{
			createColumns(g, contentBounds);

			itemHeight = itemFont.GetHeight(g) * 1.2f;
			groupTitleHeight = groupTitleFont.GetHeight(g);
			itemsHeaderHeight = itemsHeaderFont.GetHeight(g) * 1.2f;
			groupHeaderHeight = groupTitleHeight + itemsHeaderHeight;
			float afterGroupHeight = itemHeight;

			createListLayout(groupHeaderHeight, itemHeight, afterGroupHeight, 0f,
				contentBounds);
		}

		private void createColumns(Graphics g, RectangleF contentBounds)
		{
			float relWidth = Izvestaj.relWidth;
			float xGrupa = contentBounds.X + relGrupa / relWidth * contentBounds.Width;
			float xNazivGrupe = contentBounds.X + relNazivGrupe / relWidth * contentBounds.Width;
			float xBrojUplata = contentBounds.X + relBrojUplata / relWidth * contentBounds.Width;
			float xIznos = contentBounds.X + relIznos / relWidth * contentBounds.Width;
			float grupaWidth = xNazivGrupe - xGrupa;
			float nazivGrupeWidth = xBrojUplata - xNazivGrupe;
			float brojUplataWidth = xIznos - xBrojUplata;
			float iznosWidth = contentBounds.Right - xIznos;

	
			StringFormat brojUplataFormat = new StringFormat();
			brojUplataFormat.Alignment = StringAlignment.Center;

			StringFormat iznosFormat = new StringFormat();
			iznosFormat.Alignment = StringAlignment.Far;


			StringFormat grupaHeaderFormat = new StringFormat();
			grupaHeaderFormat.Alignment = StringAlignment.Near;
			grupaHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat nazivGrupeHeaderFormat = new StringFormat();
			nazivGrupeHeaderFormat.Alignment = StringAlignment.Center;
			nazivGrupeHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat brojUplataHeaderFormat = new StringFormat();
			brojUplataHeaderFormat.Alignment = StringAlignment.Center;
			brojUplataHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat iznosHeaderFormat = new StringFormat();
			iznosHeaderFormat.Alignment = StringAlignment.Far;
			iznosHeaderFormat.LineAlignment = StringAlignment.Near;


			String grupaTitle = "Grupa";
			String nazivGrupeTitle = "Naziv grupe";
			String brojUplataTitle = "Broj uplata";
			String iznosTitle = "Iznos";


			columns.Clear();
			addColumn(xGrupa, grupaWidth, grupaTitle, grupaHeaderFormat);
			addColumn(xNazivGrupe, nazivGrupeWidth, nazivGrupeTitle, 
				nazivGrupeHeaderFormat);
			addColumn(xBrojUplata, brojUplataWidth, brojUplataFormat, brojUplataTitle, 
				brojUplataHeaderFormat);
			addColumn(xIznos, iznosWidth, "F2", iznosFormat, 
				iznosTitle, iznosHeaderFormat);
		}

		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			drawGroupTitle(g, groupId, groupHeaderRect);
			drawItemsHeader(g, groupHeaderRect.Y + groupTitleHeight, true);
		}

		protected virtual void drawGroupTitle(Graphics g, int groupId, RectangleF groupHeaderRect)
		{

		}
	}

	public class DnevniPrihodiGrupeLista : GrupniPrihodiLista
	{

		public DnevniPrihodiGrupeLista(DateTime from, DateTime to, 
            List<Grupa> grupe, Izvestaj izvestaj, int pageNum, float y,
			Font itemFont, Font itemsHeaderFont, Font groupTitleFont) 
			: base(from, to, grupe, izvestaj, pageNum, y, itemFont, itemsHeaderFont,
			groupTitleFont)
		{
			fetchItems();
		}

		private void fetchItems()
		{
            items = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO()
                .getDnevniPrihodiGrupeReportItems(fromDate, toDate, grupe);
            createGroups();
		}

		private void createGroups()
		{
			groups = new List<ReportGrupa>();
			int groupStart = 0;
			int i = 0;
			while (i < items.Count)
			{
				object[] itemsRow = items[i];
				DateTime datum, currDatum;
				datum = currDatum = ((DateTime)itemsRow[4]).Date;
				decimal ukupanIznos = 0;
				int numGroupItems = 0;
				while (currDatum == datum)
				{
					ukupanIznos += (decimal)itemsRow[3];
					numGroupItems++;
					if (++i < items.Count)
					{
						itemsRow = items[i];
						currDatum = ((DateTime)itemsRow[4]).Date;
					}
					else
						break;
				}

				object[] data = new object[] { datum, ukupanIznos };
				ReportGrupa g = new ReportGrupa(data, groupStart, numGroupItems);
				groups.Add(g);
				groupStart += numGroupItems;
			}			
		}

		protected override void drawGroupTitle(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			ReportGrupa gr = groups[groupId];
			DateTime datumUplate = (DateTime)gr.Data[0];
			decimal ukupanIznos = (decimal)gr.Data[1];

			g.DrawString(datumUplate.ToShortDateString(), groupTitleFont, blackBrush, groupHeaderRect);
			StringFormat fmt = new StringFormat();
			fmt.Alignment = StringAlignment.Far;
			fmt.LineAlignment = StringAlignment.Near;
			g.DrawString(ukupanIznos.ToString("F2"), groupTitleFont, blackBrush, groupHeaderRect, fmt);
			
			float y = groupHeaderRect.Y + groupTitleHeight;
			using(Pen pen = new Pen(Color.Black, 1/72f * 1f))
			{
				g.DrawLine(pen, new PointF(groupHeaderRect.X, y), 
					new PointF(groupHeaderRect.Right, y));
			}
		}
	}
}
