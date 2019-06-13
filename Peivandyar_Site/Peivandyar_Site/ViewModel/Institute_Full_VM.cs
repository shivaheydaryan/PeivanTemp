using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModel
{
    public class Institute_Full_VM
    {
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

        public int ostan_codevm { get; set; }
        public int city_codevm { get; set; }
        public int zone_Codevm { get; set; }

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
        public string Active { get; set; }
        public string Enter_Date { get; set; }
        public List<int> Gradeid { get; set; }

        public string Google_Map { get; set; }

        public HttpPostedFileBase BackgroundFile { get; set; }
        public HttpPostedFileBase LogoFile { get; set; }
        public HttpPostedFileBase CartFile { get; set; }
        public List<HttpPostedFileBase> InstitutesPics { get; set; }
        public int? InstituteKindid { get; set; }
    }
}