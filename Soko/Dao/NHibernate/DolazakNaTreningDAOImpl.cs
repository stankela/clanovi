using System;
using System.Collections.Generic;
using NHibernate;
using Soko.Exceptions;
using Soko.Domain;
using Soko;
using System.Collections;
using Soko.Report;

namespace Bilten.Dao.NHibernate
{
    /// <summary>
    /// NHibernate-specific implementation of <see cref="DolazakNaTreningDAO"/>.
    /// </summary>
    public class DolazakNaTreningDAOImpl : GenericNHibernateDAO<DolazakNaTrening, int>, DolazakNaTreningDAO
    {

    }
}