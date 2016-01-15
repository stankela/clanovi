﻿using System;
using System.Collections.Generic;
using System.Text;
using Soko.Domain;
using Soko.Report;

namespace Bilten.Dao
{
    /// <summary>
    /// Business DAO operations related to the <see cref="Domain.DolazakNaTrening"/> entity.
    /// </summary>
    public interface DolazakNaTreningDAO : GenericDAO<DolazakNaTrening, int>
    {
        List<object[]> getEvidencijaTreningaReportItems(DateTime from, DateTime to, List<Grupa> grupe);
        List<ReportGrupa> getEvidencijaTreningaReportGrupe(DateTime from, DateTime to, List<Grupa> grupe);
    }
}