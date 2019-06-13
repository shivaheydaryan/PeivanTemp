using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class InstituteSmallVM
    {
        public Int64 id { get; set; }
        public string name { get; set; }
        public string En_Name { get; set; }
        public string InstituteKindName { get; set; }

        public string address { get; set; }
        public bool? boyOrGirl { get; set; }
        public bool? Active { get; set; }
    }
}