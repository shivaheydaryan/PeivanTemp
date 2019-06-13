using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Course_STC
    {
        public Course_STC() { }

        [Key]
        public int id { get; set; }

        public int Courseid { get; set; }
        [ForeignKey("Courseid")]
        public virtual Course Course { get; set; }


        public int Studentid { get; set; }
        [ForeignKey("Studentid")]
        public virtual Student Student { get; set; }


        public int Teacherid { get; set; }
        [ForeignKey("Teacherid")]
        public virtual Teacher Teacher { get; set; }


        public int Classid { get; set; }
        [ForeignKey("Classid")]
        public virtual Class Class { get; set; }


    }
}