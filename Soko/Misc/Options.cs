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

        private string printerNamePotvrda = null;
        public string PrinterNamePotvrda
        {
            get { return printerNamePotvrda; }
            set { printerNamePotvrda = value; }
        }

        private string printerNameIzvestaj = null;
        public string PrinterNameIzvestaj
        {
            get { return printerNameIzvestaj; }
            set { printerNameIzvestaj = value; }
        }

        private int comPortReader = 1;
        public int COMPortReader
        {
            get { return comPortReader; }
            set { comPortReader = value; }
        }

        private int comPortWriter = 2;
        public int COMPortWriter
        {
            get { return comPortWriter; }
            set { comPortWriter = value; }
        }

        private int poslednjiDanZaUplate = 10;
        public int PoslednjiDanZaUplate
        {
            get { return poslednjiDanZaUplate; }
            set { poslednjiDanZaUplate = value; }
        }

        private bool prikaziBrojClanaKodOcitavanjaKartice = true;
        public bool PrikaziBrojClanaKodOcitavanjaKartice
        {
            get { return prikaziBrojClanaKodOcitavanjaKartice; }
            set { prikaziBrojClanaKodOcitavanjaKartice = value; }

        }

        private int velicinaSlovaZaCitacKartica = 28;
        public int VelicinaSlovaZaCitacKartica
        {
            get { return velicinaSlovaZaCitacKartica; }
            set { velicinaSlovaZaCitacKartica = value; }
        }

        private Color pozadinaCitacaKartica = Color.Yellow;
        public Color PozadinaCitacaKartica
        {
            get { return pozadinaCitacaKartica; }
            set { pozadinaCitacaKartica = value; }
        }

        private bool prikaziBojeKodOcitavanja = true;
        public bool PrikaziBojeKodOcitavanja
        {
            get { return prikaziBojeKodOcitavanja; }
            set { prikaziBojeKodOcitavanja = value; }
        }

        private bool adminMode = false;
        public bool AdminMode
        {
            get { return adminMode; }
            set { adminMode = value; }
        }

        private int citacKarticaTimerInterval = 500;
        public int CitacKarticaTimerInterval
        {
            get { return citacKarticaTimerInterval; }
            set { citacKarticaTimerInterval = value; }
        }

        private int brojPokusajaCitacKartica = 2;
        public int BrojPokusajaCitacKartica
        {
            get { return brojPokusajaCitacKartica; }
            set { brojPokusajaCitacKartica = value; }
        }
    }
}
