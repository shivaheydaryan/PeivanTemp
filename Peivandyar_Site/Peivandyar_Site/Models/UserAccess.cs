using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserAccess
    {
        public UserAccess() { }

        [Key]
        public int id { get; set; }

        public virtual string Username { get; set; }
        [ForeignKey("Username")]
        public  User  User { get; set; }

        public virtual  int Accessid { get; set; }
        [ForeignKey("Accessid")]
        public Access Access { get; set; }


    }
}