using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Teacher
    {
        public Teacher() { }

        [Key]
        public int id { get; set; }
        public int? rank { get; set; }
        public virtual string Username { get; set; }
        [ForeignKey("Username")]
        public User User { get; set; }
    }
}