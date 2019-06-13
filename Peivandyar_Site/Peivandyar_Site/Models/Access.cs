using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Access
    {
        public Access(){}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string caption { get; set; }

        public string comment { get; set; }

        public bool? Active { get; set; }

        
        public int Instituteid { get; set; }
    }
}