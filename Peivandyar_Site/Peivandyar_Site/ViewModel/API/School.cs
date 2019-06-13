using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.API
{
    public class School
    {
        public int id { get; set; }
        public string schoolName { get; set; }
        public string address { get; set; }
        public string province { get; set; }
        public string town { get; set; }
        public int region { get; set; }
        public string phoneNumber { get; set; }
        public bool liked { get; set; }
        public string imageUrl { get; set; }
        public string about { get; set; }
        public string gender { get; set; }
        public string lastRefreshed { get; set; }
    }
}