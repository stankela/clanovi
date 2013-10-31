using System;
using System.Drawing;

namespace Soko.Report
{
	/// <summary>
	/// Summary description for Izvestaj.
	/// </summary>
	public class Izvestaj
	{
		private float relHeight = 24.5f;
		private float relHeaderHeight = 2.7f;
		public const float relWidth = 17.2f;
		private float relPictureWidth = 4.7f;

		private StringFormat titleFormat;
		private StringFormat subTitleFormat;
		private StringFormat dateFormat;

		private Font titleFont;
		private Font subTitleFont;
		private Font pageNumFont;
		private Font sokDruVojFont;
		private Font adresaFont;
		protected Brush blackBrush;

		protected RectangleF headerBounds;
		protected RectangleF contentBounds;
        protected RectangleF pageBounds;

		protected int lastPageNum;
		public int LastPageNum
		{
			get { return lastPageNum; }
		}

		private bool a4;
		public bool A4
		{
			get { return a4; }
            set { a4 = value; }
		}

        private string printerName;
        public string PrinterName
        {
            get { return printerName; }
            set { printerName = value; }
        }

		public Izvestaj()
		{
			createFormats();
			createFonts();
        
            A4 = true;
            PrinterName = Options.Instance.PrinterNameIzvestaj;
        }

		private string title;
		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		private string subTitle = String.Empty;
		public string SubTitle
		{
			get { return subTitle; }
			set { subTitle = value; }
		}

		private string documentName;
		public string DocumentName
		{
			get { return documentName; }
			set { documentName = value; }
		}

		private bool contentSetupDone;
		public bool ContentSetupDone
		{
			get { return contentSetupDone; }
		}

		private DateTime timeOfPrint;
		public DateTime TimeOfPrint
		{
			get { return timeOfPrint; }
			set { timeOfPrint = value; }
		}

		private void createFormats()
		{
			titleFormat = new StringFormat();
			titleFormat.Alignment = StringAlignment.Center;
			titleFormat.LineAlignment = StringAlignment.Near;

			subTitleFormat = new StringFormat();
			subTitleFormat.Alignment = StringAlignment.Center;
			subTitleFormat.LineAlignment = StringAlignment.Far;

			dateFormat = new StringFormat();
			dateFormat.Alignment = StringAlignment.Far;
			dateFormat.LineAlignment = StringAlignment.Near;
		}

		public virtual void BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			//createFonts();
			//e.Cancel = true;
		}

		private void createFonts()
		{
			titleFont = new Font("Tahoma", 14, FontStyle.Bold);
			subTitleFont = new Font("Tahoma", 10, FontStyle.Bold);
			pageNumFont = new Font("Arial", 8);
			sokDruVojFont = new Font("Arial Narrow", 8);
			adresaFont = new Font("Arial Narrow", 7);
			blackBrush = Brushes.Black;
		}

		public virtual void EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			//releaseFonts();
		}

		public virtual float getHeaderHeight(Graphics g, RectangleF marginBounds)
		{
			return relHeaderHeight / relHeight * marginBounds.Height;
		}

		public void drawPage(Graphics g, int pageNum)
		{
			drawHeader(g, pageNum);
			drawContent(g, pageNum);
		}

		public virtual void drawHeader(Graphics g, int pageNum)
		{
			float pictureWidth = relPictureWidth / relWidth * headerBounds.Width;
			float pictureHeight = headerBounds.Height * 0.8f;
			RectangleF pictureBounds = new RectangleF(headerBounds.X, headerBounds.Y, pictureWidth, pictureHeight);
			drawSokoWithCaption(g, pictureBounds);

			float lineOffset = headerBounds.Height * 0.9f;
			using(Pen pen = new Pen(Color.Black, 1/72f * 2f))
			{
				g.DrawLine(pen, new PointF(headerBounds.X, headerBounds.Y + lineOffset),
					new PointF(headerBounds.X + headerBounds.Width, headerBounds.Y + lineOffset));
			}

			float titleHeight = titleFont.GetHeight(g);
			if (subTitle != "")
				titleHeight += subTitleFont.GetHeight(g) * 1.5f;
			float titleY = headerBounds.Y + (headerBounds.Height - titleHeight) / 3;
			RectangleF titleBounds = new RectangleF(headerBounds.X, titleY, 
				headerBounds.Width, titleHeight);
				
			g.DrawString(title, titleFont, blackBrush, titleBounds, titleFormat); 
			if (subTitle != "")
				g.DrawString(subTitle, subTitleFont, blackBrush, titleBounds, subTitleFormat); 

			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			String page = resourceManager.GetString("izvestaj_header_strana");
			String from = resourceManager.GetString("izvestaj_header_od");
			string datum = TimeOfPrint.ToShortDateString();
			string vreme = TimeOfPrint.ToShortTimeString();
			g.DrawString(datum + " " + vreme, pageNumFont, blackBrush, 
				headerBounds.Right, headerBounds.Top, dateFormat); 
			g.DrawString(String.Format("{0} {1} {2} {3}", page, pageNum, from, LastPageNum), pageNumFont, blackBrush, 
				headerBounds.Right, headerBounds.Top + pageNumFont.GetHeight(g) * 1.5f, dateFormat); 
		}

		private void drawSokoWithCaption(Graphics g, RectangleF pictureBounds)
		{
			System.Resources.ResourceManager resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.SlikeResursi", this.GetType().Assembly);
			Image sokoImage = (Image)resourceManager.GetObject("slika_soko");
			resourceManager = new 
				System.Resources.ResourceManager("Soko.Resources.PreviewResursi", this.GetType().Assembly);
			string sokDruVoj = resourceManager.GetString("izvestaj_header_sok_dru_voj");
			string adresa = resourceManager.GetString("izvestaj_header_adresa");

			float ySokDruVoj = pictureBounds.Y + 0.7f * pictureBounds.Height;
			float yAdresa = pictureBounds.Y + 0.85f * pictureBounds.Height;
			RectangleF sokDruVojBounds = new RectangleF(pictureBounds.X, ySokDruVoj, 
				pictureBounds.Width, yAdresa - ySokDruVoj);
			RectangleF adresaBounds = new RectangleF(pictureBounds.X, yAdresa, 
				pictureBounds.Width, pictureBounds.Y + pictureBounds.Height - yAdresa);

			using(Pen pen = new Pen(Color.Black, 1/72f * 0.25f))
			{
				//			g.DrawRectangle(pen, pictureBounds.X, pictureBounds.Y, pictureBounds.Width, pictureBounds.Height);
			}
			float sokoHeight = 0.7f * pictureBounds.Height;
			float sokoWidth = sokoHeight;
			float sokoX = pictureBounds.X + (pictureBounds.Width - sokoWidth) / 2;
			RectangleF sokoBounds = new RectangleF(sokoX, pictureBounds.Y, sokoWidth, sokoHeight);
			g.DrawImage(sokoImage, sokoBounds);
			g.DrawString(sokDruVoj, sokDruVojFont, blackBrush, sokDruVojBounds, titleFormat); 
			g.DrawString(adresa, adresaFont, blackBrush, adresaBounds, titleFormat); 
		}

        public void setupContent(Graphics g, RectangleF marginBounds, RectangleF pageBounds)
		{
			headerBounds = getHeaderBounds(g, marginBounds);
			contentBounds = getContentBounds(g, marginBounds);
            this.pageBounds = pageBounds;
			doSetupContent(g);
			contentSetupDone = true;
		}

		private RectangleF getHeaderBounds(Graphics g, RectangleF marginBounds)
		{
			return new RectangleF(marginBounds.Location, 
				new SizeF(marginBounds.Width, getHeaderHeight(g, marginBounds)));
		}

		private RectangleF getContentBounds(Graphics g, RectangleF marginBounds)
		{
			float headerHeight = getHeaderHeight(g, marginBounds);
			return new RectangleF(marginBounds.X, 
				marginBounds.Y + headerHeight, 
				marginBounds.Width, 
				marginBounds.Height - headerHeight);
		}

		protected virtual void doSetupContent(Graphics g)
		{
		
		}

		public virtual void drawContent(Graphics g, int pageNum)
		{

		}
		
		public void QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
		{
			// Set margins to .5" all the way around
			//e.PageSettings.Margins = new Margins(50, 50, 50, 50);
		}
	}
}
