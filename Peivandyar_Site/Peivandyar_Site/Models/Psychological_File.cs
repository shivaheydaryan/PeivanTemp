using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Psychological_File
    {
        public Psychological_File() { }

        [Key]

        public int id { get; set; }

        public virtual string Usernameget { get; set; }
        [ForeignKey("Usernameget")]
        public virtual User Userget { get; set; }

        public virtual string Username_Doc { get; set; }
        [ForeignKey("Username_Doc")]
        public virtual User User_Doc { get; set; }

        public virtual string Usernameset { get; set; }
        [ForeignKey("Usernameset")]
        public virtual User Userset { get; set; }

        public byte[] Attach_File { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }
    }
}