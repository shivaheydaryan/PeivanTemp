using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class LessonPlan
    {
        public LessonPlan() { }

        public int id { get; set; }

        public int status { get; set; }

        public string date { get; set; }

        public int session { get; set; }

        public string subject { get; set; }

        public string comment { get; set; }

        public int Priority { get; set; }

        public virtual int Courseid { get; set; }
        [ForeignKey("Courseid")]
        public Course course { get; set; }

        public virtual int Teacherid { get; set; }
        [ForeignKey("Teacherid")]
        public Teacher teacher { get; set; }

    }
}