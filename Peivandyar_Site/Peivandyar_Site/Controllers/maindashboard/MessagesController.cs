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
    public class MessagesController : Controller
    {
        DataContext db = new DataContext();
        public class result_SP_ALL_MSG_COUNT_USER
        {
            public int? Inbox_count { get; set; }
            public int? Send_count { get; set; }
            public int? Save_count { get; set; }
            public int? Delete_count { get; set; }
        }
        // GET: Messages
        [Route("Message")]
        public ActionResult Index()
        {
            

            User userinfo = (User)Session["User"];



            
            if (userinfo != null)
            {

                //=====================================================================================
                //---------------------------   تعداد پیام ها  ---------


                result_SP_ALL_MSG_COUNT_USER result = db.Database.SqlQuery<result_SP_ALL_MSG_COUNT_USER>("SP_ALL_MSG_COUNT_USER @username", new SqlParameter("@username", userinfo.username)).FirstOrDefault();

                ViewBag.Inbox_count = result.Inbox_count;
                ViewBag.Send_count = result.Send_count;
                ViewBag.Save_count = result.Save_count;
                ViewBag.Delete_count = result.Delete_count;

                //---------------------------   تعداد پیام ها  ---------
                //======================================================================================



            }


            return View("~/Views/maindashboard/Messages/Index.cshtml");
            
        }

        [Route("Message/Inbox_MSG/{page?}")]
        public PartialViewResult Inbox_MSG(int? page)
        {
            User userinfo = (User)Session["User"];

            int pageIndex = 1;
            int pagesize = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.currentpage = pageIndex;
            IPagedList<MessageListVM> result = null;
            List<MessageListVM> SP_Result=new List<MessageListVM>();
            
            if (userinfo != null)
            {

                //=====================================================================================
                //---------------------------   پیام های ورودی  ---------

                SP_Result = db.Database.SqlQuery<MessageListVM>("SP_MSGLIST_Inbox_USER @username", new SqlParameter("@username", userinfo.username)).ToList();


                //---------------------------   پیام های ورودی  ---------
                //======================================================================================
            }
            ViewBag.totalitem = SP_Result.Count();
            result = SP_Result.ToPagedList(pageIndex, pagesize);
            return PartialView("~/Views/Shared/Partial/Message/_InboxMSG.cshtml",result);
        }


        [Route("Message/Send_MSG/{page?}")]
        public PartialViewResult Send_MSG(int? page)
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
                //---------------------------   پیام های ارسالی  ---------

                SP_Result = db.Database.SqlQuery<MessageListVM>("SP_MSGLIST_Send_USER @username", new SqlParameter("@username", userinfo.username)).ToList();


                //---------------------------   پیام های ارسالی  ---------
                //======================================================================================
            }
            ViewBag.totalitem = SP_Result.Count();
            result = SP_Result.ToPagedList(pageIndex, pagesize);
            return PartialView("~/Views/Shared/Partial/Message/_SendMSG.cshtml", result);
        }


        [Route("Message/Save_MSG/{page?}")]
        public PartialViewResult Save_MSG(int? page)
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
                //---------------------------   پیام های ذخیره  ---------

                SP_Result = db.Database.SqlQuery<MessageListVM>("SP_MSGLIST_Save_USER @username", new SqlParameter("@username", userinfo.username)).ToList();


                //---------------------------   پیام های ذخیره  ---------
                //======================================================================================
            }
            ViewBag.totalitem = SP_Result.Count();
            result = SP_Result.ToPagedList(pageIndex, pagesize);
            return PartialView("~/Views/Shared/Partial/Message/_SaveMSG.cshtml", result);
        }

        [Route("Message/Delete_MSG/{page?}")]
        public PartialViewResult Delete_MSG(int? page)
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
                //---------------------------   پیام های دورریخته  ---------

                SP_Result = db.Database.SqlQuery<MessageListVM>("SP_MSGLIST_Delete_USER @username", new SqlParameter("@username", userinfo.username)).ToList();


                //---------------------------   پیام های دورریخته  ---------
                //======================================================================================
            }
            ViewBag.totalitem = SP_Result.Count();
            result = SP_Result.ToPagedList(pageIndex, pagesize);
            return PartialView("~/Views/Shared/Partial/Message/_DeleteMSG.cshtml", result);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }

}