using System.Json;
namespace Mobilis.Lib.Util
{
    public class JSON
    {
        public JSON() { }

        public static string generateLoginObject(string login,string password) 
        {
            // JSON.NET
            /*
            JObject innerObject = new JObject();
            innerObject.Add("login", login);
            innerObject.Add("password", password);
            JObject outerObject = new JObject();
            outerObject.Add("user", innerObject);
            */
                
            // System.Json

            
            
            JsonObject innerObject = new JsonObject();
            innerObject.Add("login", login);
            innerObject.Add("password", password);
            JsonObject outerObject = new JsonObject();
            outerObject.Add("user", innerObject);
            return outerObject.ToString();
        }

        public static string parseToken(string content)
        {
            return null;
        }
    }
}