using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class ICC
    {

        public ICC() { }

        [Key]
        public int id { get; set; }


        public int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public Institute Institute { get; set; }

        public int Classid { get; set; }
        [ForeignKey("Classid")]
        public Class Class { get; set; }

        public int Courseid { get; set; }
        [ForeignKey("Courseid")]
        public Course Course { get; set; }

    }
}