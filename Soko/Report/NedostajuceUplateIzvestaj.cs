using System;
using System.Drawing;
using Soko.Domain;
using Soko.Exceptions;
using System.Collections.Generic;
using Bilten.Dao;

namespace Soko.Report
{
	/// <summary>
    /// Summary description for DolazakNaTreningMesecniIzvestaj.
	/// </summary>
	public class DolazakNaTreningMesecniIzvestaj : Izvestaj
	{
        private DolazakNaTreningMesecniLista lista;
        private Font itemFont;
        private Font clanFont;

        public DolazakNaTreningMesecniIzvestaj(DateTime from, DateTime to, bool samoNedostajuceUplate)
		{
            if (samoNedostajuceUplate)
                Title = "Nedostaju\u0107e uplate";
            else
                Title = "Dolazak na trening i uplate - mese\u010Dni";
            string subtitle;
            string format = "MMMM yyyy";
            if (from.Year == to.Year && from .Month == to.Month)
            {
                subtitle = from.ToString(format);
            }
            else
            {
                subtitle = from.ToString(format) + " - " + to.ToString(format);
            }
            SubTitle = subtitle;
			DocumentName = Title;

            clanFont = new Font("Arial", 10, FontStyle.Bold);
            itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = null;
			Font groupTitleFont = new Font("Courier New", 10, FontStyle.Bold);
            lista = new DolazakNaTreningMesecniLista(from, to, this, 1, 0f, 
				itemFont, itemsHeaderFont, groupTitleFont, samoNedostajuceUplate);
		}

		protected override void doSetupContent(Graphics g)
		{
            float itemHeight = g.MeasureString("0", itemFont).Height;
            float listaStartOffset;
            listaStartOffset = 1f * itemHeight;

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

	public class DolazakNaTreningMesecniLista : ReportLista
	{
		private float relClan = 0.0f;
		private float relGrupa = 9.0f;
        private float relImaUplatu = 16.5f;
		
		private Font groupTitleFont;
		private float groupTitleHeight;

        private bool samoNedostajuceUplate;

        public DolazakNaTreningMesecniLista(DateTime from, DateTime to,
			Izvestaj izvestaj, int pageNum, float y,
			Font itemFont, Font itemsHeaderFont, Font groupTitleFont, bool samoNedostajuceUplate) 
			: base(izvestaj, pageNum, y, itemFont, itemsHeaderFont)
		{
			this.groupTitleFont = groupTitleFont;
            this.samoNedostajuceUplate = samoNedostajuceUplate;
			fetchItems(from, to, samoNedostajuceUplate);
		}

        private void fetchItems(DateTime from, DateTime to, bool samoNedostajuceUplate)
		{
            items = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO()
                .getDolazakNaTreningMesecniReportItems(from, to, samoNedostajuceUplate);
            createGroups();
	
			addRedBrojClana();
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
                int year, currYear;
                int month, currMonth;
                year = currYear = (int)itemsRow[3];
                month = currMonth = (int)itemsRow[4];
                int numGroupItems = 0;
                while (currMonth == month && currYear == year)
                {
                    numGroupItems++;
                    if (++i < items.Count)
                    {
                        itemsRow = items[i];
                        currYear = (int)itemsRow[3];
                        currMonth = (int)itemsRow[4];
                    }
                    else
                        break;
                }
                object[] data = new object[] { year, month };
                ReportGrupa g = new ReportGrupa(data, groupStart, numGroupItems);
                groups.Add(g);
                groupId++;
                groupStart += numGroupItems;
            }
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
			float xGrupa = contentBounds.X + relGrupa / relWidth * contentBounds.Width;
            float xImaUplatu = contentBounds.X + relImaUplatu / relWidth * contentBounds.Width;

            float clanWidth = xGrupa - xClan;
            float grupaWidth;
            float imaUplatuWidth = 0.0f;
            if (samoNedostajuceUplate)
                grupaWidth = contentBounds.Right - xGrupa;
            else
            {
                grupaWidth = xImaUplatu - xGrupa;
                imaUplatuWidth = contentBounds.Right - xImaUplatu;
            }

			columns.Clear();
			addColumn(xClan, clanWidth);
			addColumn(xGrupa, grupaWidth);
            if (!samoNedostajuceUplate)
                addColumn(xImaUplatu, imaUplatuWidth);
        }
		
		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
            ReportGrupa gr = groups[groupId];
            int godina = (int)gr.Data[0];
            int mesec = (int)gr.Data[1];
            string godMes = new DateTime(godina, mesec, 1).ToString("MMMM yyyy");
            g.DrawString(godMes, groupTitleFont, blackBrush, groupHeaderRect);

            if (!samoNedostajuceUplate)
            {
                StringFormat fmt = new StringFormat();
                fmt.Alignment = StringAlignment.Far;
                g.DrawString("Uplata", itemFont, blackBrush, groupHeaderRect, fmt);
            }

            float xClan = columns[0].X;
			float y = groupHeaderRect.Y + groupTitleFont.GetHeight(g);
			using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
			{
				g.DrawLine(pen, new PointF(xClan, y), new PointF(groupHeaderRect.X + groupHeaderRect.Width, y));
			}
		}
    }
}
