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
namespace Peivandyar_Site.Controllers.maindashboard
{
    public class TeacherController : Controller
    {
        DataContext db = new DataContext();
        public class Terms_Active
        {
            public int id { get; set; }
            public string Description { get; set; }
        }
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }


        [Route("Teacher/List/{page?}")]
        public ActionResult Teacher_list(int? page)
        {
            int pageIndex = 1;
            int pagesize = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Teacher_EmployeListVM> result = null;

            User userinfo = (User)Session["User"];


            string current_year = DateTime.Now.Year.ToString();

            string befor_year = (int.Parse(current_year) - 1).ToString();   

            //==================================================================================
            //-----------------------  لیست ترم های فعال مدارس کاربر ------------------------
            List<Terms_Active> result_terms_active = new List<Terms_Active>();
            SqlParameter[] parameters_terms ={
                         new SqlParameter("@username",userinfo.username)
                         };
            result_terms_active = db.Database.SqlQuery<Terms_Active>("SP_ACTIVE_TERM_INSTITUE_USER @username", parameters_terms).ToList();
            ViewBag.terms_active = result_terms_active;
            //-----------------------  لیست ترم های فعال مدارس کاربر ------------------------
            //==================================================================================

            //==================================================================================
            //--------  لیست کارمندان و دبیران یک کاربر در مدارس مختلف در دو سال قبل ----


            List<Teacher_EmployeListVM> result_fanal = new List<Teacher_EmployeListVM>();
            SqlParameter[] parameters ={
                         new SqlParameter("@username",userinfo.username),
                         new SqlParameter("@startdate1",befor_year),
                         new SqlParameter("@startdate2",current_year)
                                           };
            result_fanal = db.Database.SqlQuery<Teacher_EmployeListVM>("SP_TEACHER_EMPLOYE_LIST_USER @username,@startdate1,@startdate2", parameters).ToList();
            //--------  لیست کارمندان و دبیران یک کاربر در مدارس مختلف در دو سال قبل ----
            //==================================================================================
            result = result_fanal.ToPagedList(pageIndex, pagesize);

            return View("~/Views/maindashboard/Teachers/teacherlist.cshtml", result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }

    }
}