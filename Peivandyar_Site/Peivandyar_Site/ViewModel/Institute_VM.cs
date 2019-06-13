using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class Institute_VM
    {
        public Models.Institute Institute { get; set; }

        public List<Models.Grade> Grades { get; set; }
    }
}