using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Institute_Term
    {
        public Institute_Term() { }

        [Key]

        public int id { get; set; }


        public virtual int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public Institute Institute { get; set; }


        public virtual int Termid { get; set; }
        [ForeignKey("Termid")]
        public virtual Term Term { get; set; }


        public bool? Active { get; set; }

        public bool? CurrentTerm { get; set; }
    }
}