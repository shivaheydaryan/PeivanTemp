using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class StudentPersonalInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int64 id { get; set; }

        public virtual string username { get; set; }
        [ForeignKey("username")]
        public User user { get; set; }

        public string Shsh { get; set; }

        public string MotherName { get; set; }

        public string Lastname_Mother { get; set; }

        public string Address_Primary { get; set; }

        public string Address_Secondary { get; set; }

        public string tell { get; set; }

        public string Mobile { get; set; }
    }
}