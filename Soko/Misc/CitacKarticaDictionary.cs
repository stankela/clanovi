using Bilten.Dao;
using NHibernate;
using NHibernate.Context;
using Soko.Data;
using Soko.Domain;
using Soko.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soko.Misc
{
    public class CitacKarticaDictionary
    {
        private DateTime creationDate;
        public DateTime CreationDate
        {
            get { return creationDate; }
        }

        private IDictionary<int, Clan> clanoviSaKarticom;
        private IDictionary<int, List<UplataClanarine>> ovomesecneUplate;
        private IDictionary<int, List<UplataClanarine>> prethodneUplate;
        private IDictionary<int, UplataClanarine> uplateGodisnjaClanarina;

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

        }

        public void Init()
        {
            creationDate = DateTime.Now;
            try
            {
                using (ISession session = NHibernateHelper.Instance.OpenSession())
                using (session.BeginTransaction())
                {
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
            if (!clanoviSaKarticom.ContainsKey(clan.BrojKartice.Value))
            {
                clanoviSaKarticom.Add(clan.BrojKartice.Value, clan);
            }
        }

        public void DodajUplate(List<UplataClanarine> uplate)
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

        public void UpdateNeplacaClanarinu(int brojKartice, bool neplacaClanarinu)
        {
            Clan clan = findClan(brojKartice);
            if (clan != null)
            {
                clan.NeplacaClanarinu = neplacaClanarinu;
            }
        }
    }
}
