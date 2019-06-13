using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Class_Student
    {
        public Class_Student() { }

        [Key]
        public int id { get; set; }

        
        public virtual int Classid { get; set; }
        [ForeignKey("Classid")]
        public Class classes { get; set; }

        public virtual int Studentid { get; set; }
        [ForeignKey("Studentid")]
        public Student student { get; set; }
    }
}