using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App_Start;
using global::Models;
using ViewModel;
using PagedList;
using System.Data.SqlClient;
using System.Data;

namespace Peivandyar_Site.Controllers
{
    [Authentication]
    [App_Start.Authorize(job: new string[] { "Manager", "Teacher" })]

    [LogFilter(mandatory: true)]
    
    public class ClassesController : Controller
    {
        DataContext db = new DataContext();
        private string ConnectionString;
        [Route("Classes/{Institute_id?}/{page?}")]
        // GET: Classes
        public ActionResult Index(string ClassName, Int64? Institute_id, int? page)
        {

            int Startindex = 0;
            int pagesize = 1;
            page = page.HasValue ? Convert.ToInt32(page) - 1 : 0;
            Startindex = page.HasValue ? Convert.ToInt32(page * pagesize) : 0;



            int Total_item = 0;
            int Total_page = 0;
            int? Current_page = page;

            ClassName = ClassName != null ? ClassName : "";
            ViewBag.ClassName = ClassName;
            
            List<Class_List_VM> result_list = new List<Class_List_VM>();

            if (Institute_id == null && Session["Institute_info"]!=null)
            {
                Institute_Info_Session_VM myinstitute = (Institute_Info_Session_VM)Session["Institute_info"];
                Institute_id = myinstitute.id;
            }

            if (Institute_id != null )
            {
                User userinfo = (User)Session["User"];


                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;



                

                //==============================================================
                //----------------------- لیست پایه های تحصیلی----------------
                #region Get Grades Institute
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SP_INSTITUTE_GRADES", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@InstituteId", SqlDbType.Int));
                    cmd.Parameters["@InstituteId"].Value = Institute_id;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            List<Grade> insGrades= (from DataRow dr in dataTable.Rows
                                                    select new Grade()
                                                    {
                                                        id =int.Parse( dr["id"].ToString()),
                                                        Name=dr["Name"].ToString()
                                                    }
                                  ).ToList();
                            ViewBag.Grades = insGrades;
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
                    viewbagerror.Msg = "خطا در لود پایه های تحصیلی آموزشگاه : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }
                #endregion
                //----------------------- لیست پایه های تحصیلی----------------
                //==============================================================


                //==============================================================
                //-------------- لیست ترم های مربوط به آموزشگاه ------------
                #region Get Institute Terms
                List<Term> institute_terms = new List<Term>();
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"select * from Terms
	                    where 
	                    Instituteid=@Instituteid 
	                    and (Active is null or Active =1)", conn);

                    cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.BigInt));
                    cmd.Parameters["@Instituteid"].Value = Institute_id;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            institute_terms = (from DataRow dr in dataTable.Rows
                                                     select new Models.Term()
                                                     {
                                                         id = int.Parse(dr["id"].ToString()),
                                                         start_date=dr["start_date"].ToString()!=""?DateTime.Parse(dr["start_date"].ToString()):(DateTime?)null,
                                                         end_date = dr["end_date"].ToString() != "" ? DateTime.Parse(dr["end_date"].ToString()) : (DateTime?)null,
                                                         Term_Number=dr["Term_Number"].ToString()!=""?byte.Parse(dr["Term_Number"].ToString()):(byte?)null,
                                                         Description = dr["Description"].ToString(),
                                                         CurrentTerm=dr["CurrentTerm"].ToString()!=""?bool.Parse(dr["CurrentTerm"].ToString()):false
                                                         
                                                     }
                                  ).ToList();
                            ViewBag.institute_terms = institute_terms;
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
                    viewbagerror.Msg = "خطا در لود نرم های آموزشگاه : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }
                #endregion
                //-------------- لیست ترم های مربوط به آموزشگاه ------------
                //==============================================================

                Term current_term_active = institute_terms.Where(p => p.CurrentTerm == true).FirstOrDefault();

                //------------------------- اگر مدرسه ای دیگر انتخاب شد ترم انتخاب شده خالی شود
                if (Session["Institute_info"] !=null)
                {
                    Institute_Info_Session_VM myinstitute = (Institute_Info_Session_VM)Session["Institute_info"];
                    if(myinstitute.id!= Institute_id)
                    {
                        Session["TermSelect"] = null;
                        
                    }
                }



                //==============================================================
                //----------------------  ترم جاری یا نرم انتخاب شده کاربر -
                int? current_termid=null;
                #region Analyze Current Term  - Selected Term
                if (current_term_active != null)
                {
                    Session["CurrentTerm"] = current_term_active;
                    if (Session["TermSelect"] == null)
                    {
                        Session["TermSelect"] = current_term_active;
                        current_termid = current_term_active.id;
                    }
                    else
                    {
                        Term termselect = (Term)Session["TermSelect"];
                        current_termid = termselect.id;
                    }
                }
                else
                {
                    Session["CurrentTerm"] = null;
                    if (Session["TermSelect"] == null)
                    {
                        Term mytermid_daactive = institute_terms.LastOrDefault();
                        if (mytermid_daactive != null)
                        {
                            Session["CurrentTerm"] = null;
                            Session["TermSelect"] = mytermid_daactive;
                            current_termid = mytermid_daactive.id;
                        }
                    }
                    else
                    {
                        Term termselect = (Term)Session["TermSelect"];
                        current_termid = termselect.id;
                    }
                }
                #endregion
                //----------------------  ترم جاری یا نرم انتخاب شده کاربر -
                //==============================================================


                if (current_termid!=null)
                {

                    //=============================================================
                    // ----------------- لیست کلاس های مدرسه کاربر -----------

                    #region Get Class List by Count Student
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = new SqlCommand(@"select * from
                                (
                                select Classes.id,Classes.Name,Classes.capasity
		                                                            ,(select  Count(Class_User.username) from Class_User where Class_User.Classid=Classes.id and Class_User.Jobsid=3) as student_count
									                                ,ROW_NUMBER() OVER(order by Classes.Name) AS rownum
	                                                             from Classes
                                                            where 
		                                                            Classes.Instituteid =@Instituteid
		                                                            and Classes.Termid =@Termid
                                                                    and Name like @ClassName
		                                                            and (Classes.Active is null or Classes.Active=1)
                                ) as TBL_ClassList
	                                where 
		                                (TBL_ClassList.rownum>@CurrentPage and
				                                TBL_ClassList.rownum<=(@CurrentPage+@PageSize))", conn);

                        cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.BigInt));
                        cmd.Parameters["@Instituteid"].Value = Institute_id;

                        cmd.Parameters.Add(new SqlParameter("@Termid", SqlDbType.Int));
                        cmd.Parameters["@Termid"].Value = current_termid;

                        cmd.Parameters.Add(new SqlParameter("@CurrentPage", SqlDbType.Int));
                        cmd.Parameters["@CurrentPage"].Value = Current_page;

                        cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
                        cmd.Parameters["@PageSize"].Value = pagesize;

                        cmd.Parameters.Add(new SqlParameter("@ClassName", SqlDbType.NVarChar, 50));
                        if (ClassName != null)
                        {
                            cmd.Parameters["@ClassName"].Value = "%" + ClassName + "%";
                        }
                        else
                        {
                            cmd.Parameters["@ClassName"].Value = "%" + "" + "%";
                        }




                        rdr = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();

                        dataTable.Load(rdr);

                        if (dataTable != null)
                        {
                            if (dataTable.Rows.Count > 0)
                            {
                                result_list = (from DataRow dr in dataTable.Rows
                                                   select new Class_List_VM()
                                                   {
                                                       id = int.Parse(dr["id"].ToString()),
                                                       name = dr["Name"].ToString(),
                                                       capacity = dr["capasity"].ToString() != "" ? int.Parse(dr["capasity"].ToString()) : 0,
                                                       student_count = int.Parse(dr["student_count"].ToString())



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
                        viewbagerror.Msg = "خطا در لود کلاس های آموزشگاه : " + ex.Message;
                        ViewBag.ErrorMsg = viewbagerror;
                    }
                    #endregion
                    // ----------------- لیست کلاس های مدرسه یک کاربر -----------
                    //=============================================================

                    //=============================================================
                    //------------- Get Total Item In Class List ---
                    #region Get Total Item In Class List
                    try

                    {
                        if (conn.State != ConnectionState.Open)
                        {
                            conn.Open();
                        }


                        string query = "";
                        #region Search Query 
                        query = @"select count(id) as TotalItem
	                             from Classes
                            where 
		                            Classes.Instituteid =@Instituteid
		                            and Classes.Termid =@Termid
		                            and (Classes.Active is null or Classes.Active=1)
									and Name like @ClassName";


                        #endregion

                        SqlCommand cmd = new SqlCommand(query, conn);

                        cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.BigInt));
                        cmd.Parameters["@Instituteid"].Value = Institute_id;

                        cmd.Parameters.Add(new SqlParameter("@Termid", SqlDbType.Int));
                        cmd.Parameters["@Termid"].Value = current_termid;

                        cmd.Parameters.Add(new SqlParameter("@ClassName", SqlDbType.NVarChar, 50));
                        if (ClassName != null)
                        {
                            cmd.Parameters["@ClassName"].Value = "%" + ClassName + "%";
                        }
                        else
                        {
                            cmd.Parameters["@ClassName"].Value = "%" + "" + "%";
                        }
                        

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
                    //------------- Get Total Item In Class List ---
                    //=============================================================
                    Total_page = Total_item / pagesize;

                    //==========================================
                    //---------------- Set ViewBag -------------

                    ViewBag.Total_item = Total_item;
                    ViewBag.Total_page = Total_page;
                    ViewBag.Current_page = Current_page;

                    //---------------- Set ViewBag -------------
                    //==========================================


                    //=============================================================
                    //--------------- مشخصات آموزشگاه کلی  ----------------------
                    Institute_Info_Session_VM institute_info = new Institute_Info_Session_VM();
                    #region Get Institute Info


                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = new SqlCommand(@"select Institutes.id,Institutes.name,Institutes.boyOrGirl
		                        ,(select InstituteKind.Name from InstituteKind where InstituteKind.id=Institutes.InstituteKindid) as InstituteKindName
	                        from Institutes
	                        where
	                        Institutes.id=@Institutesid
	                        and (Institutes.Active is null or Institutes.Active =1)", conn);

                        cmd.Parameters.Add(new SqlParameter("@Institutesid", SqlDbType.BigInt));
                        cmd.Parameters["@Institutesid"].Value = Institute_id;

                        rdr = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();

                        dataTable.Load(rdr);

                        if (dataTable != null)
                        {
                            if (dataTable.Rows.Count > 0)
                            {
                                DataRow dr = dataTable.Rows[0];

                                institute_info.id = Int64.Parse(dr["id"].ToString());
                                institute_info.name = dr["name"].ToString();
                                institute_info.Institute_Types_Caption = dr["InstituteKindName"].ToString();
                                if (dr["boyOrGirl"].ToString() != "")
                                {
                                    if (bool.Parse(dr["boyOrGirl"].ToString()))
                                    { institute_info.boyOrGirl = "دخترانه"; }
                                    else { institute_info.boyOrGirl = "پسرانه"; }
                                }
                                else
                                {
                                    institute_info.boyOrGirl = "پسرانه";
                                }

                                dataTable.Dispose();
                                
                                Session["Institute_info"] = institute_info;
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
                        viewbagerror.Msg = "خطا در لود اطلاعات آموزشگاه : "+ex.Message;
                        ViewBag.ErrorMsg = viewbagerror;
                    }

                    #endregion
                    //--------------- مشخصات آموزشگاه کلی  ----------------------
                    //=============================================================

                    return View("~/Views/maindashboard/Classes/Index.cshtml", result_list);
                }
                else
                {
                    ViewBagError viewbagerror = new ViewBagError();
                    viewbagerror.ClassName = "alert-danger";
                    viewbagerror.Msg = "برای این آموزشگاه ترمی تعرف نشده است. لطفا ابتدا از گزینه ترم ها ترمی را تعریف کنید .";
                    ViewBag.ErrorMsg = viewbagerror;
                }

            }//instituteid !=null
            else
            {
                ViewBagError viewbagerror = new ViewBagError();
                viewbagerror.ClassName = "alert-danger";
                viewbagerror.Msg = "آموزشگاهی بااین شناسه یافت نشد .";
                ViewBag.ErrorMsg = viewbagerror;
                
            }
            return View("~/Views/maindashboard/Index.cshtml");
        }

        [Route("Classes/Create")]
        public ActionResult Create(Class myclass)
        {
            if (Session["Institute_info"]!=null)
            {
                Institute_Info_Session_VM Institute_info = (Institute_Info_Session_VM)Session["Institute_info"];
                if (Institute_info != null)
                {
                    //--------- گرفتن ترم انتخاب شده
                    Term termselect = new Term();
                    if (Session["TermSelect"] != null)
                    {
                        termselect = (Term)Session["TermSelect"];
                        User userinfo = (User)Session["User"];


                        App_Start.ConnectionString constr = new App_Start.ConnectionString();
                        ConnectionString = constr.GetConnectionString();
                        SqlConnection conn = new SqlConnection(ConnectionString);
                        SqlDataReader rdr = null;

                        //=================================================
                        //------------ اضافه کردن کلاس -------------------
                        #region Create Class in Institute And Term Selected
                        
                        //=====================================
                        //------- Get Id For Class Table ------
                        int myclassid = 0;
                        #region Get Max Id For Class Table

                        try
                        {
                            if (conn.State != ConnectionState.Open)
                                conn.Open();

                            SqlCommand cmd = new SqlCommand(@"select Max(id) as Maxid from Classes", conn);

                            rdr = cmd.ExecuteReader();
                            DataTable dataTable = new DataTable();

                            dataTable.Load(rdr);

                            if (dataTable != null)
                            {
                                if (dataTable.Rows.Count > 0)
                                {
                                    DataRow dr = dataTable.Rows[0];
                                    myclassid = int.Parse(dr["Maxid"].ToString());
                                    dataTable.Dispose();
                                }
                            }
                        }
                        catch (Exception)
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
                            myclassid = 0;
                        }

                        #endregion
                        myclassid++;
                        //------- Get Id For Class Table ------
                        //=====================================

                        #region Add Class To Classes TBL

                        try
                        {
                            if (conn.State != ConnectionState.Open)
                                conn.Open();

                            SqlCommand cmd = new SqlCommand(@"INSERT INTO Classes
                                                     (id, Name, capasity, Active, Instituteid, Termid, Gradeid,Creator)
                            VALUES        (@id, @Name, @capasity, @Active, @Instituteid, @Termid, @Gradeid,@Creator)", conn);

                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                            cmd.Parameters["@id"].Value = myclassid;

                            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar,50));
                            cmd.Parameters["@Name"].Value = myclass.Name;

                            cmd.Parameters.Add(new SqlParameter("@capasity", SqlDbType.Int));
                            cmd.Parameters["@capasity"].Value = myclass.capasity;

                            cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit));
                            cmd.Parameters["@Active"].Value = DBNull.Value;//----------->>>>> در ویو نیست

                            cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.Int));
                            cmd.Parameters["@Instituteid"].Value = Institute_info.id;

                            cmd.Parameters.Add(new SqlParameter("@Termid", SqlDbType.Int));
                            cmd.Parameters["@Termid"].Value = termselect.id;

                            cmd.Parameters.Add(new SqlParameter("@Gradeid", SqlDbType.Int));
                            if (myclass.Gradeid != null)
                                cmd.Parameters["@Gradeid"].Value = myclass.Gradeid;
                            else
                                cmd.Parameters["@Gradeid"].Value = DBNull.Value;

                            cmd.Parameters.Add(new SqlParameter("@Creator", SqlDbType.NVarChar, 50));
                            cmd.Parameters["@Creator"].Value = userinfo.username;

                            cmd.ExecuteNonQuery();

                            ViewBagError myerror = new ViewBagError();
                            myerror.ClassName = "success";
                            myerror.Msg = "کلاس با موفقیت ایجاد شد .";
                            return RedirectToAction("Index");
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
                            viewbagerror.Msg = "خطا در ایجاد کلاس"+ " : "+ ex.Message;
                            ViewBag.ErrorMsg = viewbagerror;
                        }
                        #endregion
                        
                        #endregion

                        //------------ اضافه کردن کلاس -------------------
                        //=================================================
                        
                        return RedirectToAction("Index");
                    }


                }
            }

            return RedirectToAction("Index");
        }

        [Route("Classes/Edit")]
        public ActionResult Edit(Class myclass)
        {
            if (myclass != null)
            {
                User userinfo = (User)Session["User"];

                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;

                //==================================================
                //------------- Update Class ---
                #region Update Classes
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE       Classes
                    SET                Name =@Name, capasity =@capasity , Gradeid =@Gradeid , Creator =@Creator 
	                    where id=@id", conn);

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters["@id"].Value = myclass.id;

                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Name"].Value = myclass.Name;

                    cmd.Parameters.Add(new SqlParameter("@capasity", SqlDbType.Int));
                    if (myclass.capasity != null)
                    {
                        cmd.Parameters["@capasity"].Value = myclass.capasity;
                    }
                    else
                    {
                        cmd.Parameters["@capasity"].Value = 0;
                    }

                    cmd.Parameters.Add(new SqlParameter("@Gradeid", SqlDbType.Int));
                    if (myclass.Gradeid != null)
                        cmd.Parameters["@Gradeid"].Value = myclass.Gradeid;
                    else
                        cmd.Parameters["@Gradeid"].Value = DBNull.Value;

                    cmd.Parameters.Add(new SqlParameter("@Creator", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Creator"].Value = userinfo.username;

                    cmd.ExecuteNonQuery();

                    ViewBagError myerror = new ViewBagError();
                    myerror.ClassName = "success";
                    myerror.Msg = "کلاس با موفقیت ویرایش شد .";
                    return RedirectToAction("Index");

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
                    viewbagerror.Msg = "خطا در ویرایش کلاس" + " : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;

                }
                #endregion
                //------------- Update Class ---
                //==================================================

            }
            ViewBagError myerror2 = new ViewBagError();
            myerror2.ClassName = "danger";
            myerror2.Msg = "ویرایش کلاس انجام نشد .";
            return RedirectToAction("Index");
        }

        [Route("Classes/Delete/{id?}")]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                User userinfo = (User)Session["User"];

                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;

                //==================================================
                //----- Delete Class_User Fro Delete Class ---
                #region Delete Class_User Fro Delete Class
                //-------- حذف نمی کنیم تنها غیر فعالش میکنیم
                #endregion
                //----- Delete Class_User Fro Delete Class ---
                //==================================================

                //==================================================
                //-------- DeActive Class For Delete Class ---
                #region DeActive Class For Delete Class
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE       Classes
                    SET                Active =0 where id =@id 
					                    ", conn);

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters["@id"].Value = id;

                    cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@username"].Value = userinfo.username;

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
                    viewbagerror.Msg = "خطا در حذف کلاس" + " : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;

                }
                #endregion
                //-------- DeActive Class For Delete Class ---
                //==================================================

                return RedirectToAction("Index");

            }
            ViewBagError myerror2 = new ViewBagError();
            myerror2.ClassName = "danger";
            myerror2.Msg = "حذف کلاس انجام نشد .";
            return RedirectToAction("Index");
        }




        [Route("Classes/SwichTerm/{id}")]
        public ActionResult SwichTerm(int id)
        {
            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            if (Session["Institute_info"] != null)
            {
                Institute_Info_Session_VM myinstitute = (Institute_Info_Session_VM)Session["Institute_info"];

                if (myinstitute != null)
                {
                    //---- گرفتن ترم انتخاب شده
                    #region Get Selected Term
                    Term institute_terms_selected = new Term();
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = new SqlCommand(@"select * from Terms
	                        where 
	                        Instituteid=@Instituteid 
	                        and Terms.id = @Termid
	                        and (Active is null or Active =1)", conn);

                        cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.Int));
                        cmd.Parameters["@Instituteid"].Value = myinstitute.id;

                        cmd.Parameters.Add(new SqlParameter("@Termid", SqlDbType.Int));
                        cmd.Parameters["@Termid"].Value = id;

                        rdr = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();

                        dataTable.Load(rdr);

                        if (dataTable != null)
                        {
                            if (dataTable.Rows.Count > 0)
                            {
                                DataRow dr = dataTable.Rows[0];
                                institute_terms_selected.id = int.Parse(dr["id"].ToString());
                                institute_terms_selected.start_date = dr["start_date"].ToString() != "" ? DateTime.Parse(dr["start_date"].ToString()) : (DateTime?)null;
                                institute_terms_selected.end_date = dr["end_date"].ToString() != "" ? DateTime.Parse(dr["end_date"].ToString()) : (DateTime?)null;
                                institute_terms_selected.Term_Number = dr["Term_Number"].ToString() != "" ? byte.Parse(dr["Term_Number"].ToString()) : (byte?)null;
                                institute_terms_selected.Description = dr["Description"].ToString();
                                institute_terms_selected.CurrentTerm = dr["CurrentTerm"].ToString() != "" ? bool.Parse(dr["CurrentTerm"].ToString()) : false;
                                institute_terms_selected.Instituteid = myinstitute.id;

                                Session["TermSelect"] = institute_terms_selected;

                                dataTable.Dispose();
                            }
                        }
                    }
                    catch (Exception)
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
                }

            }



            return RedirectToAction("Index");
        }


        [Route("Classes/Load/{classid?}")]
        public ActionResult Load(int? classid)
        {
            if(classid!=null)
            {
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;


                //==================================================
                //----------- Get Class Info For Modal ---
                Class myclass = new Class();
                #region Get Class Info For Modal

                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"select * from Classes where
	                id = @id
	                and (Active is null or Active =1)", conn);

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                    cmd.Parameters["@id"].Value = classid;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow dr = dataTable.Rows[0];

                            myclass.id = int.Parse(dr["id"].ToString());
                            myclass.Name = dr["Name"].ToString();
                            myclass.capasity = dr["capasity"].ToString() != "" ? int.Parse(dr["capasity"].ToString()) : 0;
                            myclass.Gradeid = dr["Gradeid"].ToString() != "" ? int.Parse(dr["Gradeid"].ToString()) : 0;
                            
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
                    viewbagerror.Msg = "خطا در لود کلاس : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }

                #endregion
                //----------- Get Class Info For Modal ---
                //==================================================
                if(myclass!=null)
                {
                    return Json(new { myclass = myclass }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("کلاس مورد نظر یافت نشد.");
                }
                
            }
            else
            {
                throw new Exception("شناسه کلاس صحیح نیست");
            }

            
        }

        

        


        [Route("Classes/ClassInfo/{Class_id?}")]
        public ActionResult ClassInfo(int? Class_id)
        {
            if(Class_id!=null)
            {

                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;

                //=========================================
                //-----------------  Get Class Info -------
                ClassInfo_VM class_info = new ClassInfo_VM();
                #region Get Class Info
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"select id,Name,capasity,Instituteid,Termid,Gradeid 
	                from Classes
	                where
		                id= @id
		                and (Active is null or Active =1)
                ", conn);


                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters["@id"].Value = Class_id;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow dr = dataTable.Rows[0];

                            class_info.id = int.Parse(dr["id"].ToString());
                            class_info.Name = dr["Name"].ToString();
                            class_info.capasity = dr["capasity"].ToString()!=""?int.Parse(dr["capasity"].ToString()):0;
                            class_info.Instituteid= dr["Instituteid"].ToString() != "" ? Int64.Parse(dr["Instituteid"].ToString()) :0;
                            class_info.Termid= dr["Termid"].ToString() != "" ? int.Parse(dr["Termid"].ToString()) : 0;
                            class_info.Gradeid= dr["Gradeid"].ToString() != "" ? Int64.Parse(dr["Gradeid"].ToString()) : 0;
                            Session["Class_info"] = class_info;

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
                    viewbagerror.Msg = "خطا در لود اطلاعات کلاس : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }
                #endregion
                //-----------------  Get Class Info -------
                //=========================================

                if (class_info.id!=null)
                {
                    //=========================================
                    //---- Get Teacher List For This Class ----
                    List<TeacherList_VM> TeacherList = new List<TeacherList_VM>();
                    #region Get Teacher List For This Class
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = new SqlCommand(@"select  Users.username,Users.firstname,Users.lastname 
	                        from Class_User 
                        inner Join Users ON
		                        Class_User.Classid=@Classid 
		                        and Class_User.Jobsid=2
		                        and Users.username like Class_User.username
		                        and (Users.Active is null or Users.Active = 1)
                        ", conn);

                        cmd.Parameters.Add(new SqlParameter("@Classid", SqlDbType.BigInt));
                        cmd.Parameters["@Classid"].Value = class_info.id;
                        
                        rdr = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();

                        dataTable.Load(rdr);

                        if (dataTable != null)
                        {
                            if (dataTable.Rows.Count > 0)
                            {
                                TeacherList = (from DataRow dr in dataTable.Rows
                                               select new TeacherList_VM()
                                               {
                                                   username = dr["username"].ToString(),
                                                   firstname = dr["firstname"].ToString(),
                                                   lastname = dr["lastname"].ToString()
                                                   
                                               }
                                      ).ToList();
                                ViewBag.TeacherList = TeacherList;
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
                        viewbagerror.Msg = "خطا در لود لیست دبیران : " + ex.Message;
                        ViewBag.ErrorMsg = viewbagerror;
                    }
                    #endregion
                    //---- Get Teacher List For This Class ----
                    //=========================================
                }

            }

            return View("~/Views/maindashboard/Classes/Class_Info.cshtml");
        }


        [Route("Classes/BarnameHaftegiPartial")]
        public ActionResult BarnameHaftegiPartial()
        {
            List<Schedule_VM> SP_Result = new List<Schedule_VM>();
            if (Session["Class_info"]!=null)
            {
                ClassInfo_VM class_info = (ClassInfo_VM)Session["Class_info"];

                int? classid = class_info.id;

                //=====================================================================================
                //---------------------------   برنامه هفتگی  ---------

                SP_Result = db.Database.SqlQuery<Schedule_VM>("SP_BANAME_HAFTEGI @Classid", new SqlParameter("@Classid", classid)).ToList();


                //---------------------------   برنامه هفتگی  ---------
                //======================================================================================

                return PartialView("~/Views/Shared/Partial/Class/_SchedulesPartial.cshtml", SP_Result);
            }
            return PartialView("~/Views/Shared/Partial/Class/_SchedulesPartial.cshtml",null);
        }


        //====================================================================
        //--------------------------- نمایش برنامه هفتگی مودال ------------

        [Route("Classes/viewBarnameHaftegiPartial")]
        public ActionResult viewBarnameHaftegiPartial()
        {
            List<Schedule_VM> SP_Result = new List<Schedule_VM>();
            if (Session["Class_info"] != null)
            {
                ClassInfo_VM class_info = (ClassInfo_VM)Session["Class_info"];

                int? classid = class_info.id;

                //=====================================================================================
                //---------------------------   برنامه هفتگی  ---------

                SP_Result = db.Database.SqlQuery<Schedule_VM>("SP_BANAME_HAFTEGI @Classid", new SqlParameter("@Classid", classid)).ToList();


                //---------------------------   برنامه هفتگی  ---------
                //======================================================================================

                return PartialView("~/Views/Shared/Partial/Class/_ViewSchedulesPartial.cshtml", SP_Result);
            }
            return PartialView("~/Views/Shared/Partial/Class/_ViewSchedulesPartial.cshtml", null);
        }

        //--------------------------- نمایش برنامه هفتگی مودال ------------
        //====================================================================

        //==========================================
        //---------- Week Procedure Result ---------
        private class sp_week_result
        {
            public byte? order_week { get; set; }
            public string NameOfWeek { get; set; }
        }
        //---------- Week Procedure Result ---------
        //==========================================



        [HttpGet]
        [Route("Classes/Schedule/{order_week?}")]
        public ActionResult CreateSchedule(int? order_week)
        {
            Institute_Info_Session_VM Institute_info = (Institute_Info_Session_VM) Session["Institute_info"];
            if (Institute_info != null)
            {
                ViewBag.Institute_info = Institute_info;
            }

            ClassInfo_VM class_info = (ClassInfo_VM)Session["Class_info"];
            if(class_info!=null)
            {
                ViewBag.class_info = class_info;
            }

            //=====================================================
            //------------------- Teacher List From Institute_User --------------------
            SqlParameter[] parameters ={
                         new SqlParameter("@Instituteid",Institute_info.id),
                         new SqlParameter("@Termid",class_info.Termid),
                         };
            List<Username_FN_LNVM> Teachers_list = db.Database.SqlQuery<Username_FN_LNVM>("SP_INSTITUTE_TEACHERS @Instituteid,@Termid", parameters).ToList();
            //Username_FN_LNVM temp_teacher = new Username_FN_LNVM() {username="0",firstname="انتخاب کنید ...",lastname="" };
            //Teachers_list.Add(temp_teacher);
            Teachers_list = Teachers_list.OrderBy(p => p.lastname).ToList();
            ViewBag.teacherlist = Teachers_list;
            //------------------- Teacher List From Institute_User --------------------
            //=====================================================

            //=====================================================
            //------------------- Course List From Course Total --------------------
            SqlParameter[] param ={
                         new SqlParameter("@Instituteid",Institute_info.id),
                         new SqlParameter("@Termid",class_info.Termid),
                         };
            List<IdNameVM> Course_list = db.Database.SqlQuery<IdNameVM>("SP_INSTITUTE_COURSETOTAL @Instituteid,@Termid", param).ToList();
            //IdNameVM temp_course = new IdNameVM() { id=0,name="انتخاب کنید ..."};
            //Course_list.Add(temp_course);
            Course_list=Course_list.OrderBy(p => p.name).ToList();
            ViewBag.courselist = Course_list;
            //------------------- Course List From Course Total ---
            //=====================================================

            //=====================================================
            //------------------- Week Day List Distinct ----------

            SqlParameter[] paramweek ={
                         new SqlParameter("@Classid",class_info.id)
                         };
            List<sp_week_result> Weekdaylist = db.Database.SqlQuery<sp_week_result>("SP_WEEKDAY_SCHEDULE @Classid", paramweek).ToList();
            List<IdNameVM> weekresult = new List<IdNameVM>() {
                new IdNameVM() {id=1,name="شنبه" },
                new IdNameVM() {id=2,name="یک شنبه" },
                new IdNameVM() {id=3,name="دوشنبه" },
                new IdNameVM() {id=4,name="سه شنبه" },
                new IdNameVM() {id=5,name="چهارشنبه" },
                new IdNameVM() {id=6,name="پنج شنبه" },
                new IdNameVM() {id=7,name="جمعه" }

            } ;

            foreach (var item in Weekdaylist)
            {
                var itemtoremove = weekresult.SingleOrDefault(p => p.id == item.order_week);
                if(itemtoremove!=null)
                    weekresult.Remove(itemtoremove);
            }

            ViewBag.weekday = weekresult;

            //------------------- Week Day List Distinct ----------
            //=====================================================

            //=====================================================================================
            //------------------------------ Get Schedule For EDIT --------------------------------
            if(order_week != null)
            {
                List<Schedule> myschedules = db.Schedules.Where(
                                p => p.Classid == class_info.id
                                && p.order_week == order_week)
                                .OrderBy(p=>p.order_ring)
                                .ToList();
                ViewBag.currentday = order_week;
                IdNameVM weekday_edit = new IdNameVM();
                weekday_edit.id = (int)myschedules.ElementAt(0).order_week;

                weekday_edit.name = myschedules.ElementAt(0).NameOfWeek;
                weekresult.Add(weekday_edit);
                weekresult = weekresult.OrderBy(p => p.id).ToList();
                ViewBag.weekday = weekresult;
                ViewBag.EC = "1";
                Session["Old_myschedules"] = myschedules;//------- برای مقایسه در ویرایش
                return View("~/Views/maindashboard/Classes/Schedule.cshtml",myschedules);
            }
            else
            {
                ViewBag.EC = "0";
                return View("~/Views/maindashboard/Classes/Schedule.cshtml", null);
            }
            //------------------------------ Get Schedule For EDIT --------------------------------
            //=====================================================================================

            
        }

        
        [Route("Classes/InsertSchedule")]
        public ActionResult InsertSchedule(FormCollection fomr)
        {
            List<Schedule> Tbl_schedules = new List<Schedule>();

            //---- گرفتن برنامه هفتگی قدیمی در صورت وجود
            List<Schedule> Old_myschedules = null;
            //--- لیست برنامه گرفته شده از فرم
            List<Schedule> myschedule_getform = new List<Schedule>();

            if (Session["Old_myschedules"]!=null)
            {
                Old_myschedules = (List<Schedule>)Session["Old_myschedules"];
            }
            ClassInfo_VM class_info = (ClassInfo_VM)Session["Class_info"];

            if (class_info != null)
            {

                //===========================================================================
                //-------------------------- گرفتن لیست برنامه از فرم --------------------


                //---- تعداد ستون های برنامه
                int countcol = 0;
                if (Request.Form["countcol"] != null)
                {
                    string col = Request.Form["countcol"].Split(',')[0];
                    countcol = int.Parse(col);
                }

                int i = 1;
                int counter_get_col = 0;
                while (counter_get_col < countcol)
                {
                    Schedule schedule = new Schedule();

                    bool iscolvalid = false;

                    //================================================================
                    //-------------------- Get Order Week ----------------------------
                    if (Request.Form["orderweek"] != null)
                    {
                        string order_week = Request.Form["orderweek"];
                        schedule.order_week = byte.Parse(order_week);
                        //iscolvalid = true;
                    }

                    //-------------------- Get Order Week ----------------------------
                    //================================================================

                    
                    //================================================================
                    //-------------------- Get Start Time ----------------------------
                    if (Request.Form["startdate_" + i] != null)
                    {
                        StringClass_Convert classconvert = new StringClass_Convert();

                        string startdate = Request.Form["startdate_" + i];

                        if(startdate!="")
                        {
                            startdate = startdate.Substring(0, 6).Replace(" ", "");

                            startdate = classconvert.GetEnglishNumber(startdate);

                            TimeSpan ts = TimeSpan.Parse(startdate);
                            DateTime sd1 = DateTime.Now;
                            sd1 = sd1.Date + ts;

                            schedule.start_time = sd1;
                            iscolvalid = true;
                        }
                        

                    }
                    //-------------------- Get Start Time ----------------------------
                    //================================================================

                    //================================================================
                    //-------------------- Get End Time ------------------------------
                    if (Request.Form["enddate_" + i] != null)
                    {
                        StringClass_Convert classconvert = new StringClass_Convert();

                        string enddate = Request.Form["enddate_" + i];

                        if(enddate!="")
                        {
                            enddate = enddate.Substring(0, 6).Replace(" ", "");

                            enddate = classconvert.GetEnglishNumber(enddate);

                            TimeSpan ts = TimeSpan.Parse(enddate);
                            DateTime sd2 = DateTime.Now;
                            sd2 = sd2.Date + ts;

                            schedule.end_time = sd2;
                            iscolvalid = true;
                        }
                        

                    }
                    //-------------------- Get End Time ------------------------------
                    //================================================================
                                                            
                    //================================================================
                    //-------------------- Get Teacher 1 -----------------------------
                    if (Request.Form["teacher1_" + i] != null)
                    {
                        string teacher1 = Request.Form["teacher1_" + i];
                        if(teacher1!="0")
                        {
                            schedule.Teacher_one = teacher1;
                            iscolvalid = true;
                        }
                        
                    }
                    //-------------------- Get Teacher 1 -----------------------------
                    //================================================================

                    //================================================================
                    //-------------------- Get Course 1 ------------------------------
                    if (Request.Form["course1_" + i] != null)
                    {
                        string course1 = Request.Form["course1_" + i];
                        if(course1!="0")
                        {
                            schedule.Courseid_one = int.Parse(course1);
                            iscolvalid = true;
                        }
                        
                    }
                    //-------------------- Get Course 1 ------------------------------
                    //================================================================

                    //================================================================
                    //-------------------- Get Type Ring -----------------------------
                    if (Request.Form["ringtype_" + i] != null)
                    {
                        string ringtype = Request.Form["ringtype_" + i];
                        schedule.Type_Ring = byte.Parse(ringtype);
                        //iscolvalid = true;
                        if(ringtype=="1" || ringtype=="2")
                        {
                            //================================================================
                            //-------------------- Get Teacher 2 -----------------------------
                            if (Request.Form["teacher2_" + i] != null)
                            {
                                string teacher2 = Request.Form["teacher2_" + i];
                                if(teacher2!="0")
                                {
                                    schedule.Teacher_two = teacher2;
                                    iscolvalid = true;
                                }
                                
                            }
                            //-------------------- Get Teacher 2 -----------------------------
                            //================================================================

                            //================================================================
                            //-------------------- Get Course 2 ------------------------------
                            if (Request.Form["course2_" + i] != null)
                            {
                                string course2 = Request.Form["course2_" + i];
                                if(course2!="0")
                                {
                                    schedule.Courseid_two = int.Parse(course2);
                                    iscolvalid = true;
                                }
                                
                            }
                            //-------------------- Get Course 2 ------------------------------
                            //================================================================
                        }
                    }
                    //-------------------- Get Type Ring -----------------------------
                    //================================================================

                    //================================================================
                    //-------------------- Get ScheduleId  ------------------------------
                    if (Request.Form["schedule_" + i] != null)
                    {
                        string scheduleid = Request.Form["schedule_" + i];
                        if(scheduleid!="0")
                        {
                            schedule.id = int.Parse(scheduleid);
                            iscolvalid = true;
                        }
                        
                    }
                    //-------------------- Get ScheduleId ------------------------------
                    //================================================================
                    schedule.Classid = (int)class_info.id;
                    schedule.order_ring = byte.Parse((counter_get_col + 1).ToString());
                    
                    if (iscolvalid)
                    {
                        myschedule_getform.Add(schedule);
                        
                    }
                    counter_get_col++;
                    i++;
                }
                //-------------------------- گرفتن لیست برنامه از فرم --------------------
                //===========================================================================

            foreach(var item in myschedule_getform)
                {
                    Schedule myschedule_EC = new Schedule();

                    

                    if(item.id==0)
                    {
                        //------------ برنامه هفتگی جدید است 

                        int myscheduleid = 0;
                        try
                        {
                            myscheduleid = db.Schedules.Max(p => p.id);
                        }
                        catch (Exception ex) {; }

                        myscheduleid++;

                        
                        myschedule_EC.id = myscheduleid;
                        myschedule_EC.Classid = item.Classid;
                        myschedule_EC.Courseid_one = item.Courseid_one;
                        myschedule_EC.Courseid_two = item.Courseid_two;
                        myschedule_EC.end_time = item.end_time;
                        myschedule_EC.start_time = item.start_time;
                        myschedule_EC.Teacher_one = item.Teacher_one;
                        myschedule_EC.Teacher_two = item.Teacher_two;
                        myschedule_EC.Type_Ring = item.Type_Ring;
                        myschedule_EC.order_ring = item.order_ring;
                        myschedule_EC.order_week = item.order_week;
                        myschedule_EC.NameOfWeek = nameofweekfunc(item.order_week);

                        db.Schedules.Add(myschedule_EC);
                        db.SaveChanges();
                        //----------- اگر این کاربر در جدول کلاس یوزر نبود اضافه کن
                        if(!check_Class_User_Exist(item.Classid,item.Teacher_one,item.Courseid_one))
                        {
                            // ---- اگر نبود اضافه کن
                            add_Class_User(item.Classid, item.Teacher_one, item.Courseid_one);
                            
                        }
                        //---- اگر چرخشی یا تک زنگ بود
                        if(myschedule_EC.Type_Ring==1 || myschedule_EC.Type_Ring==2)
                        {
                            if (!check_Class_User_Exist(item.Classid, item.Teacher_two, item.Courseid_two))
                            {
                                // ---- اگر نبود اضافه کن
                                add_Class_User(item.Classid, item.Teacher_two, item.Courseid_two);

                            }
                        }

                    }
                    else
                    {
                        //----- برنامه هفتگی از قبل بوده
                        Schedule old_schedule = Old_myschedules.Where(p => p.id == item.id).FirstOrDefault();
                        if(old_schedule != null)
                        {
                            // ----  اگر دبیر اول و درس اول تغییر داشته
                            if(item.Teacher_one!= old_schedule.Teacher_one || item.Courseid_one!= old_schedule.Courseid_one)
                            {
                                if (old_schedule.Teacher_one == null || old_schedule.Courseid_one == null)
                                {
                                    //--- اگر برنامه تغییر داشته و جدید است
                                    add_Class_User(item.Classid, item.Teacher_one, item.Courseid_one);
                                }
                                else if (item.Teacher_one == null || item.Courseid_one == null)
                                {
                                    //---- برنامه درس اول حذف شد
                                    delete_Class_User(old_schedule.Classid,old_schedule.Teacher_one,old_schedule.Courseid_one);
                                    if ((item.Teacher_one == null || item.Courseid_one == null) && (item.Teacher_two == null || item.Courseid_two == null) )
                                        Delete_Schedule(old_schedule.id);
                                }
                                else
                                {
                                    //---- ویرایش کلاس یوزر
                                    Edit_Class_User(item.Classid, item.Teacher_one, item.Courseid_one, old_schedule.Classid, old_schedule.Teacher_one, old_schedule.Courseid_one);
                                }
                            }
                            
                            
                            //---- اگر دبیر دوم و درس دوم تغییر داشته
                            if (item.Teacher_two != old_schedule.Teacher_two || item.Courseid_two != old_schedule.Courseid_two)
                            {
                                //---- اگر دبیر دوم و درس دوم تغییر داشته
                                if(old_schedule.Teacher_two==null || old_schedule.Courseid_two==null)
                                {
                                    //--- اگر برنامه تغییر داشته و جدید است
                                    add_Class_User(item.Classid, item.Teacher_two, item.Courseid_two);
                                }
                                else if(item.Teacher_two==null || item.Courseid_two==null)
                                {
                                    //---- برنامه درس دوم حذف شد
                                    delete_Class_User(old_schedule.Classid, old_schedule.Teacher_two, old_schedule.Courseid_two);
                                    if ((item.Teacher_one == null || item.Courseid_one == null) && (item.Teacher_two == null || item.Courseid_two == null))
                                        Delete_Schedule(old_schedule.id);
                                }
                                else
                                {
                                    //---  فقط مدرس و درس تغییر کرده
                                    Edit_Class_User(item.Classid, item.Teacher_two, item.Courseid_two, old_schedule.Classid, old_schedule.Teacher_two, old_schedule.Courseid_two);
                                }

                            }
                            // --- ثبت تغییرات در برنامه هفتگی بانک
                            Schedule current_schedule= db.Schedules.Where(p => p.id == item.id).FirstOrDefault();
                            if(current_schedule!=null)
                            {
                                current_schedule.Classid = item.Classid;
                                current_schedule.Courseid_one = item.Courseid_one;
                                current_schedule.Courseid_two = item.Courseid_two;
                                current_schedule.end_time = item.end_time;
                                current_schedule.start_time = item.start_time;
                                current_schedule.Teacher_one = item.Teacher_one;
                                current_schedule.Teacher_two = item.Teacher_two;
                                // ------------ تنظیم خودکار نوع زنگ
                                current_schedule.Type_Ring = item.Type_Ring;
                                if ((item.Teacher_one==null || item.course_one==null) && (item.Teacher_two != null || item.course_two != null))
                                {
                                    current_schedule.Type_Ring = 0;
                                }
                                else if((item.Teacher_one != null || item.course_one != null) && (item.Teacher_two == null || item.course_two == null))
                                {
                                    current_schedule.Type_Ring = 0;
                                }
                                //---------------------------------
                                
                                current_schedule.order_ring = item.order_ring;
                                current_schedule.order_week = item.order_week;
                                current_schedule.NameOfWeek = nameofweekfunc(item.order_week);

                                db.SaveChanges();
                            }
                        }

                    }


                }// foreach item in form

            //---------- اگر دکمه کم کردن ستون رو زده بود
            if(Old_myschedules!=null)
                {
                    if (Old_myschedules.Count > myschedule_getform.Count)
                    {
                        int count_old = Old_myschedules.Count;
                        int count_new = myschedule_getform.Count;

                        for (int j = 0; j < (count_old - count_new); j++)
                        {
                            //--- گرفتن آیتم های ‍‍‍‍‍‍پاک شده
                            Schedule schedule_del = Old_myschedules.LastOrDefault();
                            //---- حدف از کلاس یوزر
                            delete_Class_User(schedule_del.Classid, schedule_del.Teacher_one, schedule_del.Courseid_one);
                            delete_Class_User(schedule_del.Classid, schedule_del.Teacher_two, schedule_del.Courseid_two);
                            //----- حذف برنامه
                            Delete_Schedule(schedule_del.id);





                        }
                    }
                }
            
            }

            Session["Old_myschedules"] = null;
            Session.Remove("Old_myschedules");

            db.Dispose();

            return RedirectToAction("CreateSchedule");
        }


        [Route("Classes/DeleteSchedule/{order_week?}")]
        public ActionResult DeleteSchedule(int? order_week)
        {
            
            ClassInfo_VM class_info = (ClassInfo_VM)Session["Class_info"];
            if (class_info != null)
            {
                List<Schedule> DelSchedules = db.Schedules.Where(
                                p => p.Classid == class_info.id
                                && p.order_week == order_week)
                                .ToList();
                foreach (var item in DelSchedules)
                {
                    delete_Class_User(item.Classid, item.Teacher_one, item.Courseid_one);
                    delete_Class_User(item.Classid, item.Teacher_two, item.Courseid_two);
                    Delete_Schedule(item.id);
                    

                }

            }

            //=====================================================

            Session["Old_myschedules"] = null;
            Session.Remove("Old_myschedules");


            return RedirectToAction("CreateSchedule");
        }

        [Route("Classes/CancelSchedule")]
        public ActionResult CancelSchedule()
        {
            Session["Old_myschedules"] = null;
            Session.Remove("Old_myschedules");
            return RedirectToAction("CreateSchedule");
        }

        private bool check_Class_User_Exist(int Classid,string username,int? Courseid)
        {
            bool result = false;
            var r = db.Class_Users.Where(p => p.Classid == Classid && p.username == username && p.Courseid == Courseid).Select(p=>p.id).Count();
            if(r>0)
            {
                result = true;
            }
            return result;
        }

        private void add_Class_User(int Classid, string username, int? Courseid)
        {
            if(username!=null && Courseid!=null)
            {
                Class_User class_user = new Class_User();

                class_user.Classid = Classid;
                class_user.Courseid = Courseid;
                class_user.username = username;
                db.Class_Users.Add(class_user);
                db.SaveChanges();

            }
            
        }

        private void Edit_Class_User(int Classid_new, string username_new, int? Courseid_new, int Classid_old, string username_old, int? Courseid_old)
        {
            Class_User class_user = db.Class_Users.Where(p=>p.Classid==Classid_old && p.username==username_old && p.Courseid==Courseid_old).FirstOrDefault();
            if(class_user!=null)
            {
                class_user.Classid = Classid_new;
                class_user.Courseid = Courseid_new;
                class_user.username = username_new;
                db.SaveChanges();
            }
            
        }


        private void delete_Class_User(int Classid, string username, int? Courseid)
        {
            //---------- اگر در برنامه هفتگی این کلاس با این درس دبیر باز کلاس دارد \اک نکن
            int count = db.Schedules.Where(
                                            p =>( p.Teacher_one == username || p.Teacher_two == username)
                                            && (p.Courseid_one==Courseid || p.Courseid_two==Courseid)
                                            && p.Classid==Classid
                                            ).Select(p=>p.id).Count();
            if(count==1)
            {
                Class_User class_user = db.Class_Users.Where(p => p.Classid == Classid && p.username == username && p.Courseid == Courseid).FirstOrDefault();
                if (class_user != null)
                {
                    db.Class_Users.Remove(class_user);
                    db.SaveChanges();
                }
            }
            
                
        }

        private void Delete_Schedule(int scheduleid)
        {
            Schedule myschedule = db.Schedules
                                            .Where(p => p.id == scheduleid)
                                            .FirstOrDefault();
            if(myschedule!=null)
            {
                db.Schedules.Remove(myschedule);
                db.SaveChanges();
            }
        }

        private string nameofweekfunc(byte? id)
        {
            if (id == 1)
            {
                return "شنبه";
            }
            else if (id == 2) { return "یک شنبه"; }
            else if (id == 3) { return "دوشنبه"; }
            else if (id == 4) { return "سه شنبه"; }
            else if (id == 4) { return "چهارشنبه"; }
            else if (id == 4) { return "پنج شنبه"; }
            else if (id == 4) { return "جمعه"; }
            else { return "?"; }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}