using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    public class Article
    {
        public Article() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public string Title { get; set; }
        public string En_Title { get; set; }
        public string Description { get; set; }
        public string writer { get; set; }
        public int? Download { get; set; }
        [Range(0, float.MaxValue)]
        public float? Rank { get; set; }
        public bool? Active { get; set; }
        public DateTime? Date { get; set; }
    }
}