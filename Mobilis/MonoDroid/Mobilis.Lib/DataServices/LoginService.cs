using System.Collections.Generic;
using Mobilis.Lib.Util;
using System;

namespace Mobilis.Lib.DataServices
{
    public class LoginService : RestService<string>
    {
        public void getToken(string name, string password,ResultCallback<IEnumerable<string>> callback)
        {
            string content = JSON.generateLoginObject(name, password);
            Post("sessions.json",null, content,CONTENT_TYPE_JSON, callback);
        }

        public override IEnumerable<string> parseJSON(string content,int method)
        {
            return JSON.parseToken(content);
        }
    }
}