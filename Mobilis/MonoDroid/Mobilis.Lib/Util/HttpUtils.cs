using System.Text.RegularExpressions;
using System;

namespace Mobilis.Lib.Util
{
    public class HttpUtils
    {
        static string[] months = {"janeiro","fevereiro","março","abril","maio","junho","julho","agosto","setembro","outubro","novembro","dezembro"};

        public static string Strip(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty).Replace("//s"," ").Trim();
        }

        public static string postDateToServerFormat(string date) 
        {
            return date.Substring(0, 19).Replace("T", string.Empty).Replace("-",string.Empty).Replace(":",string.Empty);
        }

        public static string discussionDateToShowFormat(string date) 
        {
            return date.Substring(0, 10).Replace("-", "/");
        }

        public static string postDateToShowFormat(string date) 
        {
            string header = "";
            int year = Convert.ToInt16(date.Substring(0, 4));
            int month = Convert.ToInt16(date.Substring(5, 2));
            int day = Convert.ToInt16(date.Substring(8, 2));
            int hour = Convert.ToInt16(date.Substring(11, 2));
            int minute = Convert.ToInt16(date.Substring(14, 2));
            if (DateTime.Today.Year == year)
            {
                if (DateTime.Today.Month == month)
                {
                    if (DateTime.Today.Day == day)
                    {
                        if (DateTime.Today.Hour == hour)
                        {
                            header = "Há "
                                + (DateTime.Today.Minute-minute)
                                + " minutos";
                        }
                        else 
                        {
                            header = "Às " + hour + "horas";
                        }
                    }
                    else 
                    {
                        if (DateTime.Today.Day - 1 == day)
                        {
                            header = "Ontem";
                        }
                        else 
                        {
                            header = "Dia " + day + " às " + hour
                                + " horas";
                        }
                    }
                }
                else 
                {
                    header = "Dia "
                        + day
                        + " de "
                        + months[month - 1];
                }
            }
            else 
            {
                header = "Dia "
                            + day
                            + " de "
                            + months[month - 1] + " de " + year;
            }
            return header;
        }

        public static byte[] toByteArray(string s) 
        {
            return System.Text.Encoding.UTF8.GetBytes(s);       
        }
    }
}