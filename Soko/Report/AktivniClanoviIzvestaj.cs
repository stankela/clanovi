using System;
using System.Drawing;
using Soko.Domain;
using Soko.Exceptions;
using System.Collections.Generic;
using NHibernate;
using Soko.Data;
using NHibernate.Context;
using Bilten.Dao;

namespace Soko.Report
{
	/// <summary>
    /// Summary description for AktivniClanoviIzvestaj.
	/// </summary>
	public class AktivniClanoviIzvestaj : Izvestaj
	{
		private AktivniClanoviLista lista;

        private DateTime fromDate;
        private DateTime toDate;

        public AktivniClanoviIzvestaj(DateTime from, DateTime to, List<Grupa> grupe)
		{
            fromDate = from.Date;
            toDate = to.Date;
            
            System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);

            Title = resourceManager.GetString("akt_clan_izvestaj_title");
            SubTitle = fromDate.ToShortDateString() + " - " + toDate.ToShortDateString();
            DocumentName = Title;
		
			Font itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = new Font("Courier New", 9);
            lista = new AktivniClanoviLista(fromDate, toDate, grupe, this, 1, 0f, itemFont, itemsHeaderFont);
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

	public class AktivniClanoviLista : ReportLista
	{
		private float relRedBroj = 0.0f;
		private float relClan = 1.0f;
        private float relGrupa = 9.0f;

        private DateTime fromDate;
        private DateTime toDate;

        public AktivniClanoviLista(DateTime from, DateTime to, List<Grupa> grupe, Izvestaj izvestaj, int pageNum, float y,
			Font itemFont, Font itemsHeaderFont) : base(izvestaj, pageNum, y, itemFont,
			itemsHeaderFont)
		{
            this.fromDate = from;
            this.toDate = to;

            fetchItems(grupe);
		}

		private void fetchItems(List<Grupa> grupe)
		{
            IDictionary<int, Mesto> mestaMap = DAOFactoryFactory.DAOFactory.GetMestoDAO().getMestaMap();
            items = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO()
                .getAktivniClanoviReportItems(fromDate, toDate, grupe, mestaMap);
		
			groups = new List<ReportGrupa>();
			groups.Add(new ReportGrupa(0, items.Count));
		}

		public void setupContent(Graphics g, RectangleF contentBounds)
		{
			createColumns(g, contentBounds);

			itemHeight = itemFont.GetHeight(g) * 1.2f;
			itemsHeaderHeight = itemsHeaderFont.GetHeight(g) * 1.2f;
			groupHeaderHeight = itemsHeaderHeight;
			float afterGroupHeight = itemHeight;

			createListLayout(groupHeaderHeight, itemHeight, afterGroupHeight, 0f,
				contentBounds);
		}

		private void createColumns(Graphics g, RectangleF contentBounds)
		{
			float relWidth = Izvestaj.relWidth;
			float xRedBroj = contentBounds.X + relRedBroj / relWidth * contentBounds.Width;
			float xClan = contentBounds.X + relClan / relWidth * contentBounds.Width;
            float xGrupa = contentBounds.X + relGrupa / relWidth * contentBounds.Width;
            float redBrojWidth = xClan - xRedBroj;
			float clanWidth = xGrupa - xClan;
            float grupaWidth = contentBounds.Right - xGrupa;

			StringFormat redBrojFormat = new StringFormat();
			redBrojFormat.Alignment = StringAlignment.Far;

			StringFormat clanFormat = new StringFormat();
			clanFormat.Alignment = StringAlignment.Near;

            StringFormat grupaFormat = new StringFormat();
            grupaFormat.Alignment = StringAlignment.Near;

            StringFormat redBrojHeaderFormat = new StringFormat();
			redBrojHeaderFormat.Alignment = StringAlignment.Far;
			redBrojHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat clanHeaderFormat = new StringFormat();
			clanHeaderFormat.Alignment = StringAlignment.Center;
			clanHeaderFormat.LineAlignment = StringAlignment.Near;

            StringFormat grupaHeaderFormat = new StringFormat();
            grupaHeaderFormat.Alignment = StringAlignment.Center;
            grupaHeaderFormat.LineAlignment = StringAlignment.Near;

            String redBrojTitle = "";
			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
            String clanTitle = resourceManager.GetString("akt_clanovi_izvestaj_clan_title");
            String grupaTitle = "Grupa";

			columns.Clear();
			addColumn(xRedBroj, redBrojWidth, redBrojFormat, redBrojTitle, redBrojHeaderFormat);
			addColumn(xClan, clanWidth, clanFormat, clanTitle, clanHeaderFormat);
            addColumn(xGrupa, grupaWidth, grupaFormat, grupaTitle, grupaHeaderFormat);
        }
		
		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			drawItemsHeader(g, groupHeaderRect.Y, true);
		}
	}
}
