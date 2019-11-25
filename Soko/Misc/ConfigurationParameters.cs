using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Soko
{
    static class ConfigurationParameters
    {
        public static string DatabaseFile
        {
            get { return "clanovi_podaci.sdf"; }
        }

        public static string Password
        {
            get { return "sdv"; }
        }

        public static string ConnectionString
        {
            get { return GetConnectionString(DatabaseFile, Password); }
        }

        public static string GetConnectionString(string fileName, string password)
        {
            // The DataSource must be surrounded with double quotes. The Password, on the other hand, must be surrounded 
            // with single quotes
            return string.Format("DataSource=\"{0}\"; Password='{1}'", fileName, password);
        }
    }
}
