using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Peivandyar_Site.Controllers.peivandashboard
{
    public class HomePeivandController : Controller
    {
        [Route("PeivandDashboard")]
        // GET: HomePeivand
        public ActionResult Index()
        {
            return View("~/Views/peivandashboard/Home/Index.cshtml");
        }
    }
}