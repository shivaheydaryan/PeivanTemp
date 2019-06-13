using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Globalization;
using System.Text;

namespace App_Start
{
    public class Date_Shamsi_Miladi
    {

        public string MiladiToShamsi(DateTime _date)
        {
            PersianCalendar pc = new PersianCalendar();
            StringBuilder sb = new StringBuilder();
            sb.Append(pc.GetYear(_date).ToString("0000"));
            sb.Append("/");
            sb.Append(pc.GetMonth(_date).ToString("00"));
            sb.Append("/");
            sb.Append(pc.GetDayOfMonth(_date).ToString("00"));
            return sb.ToString();

        }


        public string shamsitomiladi(string s)
        {
            string dat = s;
            string sal = dat.Substring(0, 4);
            string mah = dat.Substring(5, 2);
            string roz = dat.Substring(8, 2);
            PersianCalendar pc = new PersianCalendar();
            string ret = pc.ToDateTime(Convert.ToInt32(sal), Convert.ToInt32(mah), Convert.ToInt32(roz), 0, 0, 0, 0).ToString();
            return ret.ToString();
        }

        public byte WeekDayNumberMiladiToSahmsi(byte Weekday)
        {

            switch (Weekday)
            {
                case 6:
                    {
                        return 1;

                    }
                case 0:
                    {
                        return 2;

                    }
                case 1:
                    {
                        return 3;

                    }
                case 2:
                    {
                        return 4;

                    }
                case 3:
                    {
                        return 5;

                    }
                case 4:
                    {
                        return 6;

                    }
                case 5:
                    {
                        return 7;

                    }
                default:
                    {
                        return 1;

                    }
            }



        }

        public string WeekDayNumberShamsiToNameShamsi(byte Weekday)
        {

            switch (Weekday)
            {
                case 7:
                    {
                        return "جمعه";


                    }
                case 1:
                    {
                        return "شنبه";

                    }
                case 2:
                    {
                        return "یکشنبه";


                    }
                case 3:
                    {
                        return "دوشنبه";


                    }
                case 4:
                    {
                        return "سه شنبه";


                    }
                case 5:
                    {
                        return "چهارشنبه";


                    }
                case 6:
                    {
                        return "پنج شنبه";


                    }
                default:
                    {
                        return null;

                    }
            }



        }
    }
}