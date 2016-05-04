using System;
using System.Drawing;
using System.IO;

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

        private bool prikaziImeClanaKodOcitavanjaKartice = false;
        public bool PrikaziImeClanaKodOcitavanjaKartice
        {
            get { return prikaziImeClanaKodOcitavanjaKartice; }
            set { prikaziImeClanaKodOcitavanjaKartice = value; }
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

        private bool prikaziDisplejPrekoCelogEkrana = true;
        public bool PrikaziDisplejPrekoCelogEkrana
        {
            get { return prikaziDisplejPrekoCelogEkrana; }
            set { prikaziDisplejPrekoCelogEkrana = value; }
        }

        private int sirinaDispleja;
        public int SirinaDispleja
        {
            get { return sirinaDispleja; }
            set { sirinaDispleja = value; }
        }

        private int visinaDispleja;
        public int VisinaDispleja
        {
            get { return visinaDispleja; }
            set { visinaDispleja = value; }
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

        private bool uvekPitajZaLozinku = true;
        public bool UvekPitajZaLozinku
        {
            get { return uvekPitajZaLozinku; }
            set { uvekPitajZaLozinku = value; }
        }

        private int lozinkaTimerMinuti = 1;
        public int LozinkaTimerMinuti
        {
            get { return lozinkaTimerMinuti; }
            set { lozinkaTimerMinuti = value; }
        }

        private bool logToFile = false;
        public bool LogToFile
        {
            get { return logToFile; }
            set { logToFile = value; }
        }

        private int maxLogMessages = 500;
        public int MaxLogMessages
        {
            get { return maxLogMessages; }
            set { maxLogMessages = value; }
        }

        private string adminLozinka = "sdv158";
        public string AdminLozinka
        {
            get { return adminLozinka; }
            set { adminLozinka = value; }
        }

        private bool traziLozinkuPreOtvaranjaProzora = false;
        public bool TraziLozinkuPreOtvaranjaProzora
        {
            get { return traziLozinkuPreOtvaranjaProzora; }
            set { traziLozinkuPreOtvaranjaProzora = value; }
        }

        private bool citacKarticeNaPosebnomThreadu = false;
        public bool CitacKarticeNaPosebnomThreadu
        {
            get { return citacKarticeNaPosebnomThreadu; }
            set { citacKarticeNaPosebnomThreadu = value; }
        }

        private int citacKarticaThreadInterval = 500;
        public int CitacKarticaThreadInterval
        {
            get { return citacKarticaThreadInterval; }
            set { citacKarticaThreadInterval = value; }
        }
    }
}
