using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace App_Start
{
    public class ConnectionString
    {
        public String GetConnectionString()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings["DataContext"];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");
            string conString = mySetting.ConnectionString;
            return conString;
        }
    }
}