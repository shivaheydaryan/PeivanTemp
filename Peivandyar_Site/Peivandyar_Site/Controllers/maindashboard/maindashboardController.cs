using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using global::Models;
using App_Start;
using System.Data.SqlClient;
using ViewModel;
using System.Data;

namespace Peivandyar_Site.Controllers
{
    [Authentication]
    [App_Start.Authorize(job:new string[] { "Manager","Teacher"} )]
    
    [LogFilter(mandatory:true)]
    public class maindashboardController : Controller
    {
        
        private string ConnectionString;

        [Access(new string[] { "Dashboard" }, redirect: true)]
        // GET: maindashboard
        [Route("Dashboard")]
        public ActionResult Index()
        {
            return View();
        }


        //========================================================================
        //------------------- Get Message On Header page Dashboard ----------
        [Access(new string[] { "Message" },redirect: false)]
        [LogFilter(mandatory: false)]
        [Route("Dashboard/MSGList")]
        [HttpGet]

        public PartialViewResult get_list_msg()
        {

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            //======================================
            //-------------- Get Session ---
            #region Get Session
            User userinfo = new User();
            if (Session["User"] != null)
            {
                userinfo = (User)Session["User"];
            }
            else
            {
                userinfo = null;
            }
            #endregion
            //-------------- Get Session ---
            //======================================

            //=================================================
            //--------- Get Unread Msg ---
            List<MessageListVM> UserMsgList = new List<MessageListVM>();
            if (userinfo != null)
            {
                #region Get Unread Msg
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SP_MSGLIST_Inbox_UNREAD_USER", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.Int));
                    cmd.Parameters["@Username"].Value = userinfo.username;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            UserMsgList = (from DataRow dr in dataTable.Rows
                                             select new MessageListVM()
                                             {
                                                 id = int.Parse(dr["id"].ToString()),
                                                 username_sender = dr["username_sender"].ToString(),
                                                 Title = dr["Title"].ToString(),
                                                 Msg_Content = dr["Msg_Content"].ToString(),
                                                 Date = dr["Date"].ToString() != "" ? DateTime.Parse(dr["Date"].ToString()) : (DateTime?)null,
                                                 Type = dr["Type"].ToString() != "" ? byte.Parse(dr["Type"].ToString()) : (byte?)null,
                                                 instituteid = int.Parse(dr["instituteid"].ToString()),
                                                 status_Receiver = dr["status_Receiver"].ToString() != "" ? byte.Parse(dr["status_Receiver"].ToString()) : (byte?)null,
                                                 Attach = dr["Attach"].ToString() != "" ? bool.Parse(dr["Attach"].ToString()) : (bool?)null,
                                                 sender = dr["sender"].ToString(),
                                                 name = dr["name"].ToString()

                                             }
                                  ).ToList();
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
                    ViewBagError viewbagerror = new ViewBagError();
                    viewbagerror.ClassName = "alert-danger";
                    viewbagerror.Msg = "خطا در لود پیام ها : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }

                #endregion
            }

            //--------- Get Unread Notic ---
            //=================================================
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

            return PartialView("~/Views/Shared/Partial/ManagerDashboard/_UnreadMessage.cshtml", UserMsgList);


        }
        //------------------- Get Message On Header page Dashboard ----------
        //========================================================================

        
        public class msg_notic_result {
            public string sender_name { get; set; }
            public string institute_name { get; set; }
            public int instituteid { get; set; }
            public string title { get; set; }
            public byte? Type { get; set; }
            public string date { get; set; }
        }

        //========================================================================
        //------------------- Get Notification On Header page Dashboard ----------
        [Access(new string[] { "Notification" }, redirect: false)]
        [LogFilter(mandatory: false)]
        [Route("Dashboard/NoticList")]
        [HttpGet]
        public PartialViewResult get_list_notification()
        {
            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            //======================================
            //-------------- Get Session ---
            #region Get Session
            User userinfo = new User();
            if (Session["User"]!=null)
            {
                userinfo = (User)Session["User"];
            }
            else
            {
                userinfo = null;
            }
            #endregion
            //-------------- Get Session ---
            //======================================

            //=================================================
            //--------- Get Unread Notic ---
            List<NoticList_VM> UserNoticList = new List<NoticList_VM>();
            if (userinfo!=null)
            {
                #region Get Unread Notic
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SP_NOTICLIST_Inbox_UNREAD_USER", conn);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.Int));
                    cmd.Parameters["@Username"].Value = userinfo.username;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            UserNoticList = (from DataRow dr in dataTable.Rows
                                             select new NoticList_VM()
                                             {
                                                 id = int.Parse(dr["id"].ToString()),
                                                 username_sender = dr["username_sender"].ToString(),
                                                 Title = dr["Title"].ToString(),
                                                 Msg_Content = dr["Msg_Content"].ToString(),
                                                 Date = dr["Date"].ToString() != "" ? DateTime.Parse(dr["Date"].ToString()) : (DateTime?)null,
                                                 Type = dr["Type"].ToString() != "" ? byte.Parse(dr["Type"].ToString()) : (byte?)null,
                                                 instituteid = int.Parse(dr["instituteid"].ToString()),
                                                 status_Receiver = dr["status_Receiver"].ToString() != "" ? byte.Parse(dr["status_Receiver"].ToString()) : (byte?)null,
                                                 Attach = dr["Attach"].ToString() != "" ? bool.Parse(dr["Attach"].ToString()) : (bool?)null,
                                                 sender = dr["sender"].ToString(),
                                                 name = dr["name"].ToString()

                                             }
                                  ).ToList();
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
                    ViewBagError viewbagerror = new ViewBagError();
                    viewbagerror.ClassName = "alert-danger";
                    viewbagerror.Msg = "خطا در لود اعلان ها : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }

                #endregion
            }

            //--------- Get Unread Notic ---
            //=================================================
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
           return PartialView("~/Views/Shared/Partial/ManagerDashboard/_UnreadNotic.cshtml", UserNoticList);
        }
        //------------------- Get Notification On Header page Dashboard ----------
        //========================================================================


        //============================================================================
        //------------------- Get Institute List On Side Menu in page Dashboard ----------
        [Access(new string[] { "Institute_list" }, redirect: false)]
        //[Job(new string[] { "Manager","Teacher" }, redirect: false)]

        [LogFilter(mandatory: false)]
        [Route("Dashboard/InstituteList")]
        [HttpGet]
        public PartialViewResult get_list_institute()
        {
            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            //======================================
            //-------------- Get Session ---
            #region Get Session
            User userinfo = new User();
            if (Session["User"] != null)
            {
                userinfo = (User)Session["User"];
            }
            else
            {
                userinfo = null;
            }
            #endregion
            //-------------- Get Session ---
            //======================================

            //=============================================
            //--- Get List User Institute From User_Jobs --
            List<ViewModel.InstituteList_VM> instituteList = new List<InstituteList_VM>();
            #region Get List User Institute From User_Jobs
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"SP_USER_INSTITUTE", conn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.Int));
                cmd.Parameters["@Username"].Value = userinfo.username;

                rdr = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        instituteList = (from DataRow dr in dataTable.Rows
                                       select new InstituteList_VM()
                                       {
                                           id =Int64.Parse( dr["id"].ToString()),
                                           name=dr["name"].ToString(),
                                           educationalType=dr["educationalType"].ToString()
                                       }
                              ).ToList();
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
                ViewBagError viewbagerror = new ViewBagError();
                viewbagerror.ClassName = "alert-danger";
                viewbagerror.Msg = "خطا در لود لیست مدارس : " + ex.Message;
                ViewBag.ErrorMsg = viewbagerror;
            }
            #endregion
            //--- Get List User Institute From User_Jobs --
            //=============================================
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


            return PartialView("~/Views/Shared/Partial/ManagerDashboard/_InstituteListSideMenu.cshtml", instituteList);
            


            //return Json("access denied", JsonRequestBehavior.AllowGet);

        }
        //------------------- Get Class List On Side Menu in page Dashboard ----------
        //============================================================================
    }
}