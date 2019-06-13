using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Extracurricular
    {
        public Extracurricular() { }

        [Key]
        public int id { get; set; }

        public string  name { get; set; }

        public virtual int Teacherid { get; set; }
        [ForeignKey("Teacherid")]

        public virtual Teacher Teacher { get; set; }

        public virtual int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public virtual Institute Institute { get; set; }

        //public virtual int Termid { get; set; }
        //[ForeignKey("Termid")]
        //public virtual Term Term { get; set; }

        public string Comment { get; set; }

    }
}