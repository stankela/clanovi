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
	/// Summary description for CenovnikIzvestaj.
	/// </summary>
	public class CenovnikIzvestaj : Izvestaj
	{
		private CenovnikLista lista;

		public CenovnikIzvestaj()
		{
			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			Title = resourceManager.GetString("cenovnik_izvestaj_title");
			DocumentName = Title;
		
			Font itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = new Font("Courier New", 9);
			lista = new CenovnikLista(this, 1, 0f, itemFont, itemsHeaderFont);
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

	public class CenovnikLista : ReportLista
	{
		private float relGrupa = 0.5f;
		private float relIznos = 10f;
		private float relVaziOd = 12.5f;

		public CenovnikLista(Izvestaj izvestaj, int pageNum, float y,
			Font itemFont, Font itemsHeaderFont) : base(izvestaj, pageNum, y, itemFont,
			itemsHeaderFont)
		{
			fetchItems();
		}

		private void fetchItems()
		{
            items = DAOFactoryFactory.DAOFactory.GetMesecnaClanarinaDAO().getCenovnikReportItems();

            int maxLength = 0;
			foreach (object[] item in items)
			{
				string sifra = (string)item[0];
				if (sifra.Length > maxLength)
					maxLength = sifra.Length;
			}
			for (int i = 0; i < items.Count; i++)
			{
				object[] item = items[i];
				string sifra = (string)item[0];
				string naziv = (string)item[1];
				string sifraNaziv = new String(' ', maxLength - sifra.Length) + sifra
					+ "   " + naziv;
				items[i] = new object[] { sifraNaziv, item[2], item[3] };
			}
		
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
			float xGrupa = contentBounds.X + relGrupa / relWidth * contentBounds.Width;
			float xIznos = contentBounds.X + relIznos / relWidth * contentBounds.Width;
			float xVaziOd = contentBounds.X + relVaziOd / relWidth * contentBounds.Width;
			float grupaWidth = xIznos - xGrupa;
			float iznosWidth = xVaziOd - xIznos;
			float vaziOdWidth = contentBounds.Right - xVaziOd;

			StringFormat grupaFormat = new StringFormat();
			grupaFormat.Alignment = StringAlignment.Near;

			StringFormat vaziOdFormat = new StringFormat();
			vaziOdFormat.Alignment = StringAlignment.Center;

			StringFormat iznosFormat = new StringFormat();
			iznosFormat.Alignment = StringAlignment.Far;

			StringFormat grupaHeaderFormat = new StringFormat();
			grupaHeaderFormat.Alignment = StringAlignment.Center;
			grupaHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat vaziOdHeaderFormat = new StringFormat();
			vaziOdHeaderFormat.Alignment = StringAlignment.Center;
			vaziOdHeaderFormat.LineAlignment = StringAlignment.Near;

			StringFormat iznosHeaderFormat = new StringFormat();
			iznosHeaderFormat.Alignment = StringAlignment.Far;
			iznosHeaderFormat.LineAlignment = StringAlignment.Near;

			String grupaTitle = "Grupa";
			String iznosTitle = "Cena";
			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			String vaziOdTitle = resourceManager.GetString("uplate_clanova_izvestaj_vazi_od_title");

			columns.Clear();
			addColumn(xGrupa, grupaWidth, grupaFormat, grupaTitle, 
				grupaHeaderFormat);
			addColumn(xIznos, iznosWidth, "F2", iznosFormat, iznosTitle, 
				iznosHeaderFormat);
			addColumn(xVaziOd, vaziOdWidth, "dd.MM.yyyy", vaziOdFormat, vaziOdTitle, 
				vaziOdHeaderFormat);
		}
		
		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			drawItemsHeader(g, groupHeaderRect.Y, true);
		}
	}
}
