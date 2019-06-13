using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using global::Models;
using PagedList;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Peivandyar_Site.Controllers
{
    

    [LogFilter(mandatory:true)]
    public class HomeController : Controller
    {
        private string ConnectionString;

        DataContext db = new DataContext();

        [LogFilter(mandatory: false)]
        public class ostan_city
        {
            public int Code { get; set; }
            public string Pname { get; set; }
        }

        [Route("")]
        [Route("Home")]
        [Route("Index")]
        public ActionResult Index()
        {
            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            //============================================================
            //------------- Get Institute 4 Selected In Home ---------
            List<ViewModel.InstituteSmallVM> InstituteList = new List<ViewModel.InstituteSmallVM>();
            #region Get Institute 4 Selected In Home
            try

            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string query = "";
                
                query = @"select top 4 id,name,En_Name
			            ,address,boyOrGirl,(select Name from InstituteKind where InstituteKind.id=Institutes.InstituteKindid ) as InstituteKindName 
			            from Institutes
			            where
			            (Active is null or Active =1 )
			            order by [order]";
                
                SqlCommand cmd = new SqlCommand(query, conn);

                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        InstituteList = (from DataRow dr in dataTable.Rows
                                         select new ViewModel.InstituteSmallVM()
                                         {
                                             id = Int64.Parse(dr["id"].ToString()),
                                             name = dr["name"].ToString(),
                                             En_Name = dr["En_Name"].ToString(),
                                             InstituteKindName = dr["InstituteKindName"].ToString(),
                                             address = dr["address"].ToString(),
                                             boyOrGirl = dr["boyOrGirl"].ToString() != "" ? bool.Parse(dr["boyOrGirl"].ToString()) : (bool?)null
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
                    conn.Close();
                }

            }
            #endregion
            //------------- Get Institute 4 Selected In Home ---------
            //============================================================
            return View(InstituteList);
        }


        //==================================================
        //----------------- List Institute Page ------------

        [Route("Institutes")]
        public ActionResult institutes(){

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            //============================================================
            //--------------------- Get Cities ------
            List<City> tbl_ostan = new List<City>();
            #region Get Institute Cities
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"select Code,Pname from Cities where Code=State_Code and (active is null or active =1) order by Code", conn);
                
                rdr = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        tbl_ostan = (from DataRow dr in dataTable.Rows
                                           select new City()
                                           {
                                               Code = int.Parse(dr["Code"].ToString()),
                                               Pname = dr["Pname"].ToString()
                                           }
                              ).ToList();
                        ViewBag.ostanlist = tbl_ostan;
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
                viewbagerror.Msg = "خطا در لود پایه های تحصیلی آموزشگاه : " + ex.Message;
                ViewBag.ErrorMsg = viewbagerror;
            }
            #endregion
            //--------------------- Get Cities ------
            //============================================================


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

            ViewBag.Current_ostan = 0;
           
            return View();
        }

        //----------------- List Institute Page ------------
        //==================================================

        //==================================================
        //---------------- Get Institute List ---
        [LogFilter(mandatory: false)]
        [Route("Institutes/InstituteList")]
        public ActionResult InstituteList(string boy, string girl, int? ostan_code, int? city_code, int? zone_code, string search_text, int? page)
        {

            int Startindex = 0;
            int pagesize = 1;
            page = page.HasValue ? Convert.ToInt32(page) - 1 : 0;
            Startindex = page.HasValue ? Convert.ToInt32(page * pagesize) : 0;
            //pageIndex = string.IsNullOrEmpty(ViewBag.page) ? pageIndex : Convert.ToInt32(ViewBag.page);

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            // ----------  boy --------------
            if (boy == null)
            {
                boy = "1";
            }

            //---------- girl  ---------------
            if (girl == null)
            {
                girl = "1";
            }

            //===================================================================
            //-------------------  Configure City institute ---------------------
            #region Configure City institute
            string city = "";
            StringClass_Convert stringclass = new StringClass_Convert();
            if (ostan_code != 0)
            {
                city = stringclass.Convert_to_4str(ostan_code);
                if (city_code != 0)
                {
                    city = city + stringclass.Convert_to_4str(city_code);
                    if (zone_code != 0)
                    {
                        city = city + stringclass.Convert_to_4str(zone_code);
                    }
                }
            }
            else
            {
                city = "";
            }
            #endregion Configure City institute
            //-------------------  Configure City institute ---------------------
            //===================================================================


            //===================================================================
            //-------------- Boyor Girl Analyze ----------
            #region Boy Or Girl Analyze

            string boyorgirl = "";
            if (boy == "1" && girl == "0")
            {
                boyorgirl = "1";
            }
            else if (boy == "0" && girl == "1")
            {
                boyorgirl = "0";
            }
            else if (boy == "1" && girl == "1")
            {
                boyorgirl = "01";
            }
            else if (boy == "0" && girl == "0")
            {
                boyorgirl = "00";
            }
            #endregion
            //-------------- Boyor Girl Analyze ----------
            //===================================================================

            int Total_item = 0;
            int Total_page = 0;
            int? Current_page = page;



            //============================================================
            //------------------------ Get Institute List Filter ---------
            List<ViewModel.InstituteSmallVM> InstituteList = new List<ViewModel.InstituteSmallVM>();
            #region Get Institute List Filter
            try

            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                string query = "";
                #region Search Query 
                query = @"select * from
                        (
                        SELECT        Institutes.id, Institutes.name,Institutes.En_Name, Institutes.[address],Institutes.boyOrGirl,Institutes.[order],
				                        (select InstituteKind.Name from InstituteKind where InstituteKind.id=Institutes.InstituteKindid) as InstituteKindName,
				                        ROW_NUMBER() OVER(order by Institutes.[order]) AS rownum
				
                        FROM            Institutes 
				                        where 
					                        Institutes.name like @name
					                        and (Institutes.Active is null or Institutes.Active =1)
					                        and Institutes.city_code like @city_code ";
                if (boyorgirl == "1")
                {
                    query += @"and (Institutes.boyOrGirl = 1  )";
                }
                else if (boyorgirl == "0")
                {
                    query += @"and (Institutes.boyOrGirl = 0  )";
                }
                else if (boyorgirl == "01")
                {
                    query += @"and (Institutes.boyOrGirl=1 or Institutes.boyOrGirl=0 or Institutes.boyOrGirl is  null  )";
                }
                else if (boyorgirl == "00")
                {
                    query += @"";
                }
                query += @") as Tbl_Institute
		
		                        where (Tbl_Institute.rownum>@CurrentPage and
				                        Tbl_Institute.rownum<=(@CurrentPage+@PageSize))
				                        order by [order]";


                #endregion

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
                if (search_text != null)
                {
                    cmd.Parameters["@name"].Value = "%" + search_text + "%";
                }
                else
                {
                    cmd.Parameters["@name"].Value = "%" + "" + "%";
                }


                cmd.Parameters.Add(new SqlParameter("@city_code", SqlDbType.NVarChar, 50));
                cmd.Parameters["@city_code"].Value = city + "%";



                cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
                cmd.Parameters["@PageSize"].Value = pagesize;

                cmd.Parameters.Add(new SqlParameter("@CurrentPage", SqlDbType.Int));
                cmd.Parameters["@CurrentPage"].Value = page;


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        InstituteList = (from DataRow dr in dataTable.Rows
                                         select new ViewModel.InstituteSmallVM()
                                         {
                                             id = Int64.Parse(dr["id"].ToString()),
                                             name = dr["name"].ToString(),
                                             En_Name = dr["En_Name"].ToString(),
                                             InstituteKindName = dr["InstituteKindName"].ToString(),
                                             address = dr["address"].ToString(),
                                             boyOrGirl = dr["boyOrGirl"].ToString() != "" ? bool.Parse(dr["boyOrGirl"].ToString()) : (bool?)null
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
                    conn.Close();
                }

            }
            #endregion
            //------------------------ Get Institute List Filter ---------
            //============================================================

            //============================================================
            //-------------------- Get Total Item List Filter ---------
            #region Get Institute List Filter
            try

            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }


                string query = "";
                #region Search Query 
                query = @"SELECT   count(*) as TotalItem
                        FROM            Institutes 
				                        where 
					                        Institutes.name like @name
					                        and (Institutes.Active is null or Institutes.Active =1)
					                        and Institutes.city_code like @city_code ";
                if (boyorgirl == "1")
                {
                    query += @"and (Institutes.boyOrGirl = 1  )";
                }
                else if (boyorgirl == "0")
                {
                    query += @"and (Institutes.boyOrGirl = 0  )";
                }
                else if (boyorgirl == "01")
                {
                    query += @"and (Institutes.boyOrGirl=1 or Institutes.boyOrGirl=0 or Institutes.boyOrGirl is  null  )";
                }
                else if (boyorgirl == "00")
                {
                    query += @"";
                }


                #endregion

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
                if (search_text != null)
                {
                    cmd.Parameters["@name"].Value = "%" + search_text + "%";
                }
                else
                {
                    cmd.Parameters["@name"].Value = "%" + "" + "%";
                }


                cmd.Parameters.Add(new SqlParameter("@city_code", SqlDbType.NVarChar, 50));
                cmd.Parameters["@city_code"].Value = city + "%";

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
            //------------------------ Get Institute List Filter ---------
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

            return PartialView("~/Views/Shared/Partial/_InstituteList.cshtml", InstituteList);
        }
        //---------------- Get Institute List ---
        //==================================================

        //==================================================
        //----------------- Each Institute Page ------------

        [Route("Institute/{id}")]
        public ActionResult institute(int id)
        {

            if (id!=0)
            {
                App_Start.ConnectionString constr = new App_Start.ConnectionString();
                ConnectionString = constr.GetConnectionString();
                SqlConnection conn = new SqlConnection(ConnectionString);
                SqlDataReader rdr = null;
                //============================================================
                //--------------------- Get Institute Info ------
                ViewModel.InstituteInfo_VM InstituteInfo = new ViewModel.InstituteInfo_VM();
                #region Get Institute Info
                try
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"select Institutes.id,Institutes.name,Institutes.tel1,Institutes.tel2,Institutes.website,Institutes.address
		                ,Institutes.boyOrGirl,Institutes.city_code,Institutes.shoar,Institutes.Email
		                ,Institutes.Group_Channel1,Institutes.Group_Channel2,Institutes.Group_Channel3,Institutes.Group_Channel4
		                ,Institutes.Description,Institutes.mobile1,Institutes.mobile2,Institutes.Google_Map,Institutes.En_Name
		                ,(select InstituteKind.Name from InstituteKind where InstituteKind.id=Institutes.InstituteKindid) as InstituteKindName
	                 from Institutes 
	                where 
		                Institutes.id =@id
		                and (Institutes.Active is null or Institutes.Active=1) ", conn);


                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                    cmd.Parameters["@id"].Value = id;

                    rdr = cmd.ExecuteReader();
                    DataTable dataTable = new DataTable();

                    dataTable.Load(rdr);

                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                        {
                            DataRow dr = dataTable.Rows[0];

                            InstituteInfo.id = id;
                            InstituteInfo.name = dr["name"].ToString();
                            InstituteInfo.tel1 = dr["tel1"].ToString();
                            InstituteInfo.tel2 = dr["tel2"].ToString();
                            InstituteInfo.website = dr["website"].ToString();
                            InstituteInfo.address = dr["address"].ToString();
                            InstituteInfo.boyOrGirl = dr["boyOrGirl"].ToString()!=""?bool.Parse(dr["boyOrGirl"].ToString()):(bool?)null;
                            InstituteInfo.city_code = dr["city_code"].ToString();
                            InstituteInfo.shoar = dr["shoar"].ToString();
                            InstituteInfo.Email = dr["Email"].ToString();
                            InstituteInfo.Group_Channel1 = dr["Group_Channel1"].ToString();
                            InstituteInfo.Group_Channel2 = dr["Group_Channel2"].ToString();
                            InstituteInfo.Group_Channel3 = dr["Group_Channel3"].ToString();
                            InstituteInfo.Group_Channel4 = dr["Group_Channel4"].ToString();
                            InstituteInfo.Description = dr["Description"].ToString();
                            InstituteInfo.mobile1 = dr["mobile1"].ToString();
                            InstituteInfo.mobile2 = dr["mobile2"].ToString();
                            InstituteInfo.Google_Map = dr["Google_Map"].ToString();
                            InstituteInfo.En_Name = dr["En_Name"].ToString();
                            InstituteInfo.InstituteKindName = dr["InstituteKindName"].ToString();


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
                    viewbagerror.Msg = "خطا در لود اطلاعات آموزشگاه : " + ex.Message;
                    ViewBag.ErrorMsg = viewbagerror;
                }

                #endregion
                //--------------------- Get Institute Info ------
                //============================================================


                if (InstituteInfo.name!=null)
                {
                    //============================================================
                    //--------------------- Get Institute Grades ------
                    List<Grade> InstituteGrades = new List<Grade>();
                    #region Get Institute Grades
                    try
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = new SqlCommand(@"select Grades.id,Grades.Name
	                    from Institute_Grade 
                    inner join Grades ON
	                    Institute_Grade.Instituteid=@id
	                    AND Grades.id = Institute_Grade.Gradeid ", conn);


                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.BigInt));
                        cmd.Parameters["@id"].Value = id;

                        rdr = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();

                        dataTable.Load(rdr);

                        if (dataTable != null)
                        {
                            if (dataTable.Rows.Count > 0)
                            {
                                InstituteGrades = (from DataRow dr in dataTable.Rows
                                                   select new Grade()
                                                   {
                                                       id = int.Parse(dr["id"].ToString()),
                                                       Name = dr["Name"].ToString()
                                                   }
                                      ).ToList();
                                ViewBag.InstituteGrades = InstituteGrades;
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
                        viewbagerror.Msg = "خطا در لود پایه های تحصیلی آموزشگاه : " + ex.Message;
                        ViewBag.ErrorMsg = viewbagerror;
                    }
                    #endregion
                    //--------------------- Get Institute Grades ------
                    //============================================================

                    try
                    {
                        ViewBag.Images = Directory.EnumerateFiles(Server.MapPath("~/Content/images/schools/" + id + ""))
                                       .Where(fn => Path.GetFileName(fn) != InstituteInfo.id + "-logo.png"
                                       && Path.GetFileName(fn) != InstituteInfo.id + "-main.png"
                                       && Path.GetFileName(fn) != InstituteInfo.id + "-head.png"
                                       )
                                      .Select(fn => "~/Content/images/schools/" + id + "/" + Path.GetFileName(fn));
                    }
                    catch (Exception ex)
                    {
                        ;
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

                return View(InstituteInfo);
            }

            return View();
        }
        //----------------- Each Institute Page ------------
        //==================================================


        //[ChildActionOnly]
        //public PartialViewResult HeaderSlider()
        //{
        //    List<HeaderSlider> sliders = db.HeaderSliders.Where(p => p.active != false).ToList();
        //    return PartialView("~/Views/Shared/Partial/_HeaderCarouselPartial.cshtml", sliders);
        //}



        [LogFilter(mandatory: false)]
        [Route("Home/HeaderSlider")]
        public ActionResult HeaderSlider()
        {
            
            List<HeaderSlider> sliders = db.HeaderSliders.Where(p => p.active != false).OrderBy(p=>p.order).ToList();
            return PartialView("~/Views/Shared/Partial/_HeaderCarouselPartial.cshtml", sliders);
        }



        [LogFilter(mandatory: false)]
        [Route("Home/Whoareyou")]
        public ActionResult Whoareyou() {
            return PartialView("~/Views/Shared/Partial/_WhoAreYouPartial.cshtml");
        }


        //==================================================


        //==================================================
        //--------- FAQ ---
        [LogFilter(mandatory: false)]
        [Route("Home/FAQ")]
        public ActionResult FAQ()
        {
            return View("~/Views/Home/Faq.cshtml");
        }
        //--------- FAQ ---
        //==================================================

        //==================================================
        //--------- Harim ---
        [LogFilter(mandatory: false)]
        [Route("Home/Harim")]
        public ActionResult Harim()
        {
            return View("~/Views/Home/Harim.cshtml");
        }
        //--------- Harim ---
        //==================================================

        //==================================================
        //--------- About ---
        [LogFilter(mandatory: false)]
        [Route("Home/About")]
        public ActionResult About()
        {
            return View("~/Views/Home/About.cshtml");
        }
        //--------- About ---
        //==================================================

        //==================================================
        //--------- About ---
        [LogFilter(mandatory: false)]
        [Route("Home/Contact")]
        public ActionResult Contact()
        {
            return View("~/Views/Home/Contact.cshtml");
        }

        #region SendContactModel
        public class SendContactModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Subject { get; set; }
            public byte? type { get; set; }
            public string Comment { get; set; }
        }
        #endregion

        [LogFilter(mandatory: false)]
        [Route("Home/SendContact")]
        [HttpPost]
        public ActionResult SendContact(SendContactModel MyContact)
        {
            ViewModel.ViewBagError msg = new ViewModel.ViewBagError();
            msg.ClassName = "success";
            msg.Msg = "پیام شما به دست ما رسید . کارشناسان ما درخواست شما را بررسی کرده و در صورت لزوم با شما تماس خواهند گرفت .";
            TempData["msg"] = msg;

            return RedirectToAction("Contact","Home",null);
        }



        //--------- Contact ---
        //==================================================


        //==================================================
        //------------ Download Page ----

        [LogFilter(mandatory: false)]
        [Route("Home/Download")]
        public ActionResult Download()
        {
            return View("~/Views/Home/Download.cshtml");
        }
        //------------ Download Page ----
        //==================================================
        //==================================================

        [LogFilter(mandatory: false)]
        [Route("Home/Article")]
        public ActionResult Article()
        {
            return View("~/Views/Home/Article.cshtml");
        }

         //==================================================
         //------------------- Article List -----------------
         [LogFilter(mandatory: false)]
        [Route("Home/ArticleList")]
        public ActionResult ArticleList(string search_text, int? page)
        {

            int Startindex = 0;
            int pagesize = 1;
            page = page.HasValue ? Convert.ToInt32(page) - 1 : 0;
            Startindex = page.HasValue ? Convert.ToInt32(page * pagesize) : 0;
            
            
            List<ViewModel.Article_User_Rank_VM> article_list = new List<ViewModel.Article_User_Rank_VM>();

            int Total_item = 0;
            int Total_page = 0;
            int? Current_page = page;


            //===================================================================
            //--------------------- Get Session User ----------------------------
            #region Get Session User
            string username =null;
            if(Session["User"]!=null)
            {
                User currentuser = (User)Session["User"];

                username= currentuser.username;
            }

            #endregion Get Session User
            //--------------------- Get Session User ----------------------------
            //===================================================================

            //===================================================================
            //---------------------- Get Article List ---------------------------
            #region Get Article List

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();

            // 1. Instantiate the connection
            SqlConnection conn = new SqlConnection(ConnectionString);

            SqlDataReader rdr = null;

            try

            {
                conn.Open();
                SqlCommand cmd;
                if (username==null)
                {
                    cmd = new SqlCommand(@"SP_ARTICLE_LIST", conn);
                }
                else
                {
                    cmd = new SqlCommand(@"SP_ARTICLE_USER_LIST", conn);
                    cmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Username"].Value =  username ;
                }

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@search", SqlDbType.NVarChar, 50));
                cmd.Parameters["@search"].Value = "%" + search_text + "%";

                cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
                cmd.Parameters["@PageSize"].Value = pagesize;

                cmd.Parameters.Add(new SqlParameter("@CurrentPage", SqlDbType.Int));
                cmd.Parameters["@CurrentPage"].Value = page;

                
                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        
                        article_list = (from DataRow dr in dataTable.Rows
                                         select new ViewModel.Article_User_Rank_VM()
                                         {
                                             id = Convert.ToInt32(dr["id"]),
                                             Title = dr["Title"].ToString(),
                                             En_Title = dr["En_Title"].ToString(),
                                             Description = dr["Description"].ToString(),
                                             writer = dr["writer"].ToString(),
                                             Download = dr["Download"].ToString() != "" ? int.Parse(dr["Download"].ToString()) : int.Parse("0"),
                                             Date= dr["Date"].ToString() != "" ? DateTime.Parse(dr["Date"].ToString()) :(DateTime?) null
                                         }
                                  ).ToList();

                        //=====================================================
                        //---------------- Get Total item for pager -----------
                        rdr = null;
                        cmd = new SqlCommand(@"SP_TOTAL_ARTICLE_LIST", conn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@search", SqlDbType.NVarChar, 50));
                        cmd.Parameters["@search"].Value = "%" + search_text + "%";
                        rdr = cmd.ExecuteReader();
                        dataTable = new DataTable();

                        dataTable.Load(rdr);

                        if (dataTable != null)
                        {
                            if (dataTable.Rows.Count > 0)
                            {
                                DataRow dr = dataTable.Rows[0];

                                Total_item = dr["Total"].ToString() != "" ? int.Parse(dr["Total"].ToString() ) : 0;
                            }
                        }

                        //---------------- Get Total item for pager -----------
                        //=====================================================
                        rdr = null;
                        dataTable.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();
                
            }

            #endregion Get Article List
            //---------------------- Get Article List ---------------------------
            //===================================================================


            //institute_list = db.Institutes.Where(p => p.city_code.StartsWith(city)).Where(p => p.name.Contains(search_text)).OrderBy(p => p.name).Skip(Startindex).Take(pagesize).ToList();
            //Total_item = db.Institutes.Where(p => p.city_code.StartsWith(city)).Where(p => p.name.Contains(search_text)).Select(p => p.id).Count();




            Total_page = Total_item / pagesize;

            //==========================================
            //---------------- Set ViewBag -------------

            ViewBag.Total_item = Total_item;
            ViewBag.Total_page = Total_page;
            ViewBag.Current_page = Current_page;
            //---------------- Set ViewBag -------------
            //==========================================

            return PartialView("~/Views/Shared/Partial/_ArticleList.cshtml", article_list);
        }

        //------------------- Article List -----------------

        //==================================================

        //==================================================
        //----------------- Update Download Article --------
        public void ArticleDownload(int id)
        {
            //----------- update ------------
            #region Update Article

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();

            // 1. Instantiate the connection
            SqlConnection conn = new SqlConnection(ConnectionString);

            SqlDataReader rdr = null;

            try

            {
                conn.Open();
                SqlCommand cmd;
                
                cmd = new SqlCommand(@"SP_UPDATE_ARTICLE_DOWNLOAD", conn);
                

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                cmd.Parameters["@id"].Value = id ;
                cmd.ExecuteNonQuery();

                
            }
            catch (Exception ex)
            {
                conn.Close();
                conn.Dispose();

            }

            #endregion Update Article
            //----------- update ------------
        }
        //----------------- Update Download Article --------
        //==================================================

        [Route("Home/register_institute")]
        public ActionResult register_institute()
        {
            return View();
        }

        [Route("Home/Addregister_institute")]
        public ActionResult Addregister_institute(register_institute myregins)
        {
            register_institute addreg = new Models.register_institute();

            addreg.AcceptCode = myregins.AcceptCode;
            addreg.Address = myregins.Address;
            addreg.City = myregins.City;
            addreg.CodeMelli = myregins.CodeMelli;
            addreg.InstituteName = myregins.InstituteName;
            addreg.InstituteType = myregins.InstituteType;
            addreg.ManagerName = myregins.ManagerName;
            addreg.Mobile = myregins.Mobile;
            addreg.Ostan = myregins.Ostan;
            addreg.Tell = myregins.Tell;

            db.register_institutes.Add(addreg);
            db.SaveChanges();

            ViewModel.ViewBagError myerror = new ViewModel.ViewBagError();
            myerror.ClassName = "success";
            myerror.Msg = "دانش آموز با موفقیت ایجاد گردید.";

            ViewBag.ErrorMsg = myerror;

            return RedirectToAction("register_institute") ;
        }

        [LogFilter(mandatory: false)]

        [Route("Institutes/get_cities")]
        public string get_cities(int Code)
        {
            string result_cities="", result_cities0 = "";

            List<City> cities = db.Cities.Where(p => p.State_Code == Code && p.Code != p.State_Code && p.active!=0).ToList();

            result_cities0 = result_cities0 + "<option value=\"" +0+ "\">" + "شهر" + "</option>";
            foreach (var item in cities)
            {
                result_cities +=  "<option value=\"" + item.Code + "\">" + item.Pname + "</option>";
            }
            
            return result_cities0+result_cities;
        }
        [LogFilter(mandatory: false)]
        [Route("Institutes/get_zone")]
        public string get_zone(int Code)
        {
            string result_zone = "", result_zone0 = "";

            List<CityZone> zones = db.CityZones.Where(p => p.Code == Code && p.active != 0).ToList();

            result_zone0 = result_zone0 + "<option value=\"" + "0" + "\">" + "منطقه" + "</option>";
            foreach (var item in zones)
            {
                result_zone +=  "<option value=\"" + item.Zone_Code + "\">" + item.Pname + "</option>";
            }

            return result_zone0+result_zone;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

    }
}