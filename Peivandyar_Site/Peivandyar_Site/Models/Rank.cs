using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Rank
    {
        public Rank (){ }

        [Key]
        public int id { get; set; }
        public virtual string Usernameget { get; set; }
        [ForeignKey("Usernameget")]
        public virtual User Userget { get; set; }

        public virtual string Usernameset { get; set; }
        [ForeignKey("Usernameset")]
        public virtual User Userset { get; set; }


        public virtual int Classid { get; set; }
        [ForeignKey("Classid")]
        public virtual Class Class { get; set; }

        /*public virtual int Termid { get; set; }
        [ForeignKey("Termid")]
        public virtual Term Term { get; set; }*/ //داخل خود جدول class termid هست

        public int RankRange { get; set; }

    }
}