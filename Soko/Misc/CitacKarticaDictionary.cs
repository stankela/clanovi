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
    public class CitacKarticaDictionary
    {
        private DateTime initDate;
        public DateTime InitDate
        {
            get { return initDate; }
        }

        private IDictionary<int, Clan> clanoviSaKarticom;
        private IDictionary<int, List<UplataClanarine>> ovomesecneUplate;
        private IDictionary<int, List<UplataClanarine>> prethodneUplate;
        private IDictionary<int, UplataClanarine> uplateGodisnjaClanarina;

        public const string DODAJ_CLANA = "DodajClana";
        public const string DODAJ_UPLATE = "DodajUplate";
        public const string UPDATE_NEPLACA_CLANARINU = "UpdateNeplacaClanarinu";

        private static CitacKarticaDictionary instance;
        public static CitacKarticaDictionary Instance
        {
            get
            {
                if (instance == null)
                    instance = new CitacKarticaDictionary();
                return instance;
            }
        }

        protected CitacKarticaDictionary()
        {
            clanoviSaKarticom = new Dictionary<int, Clan>();
            ovomesecneUplate = new Dictionary<int, List<UplataClanarine>>();
            prethodneUplate = new Dictionary<int, List<UplataClanarine>>();
            uplateGodisnjaClanarina = new Dictionary<int, UplataClanarine>();
        }

        public void Init()
        {
            initDate = DateTime.Now;
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
                    // TODO3: Proveri da li ovde treba koristiti CurrentSessionContext ako nije JedinstenProgram
                    CurrentSessionContext.Bind(session);

                    clanoviSaKarticom = new Dictionary<int, Clan>();
                    foreach (Clan clan in DAOFactoryFactory.DAOFactory.GetClanDAO().findClanoviSaKarticom())
                    {
                        clanoviSaKarticom.Add(clan.BrojKartice.Value, clan);
                    }

                    GrupaDAO grupaDAO = DAOFactoryFactory.DAOFactory.GetGrupaDAO();
                    Grupa godisnjaClanarinaGrupa = grupaDAO.findGodisnjaClanarina();
                    int godisnjaClanarinaId = -1;
                    if (godisnjaClanarinaGrupa != null)
                    {
                        godisnjaClanarinaId = godisnjaClanarinaGrupa.Id;
                    }
                    else
                    {
                        MessageDialogs.showMessage("Ne mogu da pronadjem grupu za godisnju clanarinu", "Greska");
                    }

                    ovomesecneUplate = new Dictionary<int, List<UplataClanarine>>();
                    prethodneUplate = new Dictionary<int, List<UplataClanarine>>();
                    DateTime now = DateTime.Now;
                    DateTime from = now.AddMonths(-6);
                    DateTime to = now;
                    UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
                    foreach (UplataClanarine u in uplataClanarineDAO.findUplateVaziOd(from, to))
                    {
                        if (u.Grupa.Id == godisnjaClanarinaId)
                        {
                            continue;
                        }
                        if (u.VaziOd.Value.Month == now.Month && u.VaziOd.Value.Year == now.Year)
                        {
                            if (ovomesecneUplate.ContainsKey(u.Clan.Id))
                            {
                                ovomesecneUplate[u.Clan.Id].Add(u);
                            }
                            else
                            {
                                List<UplataClanarine> uplate = new List<UplataClanarine>();
                                uplate.Add(u);
                                ovomesecneUplate.Add(u.Clan.Id, uplate);
                            }
                        }
                        else
                        {
                            if (prethodneUplate.ContainsKey(u.Clan.Id))
                            {
                                prethodneUplate[u.Clan.Id].Add(u);
                            }
                            else
                            {
                                List<UplataClanarine> uplate = new List<UplataClanarine>();
                                uplate.Add(u);
                                prethodneUplate.Add(u.Clan.Id, uplate);
                            }
                        }
                    }

                    uplateGodisnjaClanarina = new Dictionary<int, UplataClanarine>();
                    if (godisnjaClanarinaGrupa != null)
                    {
                        DateTime firstDateTimeInYear = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
                        DateTime lastDateTimeInYear = new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0).AddSeconds(-1);
                        foreach (UplataClanarine u in uplataClanarineDAO.findUplate(godisnjaClanarinaGrupa,
                            firstDateTimeInYear, lastDateTimeInYear))
                        {
                            if (!uplateGodisnjaClanarina.ContainsKey(u.Clan.Id))
                            {
                                uplateGodisnjaClanarina.Add(u.Clan.Id, u);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialogs.showMessage(ex.Message, "Citac kartica");
            }
            finally
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
            }
        }

        public Clan findClan(int brojKartice)
        {
            if (clanoviSaKarticom.ContainsKey(brojKartice))
            {
                return clanoviSaKarticom[brojKartice];
            }
            return null;
        }

        public UplataClanarine findUplata(Clan clan)
        {
            if (uplateGodisnjaClanarina.ContainsKey(clan.Id))
            {
                return uplateGodisnjaClanarina[clan.Id];
            }
            if (ovomesecneUplate.ContainsKey(clan.Id))
            {
                return ovomesecneUplate[clan.Id][0];
            }
            if (prethodneUplate.ContainsKey(clan.Id))
            {
                List<UplataClanarine> uplate = prethodneUplate[clan.Id];
                Util.sortByVaziOdDesc(uplate);
                return uplate[0];
            }
            return null;
        }

        public void DodajClanaSaKarticom(Clan clan)
        {
            if (Options.Instance.JedinstvenProgram || !Options.Instance.IsProgramZaClanarinu)
            {
                if (!clanoviSaKarticom.ContainsKey(clan.BrojKartice.Value))
                {
                    clanoviSaKarticom.Add(clan.BrojKartice.Value, clan);
                }
            }
            else
            {
                try
                {
                    Form1.Instance.pipeServerStreamWriter.AutoFlush = true;
                    Form1.Instance.pipeServerStreamWriter.WriteLine(DODAJ_CLANA + " " + clan.Id);
                }
                catch (Exception e)
                {
                    // Exception is raised if the pipe is broken  or disconnected.
                    MessageDialogs.showMessage(e.Message, Form1.Instance.Text);
                }
            }
        }

        public void DodajUplate(List<UplataClanarine> uplate)
        {
            if (Options.Instance.JedinstvenProgram || !Options.Instance.IsProgramZaClanarinu)
            {
                foreach (UplataClanarine u in uplate)
                {
                    if (Util.isGodisnjaClanarina(u.Grupa.Naziv) && u.VaziOd.Value.Year == DateTime.Now.Year)
                    {
                        if (!uplateGodisnjaClanarina.ContainsKey(u.Clan.Id))
                        {
                            uplateGodisnjaClanarina.Add(u.Clan.Id, u);
                        }
                    }
                    else if (u.VaziOd.Value.Month == DateTime.Now.Month && u.VaziOd.Value.Year == DateTime.Now.Year)
                    {
                        if (ovomesecneUplate.ContainsKey(u.Clan.Id))
                        {
                            ovomesecneUplate[u.Clan.Id].Add(u);
                        }
                        else
                        {
                            List<UplataClanarine> uplate2 = new List<UplataClanarine>();
                            uplate2.Add(u);
                            ovomesecneUplate.Add(u.Clan.Id, uplate2);
                        }
                    }
                    else
                    {
                        if (prethodneUplate.ContainsKey(u.Clan.Id))
                        {
                            prethodneUplate[u.Clan.Id].Add(u);
                        }
                        else
                        {
                            List<UplataClanarine> uplate2 = new List<UplataClanarine>();
                            uplate2.Add(u);
                            prethodneUplate.Add(u.Clan.Id, uplate2);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    string uplateStr = String.Empty;
                    foreach (UplataClanarine u in uplate)
                    {
                        uplateStr += " " + u.Id.ToString();
                    }
                    Form1.Instance.pipeServerStreamWriter.AutoFlush = true;
                    Form1.Instance.pipeServerStreamWriter.WriteLine(DODAJ_UPLATE + uplateStr);
                }
                catch (Exception e)
                {
                    // Exception is raised if the pipe is broken  or disconnected.
                    MessageDialogs.showMessage(e.Message, Form1.Instance.Text);
                }
            }
        }

        public void UpdateNeplacaClanarinu(int brojKartice, bool neplacaClanarinu)
        {
            if (Options.Instance.JedinstvenProgram || !Options.Instance.IsProgramZaClanarinu)
            {
                Clan clan = findClan(brojKartice);
                if (clan != null)
                {
                    clan.NeplacaClanarinu = neplacaClanarinu;
                }
            }
            else
            {
                try
                {
                    Form1.Instance.pipeServerStreamWriter.AutoFlush = true;
                    Form1.Instance.pipeServerStreamWriter.WriteLine(UPDATE_NEPLACA_CLANARINU
                        + " " + brojKartice.ToString() + " " + neplacaClanarinu.ToString());
                }
                catch (Exception e)
                {
                    // Exception is raised if the pipe is broken  or disconnected.
                    MessageDialogs.showMessage(e.Message, Form1.Instance.Text);
                }
            }
        }
    }
}
