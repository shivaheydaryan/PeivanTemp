using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net;
using Microsoft.Owin;

namespace App_Start
{
    public class LogApiAttribute: System.Web.Http.Filters.ActionFilterAttribute
    {
        public bool Need { get; set; }
        public LogApiAttribute(bool need)
        {
            Need = need;
        }
        
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (Need)
            {
                
                
                string query = actionContext.Request.RequestUri.Query;

                var accessToken = HttpContext.Current;
                var TokenCode = accessToken.Request.Headers["Authorization"];
                if (TokenCode!=null)
                {
                    string t = TokenCode.Replace("Bearer ", "");
                    TokenCode = t;
                }
                

                


                //====================================================================
                //---------------------- Get Information For APILog ---------------------

                #region Get Information For Log
                string actionName = actionContext.ActionDescriptor.ActionName;
                string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var values = actionContext.ActionArguments.Values;

                string verb = Convert.ToString(actionContext.ControllerContext.Request.Method);
                string routevalue = "";

                string querrystring = string.Empty;


                for (int i = 0; i < values.Count(); i++)
                {
                    if (values.ElementAt(i) != null)
                    {
                        var item = HttpContext.Current.Request.QueryString.Keys[i];

                        string name = values.ElementAt(i).ToString();
                        string value = values.ElementAt(i).ToString();
                        routevalue = routevalue + item + "=" + value;
                    }

                    if (i + 1 < values.Count) routevalue = routevalue + "&";

                }

                string ip = GetClientIpAddress(actionContext.Request);

                string browser = GetBrowser(actionContext.Request);
                string OS = GetClientOS(actionContext.Request);

                string Description = null;
                

                var context = actionContext.RequestContext;




                #endregion Get Information For Log



                //---------------------- Get Information For Log ---------------------
                //====================================================================

                //====================================================================
                //----------------------- Insert Log to Tbl APILog ----------------------
                #region Insert Log to Tbl APILog
                string ConnectionString;
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();

                // 1. Instantiate the connection
                SqlConnection conn = new SqlConnection(ConnectionString);



                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO APILogs
                              (Controller, Action, IP, Date, OS, Browser, Description,TokenCode)
                VALUES        (@Controller, @Action, @IP, @Date, @OS, @Browser, @Description,@TokenCode)", conn);

                    cmd.Parameters.Add(new SqlParameter("@Controller", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Controller"].Value = controllerName;

                    cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Action"].Value = actionName + " - " + routevalue;

                    cmd.Parameters.Add(new SqlParameter("@IP", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@IP"].Value = ip;

                    cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.DateTime));
                    cmd.Parameters["@Date"].Value = DateTime.Now;
                    
                    cmd.Parameters.Add(new SqlParameter("@OS", SqlDbType.NVarChar, 50));
                    if (OS!=null)
                    {
                        cmd.Parameters["@OS"].Value = OS;
                    }
                    else
                    {
                        cmd.Parameters["@OS"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@Browser", SqlDbType.NVarChar, 50));
                    if (OS != null)
                    {
                        cmd.Parameters["@Browser"].Value = browser;
                    }
                    else
                    {
                        cmd.Parameters["@Browser"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar));
                    if (Description != null)
                    {
                        cmd.Parameters["@Description"].Value = Description;
                    }
                    else
                    {
                        cmd.Parameters["@Description"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@TokenCode", SqlDbType.NVarChar));
                    if (TokenCode != null)
                    {
                        cmd.Parameters["@TokenCode"].Value = TokenCode;
                    }
                    else
                    {
                        cmd.Parameters["@TokenCode"].Value = DBNull.Value;
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
            base.OnActionExecuting(actionContext);
        }


        private string GetClientIpAddress(HttpRequestMessage request)
        {
            
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress.Parse(((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress).ToString();
            }
            if (request.Properties.ContainsKey("MS_OwinContext"))
            {
                return IPAddress.Parse(((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress).ToString();
            }
            return String.Empty;
        }

        private string GetClientOS(HttpRequestMessage request)
        {
            return (((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserAgent).ToString();
        }

        private string GetBrowser(HttpRequestMessage request)
        {
            if (((HttpContextBase)request.Properties["MS_HttpContext"]).Request.Browser.IsMobileDevice)
            {
                return ((HttpContextBase)request.Properties["MS_HttpContext"]).Request.Browser.MobileDeviceManufacturer;
            }
            return String.Empty;
        }
    }
}