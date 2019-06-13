using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class Schedule_VM
    {
        public int id { get; set; }
        public int Classid { get; set; }
        public string NameOfWeek { get; set; }
        public DateTime? start_time { get; set; }
        public DateTime? end_time { get; set; }
        public string Teacher_one { get; set; }
        public string Teacher_two { get; set; }
        public byte? Type_Ring { get; set; }
        public int? Courseid_one { get; set; }
        public int? Courseid_two { get; set; }
        public byte? order_ring { get; set; }
        public byte? order_week { get; set; }
        public string teacher_one_name { get; set; }
        public string teacher_two_name { get; set; }
        
        public string Course_one_name { get; set; }
        public string Course_two_name { get; set; }
    }
}