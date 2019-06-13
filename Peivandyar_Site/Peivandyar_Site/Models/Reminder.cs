using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Reminder
    {
        public Reminder() { }

        [Key]
        public int id { get; set; }

        public string username { get; set; }
        [ForeignKey("username")]
        public virtual User user { get; set; }

        public string title { get; set; }
        public string content { get; set; }

        public string date { get; set; }

        public Byte? status { get; set; }
    }
}