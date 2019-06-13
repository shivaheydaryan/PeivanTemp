using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class register_institute
    {
        public register_institute() { }

        [Key]
        public int id { get; set; }

        public string InstituteName { get; set; }

        public string InstituteType { get; set; }

        public string AcceptCode { get; set; }

        public string ManagerName { get; set; }

        public string CodeMelli { get; set; }

        public string Mobile { get; set; }

        public string Tell { get; set; }

        public string Ostan { get; set; }

        public string City { get; set; }

        public string Address { get; set; }
    }
}