using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Institute_Grade
    {
        public Institute_Grade() { }

        [Key]
        public int id { get; set; }


        public virtual int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public Institute institute { get; set; }

        public virtual int Gradeid { get; set; }
        [ForeignKey("Gradeid")]
        public Grade grade { get; set; }

    }
}