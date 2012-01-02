using System;
using System.Drawing;

namespace Soko
{
	/// <summary>
	/// Summary description for Options.
	/// </summary>
	public class Options
	{
		private static Options instance;
		public static Options Instance
		{
			get
			{
				if (instance == null)
					instance = new Options();
				return instance;
			}
		}

		protected Options()
		{

		}

		private Font font;
		public Font Font
		{
			get { return font; }
			set { font = value; }
		}

        private string printerNamePotvrda;
        public string PrinterNamePotvrda
        {
            get { return printerNamePotvrda; }
            set { printerNamePotvrda = value; }
        }

        private string printerNameIzvestaj;
        public string PrinterNameIzvestaj
        {
            get { return printerNameIzvestaj; }
            set { printerNameIzvestaj = value; }
        }

    }
}
