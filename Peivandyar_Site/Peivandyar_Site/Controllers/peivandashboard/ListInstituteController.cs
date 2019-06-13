using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peivandyar_Site.Controllers.peivandashboard
{
    public class ListInstituteController : Controller
    {
        private string ConnectionString;
        public class ostan_city
        {
            public int Code { get; set; }
            public string Pname { get; set; }
        }

        [Route("PeivandDashboard/ListInstitute/{search?}/{page?}")]
        // GET: ListInstitute
        public ActionResult Index(string search,int? page)
        {

            //===============================================================
            //---------------------- GET Message Result ---------------------
            #region getMessage
            if (TempData["ClassName"] != null && TempData["Msg"] != null)
            {
                ViewModel.ViewBagError myerror = new ViewModel.ViewBagError();
                myerror.ClassName = TempData["ClassName"].ToString();
                myerror.Msg = TempData["Msg"].ToString();
                ViewBag.msg = myerror;

            }

            #endregion getMessage
            //---------------------- GET Message Result ---------------------
            //===============================================================

            int Startindex = 0;
            int pagesize = 10;

            page = page.HasValue ? Convert.ToInt32(page) - 1 : 0;
            Startindex = page.HasValue ? Convert.ToInt32(page * pagesize) : 0;

            search = search != null ? search : "";

            List<ViewModel.InstituteSmallVM>  InstituteList = new List<ViewModel.InstituteSmallVM>();

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();

            // 1. Instantiate the connection
            SqlConnection conn = new SqlConnection(ConnectionString);

            SqlDataReader rdr = null;

            #region Get List Institute
            

            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"SP_LIST_INSTITUTE_DASHBOARD", conn);

                cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
                cmd.Parameters["@name"].Value = "%"+search+"%";

                cmd.Parameters.Add(new SqlParameter("@PageSize", SqlDbType.Int));
                cmd.Parameters["@PageSize"].Value = pagesize;

                cmd.Parameters.Add(new SqlParameter("@CurrentPage", SqlDbType.Int));
                cmd.Parameters["@CurrentPage"].Value = page;

                cmd.CommandType = CommandType.StoredProcedure;
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
                                             id = Convert.ToInt32(dr["id"]),
                                             name = dr["name"].ToString(),
                                             En_Name = dr["En_Name"].ToString(),
                                             InstituteKindName = dr["InstituteKindName"].ToString(),
                                             address = dr["address"].ToString(),
                                             boyOrGirl = dr["boyOrGirl"].ToString() != "" ? bool.Parse(dr["boyOrGirl"].ToString()) :(bool?) null
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
                    
            }
            #endregion


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

            return View("~/Views/peivandashboard/ListInstitute/Index.cshtml",InstituteList);
        }


        [Route("PeivandDashboard/AddInstitute")]

        public ActionResult AddInstitute()
        {
            //===============================================================
            //---------------------- GET Message Result ---------------------
            #region getMessage
            if (TempData["ClassName"]!=null && TempData["Msg"]!=null)
            {
                ViewModel.ViewBagError myerror = new ViewModel.ViewBagError();
                myerror.ClassName = TempData["ClassName"].ToString();
                myerror.Msg = TempData["Msg"].ToString();
                ViewBag.msg = myerror;

            }

            #endregion getMessage
            //---------------------- GET Message Result ---------------------
            //===============================================================

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            //==================================================================
            //------------------------- Get City List --------------------------
            #region getCity
            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"select * from Cities where Code=State_Code and (active is null or active !='0')", conn);


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        List<ostan_city> ostan_bag = new List<ostan_city>();
                        
                        foreach (DataRow item in dataTable.Rows)
                        {
                            ostan_city ostan = new ostan_city();

                            ostan.Code =Convert.ToInt32(item["Code"]);
                            ostan.Pname = item["Pname"].ToString();
                            ostan_bag.Add(ostan);
                        }

                        ViewBag.ostanlist = ostan_bag;

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
            #endregion getCity
            //------------------------- Get City List --------------------------
            //==================================================================


            //==================================================================
            //------------------------- Get Grade List -------------------------

            #region getGrades
            conn = new SqlConnection(ConnectionString);
            rdr = null;

            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"select id,Name from Grades", conn);


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        List<Grade> GradesList = new List<Grade>();
                        GradesList = (from DataRow dr in dataTable.Rows
                                           select new Grade()
                                           {
                                               id = int.Parse(dr["id"].ToString()),
                                               Name = dr["name"].ToString(),
                                           }
                                  ).ToList();
                        ViewBag.GradesList = GradesList;

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
            #endregion getGrades

            //------------------------- Get Grade List -------------------------
            //==================================================================

            //==================================================================
            //------------------------- Get Institute kind List ---------------

            #region Get Institute kind List

            rdr = null;

            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"select id,Name from InstituteKind", conn);


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        List<InstituteKind> InstituteKindes = new List<InstituteKind>();

                        InstituteKindes = (from DataRow dr in dataTable.Rows
                                         select new InstituteKind()
                                         {
                                             id = int.Parse(dr["id"].ToString()),
                                             Name = dr["name"].ToString(),
                                         }
                                  ).ToList();
                        ViewBag.InstituteKindes = InstituteKindes;

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
            #endregion Institute_Types List

            //------------------------- Get Institute kind List ---------------
            //==================================================================

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

            return View("~/Views/peivandashboard/ListInstitute/Add.cshtml");
        }


        [HttpPost]
        [Route("PeivandDashboard/CreateInstitute")]
        public ActionResult CreateInstitute(ViewModel.Institute_Full_VM myinstitute)
        {

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            //==================================================================
            //------------------------- Get Max Id -----------------------------
            #region getmaxid
            int maxid = 0;

            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"select max(id) as id from Institutes", conn);


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow dr = dataTable.Rows[0];
                        maxid = Convert.ToInt32(dr["id"]);
                        maxid++;

                        dataTable.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                maxid = 0;
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

                TempData["ClassName"] = "danger";
                TempData["Msg"] = "متاسفانه خطایی در ایجاد آموزشگاه وجود داشت ." + ex.Message;
                
                return RedirectToAction("AddInstitute");
            }
            #endregion getmaxid
            //------------------------- Get Max Id -----------------------------
            //==================================================================



            if (maxid != 0)
            {
                //------------ Check Institue Values ----------
                DateTime enter_date = new DateTime();

                #region Get Date Miladi
                StringClass_Convert classconvert = new StringClass_Convert();
                myinstitute.Enter_Date = classconvert.GetEnglishNumber(myinstitute.Enter_Date);

                App_Start.Date_Shamsi_Miladi dateclass = new App_Start.Date_Shamsi_Miladi();
                string miladidatestr = dateclass.shamsitomiladi(myinstitute.Enter_Date);

                TimeSpan ts = TimeSpan.Parse(DateTime.Now.ToString("HH:mm"));

                enter_date = DateTime.Parse(miladidatestr);

                enter_date = enter_date.Date + ts;
                #endregion


                string city = "";
                
                #region Analyze CityCode
                StringClass_Convert stringclass = new StringClass_Convert();
                if (myinstitute.ostan_codevm != 0)
                {
                    city = city + stringclass.Convert_to_4str(myinstitute.ostan_codevm);
                    if (myinstitute.city_codevm != 0)
                    {
                        city = city + stringclass.Convert_to_4str(myinstitute.city_codevm);

                        if (myinstitute.zone_Codevm != 0)
                        {
                            city = city + stringclass.Convert_to_4str(myinstitute.zone_Codevm);
                        }
                    }
                }
                else
                {
                    city = "";
                }
                #endregion

                myinstitute.city_code= city;

                bool InstituteAdded = false;

                //==================================================================
                //------------------------  Insert to tbl institute ----------------
                #region addinstitute
                try
                {
                    conn = new SqlConnection(ConnectionString);

                    rdr = null;

                    if (conn.State != ConnectionState.Open)
                        conn.Open();

                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Institutes
                            (id, name,En_Name, postalCode, fax, tel1, tel2, website, educationalCode, address, boyOrGirl, city_code, Active, shoar,Email, Group_Channel1, Group_Channel2, Group_Channel3, Group_Channel4,Group_Channel5, Description, [order], mobile1, mobile2, Google_Map,Enter_Date,InstituteKindid) 
                            VALUES        
                            (@id, @name,@En_Name, @postalCode, @fax, @tel1, @tel2, @website, @educationalCode, @address, @boyOrGirl,@city_code, @Active, @shoar, @Email, @Group_Channel1, @Group_Channel2, @Group_Channel3, @Group_Channel4,@Group_Channel5, @Description, @order, @mobile1, @mobile2, @Google_Map,@Enter_Date,@InstituteKindid)"
                            , conn);

                    #region Parameter 

                    
                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                    cmd.Parameters["@id"].Value =  maxid;

                    cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@name"].Value = myinstitute.name;

                    cmd.Parameters.Add(new SqlParameter("@En_Name", SqlDbType.NVarChar, 50));
                    if(myinstitute.En_Name!=null)
                    {
                        cmd.Parameters["@En_Name"].Value = myinstitute.En_Name;
                    }
                    else
                    {
                        cmd.Parameters["@En_Name"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.NVarChar, 50));
                    if (myinstitute.postalCode!=null)
                    {
                        cmd.Parameters["@postalCode"].Value = myinstitute.postalCode;
                    }
                    else
                    {
                        cmd.Parameters["@postalCode"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@fax", SqlDbType.NVarChar, 50));
                    if(myinstitute.fax!=null)
                    {
                        cmd.Parameters["@fax"].Value = myinstitute.fax;
                    }
                    else
                    {
                        cmd.Parameters["@fax"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@tel1", SqlDbType.NVarChar, 50));
                    if (myinstitute.tel1 != null)
                    {
                        cmd.Parameters["@tel1"].Value = myinstitute.tel1;
                    }
                    else
                    {
                        cmd.Parameters["@tel1"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@tel2", SqlDbType.NVarChar, 50));
                    if (myinstitute.tel2!=null)
                    {
                        cmd.Parameters["@tel2"].Value = myinstitute.tel2;
                    }
                    else
                    {
                        cmd.Parameters["@tel2"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@website", SqlDbType.NVarChar, 50));
                    if (myinstitute.website!= null)
                    {
                        cmd.Parameters["@website"].Value = myinstitute.website;
                    }
                    else
                    {
                        cmd.Parameters["@website"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@educationalCode", SqlDbType.NVarChar, 50));
                    if (myinstitute.educationalCode!=null)
                    {
                        cmd.Parameters["@educationalCode"].Value = myinstitute.educationalCode;
                    }
                    else
                    {
                        cmd.Parameters["@educationalCode"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@address", SqlDbType.NVarChar, 50));
                    if (myinstitute.address!=null)
                    {
                        cmd.Parameters["@address"].Value = myinstitute.address;
                    }
                    else
                    {
                        cmd.Parameters["@address"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@boyOrGirl", SqlDbType.Bit));
                    if (myinstitute.boyOrGirl!=null)
                    {
                        cmd.Parameters["@boyOrGirl"].Value = myinstitute.boyOrGirl;
                    }
                    else
                    {
                        cmd.Parameters["@boyOrGirl"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@city_code", SqlDbType.NVarChar, 50));
                    if (myinstitute.city_code!=null)
                    {
                        cmd.Parameters["@city_code"].Value = myinstitute.city_code;
                    }
                    else
                    {
                        cmd.Parameters["@city_code"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit));
                    if (myinstitute.Active!=null)
                    {
                        cmd.Parameters["@Active"].Value =bool.Parse(myinstitute.Active);
                    }
                    else
                    {
                        cmd.Parameters["@Active"].Value = DBNull.Value;
                    }

                    
                    cmd.Parameters.Add(new SqlParameter("@shoar", SqlDbType.NVarChar, 50));
                    if (myinstitute.shoar!=null)
                    {
                        cmd.Parameters["@shoar"].Value = myinstitute.shoar;
                    }
                    else
                    {
                        cmd.Parameters["@shoar"].Value = DBNull.Value;
                    }

                    
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar,50));
                    if (myinstitute.Email != null)
                    {
                        cmd.Parameters["@Email"].Value = myinstitute.Email;
                    }
                    else
                    {
                        cmd.Parameters["@Email"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@Group_Channel1", SqlDbType.NVarChar, 50));
                    if (myinstitute.Group_Channel1!=null)
                    {
                        cmd.Parameters["@Group_Channel1"].Value = myinstitute.Group_Channel1;
                    }
                    else
                    {
                        cmd.Parameters["@Group_Channel1"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@Group_Channel2", SqlDbType.NVarChar, 50));
                    if (myinstitute.Group_Channel2 != null)
                    {
                        cmd.Parameters["@Group_Channel2"].Value = myinstitute.Group_Channel2;
                    }
                    else
                    {
                        cmd.Parameters["@Group_Channel2"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@Group_Channel3", SqlDbType.NVarChar, 50));
                    if (myinstitute.Group_Channel3 != null)
                    {
                        cmd.Parameters["@Group_Channel3"].Value = myinstitute.Group_Channel3;
                    }
                    else
                    {
                        cmd.Parameters["@Group_Channel3"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@Group_Channel4", SqlDbType.NVarChar, 50));
                    if (myinstitute.Group_Channel4 != null)
                    {
                        cmd.Parameters["@Group_Channel4"].Value = myinstitute.Group_Channel4;
                    }
                    else
                    {
                        cmd.Parameters["@Group_Channel4"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@Group_Channel5", SqlDbType.NVarChar, 50));
                    if (myinstitute.Group_Channel5 != null)
                    {
                        cmd.Parameters["@Group_Channel5"].Value = myinstitute.Group_Channel5;
                    }
                    else
                    {
                        cmd.Parameters["@Group_Channel5"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 50));
                    if (myinstitute.Description!=null)
                    {
                        cmd.Parameters["@Description"].Value = myinstitute.Description;
                    }
                    else
                    {
                        cmd.Parameters["@Description"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@order", SqlDbType.TinyInt));
                    if (myinstitute.order!=null)
                    {
                        cmd.Parameters["@order"].Value = myinstitute.order;
                    }
                    else
                    {
                        cmd.Parameters["@order"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@mobile1", SqlDbType.NVarChar,50));
                    if (myinstitute.mobile1!=null)
                    {
                        cmd.Parameters["@mobile1"].Value = myinstitute.mobile1;
                    }
                    else
                    {
                        cmd.Parameters["@mobile1"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@mobile2", SqlDbType.NVarChar, 50));
                    if (myinstitute.mobile2 != null)
                    {
                        cmd.Parameters["@mobile2"].Value = myinstitute.mobile2;
                    }
                    else
                    {
                        cmd.Parameters["@mobile2"].Value = DBNull.Value;
                    }
                    

                    cmd.Parameters.Add(new SqlParameter("@Google_Map", SqlDbType.NVarChar, 50));
                    if (myinstitute.Google_Map!=null)
                    {
                        cmd.Parameters["@Google_Map"].Value = myinstitute.Google_Map;
                    }
                    else
                    {
                        cmd.Parameters["@Google_Map"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@Enter_Date", SqlDbType.DateTime));
                    if (enter_date!=null)
                    {
                        cmd.Parameters["@Enter_Date"].Value = enter_date;
                    }
                    else
                    {
                        cmd.Parameters["@Enter_Date"].Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(new SqlParameter("@InstituteKindid", SqlDbType.Int));
                    if (myinstitute.InstituteKindid != null)
                    {
                        cmd.Parameters["@InstituteKindid"].Value = myinstitute.InstituteKindid;
                    }
                    else
                    {
                        cmd.Parameters["@InstituteKindid"].Value = DBNull.Value;
                    }

                    #endregion

                    cmd.ExecuteNonQuery();
                    InstituteAdded = true;

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

                    InstituteAdded = false;

                    TempData["ClassName"] = "danger";
                    TempData["Msg"] = "متاسفانه خطایی در ایجاد آموزشگاه وجود داشت ." + ex.Message;

                    return RedirectToAction("AddInstitute");
                }

                #endregion addinstitute
                //------------------------  Insert to tbl institute ----------------
                //==================================================================

                //==================================================================
                //------------------------ Insert On Institute_Grade ---------------
                #region addInstituteGrade

                try
                {
                    if (myinstitute.Gradeid!=null && InstituteAdded )
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        SqlCommand cmd = new SqlCommand(@"INSERT INTO Institute_Grade
                         (Instituteid, Gradeid) VALUES        (@Instituteid,@Gradeid)", conn);
                        cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.Int));
                        cmd.Parameters.Add(new SqlParameter("@Gradeid", SqlDbType.Int));
                        foreach (int item in myinstitute.Gradeid)
                        {

                            cmd.Parameters["@Instituteid"].Value = maxid;

                            cmd.Parameters["@Gradeid"].Value = item;

                            cmd.ExecuteNonQuery();
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

                    TempData["ClassName"] = "danger";
                    TempData["Msg"] = "متاسفانه خطایی در ایجاد آموزشگاه وجود داشت ." + ex.Message;

                    return RedirectToAction("AddInstitute");
                }

                #endregion addInstituteGrade
                //------------------------ Insert On Institute_Grade ---------------
                //==================================================================


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


                //==================================================================
                //------------------------ Upload Images ---------------------------
                #region upoloadimg

                if (InstituteAdded)
                {
                    if (myinstitute.BackgroundFile != null && myinstitute.BackgroundFile.ContentLength > 0)
                    {
                        //string passvand = myinstitute.BackgroundFile.FileName.Substring((myinstitute.BackgroundFile.FileName.Length - 3), 3);
                        string passvand = "png";
                        var fileName = Path.GetFileName(myinstitute.id + "-head" + "." + passvand);

                        var path = Path.Combine(Server.MapPath("~/Content/images/schools/" + maxid + "/"), fileName);
                        bool folder_exists3 = Directory.Exists(Server.MapPath("~/Content/images/schools/" + maxid + "/"));
                        if (!folder_exists3)
                            Directory.CreateDirectory(Server.MapPath("~/Content/images/schools/" + maxid + "/"));
                        myinstitute.BackgroundFile.SaveAs(path);

                    }

                    if (myinstitute.LogoFile != null && myinstitute.LogoFile.ContentLength > 0)
                    {
                        //string passvand = myinstitute.LogoFile.FileName.Substring((myinstitute.LogoFile.FileName.Length - 3), 3);
                        string passvand = "png";
                        var fileName = Path.GetFileName(myinstitute.id + "-logo" + "." + passvand);

                        var path = Path.Combine(Server.MapPath("~/Content/images/schools/" + maxid + "/"), fileName);
                        bool folder_exists3 = Directory.Exists(Server.MapPath("~/Content/images/schools/" + maxid + "/"));
                        if (!folder_exists3)
                            Directory.CreateDirectory(Server.MapPath("~/Content/images/schools/" + maxid + "/"));
                        myinstitute.LogoFile.SaveAs(path);

                    }

                    if (myinstitute.CartFile != null && myinstitute.CartFile.ContentLength > 0)
                    {
                        //string passvand = myinstitute.CartFile.FileName.Substring((myinstitute.CartFile.FileName.Length - 3), 3);
                        string passvand = "png";

                        var fileName = Path.GetFileName(myinstitute.id + "-main" + "." + passvand);
                        var path = Path.Combine(Server.MapPath("~/Content/images/schools/" + maxid + "/"), fileName);
                        bool folder_exists3 = Directory.Exists(Server.MapPath("~/Content/images/schools/" + maxid + "/"));
                        if (!folder_exists3)
                            Directory.CreateDirectory(Server.MapPath("~/Content/images/schools/" + maxid + "/"));
                        myinstitute.CartFile.SaveAs(path);

                    }

                    int counter = 1;
                    foreach (var item in myinstitute.InstitutesPics)
                    {
                        if (item != null && item.ContentLength > 0)
                        {
                            string passvand = item.FileName.Substring((item.FileName.Length - 3), 3);
                            var fileName = Path.GetFileName(myinstitute.id + "-" + counter + "." + passvand);
                            var path = Path.Combine(Server.MapPath("~/Content/images/schools/" + maxid + "/"), fileName);
                            bool folder_exists3 = Directory.Exists(Server.MapPath("~/Content/images/schools/" + maxid + "/"));
                            if (!folder_exists3)
                                Directory.CreateDirectory(Server.MapPath("~/Content/images/schools/" + maxid + "/"));
                            item.SaveAs(path);
                            counter++;
                        }
                    }
                }
                
                #endregion upoloadimg
                //------------------------ Upload Images ---------------------------
                //==================================================================
                
                TempData["ClassName"] = "success";
                TempData["Msg"] = "آموزشگاه " + myinstitute.name + " با موفقیت ایجاد گردید .";

                return RedirectToAction("AddInstitute");

            }
            

            return View();
        }



        [Route("PeivandDashboard/EditInstitute/{id}")]

        public ActionResult EditInstitute(int id)
        {
            //===============================================================
            //---------------------- GET Message Result ---------------------
            #region getMessage
            if (TempData["ClassName"] != null && TempData["Msg"] != null)
            {
                ViewModel.ViewBagError myerror = new ViewModel.ViewBagError();
                myerror.ClassName = TempData["ClassName"].ToString();
                myerror.Msg = TempData["Msg"].ToString();
                ViewBag.msg = myerror;

            }

            #endregion getMessage
            //---------------------- GET Message Result ---------------------
            //===============================================================

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;

            //==================================================================
            //------------------------- Get City List --------------------------
            #region getCity
            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"select * from Cities where Code=State_Code and (active is null or active =1)", conn);


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        List<ostan_city> ostan_bag = new List<ostan_city>();

                        foreach (DataRow item in dataTable.Rows)
                        {
                            ostan_city ostan = new ostan_city();

                            ostan.Code = Convert.ToInt32(item["Code"]);
                            ostan.Pname = item["Pname"].ToString();
                            ostan_bag.Add(ostan);
                        }

                        ViewBag.ostanlist = ostan_bag;
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
            #endregion getCity
            //------------------------- Get City List --------------------------
            //==================================================================

            //==================================================================
            //------------------------- Get Grade List -------------------------

            #region getGrades
            
            rdr = null;

            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"select id,Name from Grades", conn);


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        List<Grade> GradesList = new List<Grade>();
                        GradesList = (from DataRow dr in dataTable.Rows
                                           select new Grade()
                                           {
                                               id = int.Parse(dr["id"].ToString()),
                                               Name = dr["name"].ToString(),
                                           }
                                  ).ToList();
                        
                        ViewBag.GradesList = GradesList;

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
            #endregion getGrades

            //------------------------- Get Grade List -------------------------
            //==================================================================


            //==================================================================
            //------------------------- Get Institute Kind List ---------------

            #region Get Get Institute Kind List

            rdr = null;

            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"select id,Name from InstituteKind", conn);


                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        List<InstituteKind> InstituteKindes = new List<InstituteKind>();
                        InstituteKindes = (from DataRow dr in dataTable.Rows
                                           select new InstituteKind()
                                           {
                                               id = int.Parse(dr["id"].ToString()),
                                               Name = dr["name"].ToString(),
                                           }
                                  ).ToList();
                        ViewBag.InstituteKindes = InstituteKindes;

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
            #endregion Institute_Types List

            //------------------------- Get Institute Kind List ---------------
            //==================================================================

            //==================================================================
            //------------------------- Get Institute for Edit -----------------
            #region instituteinfo
            ViewModel.Institute_VM institute = new ViewModel.Institute_VM();

            //conn = new SqlConnection(ConnectionString);
            rdr = null;

            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"select * from Institutes where id=@id", conn);

                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                cmd.Parameters["@id"].Value = id;

                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow dr = dataTable.Rows[0];

                        institute.Institute = new Institute();

                        institute.Institute.id = Convert.ToInt32(dr["id"].ToString());
                        institute.Institute.name = dr["name"].ToString();
                        institute.Institute.En_Name = dr["En_Name"].ToString();
                        institute.Institute.postalCode = dr["postalCode"].ToString();
                        institute.Institute.fax = dr["fax"].ToString();
                        institute.Institute.tel1 = dr["tel1"].ToString();
                        institute.Institute.tel2 = dr["tel2"].ToString();
                        institute.Institute.website = dr["website"].ToString();
                        institute.Institute.educationalCode = dr["educationalCode"].ToString();
                        institute.Institute.address = dr["address"].ToString();
                        if (dr["boyOrGirl"].ToString() != "")
                        {
                            institute.Institute.boyOrGirl = bool.Parse(dr["boyOrGirl"].ToString());
                        } 
                        else
                        {
                            institute.Institute.boyOrGirl = null;
                        }
                        

                        institute.Institute.city_code = dr["city_code"].ToString();
                        institute.Institute.Active = dr["Active"].ToString() != "" ? bool.Parse(dr["Active"].ToString()) : true;
                        institute.Institute.shoar = dr["shoar"].ToString();
                        institute.Institute.Email = dr["Email"].ToString();

                        institute.Institute.Group_Channel1 = dr["Group_Channel1"].ToString();
                        institute.Institute.Group_Channel2 = dr["Group_Channel2"].ToString();
                        institute.Institute.Group_Channel3 = dr["Group_Channel3"].ToString();
                        institute.Institute.Group_Channel4 = dr["Group_Channel4"].ToString();
                        institute.Institute.Group_Channel5 = dr["Group_Channel5"].ToString();

                        institute.Institute.Description = dr["Description"].ToString();

                        institute.Institute.order = dr["order"].ToString() != "" ? byte.Parse(dr["order"].ToString()) : byte.Parse("0") ;
                        institute.Institute.mobile1 = dr["mobile1"].ToString();
                        institute.Institute.mobile2 = dr["mobile2"].ToString();

                        institute.Institute.Google_Map = dr["Google_Map"].ToString();
                        institute.Institute.Enter_Date = dr["Enter_Date"].ToString()!=""?DateTime.Parse(dr["Enter_Date"].ToString()):DateTime.Now;
                        institute.Institute.InstituteKindid = dr["InstituteKindid"].ToString() != "" ? int.Parse(dr["InstituteKindid"].ToString()) : 0;

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


                TempData["ClassName"] = "danger";
                TempData["Msg"] = "متاسفانه بارگزاری اطلاعات آموزشگاه با خطا مواجه شد ." + ex.Message;

                return RedirectToAction("Index");
            }

            #endregion instituteinfo
            //------------------------- Get Institute for Edit -----------------
            //==================================================================


            //==================================================================
            //------------------------- Get  Grades Institute ------------------
            #region get Grades
            conn = new SqlConnection(ConnectionString);
            rdr = null;

            try

            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"SP_INSTITUTE_GRADES", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@InstituteId", SqlDbType.Int));
                cmd.Parameters["@InstituteId"].Value =int.Parse(id.ToString());

                rdr = cmd.ExecuteReader();

                DataTable dataTable = new DataTable();

                dataTable.Load(rdr);

                if (dataTable != null)
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        institute.Grades = new List<Grade>();
                        institute.Grades = (from DataRow dr in dataTable.Rows
                                           select new Grade()
                                           {
                                               id = int.Parse(dr["id"].ToString()),
                                               Name = dr["name"].ToString(),
                                           }
                                  ).ToList();
                        
                    }
                }
                dataTable.Dispose();

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


                TempData["ClassName"] = "danger";
                TempData["Msg"] = "متاسفانه بارگزاری اطلاعات آموزشگاه با خطا مواجه شد ." + ex.Message;

                return RedirectToAction("Index");
            }
            #endregion getGrades
            //------------------------- Get  Grades Institute ------------------
            //==================================================================

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

            return View("~/Views/peivandashboard/ListInstitute/Edit.cshtml",institute);
        }

        [HttpPost]
        [Route("PeivandDashboard/UpdateInstitute")]
        public ActionResult UpdateInstitute(ViewModel.Institute_Full_VM myinstitute)
        {

            App_Start.ConnectionString constr = new App_Start.ConnectionString();
            ConnectionString = constr.GetConnectionString();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlDataReader rdr = null;



            //------------ Check Institue Values ----------
            DateTime edit_date = new DateTime();
            //--- get engilish number
            edit_date = DateTime.Now;

            string city = "";
            #region Analyze City Code
            StringClass_Convert stringclass = new StringClass_Convert();
            if (myinstitute.ostan_codevm != 0)
            {
                city = city + stringclass.Convert_to_4str(myinstitute.ostan_codevm);
                if (myinstitute.city_codevm != 0)
                {
                    city = city + stringclass.Convert_to_4str(myinstitute.city_codevm);

                    if (myinstitute.zone_Codevm != 0)
                    {
                        city = city + stringclass.Convert_to_4str(myinstitute.zone_Codevm);
                    }
                }
            }
            else
            {
                city = "";
            }
            #endregion
            myinstitute.city_code = city;


            
            //==================================================================
            //------------------------  Update tbl institute ----------------
            #region updateinstitute
            try
            {

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand(@"UPDATE    Institutes 
    SET                 name =@name , postalCode =@postalCode , fax =@fax, tel1 =@tel1, tel2 =@tel2 , website =@website , educationalCode =@educationalCode , address =@address, boyOrGirl =@boyOrGirl, city_code =@city_code , Active =@Active  , shoar =@shoar , Email =@Email , Group_Channel1 =@Group_Channel1 , Group_Channel2 =@Group_Channel2 , 
                         Group_Channel3 =@Group_Channel3, Group_Channel4 =@Group_Channel4 , Group_Channel5 =@Group_Channel5 , Description =@Description , [order] =@order, mobile1 =@mobile1 , mobile2 =@mobile2 , Google_Map =@Google_Map , Edit_Date =@Edit_Date , En_Name =@En_Name,InstituteKindid=@InstituteKindid
                            where id=@id ");
                cmd.Connection = conn;
                #region Parameters
                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                cmd.Parameters["@id"].Value = myinstitute.id;

                cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50));
                cmd.Parameters["@name"].Value = myinstitute.name;

                cmd.Parameters.Add(new SqlParameter("@EN_Name", SqlDbType.NVarChar, 50));
                if (myinstitute.En_Name != null)
                {
                    cmd.Parameters["@EN_Name"].Value = myinstitute.En_Name;
                }
                else
                {
                    cmd.Parameters["@EN_Name"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@postalCode", SqlDbType.NVarChar, 50));
                if (myinstitute.postalCode != null)
                {
                    cmd.Parameters["@postalCode"].Value = myinstitute.postalCode;
                }
                else
                {
                    cmd.Parameters["@postalCode"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@fax", SqlDbType.NVarChar, 50));
                if (myinstitute.fax != null)
                {
                    cmd.Parameters["@fax"].Value = myinstitute.fax;
                }
                else
                {
                    cmd.Parameters["@fax"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@tel1", SqlDbType.NVarChar, 50));
                if (myinstitute.tel1 != null)
                {
                    cmd.Parameters["@tel1"].Value = myinstitute.tel1;
                }
                else
                {
                    cmd.Parameters["@tel1"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@tel2", SqlDbType.NVarChar, 50));
                if (myinstitute.tel2 != null)
                {
                    cmd.Parameters["@tel2"].Value = myinstitute.tel2;
                }
                else
                {
                    cmd.Parameters["@tel2"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@website", SqlDbType.NVarChar, 50));
                if (myinstitute.website != null)
                {
                    cmd.Parameters["@website"].Value = myinstitute.website;
                }
                else
                {
                    cmd.Parameters["@website"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@educationalCode", SqlDbType.NVarChar, 50));
                if (myinstitute.educationalCode != null)
                {
                    cmd.Parameters["@educationalCode"].Value = myinstitute.educationalCode;
                }
                else
                {
                    cmd.Parameters["@educationalCode"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@address", SqlDbType.NVarChar, 50));
                if (myinstitute.address != null)
                {
                    cmd.Parameters["@address"].Value = myinstitute.address;
                }
                else
                {
                    cmd.Parameters["@address"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@boyOrGirl", SqlDbType.Bit));
                if (myinstitute.boyOrGirl != null)
                {
                    cmd.Parameters["@boyOrGirl"].Value = myinstitute.boyOrGirl;
                }
                else
                {
                    cmd.Parameters["@boyOrGirl"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@city_code", SqlDbType.NVarChar, 50));
                if (myinstitute.city_code != null)
                {
                    cmd.Parameters["@city_code"].Value = myinstitute.city_code;
                }
                else
                {
                    cmd.Parameters["@city_code"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit));
                if (myinstitute.Active != null)
                {
                    cmd.Parameters["@Active"].Value = bool.Parse(myinstitute.Active);
                }
                else
                {
                    cmd.Parameters["@Active"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@InstituteKindid", SqlDbType.Int));
                if (myinstitute.InstituteKindid != null)
                {
                    cmd.Parameters["@InstituteKindid"].Value = myinstitute.InstituteKindid;
                }
                else
                {
                    cmd.Parameters["@InstituteKindid"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@shoar", SqlDbType.NVarChar, 50));
                if (myinstitute.shoar != null)
                {
                    cmd.Parameters["@shoar"].Value = myinstitute.shoar;
                }
                else
                {
                    cmd.Parameters["@shoar"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar, 50));
                if (myinstitute.Email != null)
                {
                    cmd.Parameters["@Email"].Value = myinstitute.Email;
                }
                else
                {
                    cmd.Parameters["@Email"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@Group_Channel1", SqlDbType.NVarChar, 50));
                if (myinstitute.Group_Channel1 != null)
                {
                    cmd.Parameters["@Group_Channel1"].Value = myinstitute.Group_Channel1;
                }
                else
                {
                    cmd.Parameters["@Group_Channel1"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@Group_Channel2", SqlDbType.NVarChar, 50));
                if (myinstitute.Group_Channel2 != null)
                {
                    cmd.Parameters["@Group_Channel2"].Value = myinstitute.Group_Channel2;
                }
                else
                {
                    cmd.Parameters["@Group_Channel2"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@Group_Channel3", SqlDbType.NVarChar, 50));
                if (myinstitute.Group_Channel3 != null)
                {
                    cmd.Parameters["@Group_Channel3"].Value = myinstitute.Group_Channel3;
                }
                else
                {
                    cmd.Parameters["@Group_Channel3"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@Group_Channel4", SqlDbType.NVarChar, 50));
                if (myinstitute.Group_Channel4 != null)
                {
                    cmd.Parameters["@Group_Channel4"].Value = myinstitute.Group_Channel4;
                }
                else
                {
                    cmd.Parameters["@Group_Channel4"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@Group_Channel5", SqlDbType.NVarChar, 50));
                if (myinstitute.Group_Channel5 != null)
                {
                    cmd.Parameters["@Group_Channel5"].Value = myinstitute.Group_Channel5;
                }
                else
                {
                    cmd.Parameters["@Group_Channel5"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, 50));
                if (myinstitute.Description != null)
                {
                    cmd.Parameters["@Description"].Value = myinstitute.Description;
                }
                else
                {
                    cmd.Parameters["@Description"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@order", SqlDbType.TinyInt));
                if (myinstitute.order != null)
                {
                    cmd.Parameters["@order"].Value = myinstitute.order;
                }
                else
                {
                    cmd.Parameters["@order"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@mobile1", SqlDbType.NVarChar, 50));
                if (myinstitute.mobile1 != null)
                {
                    cmd.Parameters["@mobile1"].Value = myinstitute.mobile1;
                }
                else
                {
                    cmd.Parameters["@mobile1"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@mobile2", SqlDbType.NVarChar, 50));
                if (myinstitute.mobile2 != null)
                {
                    cmd.Parameters["@mobile2"].Value = myinstitute.mobile2;
                }
                else
                {
                    cmd.Parameters["@mobile2"].Value = DBNull.Value;
                }


                cmd.Parameters.Add(new SqlParameter("@Google_Map", SqlDbType.NVarChar, 50));
                if (myinstitute.Google_Map != null)
                {
                    cmd.Parameters["@Google_Map"].Value = myinstitute.Google_Map;
                }
                else
                {
                    cmd.Parameters["@Google_Map"].Value = DBNull.Value;
                }

                cmd.Parameters.Add(new SqlParameter("@Edit_Date", SqlDbType.DateTime));
                if (edit_date != null)
                {
                    cmd.Parameters["@Edit_Date"].Value = edit_date;
                }
                else
                {
                    cmd.Parameters["@Edit_Date"].Value = DBNull.Value;
                }

                #endregion Parameters
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

                TempData["ClassName"] = "danger";
                TempData["Msg"] = "متاسفانه خطایی در ویرایش آموزشگاه وجود داشت ." + ex.Message;

                return RedirectToAction("Index");
            }

            #endregion updateinstitute
            //------------------------  Update tbl institute ----------------
            //==================================================================

            //==================================================================
            //------------------------ Delete  Institute_Grade Old--------------
            #region deleteInstituteGrade

            try
            {
                if (myinstitute.Gradeid != null)
                {
                    SqlCommand cmd = new SqlCommand(@"DELETE FROM Institute_Grade WHERE (Instituteid = @Instituteid)", conn);
                    cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.Int));
                    cmd.Parameters["@Instituteid"].Value = myinstitute.id;
                    cmd.ExecuteNonQuery();
                    
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

                TempData["ClassName"] = "danger";
                TempData["Msg"] = "متاسفانه خطایی در ویرایش آموزشگاه وجود داشت ." + ex.Message;

                return RedirectToAction("Index");
            }

            #endregion deleteInstituteGrade
            //------------------------ Delete  Institute_Grade Old--------------
            //==================================================================

            //==================================================================
            //------------------------ Insert On Institute_Grade ---------------
            #region addInstituteGrade

            try
            {
                if (myinstitute.Gradeid != null)
                {
                    SqlCommand cmd = new SqlCommand(@"INSERT INTO Institute_Grade
                         (Instituteid, Gradeid) VALUES        (@Instituteid,@Gradeid)", conn);
                    cmd.Parameters.Add(new SqlParameter("@Instituteid", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@Gradeid", SqlDbType.Int));
                    foreach (int item in myinstitute.Gradeid)
                    {

                        cmd.Parameters["@Instituteid"].Value = myinstitute.id;

                        cmd.Parameters["@Gradeid"].Value = item;

                        cmd.ExecuteNonQuery();
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

                TempData["ClassName"] = "danger";
                TempData["Msg"] = "متاسفانه خطایی در ایجاد آموزشگاه وجود داشت ." + ex.Message;

                return RedirectToAction("AddInstitute");
            }

            #endregion addInstituteGrade
            //------------------------ Insert On Institute_Grade ---------------
            //==================================================================

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



            
            //==================================================================
            //------------------------ Upload Images ---------------------------
            #region upoloadimg

            if (myinstitute.BackgroundFile != null && myinstitute.BackgroundFile.ContentLength > 0)
            {
                //string passvand = myinstitute.BackgroundFile.FileName.Substring((myinstitute.BackgroundFile.FileName.Length - 3), 3);
                string passvand = "png";

                var fileName = Path.GetFileName(myinstitute.id + "-head" + "." + passvand);
                var path = Path.Combine(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"), fileName);
                bool folder_exists3 = Directory.Exists(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"));
                if (!folder_exists3)
                    Directory.CreateDirectory(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"));
                myinstitute.BackgroundFile.SaveAs(path);

            }

            if (myinstitute.LogoFile != null && myinstitute.LogoFile.ContentLength > 0)
            {
                //string passvand = myinstitute.LogoFile.FileName.Substring((myinstitute.LogoFile.FileName.Length - 3), 3);
                string passvand = "png";

                var fileName = Path.GetFileName(myinstitute.id + "-logo" + "." + passvand);
                var path = Path.Combine(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"), fileName);
                bool folder_exists3 = Directory.Exists(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"));
                if (!folder_exists3)
                    Directory.CreateDirectory(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"));
                myinstitute.LogoFile.SaveAs(path);

            }

            if (myinstitute.CartFile != null && myinstitute.CartFile.ContentLength > 0)
            {
                //string passvand = myinstitute.CartFile.FileName.Substring((myinstitute.CartFile.FileName.Length - 3), 3);
                string passvand = "png";

                var fileName = Path.GetFileName(myinstitute.id + "-main" + "." + passvand);
                var path = Path.Combine(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"), fileName);
                bool folder_exists3 = Directory.Exists(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"));
                if (!folder_exists3)
                    Directory.CreateDirectory(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"));
                myinstitute.CartFile.SaveAs(path);

            }

            int counter = 1;
            foreach (var item in myinstitute.InstitutesPics)
            {
                if (item != null && item.ContentLength > 0)
                {
                    //string passvand = item.FileName.Substring((item.FileName.Length - 3), 3);
                    string passvand = "png";

                    var fileName = Path.GetFileName(myinstitute.id + "-" + counter + "." + passvand);
                    var path = Path.Combine(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"), fileName);
                    bool folder_exists3 = Directory.Exists(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"));
                    if (!folder_exists3)
                        Directory.CreateDirectory(Server.MapPath("~/Content/images/schools/" + myinstitute.id + "/"));
                    item.SaveAs(path);
                    counter++;
                }
            }



            #endregion upoloadimg
            //------------------------ Upload Images ---------------------------
            //==================================================================

            TempData["ClassName"] = "success";
            TempData["Msg"] = "آموزشگاه " + myinstitute.name + " با موفقیت ویرایش گردید .";

            return RedirectToAction("Index");


            
        }

        
    }
}