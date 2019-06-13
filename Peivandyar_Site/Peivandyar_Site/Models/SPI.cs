using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class SPI
    {
        public SPI() { }

        [Key]
        public int id { get; set; }
        public int Studentid { get; set; }
        [ForeignKey("Studentid")]
        public Student Student { get; set; }

        public int Parentid { get; set; }
        [ForeignKey("Parentid")]
        

        public int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public Institute Institute { get; set; }

    }
}