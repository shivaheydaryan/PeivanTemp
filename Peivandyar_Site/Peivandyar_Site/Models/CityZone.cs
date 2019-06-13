using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CityZone
    {
        public CityZone() { }

        [Key]
        [Required]
        [Column(Order = 1)]
        public int Code { get; set; }

        [Key]
        [Required]
        [Column(Order = 2)]
        public int Zone_Code { get; set; }

        public string Pname { get; set; }

        public string Ename { get; set; }

        public byte? active { get; set; }


    }
}