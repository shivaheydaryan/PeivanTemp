using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Models
{
    public class ArticleUser
    {
        public ArticleUser() { }

        public int id { get; set; }
        [Range(0, float.MaxValue)]
        public float Rank { get; set; }
        public virtual string username { get; set; }
        [ForeignKey("username")]
        public User user { get; set; }

        public virtual int Articleid { get; set; }
        [ForeignKey("Articleid")]
        public Article article { get; set; }
    }
}