using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Discipline_Item
    {
        public Discipline_Item() { }

        public int id { get; set; }

        public string Caption { get; set; }

        public string User_type { get; set; }

        public int Type_Enco_Punish { get; set; }

        public string Comment { get; set; }
    }
}