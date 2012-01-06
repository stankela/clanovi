using Soko.Data;

namespace Soko.Data.NHibernate
{
    public class DataProviderFactory : IDataProviderFactory
	{
        public IDataContext GetDataContext()
        {
            return new NHibernateDataContext();
        }
    }
}
