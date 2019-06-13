using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Class_User
    {
        public Class_User() { }

        [Key]
        public int id { get; set; }
        
        public virtual int Classid { get; set; }
        [ForeignKey("Classid")]
        public Class classes { get; set; }

        public virtual string username { get; set; }
        [ForeignKey("username")]
        public User user { get; set; }

        
        public virtual int? Courseid { get; set; }
        [ForeignKey("Courseid")]
        public CourseTotal coursetotal { get; set; }

        public int? Jobsid { get; set; }
        
    }
}