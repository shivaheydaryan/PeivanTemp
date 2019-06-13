using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using PagedList;
using Models;

namespace Peivandyar_Site.Controllers.maindashboard
{
    public class EmployeeController : Controller
    {
        private string ConnectionString;
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        [Route("Employee/List/{page?}")]
        public ActionResult Employee_list(int? page)
        {

            int pageIndex = 1;
            int pagesize = 2;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<ViewModel.EmployeeList_VM> result = null;

            User userinfo = (User)Session["User"];

            #region Get Session
            ViewModel.Institute_Info_Session_VM Institute_info_Session = new ViewModel.Institute_Info_Session_VM();
            if (Session["Institute_info"] != null)
            {
                Institute_info_Session = (ViewModel.Institute_Info_Session_VM)Session["Institute_info"];
            }
            else
            {
                Institute_info_Session = null;
            }

            #endregion

            if (Institute_info_Session != null)
            {
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;

                //============================================================
                //--------------------- Get Employee List in Class id ---
                
                List<ViewModel.EmployeeList_VM> EmployeeList = new List<ViewModel.EmployeeList_VM>();
                #region Get Employee List in Institute id
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"select              Users.username,Users.firstname,Users.lastname,Users.gender,
					
					case 
						When User_Jobs.Jobid = 1 Then Users.Manager_Code
						When User_Jobs.Jobid = 2 Then Users.Teacher_Code
						When User_Jobs.Jobid = 5 Then Users.Employe_Code
					End as Code
					,
	                (select Jobs.Name from Jobs where Jobs.id = User_Jobs.Jobid) as JobName
                from User_Jobs
	                inner join Users On
		                User_Jobs.Instituteid =@Instituteid
		                and (User_Jobs.Jobid != 3 and User_Jobs.Jobid != 4)
		                and Users.username like User_Jobs.Username
                ", conn);


                    cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.Int));
                    cmd.Parameters["@Instituteid"].Value = Institute_info_Session.id;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            EmployeeList = (from DataRow dr in dataTable.Rows
                                            select new ViewModel.EmployeeList_VM()
                                            {
                                                username = dr["username"].ToString(),
                                                firstname = dr["firstname"].ToString(),
                                                lastname = dr["lastname"].ToString(),
                                                gender = dr["gender"].ToString() != "" ? bool.Parse(dr["gender"].ToString()) : (bool?)null,
                                                Code = dr["Code"].ToString(),
                                                JobName = dr["JobName"].ToString()
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
                    ViewModel.ViewBagError viewbagerror = new ViewModel.ViewBagError();
                    viewbagerror.ClassName = "alert-danger";
                    viewbagerror.Msg = "خطا در لود لیست کارمندان : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }
                #endregion
                //--------------------- Get Employee List in Class id ---
                //============================================================
                result = EmployeeList.ToPagedList(pageIndex, pagesize);
                return View("~/Views/maindashboard/Employee/Employeelist.cshtml", result);
            }

            else
            {
                ViewModel.ViewBagError viewbagerror = new ViewModel.ViewBagError();
                viewbagerror.ClassName = "alert-danger";
                viewbagerror.Msg = "شناسه آموزشگاه صحیح نیست.";
                ViewBag.ErrorMsg = viewbagerror;
                return View("~/Views/maindashboard/Employee/Employeelist.cshtml");
            }


        }

    }
}