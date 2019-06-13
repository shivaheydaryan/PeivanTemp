using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App_Start
{
    public class AuthenticationAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isvalid = false;

            //--------------------------
            /* اگر کاربر لاگین داشته */

            if (HttpContext.Current.Session["User"] != null)
            {
                isvalid = true;
            }
            else
            {
                isvalid = false;
                goto Notvalid;
            }

        /* اگر کاربر لاگین داشته */
        //--------------------------

        Notvalid:

            if (isvalid)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                //filterContext.Result = new RedirectResult("~/Login/Index");

                var values = new RouteValueDictionary(new
                {
                    
                    controller = "Login",
                    //code = "1"
                });
                filterContext.Result = new RedirectToRouteResult(values);
                //Method 2 also tried Login/Index?code=1
                //filterContext.Result = new RedirectResult("Login/Index/1");
            }


            base.OnActionExecuting(filterContext);
        }
    }
}