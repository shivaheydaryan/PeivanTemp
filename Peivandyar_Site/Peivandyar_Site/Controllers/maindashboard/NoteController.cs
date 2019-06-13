using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using PagedList;

namespace Peivandyar_Site.Controllers.maindashboard
{
    public class NoteController : Controller
    {
        // GET: Note
        DataContext db = new DataContext();
        [Route("Note/{page?}")]
        public ActionResult Index(int? page)
        {
            int pageIndex = 1;
            int pagesize = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            ViewBag.currentpage = pageIndex;
            IPagedList<Note> result = null;
            List<Note> note_list = new List<Note>();
            User userinfo = (User)Session["User"];
            if (userinfo != null)
            {
                //===========================================================
                //------------------  لیست یادداشت کاربر ------------------
                note_list = db.Notes.Where(p => p.username == userinfo.username).OrderByDescending(p=>p.Date).ToList();
                //------------------  لیست یادداشت کاربر ------------------
                //===========================================================

            }
            result = note_list.ToPagedList(pageIndex, pagesize);
            return View("~/Views/maindashboard/Note/Index.cshtml",result);
        }
    }
}