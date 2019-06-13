using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    public class CourseTotal
    {
        public CourseTotal() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string name { get; set; }

        public int? rank { get; set; }

        public string period_of_time { get; set; }

        public string  comment { get; set; }

    }
}