using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Register_Score
    {
        public Register_Score() { }

        public int id { get; set; }

        public string  score { get; set; }

        public string date { get; set; }

        public string  comment { get; set; }

        public virtual int Type_Scoreid { get; set; }
        [ForeignKey("Type_Scoreid")]

        public virtual Type_Score type_score { get; set; }

        public virtual int Courseid { get; set; }
        [ForeignKey("Courseid")]

        public virtual Course course { get; set; }


        public virtual int Classid { get; set; }
        [ForeignKey("Classid")]

        public virtual Class Class { get; set; }



        public virtual int Instituteid { get; set; }
        [ForeignKey("Instituteid")]

        public virtual Institute institute { get; set; }

        public virtual int Teacherid { get; set; }
        [ForeignKey("Teacherid")]
        public virtual Teacher teacher { get; set; }


        public virtual  int Studentid { get; set; }
        [ForeignKey("Studentid")]
        public virtual Student student { get; set; }




    }
}