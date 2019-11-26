using Soko;
using Bilten.Dao;
using Soko.Data;
using Soko.Domain;
using Soko.Exceptions;
using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows.Forms;
using Bilten.Dao.NHibernate;
using Iesi.Collections;

public class VersionUpdater
{
    public static bool hasUpdates()
    {
        int verzijaBaze = SqlCeUtilities.getDatabaseVersionNumber();
        return Program.VERZIJA_PROGRAMA > verzijaBaze;
    }

    public void update()
    {
        int verzijaBaze = SqlCeUtilities.getDatabaseVersionNumber();
        if (verzijaBaze == Program.VERZIJA_PROGRAMA)
            return;

        if (verzijaBaze > Program.VERZIJA_PROGRAMA)
            throw new Exception("Greska u programu. Verzija baze je veca od verzije programa.");

        int staraVerzijaBaze = verzijaBaze;
        bool converted = false;

        if (verzijaBaze == -1 /*baza ne postoji*/)
        {           
            SqlCeUtilities.ExecuteScript(ConfigurationParameters.DatabaseFile, ConfigurationParameters.Password,
                "Soko.Update.DatabaseUpdate_version1.txt", true);
            SqlCeUtilities.updateDatabaseVersionNumber(1);
            verzijaBaze = 1;
            converted = true;
        }

        if (verzijaBaze == 1 && Program.VERZIJA_PROGRAMA > 1)
        {
            SqlCeUtilities.ExecuteScript(ConfigurationParameters.DatabaseFile, ConfigurationParameters.Password,
                "Soko.Update.DatabaseUpdate_version2.txt", true);
            SqlCeUtilities.updateDatabaseVersionNumber(2);
            UpdateDolazakNaTreningMesecni();
            verzijaBaze = 2;
            converted = true;
        }

        if (verzijaBaze == 2 && Program.VERZIJA_PROGRAMA > 2)
        {
            SqlCeUtilities.updateDatabaseVersionNumber(3);
            UpdateDolazakNaTreningMesecniJedanDnevniTrening();
            verzijaBaze = 3;
            converted = true;
        }

        if (converted)
        {
            string msg = String.Format("Baza podataka je konvertovana iz verzije {0} u verziju {1}.", staraVerzijaBaze,
                verzijaBaze);
            MessageBox.Show(msg, "Bilten");

            if (File.Exists("NHibernateConfig"))
                File.Delete("NHibernateConfig");
        }
    }

    private void UpdateDolazakNaTreningMesecni()
    {
        ISession session = null;
        try
        {
            using (session = NHibernateHelper.Instance.OpenSession())
            using (session.BeginTransaction())
            {
                CurrentSessionContext.Bind(session);
                DolazakNaTreningMesecniDAO dolazakNaTreningMesecniDAO
                    = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningMesecniDAO();
                List<DolazakNaTreningMesecni> dolasciMesecni
                    = new List<DolazakNaTreningMesecni>(dolazakNaTreningMesecniDAO.FindAll());
                if (dolasciMesecni.Count > 0)
                    return;

                DolazakNaTreningDAO dolazakNaTreningDAO = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO();
                IList<DolazakNaTrening> dolasci = dolazakNaTreningDAO.FindAll();
                IDictionary<ClanGodinaMesec, DolazakNaTreningMesecni> dolasciMap
                    = new Dictionary<ClanGodinaMesec, DolazakNaTreningMesecni>();
                foreach (DolazakNaTrening d in dolasci)
                {
                    ClanGodinaMesec key = new ClanGodinaMesec(d.Clan.Id, d.DatumDolaska.Value.Year,
                        d.DatumDolaska.Value.Month);
                    if (!dolasciMap.ContainsKey(key))
                    {
                        DolazakNaTreningMesecni dolazak = new DolazakNaTreningMesecni();
                        dolazak.Clan = d.Clan;
                        dolazak.Godina = d.DatumDolaska.Value.Year;
                        dolazak.Mesec = d.DatumDolaska.Value.Month;
                        dolazak.BrojDolazaka = 1;
                        dolasciMap.Add(key, dolazak);
                    }
                    else
                    {
                        ++(dolasciMap[key].BrojDolazaka);
                    }
                }
                foreach (KeyValuePair<ClanGodinaMesec, DolazakNaTreningMesecni> entry in dolasciMap)
                {
                    dolazakNaTreningMesecniDAO.MakePersistent(entry.Value);
                }
                session.Transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            if (session != null && session.Transaction != null && session.Transaction.IsActive)
                session.Transaction.Rollback();
            throw new InfrastructureException(ex.Message, ex);
        }
        finally
        {
            CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
        }
    }

    private IDictionary<ClanGodinaMesec, ISet> getDolasciMapJedanDnevniTrening()
    {
        ISession session = null;
        try
        {
            using (session = NHibernateHelper.Instance.OpenSession())
            using (session.BeginTransaction())
            {
                CurrentSessionContext.Bind(session);
                IDictionary<ClanGodinaMesec, ISet> dolasciMap = new Dictionary<ClanGodinaMesec, ISet>();
                foreach (DolazakNaTrening d in DAOFactoryFactory.DAOFactory.GetDolazakNaTreningDAO().FindAll())
                {
                    ClanGodinaMesec key = new ClanGodinaMesec(d.Clan.Id, d.DatumDolaska.Value.Year,
                        d.DatumDolaska.Value.Month);
                    if (!dolasciMap.ContainsKey(key))
                    {
                        ISet daniSet = new HashedSet();
                        daniSet.Add(d.DatumDolaska.Value.Day);
                        dolasciMap.Add(key, daniSet);
                    }
                    else
                    {
                        ISet daniSet = dolasciMap[key];
                        if (!daniSet.Contains(d.DatumDolaska.Value.Day))
                        {
                            daniSet.Add(d.DatumDolaska.Value.Day);
                        }
                    }
                }
                return dolasciMap;
            }
        }
        catch (Exception ex)
        {
            if (session != null && session.Transaction != null && session.Transaction.IsActive)
                session.Transaction.Rollback();
            throw new InfrastructureException(ex.Message, ex);
        }
        finally
        {
            CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
        }
    }

    private void UpdateDolazakNaTreningMesecniJedanDnevniTrening()
    {
        IDictionary<ClanGodinaMesec, ISet> dolasciMap = getDolasciMapJedanDnevniTrening();
        ISession session = null;
        try
        {
            using (session = NHibernateHelper.Instance.OpenSession())
            using (session.BeginTransaction())
            {
                CurrentSessionContext.Bind(session);
                DolazakNaTreningMesecniDAO dolazakNaTreningMesecniDAO
                    = DAOFactoryFactory.DAOFactory.GetDolazakNaTreningMesecniDAO();

                IDictionary<ClanGodinaMesec, DolazakNaTreningMesecni> dolasciMesecniMap
                    = new Dictionary<ClanGodinaMesec, DolazakNaTreningMesecni>();
                foreach (DolazakNaTreningMesecni d in dolazakNaTreningMesecniDAO.FindAll())
                {
                    ClanGodinaMesec key = new ClanGodinaMesec(d.Clan.Id, d.Godina, d.Mesec);
                    if (!dolasciMesecniMap.ContainsKey(key))
                    {
                        dolasciMesecniMap.Add(key, d);
                    }
                    else
                    {
                        throw new Exception("Greska prilikom azuriranja programa");
                    }
                }
                foreach (KeyValuePair<ClanGodinaMesec, ISet> entry in dolasciMap)
                {
                    if (!dolasciMesecniMap.ContainsKey(entry.Key))
                    {
                        throw new Exception("Greska prilikom azuriranja programa");
                    }
                    DolazakNaTreningMesecni dolazakMesecni = dolasciMesecniMap[entry.Key];
                    dolazakMesecni.BrojDolazaka = entry.Value.Count;
                    dolazakNaTreningMesecniDAO.MakePersistent(dolazakMesecni);
                }
                session.Transaction.Commit();
            }
        }
        catch (Exception ex)
        {
            if (session != null && session.Transaction != null && session.Transaction.IsActive)
                session.Transaction.Rollback();
            throw new InfrastructureException(ex.Message, ex);
        }
        finally
        {
            CurrentSessionContext.Unbind(NHibernateHelper.Instance.SessionFactory);
        }
    }
}
