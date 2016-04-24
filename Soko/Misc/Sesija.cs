using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Soko.Misc
{
    public class Sesija
    {
        private class Ocitavanje
        {
            public int brojKartice;
            public DateTime vremeOcitavanja;

            public Ocitavanje(int brojKartice, DateTime vremeOcitavanja)
            {
                this.brojKartice = brojKartice;
                this.vremeOcitavanja = vremeOcitavanja;
            }
        }

        private DateTime startTime;
        private DateTime endTime;
        private List<Ocitavanje> ocitavanja;
        private List<string> logMessages;
        private StreamWriter logStreamWriter;
        private bool iskljuciLogovanjeNaKraju = false;

        private static Sesija instance;
        public static Sesija Instance
        {
            get
            {
                if (instance == null)
                    instance = new Sesija();
                return instance;
            }
        }

        protected Sesija()
        {

        }

        public void InitSession()
        {
            startTime = DateTime.Now;
            ocitavanja = new List<Ocitavanje>();
            logMessages = new List<string>();
        }

        public void EndSession()
        {
            endTime = DateTime.Now;
            if (Options.Instance.LogToFile)
            {
                createLogStreamWriter();

                logStreamWriter.WriteLine("START TIME: " + startTime.ToString("dd.MM.yyyy HH:mm:ss"));
                logStreamWriter.WriteLine("END TIME: " + endTime.ToString("dd.MM.yyyy HH:mm:ss"));

                logStreamWriter.WriteLine(String.Format("{0} OCITAVANJA KARTICE", ocitavanja.Count));
                foreach (Ocitavanje o in ocitavanja)
                {
                    logStreamWriter.WriteLine(o.brojKartice.ToString() + " " + o.vremeOcitavanja.ToString("dd.MM.yyyy HH:mm:ss"));
                }

                foreach (string s in logMessages)
                {
                    logStreamWriter.WriteLine(s);
                }

                closeLogStreamWriter();
                if (iskljuciLogovanjeNaKraju)
                {
                    Options.Instance.LogToFile = false;
                }
            }
        }

        public void OnOcitavanjeKartice(int brojKartice, DateTime vremeOcitavanja)
        {
            ocitavanja.Add(new Ocitavanje(brojKartice, vremeOcitavanja));
        }

        private void createLogStreamWriter()
        {
            String dirName = @"..\Log";
            System.IO.Directory.CreateDirectory(dirName);
            String fileName = String.Format("log_{0}_{1}_{2}_{3}_{4}_{5}.txt", startTime.Year, startTime.Month,
                startTime.Day, startTime.Hour, startTime.Minute, startTime.Second);
            logStreamWriter = File.AppendText(Path.Combine(dirName, fileName));
        }

        private void closeLogStreamWriter()
        {
            logStreamWriter.Close();
        }

        public void Log(string logMessage, bool ukljuciLogovanje = false)
        {
            if (logMessages.Count >= Options.Instance.MaxLogMessages)
            {
                return;
            }
            if (ukljuciLogovanje)
            {
                if (!Options.Instance.LogToFile)
                {
                    Options.Instance.LogToFile = true;
                    iskljuciLogovanjeNaKraju = true;
                }
            }
            if (Options.Instance.LogToFile)
            {
                string msg = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "   " + logMessage;
                logMessages.Add(msg);
            }
        }
    }
}
