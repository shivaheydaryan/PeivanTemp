using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Encouragement_Punishment
    {
        public Encouragement_Punishment() { }

        [Key]
        public int id { get; set; }

        public virtual string Usernameset { get; set; }
        [ForeignKey("Usernameset")]
        public virtual User Userset { get; set; }

        public virtual string Usernameget { get; set; }
        [ForeignKey("Usernameget")]
        public virtual User Userget { get; set; }

        public virtual int Discipline_item_id { get; set; }
        [ForeignKey("Discipline_item_id")]
        public virtual Discipline_Item Discipline_Item { get; set; }

        public string Date { get; set; }

        public string Comment { get; set; }
    }
}