using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Message_Content
    {
        public Message_Content() { }

        [Key]
        public int id { get; set; }

        public string username_sender { get; set; }
        [ForeignKey("username_sender")]
        public virtual User user { get; set; }

        public string title { get; set; }
        public string msg_text { get; set; }

        public string date { get; set; }

        public bool? autosend { get; set; }

        public Byte? type { get; set; }
        public int instituteid { get; set; }
        [ForeignKey("instituteid")]
        public virtual Institute institute { get; set; }

    }
}