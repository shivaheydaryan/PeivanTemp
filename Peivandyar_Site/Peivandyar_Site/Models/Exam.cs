using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Exam
    {
        public Exam() { }

        public int id { get; set; }

        
        public string day { get; set; }

        public string start_time { get; set; }

        public string end_time { get; set; }

        public virtual int courseid { get; set; }
        [ForeignKey("courseid")]
        public virtual Course course { get; set; }

        public virtual int Classid { get; set; }
        [ForeignKey("Classid")]
        public virtual Class Class { get; set; }


        public virtual int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public virtual Institute institute { get; set; }

    }
}