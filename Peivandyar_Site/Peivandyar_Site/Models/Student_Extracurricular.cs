using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Student_Extracurricular
    {
        public Student_Extracurricular() { }

        [Key]
        public int id { get; set; }

        public virtual int Studentid { get; set; }
        [ForeignKey("Studentid")]
        public virtual Student Student { get; set; }

        public virtual int Extracurricularid { get; set; }
        [ForeignKey("Extracurricularid")]
        public Extracurricular Extracurricular { get; set; }

        public string Score { get; set; }

        public string Comment { get; set; }
    }
}