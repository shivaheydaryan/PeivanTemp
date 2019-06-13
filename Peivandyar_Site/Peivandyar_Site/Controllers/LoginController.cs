using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Peivandyar_Site.Controllers
{
    public class LoginController : Controller
    {
        private string ConnectionString;
        // GET: Login
        [Route("Login")]
        public ActionResult Index(string username, string password)
        {
            if (username!=null && password!=null)
            {
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;


                try
                {
                    //=====================================
                    //------------ Get User Info --------
                    User user = new User();
                    #region Get User Info
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = new SqlCommand(@"select username,firstname,lastname,gender 
                                        from Users where username like @username and password like @password and (Active is null or Active =1 )
                                ", conn);

                        cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                        cmd.Parameters["@username"].Value = username;

                        cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar));
                        cmd.Parameters["@password"].Value = password;

                        rdr = cmd.ExecuteReader();

                        DataTable dataTable = new DataTable();

                        dataTable.Load(rdr);

                        if (dataTable != null)
                        {
                            if (dataTable.Rows.Count > 0)
                            {
                                
                                DataRow dr = dataTable.Rows[0];

                                user.username = dr["username"].ToString();
                                user.firstname = dr["firstname"].ToString();
                                user.lastname = dr["lastname"].ToString();
                                user.gender = dr["gender"].ToString() != "" ? bool.Parse(dr["gender"].ToString()) :(bool?) null;

                                Session["User"] = user;
                                dataTable.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        if (rdr != null)
                        {
                            rdr.Close();
                            rdr = null;
                        }
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Dispose();
                            conn.Close();
                        }

                    }
                    #endregion
                    //------------ Get User Info --------
                    //=====================================

                    
                    if (user.username != null)
                    {
                        
                        //=====================================
                        //------------ get user access --------
                        #region Get User Access
                        try
                        {
                            if (conn.State != ConnectionState.Open)
                                conn.Open();

                            SqlCommand cmd = new SqlCommand(@"select Accesses.caption,Accesses.Instituteid 
		                                from UserAccesses 
	                                inner join Accesses ON
		                                UserAccesses.Username like @username
		                                and Accesses.id=UserAccesses.id 
		                                and (Accesses.Active is null or Accesses.Active=1)
                                ", conn);

                            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                            cmd.Parameters["@username"].Value = username;

                            rdr = cmd.ExecuteReader();

                            DataTable dataTable = new DataTable();

                            dataTable.Load(rdr);

                            if (dataTable != null)
                            {
                                if (dataTable.Rows.Count > 0)
                                {
                                    List<Access> accesses = new List<Access>();
                                    accesses = (from DataRow dr in dataTable.Rows
                                                     select new Access()
                                                     {
                                                         caption = dr["caption"].ToString(),
                                                         Instituteid =int.Parse( dr["Instituteid"].ToString())
                                                     }
                                  ).ToList();
                                    Session["Access"] = accesses;
                                    dataTable.Dispose();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr = null;
                            }
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Dispose();
                                conn.Close();
                            }

                        }
                        #endregion
                        //------------ get user access --------
                        //=====================================

                        //=====================================
                        //------------ get user job -----------
                        #region User Jobs

                        try
                        {
                            if (conn.State != ConnectionState.Open)
                                conn.Open();

                            SqlCommand cmd = new SqlCommand(@"select Jobs.id,Jobs.Caption,User_Jobs.Instituteid from User_Jobs 
	                                        inner join Jobs ON
		                                        User_Jobs.Username like @Username
		                                        and Jobs.id = User_Jobs.Jobid
		                                        and (Jobs.Active is null or Jobs.Active =1)
                                ", conn);

                            cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 50));
                            cmd.Parameters["@Username"].Value = username;

                            rdr = cmd.ExecuteReader();

                            DataTable dataTable = new DataTable();

                            dataTable.Load(rdr);

                            if (dataTable != null)
                            {
                                if (dataTable.Rows.Count > 0)
                                {
                                    List<ViewModel.User_Jobs_VM> jobs = new List<ViewModel.User_Jobs_VM>();
                                    jobs = (from DataRow dr in dataTable.Rows
                                                select new ViewModel.User_Jobs_VM()
                                                {
                                                    id=Int64.Parse(dr["id"].ToString()),
                                                    Caption = dr["Caption"].ToString(),
                                                    Instituteid = Int64.Parse(dr["Instituteid"].ToString())
                                                }
                                  ).ToList();
                                    Session["Job"] = jobs;
                                    dataTable.Dispose();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (rdr != null)
                            {
                                rdr.Close();
                                rdr = null;
                            }
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Dispose();
                                conn.Close();
                            }

                        }

                        #endregion
                        //------------ get user job -----------
                        //=====================================


                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.result = "نام کاربری یا رمز عبور صحیح نیست .";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.result = "سیستم با خطا مواجه شد .";
                    
                }
            }
            return View("Index",null);
        }


        public ActionResult Exit()
        {
            Session.Remove("User");
            Session.Remove("Access");
            Session.Remove("Job");

            Session.Abandon();

            return RedirectToAction("Index","Home");
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

        }

    }

    
}