using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Message_Receiver
    {
        public Message_Receiver() { }

        [Key]
        public int id { get; set; }

        public string username_receiver { get; set; }
        [ForeignKey("username_receiver")]
        public virtual User user { get; set; }

        public int message_content_id { get; set; }
        [ForeignKey("message_content_id")]
        public virtual Message_Content message_content { get; set; }


        

        public Byte? status { get; set; }
    }
}