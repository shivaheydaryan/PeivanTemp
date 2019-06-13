using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Institute_CourseTotal
    {
        public Institute_CourseTotal() { }

        [Key]
        public int id { get; set; }

        public virtual int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public Institute Institute { get; set; }

        public virtual int Termid { get; set; }
        [ForeignKey("Termid")]
        public Term term { get; set; }

        public virtual int CourseTotalid { get; set; }
        [ForeignKey("CourseTotalid")]
        public CourseTotal coursetotal { get; set; }

    }
}