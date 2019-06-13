using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ViewModel;
using Models;
using App_Start;
using System.Data.SqlClient;
using System.Data;

namespace Peivandyar_Site.Controllers.maindashboard
{
    public class StudentController : Controller
    {
        private string ConnectionString;

        public class Terms_Active
        {
            public int id { get; set; }
            public string Description { get; set; }
        }
        [Route("Students/{Classid?}/{page?}")]
        // GET: Student
        public ActionResult Index(int? Classid,int? page)
        {
                
           return View("~/Views/maindashboard/Students/Index.cshtml");
        }


        //==============================================================
        //---------------------- Student List ---
        [Route("Students/List")]
        public ActionResult Student_list(string Name,string CodeMelli,string StudentCode, int? page)
        {

            int Startindex = 0;
            int pagesize = 1;
            page = page.HasValue ? Convert.ToInt32(page) - 1 : 0;
            Startindex = page.HasValue ? Convert.ToInt32(page * pagesize) : 0;

            int Total_item = 0;
            int Total_page = 0;
            int? Current_page = page;

            Name = Name != null ? Name : "";
            ViewBag.Name = Name;

            CodeMelli = CodeMelli != null ? CodeMelli : "";
            ViewBag.CodeMelli = CodeMelli;

            StudentCode= StudentCode != null ? StudentCode : "";
            ViewBag.StudentCode = StudentCode;

            List<StudentList_VM> result = new List<StudentList_VM>();



            #region Get Session
            User userinfo = (User)Session["User"];

            Institute_Info_Session_VM Institute_info_Session = new Institute_Info_Session_VM();
            if (Session["Institute_info"]!=null)
            {
                Institute_info_Session = (Institute_Info_Session_VM)Session["Institute_info"];
            }
            

            ClassInfo_VM class_info = new ClassInfo_VM();
            if (Session["Class_info"]!=null)
            {
                class_info=(ClassInfo_VM)Session["Class_info"];
            }
            #endregion

            if (class_info.id != null)
            {
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;

                //============================================================
                //--------------------- Get Student List in Class id ---
                List<StudentList_VM> StudentList = new List<StudentList_VM>();
                #region Get Student List in Class id
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"select * from 
                        (
                        select  Users.username,Users.firstname,Users.lastname,Users.gender,Users.Code_Melli,Users.Fathername,Users.Active
		                                        ,Users.Student_Code as Code
						                        ,ROW_NUMBER() OVER(order by Users.lastname) AS rownum
	                                        from Class_User 
                                        inner join Users ON
		                                        Class_User.Classid=@Classid
		                                        and Class_User.Jobsid=3
		                                        and Users.username like Class_User.username
				                        where
						                        (Users.firstname like @firstname or Users.lastname like @lastname )
						                        and (Users.Code_Melli like @Code_Melli or Users.Code_Melli is null)
						                        and (Users.Student_Code like @Student_Code or Users.Student_Code is null)
                        ) as TBL_Student
	                        where
		                        (TBL_Student.rownum>@CurrentPage and
				                                                        TBL_Student.rownum<=(@CurrentPage+@PageSize))
                        ", conn);


                    cmd.Parameters.Add(new SqlParameter("@Classid", SqlDbType.Int));
                    cmd.Parameters["@Classid"].Value = class_info.id;

                    cmd.Parameters.Add(new SqlParameter("@firstname", SqlDbType.NVarChar));
                    cmd.Parameters["@firstname"].Value = "%"+Name+ "%";

                    cmd.Parameters.Add(new SqlParameter("@lastname", SqlDbType.NVarChar));
                    cmd.Parameters["@lastname"].Value = "%" + Name + "%";

                    cmd.Parameters.Add(new SqlParameter("@Code_Melli", SqlDbType.NVarChar));
                    cmd.Parameters["@Code_Melli"].Value = "%" + CodeMelli + "%";

                    cmd.Parameters.Add(new SqlParameter("@Student_Code", SqlDbType.NVarChar));
                    cmd.Parameters["@Student_Code"].Value = "%" + StudentCode + "%";

                    cmd.Parameters.Add(new SqlParameter("@CurrentPage", SqlDbType.Int));
                    cmd.Parameters["@CurrentPage"].Value = Current_page;

                    cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
                    cmd.Parameters["@PageSize"].Value = pagesize;



                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            StudentList = (from DataRow dr in dataTable.Rows
                                           select new StudentList_VM()
                                           {
                                               username = dr["username"].ToString(),
                                               firstname = dr["firstname"].ToString(),
                                               lastname = dr["lastname"].ToString(),
                                               Fathername=dr["Fathername"].ToString(),
                                               gender = dr["gender"].ToString() != "" ? bool.Parse(dr["gender"].ToString()) : (bool?)null,
                                               Code_Melli = dr["Code_Melli"].ToString(),
                                               Code = dr["Code"].ToString(),
                                               Active=dr["Active"].ToString()!=""?bool.Parse(dr["Active"].ToString()):(bool?)null
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
                    viewbagerror.Msg = "خطا در لود لیست دانش آموزان : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }
                #endregion
                //--------------------- Get Student List in Class id ---
                //============================================================

                //============================================================
                //--------------------- Get Total Item Student ---
                #region Get Total Item Student
                try

                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }


                    string query = "";
                    #region Search Query 
                    query = @"select Count( Users.username) as TotalItem
	                                        from Class_User 
                                        inner join Users ON
		                                        Class_User.Classid=@Classid
		                                        and Class_User.Jobsid=3
		                                        and Users.username like Class_User.username
				                        where
						                        (Users.firstname like @firstname or Users.lastname like @lastname )
						                        and (Users.Code_Melli like @Code_Melli or Users.Code_Melli is null)
						                        and (Users.Student_Code like @Student_Code or Users.Student_Code is null)";
                    


                    #endregion

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.Add(new SqlParameter("@Classid", SqlDbType.Int));
                    cmd.Parameters["@Classid"].Value = class_info.id;

                    cmd.Parameters.Add(new SqlParameter("@firstname", SqlDbType.NVarChar));
                    cmd.Parameters["@firstname"].Value = "%" + Name + "%";

                    cmd.Parameters.Add(new SqlParameter("@lastname", SqlDbType.NVarChar));
                    cmd.Parameters["@lastname"].Value = "%" + Name + "%";

                    cmd.Parameters.Add(new SqlParameter("@Code_Melli", SqlDbType.NVarChar));
                    cmd.Parameters["@Code_Melli"].Value = "%" + CodeMelli + "%";

                    cmd.Parameters.Add(new SqlParameter("@Student_Code", SqlDbType.NVarChar));
                    cmd.Parameters["@Student_Code"].Value = "%" + StudentCode + "%";



                    rdr = cmd.ExecuteReader();

                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow dr = dataTable.Rows[0];
                            Total_item = dr["TotalItem"].ToString() != "" ? int.Parse(dr["TotalItem"].ToString()) : 0;
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
                        conn.Close();
                    }


                }
                #endregion
                //--------------------- Get Total Item Student ---
                //============================================================

                if (rdr != null)
                {
                    rdr.Close();
                    rdr = null;
                }
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                Total_page = Total_item / pagesize;

                //==========================================
                //---------------- Set ViewBag -------------

                ViewBag.Total_item = Total_item;
                ViewBag.Total_page = Total_page;
                ViewBag.Current_page = Current_page;
                //---------------- Set ViewBag -------------
                //==========================================

                return View("~/Views/maindashboard/Students/studentlist.cshtml", StudentList);
            }
            else
            {
                ViewBagError viewbagerror = new ViewBagError();
                viewbagerror.ClassName = "alert-danger";
                viewbagerror.Msg = "شناسه کلاسی پیدا نشد.";
                ViewBag.ErrorMsg = viewbagerror;
                return View("~/Views/maindashboard/Students/studentlist.cshtml");
            }

            
        }
        //---------------------- Student List ---
        //==============================================================


        //==============================================================
        //--------------------- Student Info ---
        [Route("Students/StudentInfo/{StudentId?}")]
        public ActionResult StudentInfo(string StudentId)
        {
            if(StudentId != null)
            {
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;

                //=========================================
                //----------- Get Student Info ---
                StudentInfoesVM StudentInfo = new StudentInfoesVM();
                #region Get Student Info
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"select Users.firstname,Users.lastname,Users.Student_Code,Users.Code_Melli,Users.Fathername,Users.birtday,Users.gender 
		                    ,StudentPersonalInfoes.Shsh,StudentPersonalInfoes.MotherName,StudentPersonalInfoes.Lastname_Mother,StudentPersonalInfoes.Address_Primary
		                    ,StudentPersonalInfoes.Address_Secondary,StudentPersonalInfoes.tell,StudentPersonalInfoes.Mobile
	                    from Users 
                    left join StudentPersonalInfoes ON
		                    Users.username like @username
		                    and StudentPersonalInfoes.username like @username
	                    where
		                    Users.username like @username
		                    ", conn);


                    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar,50));
                    cmd.Parameters["@username"].Value = StudentId;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow dr = dataTable.Rows[0];

                            StudentInfo.firstname = dr["firstname"].ToString();
                            StudentInfo.lastname = dr["lastname"].ToString();
                            StudentInfo.Student_Code = dr["Student_Code"].ToString();
                            StudentInfo.Code_Melli = dr["Code_Melli"].ToString();
                            StudentInfo.Fathername = dr["Fathername"].ToString();
                            StudentInfo.birtday = dr["birtday"].ToString()!=""?DateTime.Parse(dr["birtday"].ToString()):(DateTime?)null;
                            StudentInfo.gender = dr["gender"].ToString() != "" ? bool.Parse(dr["gender"].ToString()) : (bool?)null;
                            StudentInfo.Shsh = dr["Shsh"].ToString();
                            StudentInfo.MotherName = dr["MotherName"].ToString();
                            StudentInfo.Lastname_Mother = dr["Lastname_Mother"].ToString();
                            StudentInfo.Address_Primary = dr["Address_Primary"].ToString();
                            StudentInfo.Address_Secondary = dr["Address_Secondary"].ToString();
                            StudentInfo.tell = dr["tell"].ToString();
                            StudentInfo.Mobile = dr["Mobile"].ToString();
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
                    viewbagerror.Msg = "خطا در لود اطلاعات دانش آموز : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }

                #endregion
                //----------- Get Student Info ---
                //=========================================
                return View("~/Views/maindashboard/Students/StudentInfo.cshtml", StudentInfo);
            }
            return View("~/Views/maindashboard/Students/StudentInfo.cshtml");
        }

        //--------------------- Student Info ---
        //==============================================================

        [Route("Students/Create")]
        public ActionResult Create()
        {
            return View("~/Views/maindashboard/Students/Create.cshtml");
        }
        
        [Route("Students/Edit")]
        public ActionResult Edit(string Studentid)
        {
            if (Studentid!=null)
            {


                #region Get Session
                User userinfo = (User)Session["User"];
                Institute_Info_Session_VM Institute_info_Session = new Institute_Info_Session_VM();
                if (Session["Institute_info"] != null)
                {
                    Institute_info_Session = (Institute_Info_Session_VM)Session["Institute_info"];
                }


                ClassInfo_VM class_info = new ClassInfo_VM();
                if (Session["Class_info"] != null)
                {
                    class_info = (ClassInfo_VM)Session["Class_info"];
                }
                #endregion

                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;

                //============================================
                //---------------- Get Student Info ---
                StudentInfoesVM mystudentinfo = new StudentInfoesVM();
                #region Get Student Info
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SP_STUDENTINFO", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar,50));
                    cmd.Parameters["@username"].Value = Studentid;

                    cmd.Parameters.Add(new SqlParameter("@Adminusername", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Adminusername"].Value = userinfo.username;

                    cmd.Parameters.Add(new SqlParameter("@Classid", SqlDbType.BigInt));
                    cmd.Parameters["@Classid"].Value = class_info.id;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow dr = dataTable.Rows[0];

                            mystudentinfo.username = Studentid;
                            mystudentinfo.firstname = dr["firstname"].ToString();
                            mystudentinfo.lastname = dr["lastname"].ToString();
                            mystudentinfo.Fathername = dr["Fathername"].ToString();
                            mystudentinfo.Code_Melli = dr["Code_Melli"].ToString();
                            mystudentinfo.Shsh = dr["Shsh"].ToString();
                            mystudentinfo.MotherName = dr["MotherName"].ToString();
                            mystudentinfo.Lastname_Mother= dr["Lastname_Mother"].ToString();
                            mystudentinfo.Address_Primary = dr["Address_Primary"].ToString();
                            mystudentinfo.Address_Secondary = dr["Address_Secondary"].ToString();
                            mystudentinfo.Mobile = dr["Mobile"].ToString();
                            mystudentinfo.tell = dr["tell"].ToString();
                            mystudentinfo.birtday = dr["birtday"].ToString() != "" ? DateTime.Parse(dr["birtday"].ToString()) : (DateTime?)null;
                            mystudentinfo.Student_Code = dr["Student_Code"].ToString();
                            mystudentinfo.Active = dr["Active"].ToString() != "" ? bool.Parse(dr["Active"].ToString()) : (bool?)null;

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
                    viewbagerror.Msg = "خطا در لود اطلاعات دانش آموز : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }
                #endregion
                //---------------- Get Student Info ---
                //============================================
                return View("~/Views/maindashboard/Students/Edit.cshtml", mystudentinfo);
            }
            else
            {
                ViewBagError viewbagerror = new ViewBagError();
                viewbagerror.ClassName = "alert-danger";
                viewbagerror.Msg = "شناسه دانش آموز معتبر نیست " ;
                ViewBag.ErrorMsg = viewbagerror;
            }
            return View("~/Views/maindashboard/Students/Edit.cshtml");
        }


        //[ValidateAntiForgeryToken]
        [HttpPost]
        [Route("Students/Update")]
        public ActionResult Update(StudentInfoesVM MyStudent,string birtdaystr)
        {
            if (MyStudent!=null)
            {
                MyStudent.birtday = null;
                if (birtdaystr!=null)
                {
                    try
                    {
                        Date_Shamsi_Miladi DateConverter = new Date_Shamsi_Miladi();
                        StringClass_Convert StrConverter = new StringClass_Convert();
                        birtdaystr = StrConverter.GetEnglishNumber(birtdaystr);
                        string STRdate = DateConverter.shamsitomiladi(birtdaystr);
                        DateTime MyDate = DateTime.Parse(STRdate);

                        MyStudent.birtday = MyDate;
                    }
                    catch { MyStudent.birtday = null; }
                }
                

                #region Get Session
                User userinfo = (User)Session["User"];
                #endregion

                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;


                //==================================================
                //------------- Update Student Users---
                bool Updated = false;
                #region Update Student
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE       Users
                        SET                firstname =@firstname, lastname =@lastname , Fathername =@Fathername , birtday =@birtday , gender =@gender , Student_Code =@Student_Code , Code_Melli =@Code_Melli ,Active=@Active
                        where username like @username", conn);

                    #region Parameters
                    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@username"].Value = MyStudent.username;

                    cmd.Parameters.Add(new SqlParameter("@firstname", SqlDbType.NVarChar, 50));
                    if (MyStudent.firstname != null)
                    {
                        cmd.Parameters["@firstname"].Value = MyStudent.firstname;
                    }
                    else
                    {
                        cmd.Parameters["@firstname"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@lastname", SqlDbType.NVarChar, 50));
                    if (MyStudent.lastname != null)
                    {
                        cmd.Parameters["@lastname"].Value = MyStudent.lastname;
                    }
                    else
                    {
                        cmd.Parameters["@lastname"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@Fathername", SqlDbType.NVarChar, 50));
                    if (MyStudent.Fathername != null)
                    {
                        cmd.Parameters["@Fathername"].Value = MyStudent.Fathername;
                    }
                    else
                    {
                        cmd.Parameters["@Fathername"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@birtday", SqlDbType.DateTime));
                    if (MyStudent.birtday != null)
                    {
                        cmd.Parameters["@birtday"].Value = MyStudent.birtday;
                    }
                    else
                    {
                        cmd.Parameters["@birtday"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@gender", SqlDbType.Bit));
                    if (MyStudent.gender != null)
                    {
                        cmd.Parameters["@gender"].Value = MyStudent.gender;
                    }
                    else
                    {
                        cmd.Parameters["@gender"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@Student_Code", SqlDbType.NVarChar, 50));
                    if (MyStudent.Student_Code != null)
                    {
                        cmd.Parameters["@Student_Code"].Value = MyStudent.Student_Code;
                    }
                    else
                    {
                        cmd.Parameters["@Student_Code"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@Code_Melli", SqlDbType.NVarChar, 50));
                    if (MyStudent.Code_Melli != null)
                    {
                        cmd.Parameters["@Code_Melli"].Value = MyStudent.Code_Melli;
                    }
                    else
                    {
                        cmd.Parameters["@Code_Melli"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit));
                    if (MyStudent.Active != null)
                    {
                        cmd.Parameters["@Active"].Value = MyStudent.Active;
                    }
                    else
                    {
                        cmd.Parameters["@Active"].Value = DBNull.Value;
                    }
                    #endregion


                    cmd.ExecuteNonQuery();
                    Updated = true;


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
                    viewbagerror.Msg = "خطا در ویرایش اطلاعات دانش آموز" + " : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;

                }
                #endregion
                //------------- Update Student Users---
                //==================================================

                if (Updated)
                {
                    //==================================================
                    //------------- Update StudentPersonalInfoes ---
                    #region Update StudentPersonalInfoes
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = new SqlCommand(@"UPDATE       StudentPersonalInfoes
                            SET                Shsh =@Shsh, MotherName =@MotherName, Lastname_Mother =@Lastname_Mother, Address_Primary =@Address_Primary,
					                             Address_Secondary =@Address_Secondary, tell =@tell, Mobile =@Mobile
	                            where username like @username", conn);

                        #region Parameters

                        cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                        cmd.Parameters["@username"].Value = MyStudent.username;

                        cmd.Parameters.Add(new SqlParameter("@Shsh", SqlDbType.NVarChar, 50));
                        if (MyStudent.Shsh != null)
                        {
                            cmd.Parameters["@Shsh"].Value = MyStudent.Shsh;
                        }
                        else
                        {
                            cmd.Parameters["@Shsh"].Value = DBNull.Value;
                        }

                        cmd.Parameters.Add(new SqlParameter("@MotherName", SqlDbType.NVarChar, 50));
                        if (MyStudent.MotherName != null)
                        {
                            cmd.Parameters["@MotherName"].Value = MyStudent.MotherName;
                        }
                        else
                        {
                            cmd.Parameters["@MotherName"].Value = DBNull.Value;
                        }


                        cmd.Parameters.Add(new SqlParameter("@Lastname_Mother", SqlDbType.NVarChar, 50));
                        if (MyStudent.Lastname_Mother != null)
                        {
                            cmd.Parameters["@Lastname_Mother"].Value = MyStudent.Lastname_Mother;
                        }
                        else
                        {
                            cmd.Parameters["@Lastname_Mother"].Value = DBNull.Value;
                        }

                        
                        cmd.Parameters.Add(new SqlParameter("@Address_Primary", SqlDbType.NVarChar));
                        if (MyStudent.Address_Primary != null)
                        {
                            cmd.Parameters["@Address_Primary"].Value = MyStudent.Address_Primary;
                        }
                        else
                        {
                            cmd.Parameters["@Address_Primary"].Value = DBNull.Value;
                        }


                        cmd.Parameters.Add(new SqlParameter("@Address_Secondary", SqlDbType.NVarChar));
                        if (MyStudent.Address_Secondary!= null)
                        {
                            cmd.Parameters["@Address_Secondary"].Value = MyStudent.Address_Secondary;
                        }
                        else
                        {
                            cmd.Parameters["@Address_Secondary"].Value = DBNull.Value;
                        }


                        cmd.Parameters.Add(new SqlParameter("@tell", SqlDbType.NVarChar, 50));
                        if (MyStudent.tell != null)
                        {
                            cmd.Parameters["@tell"].Value = MyStudent.tell;
                        }
                        else
                        {
                            cmd.Parameters["@tell"].Value = DBNull.Value;
                        }

                        cmd.Parameters.Add(new SqlParameter("@Mobile", SqlDbType.NVarChar, 50));
                        if (MyStudent.Mobile != null)
                        {
                            cmd.Parameters["@Mobile"].Value = MyStudent.Mobile;
                        }
                        else
                        {
                            cmd.Parameters["@Mobile"].Value = DBNull.Value;
                        }

                        #endregion


                        cmd.ExecuteNonQuery();
                        
                        ViewBagError myerror = new ViewBagError();
                        myerror.ClassName = "success";
                        myerror.Msg = "اطلاعات دانش آموز با موفقیت ویرایش شد .";
                        ViewBag.msg = myerror;
                        return RedirectToAction("Edit","Student",new { Studentid= MyStudent.username });

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
                        viewbagerror.Msg = "خطا در ویرایش اطلاعات دانش آموز" + " : " + ex.Message;
                        ViewBag.ErrorMsg = viewbagerror;

                    }
                    #endregion
                    //------------- Update StudentPersonalInfoes ---
                    //==================================================
                }


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

                ViewBagError myerror2 = new ViewBagError();
                myerror2.ClassName = "danger";
                myerror2.Msg = "ویرایش اطلاعات دانش آموز انجام نشد .";
                ViewBag.msg = myerror2;
                return RedirectToAction("Edit", "Student", new { Studentid = MyStudent.username });

            }

            ViewBagError myerror3 = new ViewBagError();
            myerror3.ClassName = "danger";
            myerror3.Msg = "شناسه دانش آموز صحیح نیست";
            ViewBag.msg = myerror3;
            return RedirectToAction("Student_list", "Student", null);
            
        }


        //[ValidateAntiForgeryToken]
        [HttpPost]
        [Route("Students/Add")]
        public ActionResult Add(StudentInfoesVM MyStudent, string birtdaystr)
        {


            #region Get Session
            User userinfo = (User)Session["User"];
            Institute_Info_Session_VM Institute_info_Session = new Institute_Info_Session_VM();
            if (Session["Institute_info"] != null)
            {
                Institute_info_Session = (Institute_Info_Session_VM)Session["Institute_info"];
            }


            ClassInfo_VM class_info = new ClassInfo_VM();
            if (Session["Class_info"] != null)
            {
                class_info = (ClassInfo_VM)Session["Class_info"];
            }

            Term institute_terms_selected = new Term();
            if (Session["TermSelect"] != null)
            {
                institute_terms_selected = (Term)Session["TermSelect"];
            }
            {

            }
            #endregion


            if (MyStudent!=null && Institute_info_Session != null && class_info != null && institute_terms_selected != null)
            {
                MyStudent.birtday = null;
                #region Set Converted Date
                if (birtdaystr != null)
                {
                    try
                    {
                        Date_Shamsi_Miladi DateConverter = new Date_Shamsi_Miladi();
                        StringClass_Convert StrConverter = new StringClass_Convert();
                        birtdaystr = StrConverter.GetEnglishNumber(birtdaystr);
                        string STRdate = DateConverter.shamsitomiladi(birtdaystr);
                        DateTime MyDate = DateTime.Parse(STRdate);

                        MyStudent.birtday = MyDate;
                    }
                    catch { MyStudent.birtday = null; }
                }
                #endregion


                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;

                //==================================================
                //---------- Add Student To Users TBL ---
                bool StudentAdded = false;
                #region Add Student To Users TBL
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Users
                                             (username, password, firstname, lastname, Fathername,birtday,gender,Student_Code, Code_Melli,Active)
                    VALUES        (@username, @password, @firstname, @lastname, @Fathername,@birtday,@gender,@Student_Code, @Code_Melli,@Active)", conn);

                    #region Parameteres
                    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@username"].Value = MyStudent.Code_Melli;

                    cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@password"].Value = MyStudent.Code_Melli;

                    cmd.Parameters.Add(new SqlParameter("@firstname", SqlDbType.NVarChar, 50));
                    if (MyStudent.firstname != null)
                    {
                        cmd.Parameters["@firstname"].Value = MyStudent.firstname;
                    }
                    else
                    {
                        cmd.Parameters["@firstname"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@lastname", SqlDbType.NVarChar, 50));
                    if (MyStudent.lastname != null)
                    {
                        cmd.Parameters["@lastname"].Value = MyStudent.lastname;
                    }
                    else
                    {
                        cmd.Parameters["@lastname"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@Fathername", SqlDbType.NVarChar, 50));
                    if (MyStudent.Fathername != null)
                    {
                        cmd.Parameters["@Fathername"].Value = MyStudent.Fathername;
                    }
                    else
                    {
                        cmd.Parameters["@Fathername"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@birtday", SqlDbType.DateTime));
                    if (MyStudent.birtday != null)
                    {
                        cmd.Parameters["@birtday"].Value = MyStudent.birtday;
                    }
                    else
                    {
                        cmd.Parameters["@birtday"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@gender", SqlDbType.Bit));
                    if (MyStudent.gender != null)
                    {
                        cmd.Parameters["@gender"].Value = MyStudent.gender;
                    }
                    else
                    {
                        cmd.Parameters["@gender"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@Student_Code", SqlDbType.NVarChar, 50));
                    if (MyStudent.Student_Code != null)
                    {
                        cmd.Parameters["@Student_Code"].Value = MyStudent.Student_Code;
                    }
                    else
                    {
                        cmd.Parameters["@Student_Code"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@Code_Melli", SqlDbType.NVarChar, 50));
                    if (MyStudent.Code_Melli != null)
                    {
                        cmd.Parameters["@Code_Melli"].Value = MyStudent.Code_Melli;
                    }
                    else
                    {
                        cmd.Parameters["@Code_Melli"].Value = DBNull.Value;
                    }


                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit));
                    if (MyStudent.Active != null)
                    {
                        cmd.Parameters["@Active"].Value = MyStudent.Active;
                    }
                    else
                    {
                        cmd.Parameters["@Active"].Value = DBNull.Value;
                    }
                    #endregion

                    cmd.ExecuteNonQuery();
                    StudentAdded = true;

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
                    viewbagerror.Msg = "خطا در ایجاد دانش آموز" + " : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;

                    StudentAdded = false;
                }
                #endregion
                //---------- Add Student To Users TBL ---
                //==================================================

                if (StudentAdded)
                {
                    
                    //==================================================
                    //---------- Add Student To Class_User ---
                    bool Class_UserAdddes = false;
                    #region Add Student To Class_User
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = new SqlCommand(@"INSERT INTO Class_User
                                                 (Classid, username, Jobsid)
                        VALUES        (@Classid, @username, @Jobsid)", conn);

                        #region Parameteres

                        cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                        cmd.Parameters["@username"].Value = MyStudent.Code_Melli;

                        cmd.Parameters.Add(new SqlParameter("@Classid", SqlDbType.BigInt));
                        cmd.Parameters["@Classid"].Value = class_info.id;

                        cmd.Parameters.Add(new SqlParameter("@Jobsid", SqlDbType.Int));
                        cmd.Parameters["@Jobsid"].Value = 3;
                        #endregion


                        cmd.ExecuteNonQuery();
                        Class_UserAdddes = true;

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
                        viewbagerror.Msg = "خطا در اختصاص کلاس به دانش آموز" + " : " + ex.Message;
                        ViewBag.ErrorMsg = viewbagerror;

                        Class_UserAdddes = false;
                    }
                    #endregion
                    //---------- Add Student To Class_User ---
                    //==================================================

                    if (Class_UserAdddes)
                    {
                        //==================================================
                        //---------- Add Student To User_Jobs ---
                        bool User_JobsAdddes = false;
                        #region Add Student To User_Jobs
                        try
                        {
                            if (conn.State != ConnectionState.Open)
                                conn.Open();

                            SqlCommand cmd = new SqlCommand(@"INSERT INTO User_Jobs
                                                     (Username, Jobid, Instituteid)
                            VALUES        (@Username, @Jobid, @Instituteid)", conn);

                            #region Parameteres

                            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                            cmd.Parameters["@username"].Value = MyStudent.Code_Melli;

                            cmd.Parameters.Add(new SqlParameter("@Jobid", SqlDbType.Int));
                            cmd.Parameters["@Jobid"].Value = 3;

                            cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.BigInt));
                            cmd.Parameters["@Instituteid"].Value = Institute_info_Session.id;
                            
                            #endregion


                            cmd.ExecuteNonQuery();
                            User_JobsAdddes = true;

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
                            viewbagerror.Msg = "خطا در تعریف شغل دانش آموز" + " : " + ex.Message;
                            ViewBag.ErrorMsg = viewbagerror;

                            User_JobsAdddes = false;
                        }
                        #endregion
                        //---------- Add Student To User_Jobs ---
                        //==================================================

                        //==================================================
                        //---------- Add Student To User_Access ---
                        #region Add Student To User_Access

                        #endregion
                        //---------- Add Student To User_Access ---
                        //==================================================


                        //==================================================
                        //------------- Insert StudentPersonalInfoes ---
                        #region Insert StudentPersonalInfoes
                        try
                        {
                            if (conn.State != ConnectionState.Open)
                                conn.Open();

                            SqlCommand cmd = new SqlCommand(@"INSERT INTO StudentPersonalInfoes
                                                 (username, Shsh, MotherName, Lastname_Mother, Address_Primary, Address_Secondary, tell, Mobile)
                        VALUES        (@username, @Shsh, @MotherName, @Lastname_Mother, @Address_Primary, @Address_Secondary, @tell, @Mobile)", conn);

                            #region Parameters

                            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                            cmd.Parameters["@username"].Value = MyStudent.Code_Melli;

                            cmd.Parameters.Add(new SqlParameter("@Shsh", SqlDbType.NVarChar, 50));
                            if (MyStudent.Shsh != null)
                            {
                                cmd.Parameters["@Shsh"].Value = MyStudent.Shsh;
                            }
                            else
                            {
                                cmd.Parameters["@Shsh"].Value = DBNull.Value;
                            }

                            cmd.Parameters.Add(new SqlParameter("@MotherName", SqlDbType.NVarChar, 50));
                            if (MyStudent.MotherName != null)
                            {
                                cmd.Parameters["@MotherName"].Value = MyStudent.MotherName;
                            }
                            else
                            {
                                cmd.Parameters["@MotherName"].Value = DBNull.Value;
                            }


                            cmd.Parameters.Add(new SqlParameter("@Lastname_Mother", SqlDbType.NVarChar, 50));
                            if (MyStudent.Lastname_Mother != null)
                            {
                                cmd.Parameters["@Lastname_Mother"].Value = MyStudent.Lastname_Mother;
                            }
                            else
                            {
                                cmd.Parameters["@Lastname_Mother"].Value = DBNull.Value;
                            }


                            cmd.Parameters.Add(new SqlParameter("@Address_Primary", SqlDbType.NVarChar));
                            if (MyStudent.Address_Primary != null)
                            {
                                cmd.Parameters["@Address_Primary"].Value = MyStudent.Address_Primary;
                            }
                            else
                            {
                                cmd.Parameters["@Address_Primary"].Value = DBNull.Value;
                            }


                            cmd.Parameters.Add(new SqlParameter("@Address_Secondary", SqlDbType.NVarChar));
                            if (MyStudent.Address_Secondary != null)
                            {
                                cmd.Parameters["@Address_Secondary"].Value = MyStudent.Address_Secondary;
                            }
                            else
                            {
                                cmd.Parameters["@Address_Secondary"].Value = DBNull.Value;
                            }


                            cmd.Parameters.Add(new SqlParameter("@tell", SqlDbType.NVarChar, 50));
                            if (MyStudent.tell != null)
                            {
                                cmd.Parameters["@tell"].Value = MyStudent.tell;
                            }
                            else
                            {
                                cmd.Parameters["@tell"].Value = DBNull.Value;
                            }

                            cmd.Parameters.Add(new SqlParameter("@Mobile", SqlDbType.NVarChar, 50));
                            if (MyStudent.Mobile != null)
                            {
                                cmd.Parameters["@Mobile"].Value = MyStudent.Mobile;
                            }
                            else
                            {
                                cmd.Parameters["@Mobile"].Value = DBNull.Value;
                            }

                            #endregion


                            cmd.ExecuteNonQuery();

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
                            viewbagerror.Msg = "خطا در ثبت اطلاعات دانش آموز" + " : " + ex.Message;
                            ViewBag.ErrorMsg = viewbagerror;

                        }
                        #endregion
                        //------------- Insert StudentPersonalInfoes ---
                        //==================================================


                        ViewBagError myerror = new ViewBagError();
                        myerror.ClassName = "success";
                        myerror.Msg = "دانش آموز با موفقیت ایجاد گردید.";
                        ViewBag.msg = myerror;
                        return RedirectToAction("Student_list");
                    }
                }

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
            return View("~/Views/maindashboard/Students/Create.cshtml");

        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        [Route("Students/Delete")]
        public ActionResult Delete(string Studentid)
        {
            if (Studentid != null)
            {
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;


                //==================================================
                //------------- Update Student Users---
                bool Updated = false;
                #region Update Student
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE       Users
                        SET                Active =@Active
                        where username like @username", conn);

                    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@username"].Value = Studentid;

                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit));
                    cmd.Parameters["@Active"].Value = bool.Parse("false");

                    cmd.ExecuteNonQuery();

                    ViewBagError viewbagerror = new ViewBagError();
                    viewbagerror.ClassName = "alert-success";
                    viewbagerror.Msg = "دانش آموز مورد نظر حذف گردید" + " : " ;
                    ViewBag.ErrorMsg = viewbagerror;

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
                    viewbagerror.Msg = "خطا در ویرایش اطلاعات دانش آموز" + " : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;

                }
                #endregion
                //------------- Update Student Users---
                //==================================================
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
            else
            {
                ViewBagError viewbagerror = new ViewBagError();
                viewbagerror.ClassName = "alert-danger";
                viewbagerror.Msg = "شناسه دانش آموز صحیح نیست";
                ViewBag.ErrorMsg = viewbagerror;
            }

            return RedirectToAction("Student_list","Student",null);
        }




    }
}