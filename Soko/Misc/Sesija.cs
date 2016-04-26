using Bilten.Dao;
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

        private DateTime startTime;
        private DateTime endTime;
        private List<Ocitavanje> ocitavanja;
        private Queue<string> logMessages;
        private StreamWriter logStreamWriter;
        private bool iskljuciLogovanjeNaKraju = false;
        private const string OCITAVANJA_KARTICE = "OCITAVANJA KARTICE";
        private const string START_TIME = "START TIME";
        private const string END_TIME = "END TIME";

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
            String fileName = String.Format("log_{0}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}.txt", startTime.Year, startTime.Month,
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
                //return;
                logMessages.Dequeue();
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
                logMessages.Enqueue(msg);
            }
        }

        public void proveriOcitavanja(string fileName, out string msg)
        {
            msg = String.Empty;
            string failMsg = "   " + "FAIL" + "   ";
            string okMsg = "   " + "OK" + "   ";

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
                msg = Path.GetFileName(fileName) + failMsg + "(Lose formatiran fajl)";
                return;
            }

            string[] line = startTime.Split(' ');
            DateTime from = DateTime.Parse(line[2] + " " + line[3]);
            line = endTime.Split(' ');
            DateTime to = DateTime.Parse(line[2] + " " + line[3]);

            IList<DolazakNaTrening> dolasci = null;
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    CurrentSessionContext.Bind(session);
                    dolasci = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO().getDolazakNaTrening(from, to);
                }
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(ex.Message, "Citac kartica");
                return;
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }

            int brojOcitavanja = Int32.Parse(lines[beginOcitavanje].Trim().Split(' ')[0]);
            List<Ocitavanje> listaOcitavanja = new List<Ocitavanje>();
            for (int i = 1; i <= brojOcitavanja; ++i)
            {
                line = lines[beginOcitavanje + i].Split(' ');
                int brojKartice = Int32.Parse(line[0]);
                DateTime vremeOcitavanja = DateTime.Parse(line[1] + " " + line[2]);
                listaOcitavanja.Add(new Ocitavanje(brojKartice, vremeOcitavanja));
            }

            bool error = false;
            if (dolasci.Count != listaOcitavanja.Count)
            {
                msg += String.Format("(Razlicit broj ocitavanja. Baza: {0} Fajl: {1})", dolasci.Count, listaOcitavanja.Count);
                error = true;
            }
            else
            {
                for (int i = 0; i < dolasci.Count; ++i)
                {
                    DolazakNaTrening d = dolasci[i];
                    Ocitavanje o = listaOcitavanja[i];
                    if (d.Clan.BrojKartice != o.brojKartice)
                    {
                        msg += String.Format("(Broj kartice se ne poklapa. Baza: {0} Fajl: {1})",
                            d.Clan.BrojKartice, o.brojKartice);
                        error = true;
                        break;
                    }
                    else if (d.DatumVremeDolaska != o.vremeOcitavanja)
                    {
                        msg += String.Format("(Vreme dolaska se ne poklapa. Baza: {0} Fajl: {1})",
                            d.DatumVremeDolaska, o.vremeOcitavanja);
                        error = true;
                        break;
                    }
                }
            }
            if (error)
            {
                msg = Path.GetFileName(fileName) + failMsg + msg;
            }
            else
            {
                msg = Path.GetFileName(fileName) + okMsg + String.Format("(Broj ocitavanja: {0})", dolasci.Count);
            }
        }
    }
}
