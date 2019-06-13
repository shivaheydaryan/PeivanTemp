using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Institute
    {
        public Institute() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string name { get; set; }
        public string En_Name { get; set; }
        public string postalCode { get; set; }
        public string fax { get; set; }
        public string tel1 { get; set; }
        public string tel2 { get; set; }

        public string mobile1 { get; set; }

        public string mobile2 { get; set; }
        public string website { get; set; }
        public string educationalCode { get; set; }
        
        
        public string address { get; set; }
        public bool? boyOrGirl { get; set; }
        public string city_code { get; set; }
        
        public string shoar { get; set; }

        public string Email { get; set; }
        
        public string Group_Channel1 { get; set; }
        public string Group_Channel2 { get; set; }
        public string Group_Channel3 { get; set; }
        public string Group_Channel4 { get; set; }
        public string Group_Channel5 { get; set; }

        public byte? order { get; set; }

        public string Description { get; set; }
        public bool? Active { get; set; }



        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy:MM:dd}")]
        public DateTime? Enter_Date { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy:MM:dd}")]
        public DateTime? Edit_Date { get; set; }

        public string Google_Map { get; set; }

        public int? InstituteKindid { get; set; }
        
    }
}