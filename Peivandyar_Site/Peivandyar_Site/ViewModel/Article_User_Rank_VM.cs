using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class Article_User_Rank_VM
    {
        public Article_User_Rank_VM() { }

        public int id { get; set; }

        public string Title { get; set; }
        public string En_Title { get; set; }
        public string Description { get; set; }
        public string writer { get; set; }
        public int? Download { get; set; }
        
        public float? Rank { get; set; }
        public bool? Active { get; set; }
        public DateTime? Date { get; set; }

        public float? User_Rank { get; set; }

    }
}