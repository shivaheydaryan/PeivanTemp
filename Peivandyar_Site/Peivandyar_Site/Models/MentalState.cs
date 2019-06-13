using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class MentalState
    {
        public MentalState() { }

        public int id { get; set; }
        public string caption { get; set; }
        public string comment { get; set; }
    }
}