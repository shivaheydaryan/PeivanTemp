using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Note
    {
        public Note() { }

        [Key]
        public int id { get; set; }

        public string username{ get; set; }
        [ForeignKey("username")]

        public virtual User user { get; set; }

        public string Title { get; set; }
        public string Msg_Content { get; set; }

        public string Date { get; set; }

        public Byte? Type { get; set; }

        public Byte? status { get; set; }

        public bool? Attach { get; set; }



    }
}