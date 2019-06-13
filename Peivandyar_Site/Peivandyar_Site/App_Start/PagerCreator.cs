using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace App_Start
{
    public class PagerCreator
    {
        public string PagerCreator_Span(int Current_page,int Total_page,string Scroll_id)
        {
            string html = "";


            if(Total_page>0)
            {
                if (Current_page == 1)
                {
                    html += "<li class=\"page-item btn-prev disabled\">";
                    html += "<a href=\""+ Scroll_id + "\" class=\"page-link\"  tabindex=\"-1\">قبلی</a>";
                    html += "</li>";
                }
                else
                {
                    html += "<li class=\"page-item btn-prev\">";
                    html += "<a href=\"" + Scroll_id + "\" class=\"page-link\" title=\"" + (Current_page - 1) + "\" tabindex=\"-1\">قبلی</a>";
                    html += "</li>";
                }




                for (int i = 1; i <= Total_page; i++)
                {
                    if (Current_page == i)
                    {
                        html += "<li class=\"page-item active disabled\"><a href=\"" + Scroll_id + "\" class=\"page-link\" title=\"" + i + "\" >" + i + "</a></li>";
                    }
                    else
                    {
                        html += "<li class=\"page-item\"><a href=\"" + Scroll_id + "\"  class=\"page-link\" title=\"" + i + "\">" + i + "</a></li>";
                    }

                }


                if (Current_page == Total_page)
                {
                    html += "<li class=\"page-item btn-next disabled\" >";
                    html += "<a href=\"" + Scroll_id + "\" class=\"page-link\">بعدی</a>";
                    html += "</li>";

                }
                else
                {
                    html += "<li class=\"page -item btn-next\" >";
                    html += "<a href=\"" + Scroll_id + "\"  class=\"page-link\" title=\"" + (Current_page + 1) + "\">بعدی</a>";
                    html += "</li>";


                }
            }

            

            return html;
        }
    }
}