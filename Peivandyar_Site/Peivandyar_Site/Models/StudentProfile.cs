using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class StudentProfile
    {
        public StudentProfile() { }

        public int id { get; set; }

        public string email { get; set; }

        public string  address { get; set; }

        public string phone { get; set; }
    }
}