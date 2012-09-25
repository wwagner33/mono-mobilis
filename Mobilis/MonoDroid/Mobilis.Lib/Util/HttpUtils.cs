using System.Text.RegularExpressions;

namespace Mobilis.Lib.Util
{
    public class HttpUtils
    {
        public static string Strip(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty).Replace("//s"," ").Trim();
        }
    }
}