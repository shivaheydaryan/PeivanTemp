using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class StudentInfoesVM
    {
        
        public virtual string username { get; set; }
        public virtual string password { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }
        public string Fathername { get; set; }
        public DateTime? birtday { get; set; }
        public bool? gender { get; set; }
        public string Student_Code { get; set; }

        public string Code_Melli { get; set; }

        public string Shsh { get; set; }

        public string MotherName { get; set; }

        public string Lastname_Mother { get; set; }

        public string Address_Primary { get; set; }

        public string Address_Secondary { get; set; }

        public string tell { get; set; }

        public string Mobile { get; set; }

        public bool? Active { get; set; }
    }
}