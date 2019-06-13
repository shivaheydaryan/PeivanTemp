using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User_Health
    {
        public User_Health() { }

        [Key]
        public int id { get; set; }

        public virtual int Userid { get; set; }
        [ForeignKey("Userid")]
        public virtual User User { get; set; }

        public virtual int Healthid { get; set; }
        [ForeignKey("Healthid")]
        public virtual Health Health { get; set; }
    }
}