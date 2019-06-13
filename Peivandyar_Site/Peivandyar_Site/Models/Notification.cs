using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Notification
    {
        public  Notification() { }

        [Key]
        public int id { get; set; }

        public string username_sender { get; set; }
        [ForeignKey("username_sender")]

        public virtual User user_sender { get; set; }
        public string username_receiver { get; set; }
        [ForeignKey("username_receiver")]

        public virtual User user_receiver { get; set; }

        public string Title { get; set; }
        public string Msg_Content { get; set; }

        public DateTime Date { get; set; }

        public bool? Autosend { get; set; }

        public Byte? Type { get; set; }

        public Byte? status_Sender { get; set; }
        public Byte? status_Receiver { get; set; }

        public bool? Attach { get; set; }

        public int instituteid { get; set; }
        [ForeignKey("instituteid")]
        public virtual Institute institute { get; set; }

    }
}