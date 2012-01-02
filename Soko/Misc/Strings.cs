using System;

namespace Soko
{
    public class Strings
    {
        public static readonly string DatabaseAccessExceptionMessage =
            "Greska prilikom pristupa bazi podataka.";

        public static string getFullDatabaseAccessExceptionMessage(string exceptionMsg)
        {
            return String.Format(
                "{0} \n\n{1}", Strings.DatabaseAccessExceptionMessage, exceptionMsg);
        }
    }
}
