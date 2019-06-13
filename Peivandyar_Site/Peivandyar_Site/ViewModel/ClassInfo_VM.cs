using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class ClassInfo_VM
    {
        public int? id { get; set; }
        public string Name { get; set; }
        public int? capasity { get; set; }
        public Int64 Instituteid { get; set; }
        public int Termid { get; set; }
        public Int64 Gradeid { get; set; }

    }
}