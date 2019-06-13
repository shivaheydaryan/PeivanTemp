using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ViewModel;
using Models;
using App_Start;
using System.Web.Services;
using System.Data.SqlClient;

namespace Peivandyar_Site.Controllers.maindashboard
{
    public class NotificationsController : Controller
    {
        DataContext db = new DataContext();
        public class result_SP_ALL_MSG_COUNT_USER
        {
            public int? Inbox_count { get; set; }
            public int? Save_count { get; set; }
            
        }
        // GET: Notifications
        [Route("Notifications")]
        public ActionResult Index()
        {
            User userinfo = (User)Session["User"];
            if (userinfo != null)
            {
                //===================================================================================
                //---------------------------   تعداد اعلان ها  ---------


                result_SP_ALL_MSG_COUNT_USER result = db.Database.SqlQuery<result_SP_ALL_MSG_COUNT_USER>("SP_ALL_NOTIC_COUNT_USER @username", new SqlParameter("@username", userinfo.username)).FirstOrDefault();

                ViewBag.Inbox_count = result.Inbox_count;
                ViewBag.Save_count = result.Save_count;
                

                //---------------------------   تعداد اعلان ها  ---------
                //======================================================================================
            }

            return View("~/Views/maindashboard/Notifications/Index.cshtml");
        }


        [Route("Notifications/Inbox_Notic/{page?}")]
        public PartialViewResult Inbox_Notic(int? page)
        {
            User userinfo = (User)Session["User"];

            int pageIndex = 1;
            int pagesize = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.currentpage = pageIndex;
            IPagedList<MessageListVM> result = null;
            List<MessageListVM> SP_Result = new List<MessageListVM>();

            if (userinfo != null)
            {

                //=====================================================================================
                //---------------------------   اعلان های ورودی  ---------

                SP_Result = db.Database.SqlQuery<MessageListVM>("SP_NOTICLIST_Inbox_USER @username", new SqlParameter("@username", userinfo.username)).ToList();


                //---------------------------   اعلان های ورودی  ---------
                //======================================================================================
            }
            ViewBag.totalitem = SP_Result.Count();
            result = SP_Result.ToPagedList(pageIndex, pagesize);
            return PartialView("~/Views/Shared/Partial/Notifications/_InboxNotic.cshtml", result);
        }


        [Route("Notifications/Save_Notic/{page?}")]
        public PartialViewResult Save_Notic(int? page)
        {
            User userinfo = (User)Session["User"];

            int pageIndex = 1;
            int pagesize = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.currentpage = pageIndex;
            IPagedList<MessageListVM> result = null;
            List<MessageListVM> SP_Result = new List<MessageListVM>();

            if (userinfo != null)
            {

                //=====================================================================================
                //---------------------------   اعلان های ذخیره  ---------

                SP_Result = db.Database.SqlQuery<MessageListVM>("SP_NOTICLIST_Save_USER @username", new SqlParameter("@username", userinfo.username)).ToList();


                //---------------------------   اعلان های ذخیره  ---------
                //======================================================================================
            }
            ViewBag.totalitem = SP_Result.Count();
            result = SP_Result.ToPagedList(pageIndex, pagesize);
            return PartialView("~/Views/Shared/Partial/Notifications/_SaveNotic.cshtml", result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}