using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class StringClass_Convert
    {
        public StringClass_Convert (){}

        public string Convert_to_4str(int? code)
        {
            string result = "";

            result = code.ToString();
            int counter = result.Length;
            for (int i = 0; i < 4 - counter; i++)
            {
                result = "0" + result;
            }

            return result;
        }

        public string Convert_4str_To_orginal(string Code)
        {
            
            while(Code.StartsWith("0"))
            {
                Code = Code.Substring(1,Code.Length-1);
            }
            return Code;
        }

        public string GetEnglishNumber(string persianNumber)
        {
            string englishNumber = "";
            englishNumber = persianNumber.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
            return englishNumber;
        }

        public string GetPersianNumber(string englishNumber)
        {
            string persianNumber = "";
            persianNumber = englishNumber.Replace("0","۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").Replace("5", "۵").Replace("6", "۶").Replace("7", "۷").Replace("8", "۸").Replace("9", "۹");
            return persianNumber;
        }

    }
}


