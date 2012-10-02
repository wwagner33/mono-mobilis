using System.Text.RegularExpressions;

namespace Mobilis.Lib.Util
{
    public class HttpUtils
    {
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
    }
}