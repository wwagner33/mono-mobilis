using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
namespace Mobilis.Lib.WP7Util
{
    public class JSON
    {
        public static string generateLoginObject(string login, string password)
        {
            JObject innerObject = new JObject();
            innerObject.Add("login", login);
            innerObject.Add("password", password);
            JObject outerObject = new JObject();
            outerObject.Add("user", innerObject);
            return outerObject.ToString();
        }

        public static IEnumerable<string> parseToken(string content)
        {
            JObject teste = JObject.Parse(content);
            var innerObject = teste.SelectToken("session");
            string token = (string)innerObject.SelectToken("auth_token");
            yield return token;
        }
    }
}
