using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Notification_Content
    {
        public Notification_Content() { }

        [Key]
        public int id { get; set; }

        public string username_sender { get; set; }
        [ForeignKey("username_sender")]
        public virtual User user { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Date { get; set; }

        public Byte? Type { get; set; }
        public int instituteid { get; set; }
        [ForeignKey("instituteid")]
        public virtual Institute institute { get; set; }


    }
}