using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App_Start
{
    public class AccessAttribute : ActionFilterAttribute
    {

        public string[] Access_need { get; set; }

        public bool Redirect { get; set; }

        public AccessAttribute(string[] access_need,bool redirect)
        {
            Access_need = access_need;
            Redirect = redirect;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            bool isvalid = false;

            if (HttpContext.Current.Session["Access"] != null)
            {
                List<Models.Access> user_access = (List<Models.Access>)HttpContext.Current.Session["Access"];
                if (user_access.Select(p => Access_need.Contains(p.caption)).Count()>0)
                {
                    isvalid = true;

                }
                else
                {
                    isvalid = false;
                }
            }
            else
            {
                isvalid = true;
            }

            if (isvalid)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                if(Redirect)
                {
                    //filterContext.Result = new RedirectResult("~/Login/Index");

                    var values = new RouteValueDictionary(new
                    {
                        //action = "Index",
                        controller = "Login",//------- اینجا باید به صفحه دسترسی ندارد برود
                                             //code = "1"
                    });
                    filterContext.Result = new RedirectToRouteResult(values);
                    //Method 2 also tried Login/Index?code=1
                    //filterContext.Result = new RedirectResult("Login/Index/1");
                }
                else
                {
                    filterContext.Result = new EmptyResult();
                }

            }

            
        }


    }
}