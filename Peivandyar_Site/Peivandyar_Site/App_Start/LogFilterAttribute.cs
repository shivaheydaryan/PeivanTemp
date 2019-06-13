using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using App_Start;
using System.Data;
using System.Data.SqlClient;

namespace Models
{
    public class LogFilterAttribute:ActionFilterAttribute
    {
        
        public bool Mandatory { get; set; }

        public  LogFilterAttribute(bool mandatory)
        {
            Mandatory = mandatory;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Mandatory)
            {
                //====================================================================
                //---------------------- Get Information For Log ---------------------
                #region Get Information For Log
                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var values = filterContext.ActionParameters.Values;
                string verb = Convert.ToString(filterContext.HttpContext.Request.HttpMethod);
                string routevalue = "";
                for (int i = 0; i < values.Count(); i++)
                {
                    if (values.ElementAt(i) != null)
                    {
                        string name = values.ElementAt(i).ToString();
                        string value = values.ElementAt(i).ToString();
                        routevalue = routevalue + name + "=" + value;
                    }

                    if (i + 1 < values.Count) routevalue = routevalue + "&";

                }

                string ip = filterContext.HttpContext.Request.UserHostAddress;
                string browser = filterContext.HttpContext.Request.Browser.Browser;
                string OS = filterContext.HttpContext.Request.Browser.Platform;

                string Description = null;

                
                string Username = null;
                
                if (HttpContext.Current.Session["User"] != null)
                {
                    User userinfo = (Models.User)HttpContext.Current.Session["User"];
                    Username = userinfo.username;
                    
                }
                
                #endregion Get Information For Log
                //---------------------- Get Information For Log ---------------------
                //====================================================================

                //====================================================================
                //----------------------- Insert Log to Tbl Log ----------------------
                #region Insert Log to Tbl Log
                string ConnectionString;
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();

                // 1. Instantiate the connection
                SqlConnection conn = new SqlConnection(ConnectionString);



                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Logs
                         (Controller, Action, IP, Date,  Username, OS, Browser, Description)
           VALUES        (@Controller, @Action, @IP, @Date, @Username, @OS, @Browser, @Description)", conn);

                    cmd.Parameters.Add(new SqlParameter("@Controller", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Controller"].Value = controllerName;

                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Action"].Value = actionName + " - " + routevalue;

                    cmd.Parameters.Add(new SqlParameter("@IP", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@IP"].Value = ip;

                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime));
                    cmd.Parameters["@Date"].Value = DateTime.Now;

                    cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 50));
                    if (Username != null)
                    {
                        cmd.Parameters["@Username"].Value = Username;
                    }
                    else
                    {
                        cmd.Parameters["@Username"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@OS", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@OS"].Value = OS;

                    cmd.Parameters.Add(new SqlParameter("@Browser", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Browser"].Value = browser;

                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar));
                    if (Description != null)
                    {
                        cmd.Parameters["@Description"].Value = Description;
                    }
                    else
                    {
                        cmd.Parameters["@Description"].Value = DBNull.Value;
                    }



                    cmd.ExecuteNonQuery();


                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                }
                #endregion Insert Log to Tbl Log
                //----------------------- Insert Log to Tbl Log ----------------------
                //====================================================================

            }

            base.OnActionExecuting(filterContext);
        }


        
    }
}