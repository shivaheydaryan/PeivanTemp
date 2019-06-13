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
using System.Web.Http;

namespace App_Start
{
    public class TokenAttribute: System.Web.Http.Filters.ActionFilterAttribute
    {
        private string ConnectionString;
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var currentcontext = HttpContext.Current;
            var token = currentcontext.Request.Headers["Authorization"];
            if (token!=null)
            {
                string t = token.Replace("Bearer ", "");
                token = t;
            }
            

            string ip = GetClientIpAddress(actionContext.Request);

            bool valid=false;

            if (token != null)
            {
                Models.Token Token = new Models.Token();

                //=============================================================
                //-------------- Get Token From DataBase -----
                #region Get Token From DataBase
                ConnectionString constr = new ConnectionString();
                ConnectionString = constr.GetConnectionString();

                // 1. Instantiate the connection
                SqlConnection conn = new SqlConnection(ConnectionString);

                SqlDataReader rdr = null;

                try
                {
                    if (conn.State==ConnectionState.Closed)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"select * from Token Where TokenCode  like @TokenCode and Valid =1 ", conn);

                    cmd.Parameters.Add(new SqlParameter("@TokenCode", SqlDbType.NVarChar));
                    cmd.Parameters["@TokenCode"].Value = token;

                    //cmd.Parameters.Add(new SqlParameter("@IP", SqlDbType.NVarChar,50));
                    //cmd.Parameters["@IP"].Value = ip;

                    rdr = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);
                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow dr = dataTable.Rows[0];

                            Token.Date = dr["Date"].ToString() != "" ? DateTime.Parse(dr["Date"].ToString()) :(DateTime?) null;
                            Token.id = Int64.Parse(dr["id"].ToString());
                            Token.TokenCode = dr["TokenCode"].ToString();


                            //=================================================
                            //----------------- Check Token Is Valid --
                            #region Check Token Is Valid

                            DateTime TokenDate =DateTime.Parse(Token.Date.ToString()) ;

                            DateTime Tommorow = DateTime.Now;

                            DateTime OneDayafterTokenDate = TokenDate.AddDays(1);

                            if (OneDayafterTokenDate> Tommorow)
                            {
                                valid = true;

                            }
                            else
                            {
                                //----- token Expired
                                cmd = new SqlCommand(@"update Token set Valid=0 where TokenCode  like @TokenCode", conn);

                                cmd.Parameters.Add(new SqlParameter("@TokenCode", SqlDbType.NVarChar));
                                cmd.Parameters["@TokenCode"].Value = token;

                                cmd.ExecuteNonQuery();
                            }

                            #endregion
                            //----------------- Check Token Is Valid --
                            //=================================================

                        }
                    }

                    if (rdr != null)
                    {
                        rdr.Close();
                        rdr = null;
                    }
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();


                }
                catch(Exception ex)
                {
                    if (rdr!=null)
                    {
                        rdr.Close();
                        rdr = null;
                    }
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                }
                #endregion
                //-------------- Get Token From DataBase -----
                //=============================================================
            }

            if (!valid)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Request Is Not Valid!!!" };
                throw new HttpResponseException(msg);
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
    }
}