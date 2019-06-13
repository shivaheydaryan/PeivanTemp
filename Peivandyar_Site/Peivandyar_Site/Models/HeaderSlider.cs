using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class HeaderSlider
    {
        public HeaderSlider() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Display(Name = "عنوان")]
        [StringLength(50)]
        [Required]
        public string header { get; set; }

        [Display(Name = "متن")]
        public string desc { get; set; }

        [Display(Name = "توضیحات")]
        public string comment { get; set; }

        [Display(Name = "لینک")]
        public string href { get; set; }

        [Display(Name = "اولویت")]
        public int? order { get; set; }

        [Display(Name = "فعال")]
        public bool? active { get; set; }
    }
}