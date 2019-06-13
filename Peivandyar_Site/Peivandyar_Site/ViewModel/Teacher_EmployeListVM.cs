using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class Teacher_EmployeListVM
    {
        public string image { get; set; }
        public string username { get; set; }
        public int max_Instituteid { get; set; }
        public int Max_termid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
              
        public string Code_Melli { get; set; }
        public bool? gender { get; set; }
        public string Fathername { get; set; }
        public bool? Teacher { get; set; }
        public bool? Employe { get; set; }
        public string Teacher_Code { get; set; }
        public string Employe_Code { get; set; }
        public string Institute_name { get; set; }
        public string educationalType { get; set; }
    }
}