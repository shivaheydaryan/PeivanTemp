using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class Jobs
    {
        public Int64 id { get; set; }
        public string Caption { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
    }
}