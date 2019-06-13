using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Class
    {
        public Class() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string  Name { get; set; }

        public int? capasity { get; set; }
               
        public bool? Active { get; set; }

        

        public virtual int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public Institute institute { get; set; }


        public virtual int Termid { get; set; }
        [ForeignKey("Termid")]

        public Term term { get; set; }


        public virtual int? Gradeid { get; set; }
        [ForeignKey("Gradeid")]
        public Grade grade { get; set; }
        public string Creator { get; set; }


    }

}