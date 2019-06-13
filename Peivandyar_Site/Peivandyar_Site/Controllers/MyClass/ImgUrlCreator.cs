using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MyClass
{
    public class ImgUrlCreator
    {
        public string InstituteGetURL(Int64 id)
        {
            string url = "";
            try
            {
                if ( id != 0)
                {
                    url = "~/Content/images/schools/" + id+ "/" +  id+"-main" + ".png";
                    bool file_exists = File.Exists(HttpContext.Current.Server.MapPath(url));
                    if (file_exists)
                    {
                        url = url;
                    }
                    else
                    {
                        url = "~/Content/images/schools/fake-school.png";
                    }
                }
                else
                {
                    url = "~/Content/images/schools/fake-school.png";
                }
            }
            catch (Exception ex)
            {
                url = "~/Content/images/schools/fake-school.png";
            }


            return url;
        }


        public string InstituteLogoGetURL(Int64 id)
        {
            string url = "";
            try
            {
                if (id != 0)
                {
                    url = "~/Content/images/schools/" + id + "/" + id + "-logo" + ".png";
                    bool file_exists = File.Exists(HttpContext.Current.Server.MapPath(url));
                    if (file_exists)
                    {
                        url = url;
                    }
                    else
                    {
                        url = "~/Content/images/schools/fake-logo.png";
                    }
                }
                else
                {
                    url = "~/Content/images/schools/fake-logo.png";
                }
            }
            catch (Exception ex)
            {
                url = "~/Content/images/schools/fake-logo.png";
            }


            return url;
        }


    }
}