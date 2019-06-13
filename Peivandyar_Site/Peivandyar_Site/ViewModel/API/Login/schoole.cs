using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.API.Login
{
    public class School
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<classInfo> classes { get; set; }
    }
}