﻿using Bilten.Dao;
using Iesi.Collections;
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
        private IDictionary<int, List<UplataClanarine>> prethodneUplate;
        private IDictionary<int, UplataClanarine> uplateGodisnjaClanarina;
        private IDictionary<int, UplataClanarine> uplateGodisnjaClanarinaPrethGod;
        private ISet danasnjaOcitavanja;

        public ISet DanasnjaOcitavanja
        {
            get { return danasnjaOcitavanja; }
        }

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
            prethodneUplate = new Dictionary<int, List<UplataClanarine>>();
            uplateGodisnjaClanarina = new Dictionary<int, UplataClanarine>();
            uplateGodisnjaClanarinaPrethGod = new Dictionary<int, UplataClanarine>();
            danasnjaOcitavanja = new HashedSet();
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
                    IList<Grupa> godisnjaClanarinaGrupe = grupaDAO.findGodisnjaClanarina();
                    if (godisnjaClanarinaGrupe.Count == 0)
                    {
                        // TODO3: Da li je potrebna ova provera? Ako se grupe sa godisnjom clanarinom zadaju
                        // u programu, trebalo bi da je dozvoljeno da ne bude zadata nijedna grupa.
                        MessageDialogs.showMessage("Ne mogu da pronadjem grupu za godisnju clanarinu", "Greska");
                    }

                    prethodneUplate = new Dictionary<int, List<UplataClanarine>>();
                    DateTime now = DateTime.Now;
                    DateTime from = now.AddMonths(-6);

                    // Kao krajnji datum za uplate se uzima sledeci mesec zbog sledece situacije: Dolazi potpuno novi clan
                    // (prvi put se upisuje u sokolsko drustvo), i placa clanarinu za sledeci mesec (da pocinje od
                    // sledeceg meseca da vezba). Tada je jedina uplate koja postoji uplata za sledeci mesec (i treba
                    // da svetli zeleno, i da se grupa za tu uplatu prikazije na ekranu, i da se ta grupa veze za
                    // DolazakNaTrening).
                    DateTime sledeciMesec = now.AddMonths(1);

                    UplataClanarineDAO uplataClanarineDAO = DAOFactoryFactory.DAOFactory.GetUplataClanarineDAO();
                    foreach (UplataClanarine u in uplataClanarineDAO.findUplateVaziOd(from, sledeciMesec))
                    {
                        foreach (Grupa g in godisnjaClanarinaGrupe)
                        {
                            if (g.Id == u.Grupa.Id)
                            {
                                continue;
                            }
                        }
                        // Ako ne postoji uplata za ovaj mesec ali postoji uplata za sledeci mesec, ta uplata ce biti
                        // stavljena u prethodneUplate. Ta uplata ce biti izabrana u metodu findUplata zato sto
                        // findUplata sortira prethodne uplate opadajuce po datumu vazenja.
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

                    uplateGodisnjaClanarina = new Dictionary<int, UplataClanarine>();
                    if (godisnjaClanarinaGrupe.Count > 0)
                    {
                        DateTime firstDateTimeInYear = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
                        DateTime lastDateTimeInYear = new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0).AddSeconds(-1);
                        foreach (UplataClanarine u in uplataClanarineDAO.findUplate(godisnjaClanarinaGrupe,
                            firstDateTimeInYear, lastDateTimeInYear))
                        {
                            if (!uplateGodisnjaClanarina.ContainsKey(u.Clan.Id))
                            {
                                uplateGodisnjaClanarina.Add(u.Clan.Id, u);
                            }
                        }
                    }

                    uplateGodisnjaClanarinaPrethGod = new Dictionary<int, UplataClanarine>();
                    if (godisnjaClanarinaGrupe.Count > 0)
                    {
                        DateTime firstDateTimeInYear = new DateTime(DateTime.Now.Year  - 1, 1, 1, 0, 0, 0);
                        DateTime lastDateTimeInYear = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0).AddSeconds(-1);
                        foreach (UplataClanarine u in uplataClanarineDAO.findUplate(godisnjaClanarinaGrupe,
                            firstDateTimeInYear, lastDateTimeInYear))
                        {
                            if (!uplateGodisnjaClanarinaPrethGod.ContainsKey(u.Clan.Id))
                            {
                                uplateGodisnjaClanarinaPrethGod.Add(u.Clan.Id, u);
                            }
                        }
                    }

                    DateTime pocetakDana = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    DateTime sutra = pocetakDana.AddDays(1);
                    DateTime krajDana = new DateTime(sutra.Year, sutra.Month, sutra.Day, 0, 0, 0).AddSeconds(-1);
                    DolazakNaTreningDAO dolazakDAO = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO();
                    IList<DolazakNaTrening> danasnjiDolasci =
                        DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO().getDolazakNaTrening(pocetakDana, krajDana);
                    danasnjaOcitavanja = new HashedSet();
                    foreach (DolazakNaTrening d in danasnjiDolasci)
                    {
                        danasnjaOcitavanja.Add(d.Clan.Id);
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
            // TODO3: Treba razresiti situaciju kada postoji godisnja uplata za prethodnu godinu i obicna uplata za neki
            // mesec ove godine, tj. koju uplatu vratiti u tom slucaju, tj. da li tretirati clana kao regularnog clana
            // ili clana sa godisnjom clanarinom. Trenutno prednost ima godisnja clanarina.
            if (uplateGodisnjaClanarinaPrethGod.ContainsKey(clan.Id))
            {
                return uplateGodisnjaClanarinaPrethGod[clan.Id];
            }
            if (prethodneUplate.ContainsKey(clan.Id))
            {
                List<UplataClanarine> uplate = prethodneUplate[clan.Id];
                Util.sortByVaziOdDesc(uplate);
                // Ako ne postoji uplata za ovaj mesec ali postoji uplata za sledeci mesec, ta uplata ce biti u uplate[0].
                // TODO3: Ako postoje uplate i za ovaj i za sledeci mesec, izaberi uplatu za ovaj mesec.
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
                Form1.Instance.sendToPipeClient(DODAJ_CLANA + " " + clan.Id);
            }
        }

        public void DodajUplate(IList<UplataClanarine> uplate)
        {
            if (Options.Instance.JedinstvenProgram || !Options.Instance.IsProgramZaClanarinu)
            {
                DateTime sledeciMesec = DateTime.Now.AddMonths(1);
                foreach (UplataClanarine u in uplate)
                {

                    if (u.Grupa.ImaGodisnjuClanarinu && u.VaziOd.Value.Year == DateTime.Now.Year)
                    {
                        if (!uplateGodisnjaClanarina.ContainsKey(u.Clan.Id))
                        {
                            uplateGodisnjaClanarina.Add(u.Clan.Id, u);
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
                string uplateStr = String.Empty;
                foreach (UplataClanarine u in uplate)
                {
                    uplateStr += " " + u.Id.ToString();
                }
                Form1.Instance.sendToPipeClient(DODAJ_UPLATE + uplateStr);
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
                Form1.Instance.sendToPipeClient(UPDATE_NEPLACA_CLANARINU
                    + " " + brojKartice.ToString() + " " + neplacaClanarinu.ToString());
            }
        }
    }
}
