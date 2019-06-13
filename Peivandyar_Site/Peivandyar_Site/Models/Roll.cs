using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Models
{
    public class Roll
    {
        public Roll() { }

        public int id { get; set; }

        public string date { get; set; }

        public string comment { get; set; }

        public virtual int Studentid { get; set; }
        [ForeignKey("Studentid")]
        public Student student { get; set; }

        public virtual int Teacherid { get; set; }
        [ForeignKey("Teacherid")]
        public Teacher teacher { get; set; }

        public virtual int Courseid { get; set; }
        [ForeignKey("Courseid")]
        public Course course { get; set; }


        public virtual int Classid { get; set; }
        [ForeignKey("Classid")]
        public Class Class { get; set; }


        public virtual int MentalStateid { get; set; }
        [ForeignKey("MentalStateid")]
        public MentalState mentalstate { get; set; }

    }
}