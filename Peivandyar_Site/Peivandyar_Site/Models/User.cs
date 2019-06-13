using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User
    {
        public User() { }

        [Key]
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }

        public string lastname { get; set; }
        public string Fathername { get; set; }

        public DateTime? birtday { get; set; }

        public bool? gender { get; set; }
        public bool? Active { get; set; }

        public bool? Manager { get; set; }
        public string Manager_Code { get; set; }

        public bool? Teacher { get; set; }
        public string Teacher_Code { get; set; }

        public bool? Student { get; set; }
        public string Student_Code { get; set; }

        public bool? Parent { get; set; }
        public string Parent_Code { get; set; }

        public bool? Employe { get; set; }
        public string Employe_Code { get; set; }

        [MaxLength(10)]
        public string Code_Melli { get; set; }



        //userHealth table
    }
}