using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class Institute_Term_VM
    {
        public int id { get; set; }

        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }

        public byte? Term_Number { get; set; }
        public string Description { get; set; }

        public bool? Active { get; set; }
        public bool? CurrentTerm { get; set; }
    }
}