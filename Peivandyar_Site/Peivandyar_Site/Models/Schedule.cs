using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Schedule
    {
        public Schedule() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public virtual int Classid { get; set; }
        [ForeignKey("Classid")]
        public virtual Class Class { get; set; }

        public string NameOfWeek { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime? start_time { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime? end_time { get; set; }

        public Byte? order_ring { get; set; }
        public Byte? order_week { get; set; }




        public virtual int? Courseid_one { get; set; }
        [ForeignKey("Courseid_one")]
        public virtual CourseTotal course_one { get; set; }


        public virtual int? Courseid_two { get; set; }
        [ForeignKey("Courseid_two")]
        public virtual CourseTotal course_two { get; set; }



        public virtual string Teacher_one { get; set; }
        [ForeignKey("Teacher_one")]
        public User user_one { get; set; }

        public virtual string Teacher_two { get; set; }
        [ForeignKey("Teacher_two")]

        public User user_two { get; set; }

        public Byte? Type_Ring { get; set; }








    }
}