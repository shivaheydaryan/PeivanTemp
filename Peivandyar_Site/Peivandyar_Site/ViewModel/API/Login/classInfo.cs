using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel.API.Login
{
    public class classInfo
    {
        public int id { get; set; }
        public int Instituteid { get; set; }
        public string name { get; set; }
        public string courseTitle { get; set; }
        public int studentsNumber { get; set; }

        public List<classSchedule> schedule;
        
    }
}