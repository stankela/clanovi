using System;
using System.Drawing;
using Soko.Dao;
using Soko.Domain;
using Soko.Exceptions;
using System.Collections.Generic;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for DnevniPrihodiIzvestaj.
	/// </summary>
	public class DnevniPrihodiIzvestaj : Izvestaj
	{
		private DateTime fromDate;
		private DateTime toDate;
        private List<SifraGrupe> grupe;

		private ReportText ukupanPrihodIznosReportText;
		private ReportText ukupanPrihodCaptionReportText;
		private DnevniPrihodiClanoviLista lista;

		private Font ukupanPrihodCaptionFont;
		private Font ukupanPrihodIznosFont;

        public DnevniPrihodiIzvestaj(DateTime from, DateTime to, List<SifraGrupe> grupe)
		{
			this.fromDate = from.Date;
			this.toDate = to.Date;
			this.grupe = grupe;

			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			Title = resourceManager.GetString("prihodi_izvestaj_dnevni_title");
			SubTitle = fromDate.ToShortDateString() + " - " + 
				toDate.ToShortDateString();
			DocumentName = Title;
		
			ukupanPrihodCaptionFont = new Font("Arial", 10);
			ukupanPrihodIznosFont = new Font("Courier New", 10, FontStyle.Bold);
			
			Font itemFont = new Font("Courier New", 9);
			Font itemsHeaderFont = null;
			Font groupTitleFont = new Font("Courier New", 9, FontStyle.Bold);
			Font masterGroupTitleFont = new Font("Courier New", 10, FontStyle.Bold);
			lista = new DnevniPrihodiClanoviLista(fromDate, toDate, grupe, this, 1, 0f, 
				itemFont, itemsHeaderFont, groupTitleFont, masterGroupTitleFont);
		}

		protected override void doSetupContent(Graphics g)
		{
			string ukupanPrihodText = "Ukupan prihod za period ";
			ukupanPrihodText += fromDate.ToShortDateString() + " - "
				+ toDate.ToShortDateString();

			decimal ukupanPrihod = MapperRegistry.uplataClanarineDAO().getUkupanPrihod(fromDate, toDate, grupe);
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

	public class DnevniPrihodiClanoviLista : ReportLista
	{
		private DateTime fromDate;
		private DateTime toDate;
        private List<SifraGrupe> grupe;
		
		private float relClan = 0.5f;
		private float relVaziOd = 12.5f;
		private float relClanarina = 15f;

		private Font groupTitleFont;
		private float groupTitleHeight;
		
		private Font masterGroupTitleFont;
		
		private List<ReportGrupa> masterGroups;

        public DnevniPrihodiClanoviLista(DateTime from, DateTime to, 
            List<SifraGrupe> grupe, Izvestaj izvestaj, int pageNum, float y, Font itemFont, 
			Font itemsHeaderFont, Font groupTitleFont, Font masterGroupTitleFont) 
			: base(izvestaj, pageNum, y, itemFont, itemsHeaderFont)
		{
			this.fromDate = from;
			this.toDate = to;
			this.grupe = grupe;
			this.groupTitleFont = groupTitleFont;
			this.masterGroupTitleFont = masterGroupTitleFont;

			fetchItems();
		}

		private void fetchItems()
		{
			items = MapperRegistry.uplataClanarineDAO().getDnevniPrihodiClanoviReportItems(fromDate, toDate, grupe);
			groups = MapperRegistry.uplataClanarineDAO().getDnevniPrihodiClanoviReportGrupeDanGrupa(fromDate, toDate, grupe);
			masterGroups = MapperRegistry.uplataClanarineDAO().getDnevniPrihodiClanoviReportGrupeDan(fromDate, toDate, grupe);

			int start = 0;
			for (int i = 0; i < groups.Count; i++)
			{
				ReportGrupa g = groups[i];
				g.Start = start;
				start += g.Count;
			}

			int j = 0;
			for (int i = 0; i < masterGroups.Count; i ++)
			{
				ReportGrupa mg = masterGroups[i];
				ReportGrupa g = groups[j];
				int startDetailGroup = j;
				int detailBrojClanova = g.Count;
				while (detailBrojClanova <= mg.Count)
				{
					g.MasterGrupa = mg;
					if (++j < groups.Count)
					{
						g = groups[j];
						detailBrojClanova += g.Count;
					}
					else
						break;
				}
				mg.DetailGrupeStart = startDetailGroup;
				mg.DetailGrupeCount = j - startDetailGroup;
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
			masterGroupHeaderHeight = masterGroupTitleFont.GetHeight(g) * 1.4f;
			float afterGroupHeight = itemHeight * 0.2f;
			float afterMasterGroupHeight = itemHeight * 0.75f;

			createListLayout(groupHeaderHeight, itemHeight, afterGroupHeight, 0f, 
				contentBounds, masterGroupHeaderHeight, afterMasterGroupHeight);
		}

		private void createColumns(Graphics g, RectangleF contentBounds)
		{
			float relWidth = Izvestaj.relWidth;
			float xClan = contentBounds.X + relClan / relWidth * contentBounds.Width;
			float xVaziOd = contentBounds.X + relVaziOd / relWidth * contentBounds.Width;
			float xClanarina = contentBounds.X + relClanarina / relWidth * contentBounds.Width;
			float clanWidth = xVaziOd - xClan;
			float vaziOdWidth = xClanarina - xVaziOd;
			float clanarinaWidth = contentBounds.Right - xClanarina;

			StringFormat vaziOdFormat = new StringFormat();
			vaziOdFormat.Alignment = StringAlignment.Center;

			StringFormat clanarinaFormat = new StringFormat();
			clanarinaFormat.Alignment = StringAlignment.Far;

			columns.Clear();
			addColumn(xClan, clanWidth);
			addColumn(xVaziOd, vaziOdWidth, vaziOdFormat, "d");
			addColumn(xClanarina, clanarinaWidth, clanarinaFormat, "F2");

		}
		
		protected override void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
			ReportGrupa gr = groups[groupId];
			string sifra = ((SifraGrupe)gr.Data[0]).Value;
			string naziv = (string)gr.Data[1];
			string sifNaz = sifra + " - " + naziv;
			decimal ukupanIznos = (decimal)gr.Data[2];

			float xClan = columns[0].X;
			float xOffset = xClan - groupHeaderRect.X;
			groupHeaderRect.X += xOffset;
			groupHeaderRect.Width -= xOffset;
			g.DrawString(sifNaz, groupTitleFont, blackBrush, groupHeaderRect);
			StringFormat clanarinaFormat = columns[2].ItemRectFormat;
			g.DrawString(ukupanIznos.ToString("F2"), groupTitleFont, blackBrush, 
				groupHeaderRect, clanarinaFormat);
			
			float y = groupHeaderRect.Y + groupTitleFont.GetHeight(g);
			using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
			{
				g.DrawLine(pen, new PointF(xClan, y), 
					new PointF(groupHeaderRect.Right, y));
			}
		}

		protected override void drawMasterGroupHeader(Graphics g, int groupId, RectangleF masterGroupHeaderRect)
		{
			ReportGrupa gr = groups[groupId];
			ReportGrupa mg = gr.MasterGrupa;

			DateTime datumUplate = (DateTime)mg.Data[0];
			decimal ukupanIznos = (decimal)mg.Data[1];
			g.DrawString(datumUplate.ToShortDateString(), masterGroupTitleFont, blackBrush, masterGroupHeaderRect);
			StringFormat clanarinaFormat = columns[2].ItemRectFormat;
			g.DrawString(ukupanIznos.ToString("F2"), masterGroupTitleFont, blackBrush,
				masterGroupHeaderRect, clanarinaFormat);
			
			float y = masterGroupHeaderRect.Y + masterGroupTitleFont.GetHeight(g);
			using(Pen pen = new Pen(Color.Black, 1/72f * 1f))
			{
				g.DrawLine(pen, new PointF(masterGroupHeaderRect.X, y), 
					new PointF(masterGroupHeaderRect.Right, y));
			}
		}
	}
}
