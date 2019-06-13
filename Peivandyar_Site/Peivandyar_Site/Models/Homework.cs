using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Homework
    {
        public Homework() { }

        [Key]

        public int id { get; set; }

        public virtual int Classid { get; set; }
        [ForeignKey("Classid")]
        public virtual Class Class { get; set; }

        public virtual int Courseid { get; set; }
        [ForeignKey("Courseid")]
        public virtual Course Course { get; set; }

        public string Date_Start { get; set; }

        public string Date_Expire { get; set; }

        public string Description { get; set; }

        public byte[] Attach_File { get; set; }

        public int Defficalty  { get; set; }

        public string Comment { get; set; }



    }
}