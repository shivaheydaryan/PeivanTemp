using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Institute_Course
    {
        public Institute_Course() { }

        [Key]
        public int id { get; set; }

        public bool? Active { get; set; }

        public virtual int CourseTotalid { get; set; }

        [ForeignKey("CourseTotalid")]

        public CourseTotal coursetotal { get; set; }

        public virtual int? Classid { get; set; }
        [ForeignKey("Classid")]
        public virtual Models.Class classs { get; set; }

        public virtual int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public Institute Institute { get; set; }

        public int? Count_CourseInInstitute { get; set; }
    }
}