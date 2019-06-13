using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Quantitative_Score
    {
        public Quantitative_Score() { }

        public int id { get; set; }

        public int max { get; set; }

        public string comment { get; set; }
    }
}