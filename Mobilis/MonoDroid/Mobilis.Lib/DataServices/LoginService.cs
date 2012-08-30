using System.Collections.Generic;
#if MONODROID || MONOTOUCH
using Mobilis.Lib.Util;
#endif
#if WINDOWS_PHONE
using Mobilis.Lib.WP7Util;
#endif


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