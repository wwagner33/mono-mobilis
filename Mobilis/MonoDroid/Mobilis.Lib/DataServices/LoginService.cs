using System.Collections.Generic;
using Mobilis.Lib.Util;

namespace Mobilis.Lib.DataServices
{
    public class LoginService : RestService<string>
    {
        public void getToken(string name, string password,ResultCallback<IEnumerable<string>> callback)
        {
            string content = JSON.generateLoginObject(name, password);
            Post("sessions.json", content, callback);
        }

        public override IEnumerable<string> parseJSON(string content)
        {
            return JSON.parseToken(content);
        }
    }
}