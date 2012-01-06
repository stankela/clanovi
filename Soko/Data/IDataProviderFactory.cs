using System;

namespace Soko.Data
{
	/// <summary>
	/// Summary description for IDataAccessProviderFactory.
	/// </summary>
	public interface IDataProviderFactory
	{
        IDataContext GetDataContext();
	}
}
