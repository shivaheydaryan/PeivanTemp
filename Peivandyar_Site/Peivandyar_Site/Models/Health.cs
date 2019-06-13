using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Health
    {
        public Health() { }

        [Key]
        public int id { get; set; }


        public string illness_record { get; set; }//سابقه بیماری

        public string illness_Special { get; set; }//بیماری خاص

        public string allergy_medicine { get; set; }

        public string Used_medicine { get; set; }

        public float Height { get; set; }

        public float Weight { get; set; }

        public string Blood { get; set; }

        public string Physical_Status { get; set; }

        public string Doctor_name { get; set; }

        public string Doctor_Code { get; set; }

        public string Doctor_Address { get; set; }

    }
}