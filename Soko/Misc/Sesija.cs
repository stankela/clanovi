using Bilten.Dao;
using Bilten.Dao.NHibernate;
using NHibernate;
using NHibernate.Context;
using Soko.Data;
using Soko.Domain;
using Soko.UI;
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

        public const string LOG_DIR = @"..\Log";
        
        private DateTime startTime;
        private DateTime endTime;
        private List<Ocitavanje> ocitavanja;
        private Queue<string> logMessages;
        private StreamWriter logStreamWriter;
        private const string OCITAVANJA_KARTICE = "OCITAVANJA KARTICE";
        private const string START_TIME = "START TIME";
        private const string END_TIME = "END TIME";
        private Object logLock = new Object();

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
            logMessages = new Queue<string>();
        }

        public void EndSession()
        {
            endTime = DateTime.Now;
            if (Options.Instance.LogToFile)
            {
                createLogStreamWriter();

                logStreamWriter.WriteLine(START_TIME + ": " + startTime.ToString("dd.MM.yyyy HH:mm:ss"));
                logStreamWriter.WriteLine(END_TIME + ": " + endTime.ToString("dd.MM.yyyy HH:mm:ss"));

                logStreamWriter.WriteLine(ocitavanja.Count.ToString() + " " + OCITAVANJA_KARTICE);
                foreach (Ocitavanje o in ocitavanja)
                {
                    logStreamWriter.WriteLine(o.brojKartice.ToString() + " " + o.vremeOcitavanja.ToString("dd.MM.yyyy HH:mm:ss"));
                }

                // Proveri ocitavanja
                string msg;
                bool result = proveriOcitavanja(startTime, endTime, ocitavanja, out msg);
                if (!result)
                {
                    logStreamWriter.WriteLine(formatirajProveraOcitavanjaMsg(result, msg, ""));
                }

                foreach (string s in logMessages)
                {
                    logStreamWriter.WriteLine(s);
                }

                closeLogStreamWriter();
            }
        }

        public void OnOcitavanjeKartice(int brojKartice, DateTime vremeOcitavanja)
        {
            ocitavanja.Add(new Ocitavanje(brojKartice, vremeOcitavanja));
        }

        private void createLogStreamWriter()
        {
            System.IO.Directory.CreateDirectory(LOG_DIR);
            String fileName = String.Format("log_{0}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}.txt", startTime.Year, startTime.Month,
                startTime.Day, startTime.Hour, startTime.Minute, startTime.Second);
            logStreamWriter = File.AppendText(Path.Combine(LOG_DIR, fileName));
        }

        private void closeLogStreamWriter()
        {
            logStreamWriter.Close();
        }

        public void Log(string logMessage)
        {
            if (Options.Instance.LogToFile)
            {
                lock (logLock)
                {
                    if (logMessages.Count >= Options.Instance.MaxLogMessages)
                    {
                        logMessages.Dequeue();
                    }
                    string msg = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "   " + logMessage;
                    logMessages.Enqueue(msg);
                }
            }
        }

        public void LogException(string kind, Exception ex)
        {
            Log(kind);
            if (ex.Message != null)
            {
                Log(ex.Message);
            }
        }

        public void proveriOcitavanja(string fileName, out string msg)
        {
            DateTime from;
            DateTime to;
            List<Ocitavanje> listaOcitavanja = new List<Ocitavanje>();
            if (!parseOcitavanjaFromLogFile(fileName, listaOcitavanja, out from, out to))
            {
                msg = formatirajProveraOcitavanjaMsg(false, "(Lose formatiran fajl)", fileName);
            }
            else
            {
                bool result = proveriOcitavanja(from, to, listaOcitavanja, out msg);
                msg = formatirajProveraOcitavanjaMsg(result, msg, fileName);
            }
        }

        private bool parseOcitavanjaFromLogFile(string fileName, List<Ocitavanje> listaOcitavanja, out DateTime from,
            out DateTime to)
        {
            string[] lines = System.IO.File.ReadAllLines(fileName);
            string startTime = String.Empty;
            string endTime = String.Empty;
            int beginOcitavanje = -1;
            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains(START_TIME))
                {
                    startTime = lines[i];
                }
                else if (lines[i].Contains(END_TIME))
                {
                    endTime = lines[i];
                }
                else if (lines[i].Contains(OCITAVANJA_KARTICE))
                {
                    beginOcitavanje = i;
                }
                if (startTime != String.Empty && endTime != String.Empty && beginOcitavanje != -1)
                    break;
            }
            if (startTime == String.Empty || endTime == String.Empty || beginOcitavanje == -1)
            {
                from = DateTime.Now;
                to = DateTime.Now;
                return false;
            }

            string[] line = startTime.Split(' ');
            from = DateTime.Parse(line[2] + " " + line[3]);
            line = endTime.Split(' ');
            to = DateTime.Parse(line[2] + " " + line[3]);

            int brojOcitavanja = Int32.Parse(lines[beginOcitavanje].Trim().Split(' ')[0]);
            for (int i = 1; i <= brojOcitavanja; ++i)
            {
                line = lines[beginOcitavanje + i].Split(' ');
                int brojKartice = Int32.Parse(line[0]);
                DateTime vremeOcitavanja = DateTime.Parse(line[1] + " " + line[2]);
                listaOcitavanja.Add(new Ocitavanje(brojKartice, vremeOcitavanja));
            }
            return true;
        }

        private string formatirajProveraOcitavanjaMsg(bool ok, string msg, string fileName)
        {
            string result = (ok ? "OK  " : "FAIL") + "   " + msg;
            if (!String.IsNullOrEmpty(fileName))
                result += "   " + Path.GetFileName(fileName);
            return result;
        }

        private bool proveriOcitavanja(DateTime from, DateTime to, List<Ocitavanje> listaOcitavanja, out string msg)
        {
            IList<DolazakNaTrening> dolasci = null;
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    DolazakNaTreningDAOImpl dolazakNaTreningDAO =
                        DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO() as DolazakNaTreningDAOImpl;
                    dolazakNaTreningDAO.Session = session;

                    dolasci = dolazakNaTreningDAO.getDolazakNaTrening(from, to);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }

            if (dolasci.Count != listaOcitavanja.Count)
            {
                msg = String.Format("(Razlicit broj ocitavanja. Baza: {0} Fajl: {1})", dolasci.Count, listaOcitavanja.Count);
                return false;
            }
            else
            {
                for (int i = 0; i < dolasci.Count; ++i)
                {
                    DolazakNaTrening d = dolasci[i];
                    Ocitavanje o = listaOcitavanja[i];
                    if (d.Clan.BrojKartice != o.brojKartice)
                    {
                        msg = String.Format("(Broj kartice se ne poklapa. Baza: {0} Fajl: {1})",
                            d.Clan.BrojKartice, o.brojKartice);
                        return false;
                    }
                    else if (d.DatumVremeDolaska.Value.ToString("dd.MM.yyyy HH:mm:ss")
                        != o.vremeOcitavanja.ToString("dd.MM.yyyy HH:mm:ss"))
                    {
                        msg = String.Format("(Vreme dolaska se ne poklapa. Baza: {0} Fajl: {1})",
                            d.DatumVremeDolaska, o.vremeOcitavanja);
                        return false;
                    }
                }
            }

            msg = String.Format("(Broj ocitavanja: {0})", dolasci.Count);
            return true;
        }
    }
}
