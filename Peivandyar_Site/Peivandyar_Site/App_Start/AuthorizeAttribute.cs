using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace App_Start
{
    public class AuthorizeAttribute:ActionFilterAttribute
    {
        public string[] Jobs_need { get; set; }

        public AuthorizeAttribute(string[] job)
        {
            Jobs_need = job;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)

        {


            bool isvalid = false;
            

            if (HttpContext.Current.Session["Job"]!=null)
            {
                List<ViewModel.User_Jobs_VM> user_job = (List<ViewModel.User_Jobs_VM>)HttpContext.Current.Session["Job"];
                
                if (user_job.Select(p => Jobs_need.Contains(p.Caption)).FirstOrDefault())
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
                isvalid = false;
            }

            

            if (isvalid)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                //filterContext.Result = new RedirectResult("~/Login/Index");

                var values = new RouteValueDictionary(new
                {
                    //action = "Index",
                    controller = "Login",
                    //code = "1"
                });
                filterContext.Result = new RedirectToRouteResult(values);
                //Method 2 also tried Login/Index?code=1
                //filterContext.Result = new RedirectResult("Login/Index/1");
            }

        }



    }
}