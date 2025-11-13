using System;
using System.Configuration;

namespace DVLD_DataAccessLayer
{
    internal class clsDataAccessSettings
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
    }
}
