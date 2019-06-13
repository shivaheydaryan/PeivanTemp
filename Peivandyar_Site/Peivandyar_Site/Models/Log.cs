using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Log
    {
        public Log() { }

        [Key]
        public int id { get; set; }

        

        public string Controller { get; set; }
        public string Action { get; set; }
        public string Ip { get; set; }
        public DateTime? Date { get; set; }
        public string Username { get; set; }



        public string OS { get; set; }

        public string Browser { get; set; }

        public string Description { get; set; }
        

        

        

    }
}