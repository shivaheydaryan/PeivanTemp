using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Qualitative_Score
    {
        public Qualitative_Score() { }

        public int id { get; set; }

        public string  max { get; set; }
        public int value { get; set; }

        public string comment { get; set; }
    }
}