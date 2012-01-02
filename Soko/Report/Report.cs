using System;
using System.Drawing;
using Soko.Exceptions;
using System.Collections.Generic;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for ReportColumn.
	/// </summary>
	public class ReportColumn
	{
		private float x;
		private float width;
		private int itemsColumnIndex;
		private string format;
		private StringFormat itemRectFormat = new StringFormat();

		public ReportColumn()
		{

		}

		public ReportColumn(int index, float x, float width, string headerTitle)
		{
			this.itemsColumnIndex = index;
			this.x = x;
			this.width = width;
			this.headerTitle = headerTitle;
		}

		public float X
		{
			get { return x; }
			set { x = value; }
		}

		public float Width
		{
			get { return width; }
			set { width = value; }
		}

		public int ItemsColumnIndex
		{
			get { return itemsColumnIndex; }
			set { itemsColumnIndex = value; }
		}

		public string Format
		{
			set { format = value; }
		}

		public StringFormat ItemRectFormat
		{
			get { return itemRectFormat; }
			set { itemRectFormat = value; }
		}

		private string headerTitle;
		public string HeaderTitle
		{
			get 
			{
				if (headerTitle == null)
					return String.Empty;
				return headerTitle; 
			}
			set { headerTitle = value; }
		}

		private StringFormat headerFormat = new StringFormat();
		public StringFormat HeaderFormat
		{
			get { return headerFormat; }
			set { headerFormat = value; }
		}
		
		public string getFormattedString(object[] itemsRow)
		{
			object item = itemsRow[itemsColumnIndex];
			if (item == null)
				return String.Empty;
			else if (String.IsNullOrEmpty(format))
				return item.ToString();
			else
			{
				string fmt = "{0:" + format + "}";
				return String.Format(fmt, item);
			}
		}
	}

	public class ReportGrupa
	{
		private int start;
		public int Start
		{
			get { return start; }
			set { start = value; }
		}

		private int count;
		public int Count
		{
			get { return count; }
			set { count = value; }
		}

		private object[] data;
		public object[] Data
		{
			get { return data; }
			set { data = value; }
		}

		private ReportGrupa masterGrupa;
		public ReportGrupa MasterGrupa
		{
			get { return masterGrupa; }
			set { masterGrupa = value; }
		}

		private int detailGrupeStart;
		public int DetailGrupeStart
		{
			get { return detailGrupeStart; }
			set { detailGrupeStart = value; }
		}

		private int detailGrupeCount;
		public int DetailGrupeCount
		{
			get { return detailGrupeCount; }
			set { detailGrupeCount = value; }
		}

		public ReportGrupa()
		{

		}

		public ReportGrupa(object[] data, int count)
		{
			this.data = data;
			this.count = count;
		}
		
		public ReportGrupa(object[] data, int start, int count)
		{
			this.data = data;
			this.start = start;
			this.count = count;
		}
		
		public ReportGrupa(int start, int count)
		{
			this.start = start;
			this.count = count;
		}
	}

	public abstract class ReportLista
	{
        protected List<ReportColumn> columns = new List<ReportColumn>();
		protected Izvestaj izvestaj;
		private int firstPageNum;

		private float startY;
		public float StartY
		{
			get { return startY; }
			set { startY = value; }
		}

		protected int lastPageNum;
		public int LastPageNum
		{
			get { return lastPageNum; }
		}

		protected float endY;
		public float EndY
		{
			get { return endY; }
		}

		protected Font itemFont;
		public Font ItemFont
		{
			get { return itemFont; }
			set { itemFont = value; }
		}

		protected Font itemsHeaderFont;
		protected Brush blackBrush;

		protected float itemHeight;
		protected float groupHeaderHeight;
		protected float itemsHeaderHeight;
		protected float masterGroupHeaderHeight;

		protected List<object[]> items;
        public List<object[]> Items
		{
			get { return items; }
		}

        protected List<ReportGrupa> groups;
        protected IDictionary<int, List<ReportGroupPart>> listLayout;

		public ReportLista(Izvestaj izvestaj, int pageNum, float y, Font itemFont,
			Font itemsHeaderFont)
		{
			this.izvestaj = izvestaj;
			this.firstPageNum = pageNum;
			this.startY = y;

			this.itemFont = itemFont;
			this.itemsHeaderFont = itemsHeaderFont;
			blackBrush = Brushes.Black;
		}

		protected ReportColumn addColumn(float x, float width)
		{
			ReportColumn result = new ReportColumn(columns.Count, x, width, String.Empty);
			columns.Add(result);
			return result;
		}

		protected ReportColumn addColumn(float x, float width, string headerTitle,
			StringFormat headerFormat)
		{
			ReportColumn result = new ReportColumn(columns.Count, x, width, headerTitle);
			result.HeaderFormat = headerFormat;
			columns.Add(result);
			return result;
		}

		protected ReportColumn addColumn(float x, float width,
			StringFormat itemRectFormat, string headerTitle,
			StringFormat headerFormat)
		{
			ReportColumn result = new ReportColumn(columns.Count, x, width, headerTitle);
			result.ItemRectFormat = itemRectFormat;
			result.HeaderFormat = headerFormat;
			columns.Add(result);
			return result;
		}

		protected ReportColumn addColumn(float x, float width, string format,
			StringFormat itemRectFormat, string headerTitle,
			StringFormat headerFormat)
		{
			ReportColumn result = new ReportColumn(columns.Count, x, width, headerTitle);
			result.Format = format;
			result.ItemRectFormat = itemRectFormat;
			result.HeaderFormat = headerFormat;
			columns.Add(result);
			return result;
		}

		protected ReportColumn addColumn(float x, float width,
			StringFormat itemRectFormat, string format)
		{
			ReportColumn result = new ReportColumn(columns.Count, x, width, String.Empty);
			result.Format = format;
			result.ItemRectFormat = itemRectFormat;
			columns.Add(result);
			return result;
		}

		protected float getColumnMaxWidth(int colIndex, Graphics g, Font f)
		{
			ReportColumn col = columns[colIndex];
			float max = 0;
			for (int i = 0; i < items.Count; i++)
			{
				object[] itemsRow = items[i];
				string str = col.getFormattedString(itemsRow);
				max = Math.Max(max, g.MeasureString(str, f).Width);
			}
			return max;
		}

		protected void createListLayout(float groupHeaderHeight, float itemHeight, 
			float afterGroupHeight, float newPageTopOffset, RectangleF contentBounds, 
			float masterGroupHeaderHeight, float afterMasterGroupHeight)
		{
			float minHeight = masterGroupHeaderHeight + groupHeaderHeight + itemHeight;
			if (minHeight > contentBounds.Height)
				throw new SmallPageSizeException();

			int pageNum = firstPageNum;
			float y = StartY;

            listLayout = new Dictionary<int, List<ReportGroupPart>>();
			
			ReportGrupa gr = null;
			ReportGrupa prevGrupa;
			
			for (int i = 0; i < groups.Count; i++)
			{
				prevGrupa = gr;
				gr = groups[i];
				
				int recNum = gr.Start;
				int endRec = gr.Start + gr.Count;

				while (recNum < endRec)
				{
					bool newPage = false;
					if (recNum == gr.Start && shouldStartOnNewPage(gr, prevGrupa)
						|| (y + minHeight > contentBounds.Bottom))
					{
						newPage = true;
						pageNum++;
						y = contentBounds.Y + newPageTopOffset;
					}

					float groupPartY = y;

					bool masterGroupHeader = false;
					bool groupHeader = false;
					if (recNum == gr.Start)
					{
						if (masterGroupHeaderHeight != 0f)
						{
							if (prevGrupa == null
								|| !object.ReferenceEquals(gr.MasterGrupa, prevGrupa.MasterGrupa))
							{
								if (prevGrupa != null && !newPage)
								{
									y += afterMasterGroupHeight;
									groupPartY = y;
								}
								masterGroupHeader = true;
								y += masterGroupHeaderHeight;
							}
						}

						groupHeader = true;
						y += groupHeaderHeight;
					}

					int numItems;
					if (y + (endRec - recNum) * itemHeight <= contentBounds.Bottom)
						numItems = endRec - recNum;
					else
						numItems = (int)Math.Floor((contentBounds.Bottom - y) / itemHeight); 

					ReportGroupPart part = new ReportGroupPart(pageNum, i, recNum, 
						numItems, groupPartY, groupHeader, masterGroupHeader);

                    List<ReportGroupPart> partList;
                    if (listLayout.ContainsKey(pageNum))
                    {
                        partList = listLayout[pageNum];
                    }
                    else
                    {
                        partList = new List<ReportGroupPart>();
                        listLayout.Add(pageNum, partList);
                    }
					partList.Add(part);

					y += numItems * itemHeight;
					recNum += numItems;
				}
				y += afterGroupHeight;
			}
			this.lastPageNum = pageNum;
			this.endY = y;
		}
		
		protected void createListLayout(float groupHeaderHeight,
			float itemHeight, float afterGroupHeight, float newPageTopOffset, 
			RectangleF contentBounds)
		{
			createListLayout(groupHeaderHeight, itemHeight, afterGroupHeight, 
				newPageTopOffset, contentBounds, 0f, 0f);
		}

		protected virtual bool shouldStartOnNewPage(ReportGrupa grupa, 
			ReportGrupa prevGrupa)
		{
			return false;
		}

		public void drawContent(Graphics g, RectangleF contentBounds, int pageNum)
		{
            if (!listLayout.ContainsKey(pageNum))
                return;

            List<ReportGroupPart> parts = listLayout[pageNum];
			foreach (ReportGroupPart part in parts)
			{
				float y = part.Y;
				if (part.MasterGroupHeader)
				{
					RectangleF masterGroupHeaderRect = new RectangleF(
						contentBounds.X, y, contentBounds.Width, masterGroupHeaderHeight);
					drawMasterGroupHeader(g, part.GroupId, masterGroupHeaderRect);
					y += masterGroupHeaderHeight;
				}
				if (part.GroupHeader)
				{
					RectangleF groupHeaderRect = new RectangleF(
						contentBounds.X, y, contentBounds.Width, groupHeaderHeight);
					drawGroupHeader(g, part.GroupId, groupHeaderRect);
					y += groupHeaderHeight;
				}

				RectangleF[] itemRect = new RectangleF[columns.Count];
				foreach (ReportColumn col in columns)
				{
					itemRect[col.ItemsColumnIndex] = new RectangleF(
						col.X, y, col.Width, itemHeight);
				}

				for (int i = part.RecNum; i < part.RecNum + part.NumItems; i++)
				{
					object[] itemsRow = items[i];
					foreach (ReportColumn col in columns)
					{
						string item = col.getFormattedString(itemsRow);
						g.DrawString(item, itemFont, blackBrush, 
							itemRect[col.ItemsColumnIndex], col.ItemRectFormat); 
						itemRect[col.ItemsColumnIndex].Y += itemHeight;
					}
				}
			}
		}

		protected virtual void drawMasterGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
		
		}

		protected virtual void drawGroupHeader(Graphics g, int groupId, RectangleF groupHeaderRect)
		{
		
		}

		protected virtual void drawItemsHeader(Graphics g, float y, bool drawLine)
		{
			foreach (ReportColumn col in columns)
			{
				RectangleF columnHeaderRect = new RectangleF(
					col.X, y, col.Width, itemsHeaderHeight);
				g.DrawString(col.HeaderTitle, itemsHeaderFont, blackBrush, 
					columnHeaderRect, col.HeaderFormat);
			}

			if (drawLine)
			{
				y += itemsHeaderFont.GetHeight(g);
				using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
				{
					ReportColumn firstCol = columns[0];
					ReportColumn lastCol = columns[columns.Count - 1];
					g.DrawLine(pen, new PointF(firstCol.X, y), 
						new PointF(lastCol.X + lastCol.Width, y));
				}
			}
		}
	}

	public class ReportGroupPart
	{
		private int groupId;
		public int GroupId
		{
			get { return groupId; }
			set { groupId = value; }
		}

		private int pageNum;
		public int PageNum
		{
			get { return pageNum; }
			set { pageNum = value; }
		}

		private int recNum;
		public int RecNum
		{
			get { return recNum; }
			set { recNum = value; }
		}

		private int numItems;
		public int NumItems
		{
			get { return numItems; }
			set { numItems = value; }
		}

		private float y;
		public float Y
		{
			get { return y; }
			set { y = value; }
		}

		private bool groupHeader;
		public bool GroupHeader
		{
			get { return groupHeader; }
			set { groupHeader = value; }
		}

		private bool masterGroupHeader;
		public bool MasterGroupHeader
		{
			get { return masterGroupHeader; }
			set { masterGroupHeader = value; }
		}

		public ReportGroupPart(int pageNum, int groupId, int recNum, int numItems, 
			float y, bool groupHeader, bool masterGroupHeader)
		{
			this.pageNum = pageNum;
			this.groupId = groupId;
			this.recNum = recNum;
			this.numItems = numItems;
			this.y = y;
			this.groupHeader = groupHeader;
			this.masterGroupHeader = masterGroupHeader;
		}

	}

	public class ReportText
	{
		public ReportText()
		{
			
		}

		public ReportText(string text, Font font, Brush brush, RectangleF rect, StringFormat format)
		{
			this.text = text;
			this.font = font;
			this.brush = brush;
			this.rect = rect;
			this.format = format;
		}

		private string text;
		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		private Font font;
		public Font Font
		{
			get { return font; }
			set { font = value; }
		}

		private Brush brush;
		public Brush Brush
		{
			get { return brush; }
			set { brush = value; }
		}

		private RectangleF rect;
		public RectangleF Rect
		{
			get { return rect; }
			set { rect = value; }
		}

		private StringFormat format;
		public StringFormat Format
		{
			get { return format; }
			set { format = value; }
		}

		public void draw(Graphics g)
		{
			g.DrawString(text, font, brush, rect, format); 
		}
	}
}
