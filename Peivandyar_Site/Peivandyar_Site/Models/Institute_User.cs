using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Institute_User
    {

        public Institute_User() { }

        [Key]
        public int id { get; set; }

        public virtual int Instituteid { get; set; }
        [ForeignKey("Instituteid")]
        public Institute Institute { get; set; }

        public virtual string username { get; set; }
        [ForeignKey("username")]
        public User user { get; set; }

        public Byte Manager_Teacher_Student_Parent { get; set; }

        public virtual int Termid { get; set; }
        [ForeignKey("Termid")]
        public Term term { get; set; }
    }
}