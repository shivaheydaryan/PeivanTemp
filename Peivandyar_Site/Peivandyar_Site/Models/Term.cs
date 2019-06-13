using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Term
    {
        public Term() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }

        public byte? Term_Number { get; set; }
        public string Description { get; set; }

        public bool? Active { get; set; }

        public Int64 Instituteid { get; set; }

        public bool? CurrentTerm { get; set; }

    }
}