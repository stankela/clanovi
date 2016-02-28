using System;
using System.Drawing;
using Soko.Domain;
using Soko.Exceptions;
using Bilten.Dao;
using System.Collections.Generic;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for UplateClanovaIzvestaj.
	/// </summary>
	public class UplateClanovaIzvestaj : Izvestaj
	{
		private UplateClanovaLista lista;

		public UplateClanovaIzvestaj(bool ceoIzvestaj, int idClana)
		{
			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			Title = resourceManager.GetString("uplate_clanova_izvestaj_title");
			DocumentName = Title;

			Font itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = new Font("Courier New", 9);
			Font groupTitleFont = new Font("Courier New", 10, FontStyle.Bold);
			lista = new UplateClanovaLista(ceoIzvestaj, idClana, this, 1, 0f, itemFont,
				itemsHeaderFont, groupTitleFont);
		}

		protected override void doSetupContent(Graphics g)
		{
			lista.StartY = contentBounds.Y;
			lista.setupContent(g, contentBounds);
			lastPageNum = lista.LastPageNum;
		}

		public override void drawContent(Graphics g, int pageNum)
		{
			lista.drawContent(g, contentBounds, pageNum);
		}
	}

	public class UplateClanovaLista : ReportLista
	{
		private bool ceoIzvestaj;
		private int idClana;
		private Font groupTitleFont;
		
		private float relDatumUplate = 0.5f;
		private float relVremeUplate = 3.2f;
		private float relGrupa = 6.5f;
		private float relVaziOd = 8f;
		private float relIznos = 10.25f;
		private float relNapomena = 12.25f;
		private float relBlagajnik = 15f;
		
		private float groupTitleHeight;
		
		public UplateClanovaLista(bool ceoIzvestaj, int idClana,
			Izvestaj izvestaj, int pageNum, float y,
			Font itemFont, Font itemsHeaderFont, Font groupTitleFont) 
			: base(izvestaj, pageNum, y, itemFont, itemsHeaderFont)
		{
			this.ceoIzvestaj = ceoIzvestaj;
			this.idClana = idClana;
			this.groupTitleFont = groupTitleFont;

			fetchItems();
		}

		private void fetchItems()
		{
            IDictionary<int, Mesto> mestaMap = DAOFactoryFactory.DAOFactory.GetMestoDAO().getMestaMap();
            if (ceoIzvestaj)
                items = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO()
					.getUplateClanovaReportItems(-1);
			else
                items = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO()
					.getUplateClanovaReportItems(idClana);
		
			if (ceoIzvestaj)
                groups = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO()
                    .getUplateClanovaReportGroups(-1, mestaMap);
			else
                groups = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO()
                    .getUplateClanovaReportGroups(idClana, mestaMap);

			int start = 0;
			for (int i = 0; i < groups.Count; i++)
			{
				ReportGrupa g = groups[i];
				g.Start = start;
				start += g.Count;
			}
		}

		public void setupContent(Graphics g, RectangleF contentBounds)
		{
			createColumns(g, contentBounds);

			itemHeight = itemFont.GetHeight(g) * 1.2f;
			groupTitleHeight = groupTitleFont.GetHeight(g) * 1.2f;
			itemsHeaderHeight = itemsHeaderFont.GetHeight(g) * 1.2f;
			groupHeaderHeight = groupTitleHeight + itemsHeaderHeight;
			float afterGroupHeight = itemHeight;

			createListLayout(groupHeaderHeight, itemHeight, afterGroupHeight, 0f,
				contentBounds);
		}

		private void createColumns(Graphics g, RectangleF contentBounds)
		{
			float relWidth = Izvestaj.relWidth;
			float xDatumUplate = contentBounds.X + relDatumUplate / relWidth * contentBounds.Width;
			float xVremeUplate = contentBounds.X + relVremeUplate / relWidth * contentBounds.Width;
			float xGrupa = contentBounds.X + relGrupa / relWidth * contentBounds.Width;
			float xVaziOd = contentBounds.X + relVaziOd / relWidth * contentBounds.Width;
			float xIznos = contentBounds.X + relIznos / relWidth * contentBounds.Width;
			float xNapomena = contentBounds.X + relNapomena / relWidth * contentBounds.Width;
			float xBlagajnik = contentBounds.X + relBlagajnik / relWidth * contentBounds.Width;

			float datumUplateWidth = xVremeUplate - xDatumUplate;
			float vremeUplateWidth = xGrupa - xVremeUplate;
			float grupaWidth = xVaziOd - xGrupa;
			float vaziOdWidth = xIznos - xVaziOd;
			float iznosWidth = xNapomena - xIznos;
			float napomenaWidth = xBlagajnik - xNapomena;
			float blagajnikWidth = contentBounds.Right - xBlagajnik;


			StringFormat datumUplateFormat = new StringFormat();
			datumUplateFormat.Alignment = StringAlignment.Far;

			StringFormat vremeUplateFormat = new StringFormat();
			vremeUplateFormat.Alignment = StringAlignment.Center;

			StringFormat grupaFormat = new StringFormat();
			grupaFormat.Alignment = StringAlignment.Center;

			StringFormat vaziOdFormat = new StringFormat();
			vaziOdFormat.Alignment = StringAlignment.Far;

			StringFormat iznosFormat = new StringFormat();
			iznosFormat.Alignment = StringAlignment.Far;

			StringFormat napomenaFormat = new StringFormat();
			napomenaFormat.Alignment = StringAlignment.Center;

			StringFormat blagajnikFormat = new StringFormat();
			blagajnikFormat.Alignment = StringAlignment.Center;


			StringFormat datumUplateHeaderFormat = new StringFormat();
			datumUplateHeaderFormat.Alignment = StringAlignment.Far;
			datumUplateHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat vremeUplateHeaderFormat = new StringFormat();
			vremeUplateHeaderFormat.Alignment = StringAlignment.Center;
			vremeUplateHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat grupaHeaderFormat = new StringFormat();
			grupaHeaderFormat.Alignment = StringAlignment.Center;
			grupaHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat vaziOdHeaderFormat = new StringFormat();
			vaziOdHeaderFormat.Alignment = StringAlignment.Far;
			vaziOdHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat iznosHeaderFormat = new StringFormat();
			iznosHeaderFormat.Alignment = StringAlignment.Far;
			iznosHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat napomenaHeaderFormat = new StringFormat();
			napomenaHeaderFormat.Alignment = StringAlignment.Center;
			napomenaHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat blagajnikHeaderFormat = new StringFormat();
			blagajnikHeaderFormat.Alignment = StringAlignment.Far;
			blagajnikHeaderFormat.LineAlignment = StringAlignment.Near;


			String datumUplateTitle = "Datum uplate";
			String vremeUplateTitle = "Vreme uplate";
			String grupaTitle = "Grupa";
			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			String vaziOdTitle = "Za mesec";
            String iznosTitle = "Iznos";
			String napomenaTitle = "Napomena";
			String blagajnikTitle = "Blagajnik";


			columns.Clear();
			addColumn(xDatumUplate, datumUplateWidth, "dd.MM.yyyy", datumUplateFormat, 
				datumUplateTitle, datumUplateHeaderFormat);
			addColumn(xVremeUplate, vremeUplateWidth, vremeUplateFormat, 
				vremeUplateTitle, vremeUplateHeaderFormat);
			addColumn(xGrupa, grupaWidth, grupaFormat, grupaTitle, grupaHeaderFormat);
			addColumn(xVaziOd, vaziOdWidth, "MMM yyyy", vaziOdFormat, 
				vaziOdTitle, vaziOdHeaderFormat);
			addColumn(xIznos, iznosWidth, "F2", iznosFormat, 
				iznosTitle, iznosHeaderFormat);
			addColumn(xNapomena, napomenaWidth, napomenaFormat, napomenaTitle, 
				napomenaHeaderFormat);
			addColumn(xBlagajnik, blagajnikWidth, blagajnikFormat, blagajnikTitle, 
				blagajnikHeaderFormat);
		}

		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			ReportGrupa gr = groups[groupId];
			string clan = (string)gr.Data[0];
			g.DrawString(clan, groupTitleFont, blackBrush, groupHeaderRect);

			drawItemsHeader(g, groupHeaderRect.Y + groupTitleHeight, true);
		}
	}
}
