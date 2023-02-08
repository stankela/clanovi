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

        private int poslednjiMesecZaGodisnjeClanarine = 2;
        public int PoslednjiMesecZaGodisnjeClanarine
        {
            get { return poslednjiMesecZaGodisnjeClanarine; }
            set { poslednjiMesecZaGodisnjeClanarine = value; }
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

        private int citacKarticaThreadInterval = 500;
        public int CitacKarticaThreadInterval
        {
            get { return citacKarticaThreadInterval; }
            set { citacKarticaThreadInterval = value; }
        }

        private int citacKarticaThreadSkipCount = 2;
        public int CitacKarticaThreadSkipCount
        {
            get { return citacKarticaThreadSkipCount; }
            set { citacKarticaThreadSkipCount = value; }
        }

        private int citacKarticaThreadVisibleCount = 5;
        public int CitacKarticaThreadVisibleCount
        {
            get { return citacKarticaThreadVisibleCount; }
            set { citacKarticaThreadVisibleCount = value; }
        }

        private int citacKarticaThreadPauzaZaBrisanje = 200;
        public int CitacKarticaThreadPauzaZaBrisanje
        {
            get { return citacKarticaThreadPauzaZaBrisanje; }
            set { citacKarticaThreadPauzaZaBrisanje = value; }
        }

        private bool jedinstvenProgram = true;
        public bool JedinstvenProgram
        {
            get { return jedinstvenProgram; }
            set { jedinstvenProgram = value; }
        }

        private bool isProgramZaClanarinu = true;
        public bool IsProgramZaClanarinu
        {
            get { return isProgramZaClanarinu; }
            set { isProgramZaClanarinu = value; }
        }

        private string clientPath = @"..\CitacKartica\CitacKartica.exe";
        public string ClientPath
        {
            get { return clientPath; }
            set { clientPath = value; }
        }

        private string pipeHandle;
        public string PipeHandle
        {
            get { return pipeHandle; }
            set { pipeHandle = value; }
        }

        private bool useWaitAndReadLoop = false;
        public bool UseWaitAndReadLoop
        {
            get { return useWaitAndReadLoop; }
            set { useWaitAndReadLoop = value; }
        }

        private int numSecondsWaitAndRead = 3600;
        public int NumSecondsWaitAndRead
        {
            get { return numSecondsWaitAndRead; }
            set { numSecondsWaitAndRead = value; }
        }

        private DateTime nedostajuceUplateStartDate = new DateTime(2016, 9, 1, 0, 0, 0);
        public DateTime NedostajuceUplateStartDate
        {
            get { return nedostajuceUplateStartDate; }
            set { nedostajuceUplateStartDate = value; }
        }

        private bool useCardReaderLog = false;
        public bool UseCardReaderLog
        {
            get { return useCardReaderLog; }
            set { useCardReaderLog = value; }
        }

        // TODO4: Izbaci ovu opciju kada zavrsis prelazak na novi format kartica (takodje i sledece dve opcije)
        // Moguce vrednosti za (UseNewCardFormat, WritePanonitDataCard)
        // (true, false) - Kada hocu karticu citam i pisem u NovomFormatu. Kartica moze da bude u bilo kojem formatu
        //                 (Prazna, Panonit, NoviFormat).
        // (true, true) - Kada hocu da napravim karticu u starom Panonit formatu. Kartica mora da bude u NovomFormatu.
        // (false, false) - Kada hocu da karticu koja je u starom Panonit formatu citam i pisem u PanonitFormatu

        private bool useNewCardFormat = true;
        public bool UseNewCardFormat
        {
            get { return useNewCardFormat; }
            set { useNewCardFormat = value; }
        }

        private bool writePanonitDataCard = false;
        public bool WritePanonitDataCard
        {
            get { return writePanonitDataCard; }
            set { writePanonitDataCard = value; }
        }

        private bool writePraznaDataCard = false;
        public bool WritePraznaDataCard
        {
            get { return writePraznaDataCard; }
            set { writePraznaDataCard = value; }
        }
    }
}
