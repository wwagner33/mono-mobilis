using Mobilis.Lib.Model;
using System.Collections.Generic;
using Mobilis.Lib.Util;
using System;

namespace Mobilis.Lib.DataServices
{
    public class ClassService : RestService<Class>
    {
        public void getClasses(string source,string token,ResultCallback<IEnumerable<Class>> callback) 
        {
            Get(source, token, callback);
        }

        public override IEnumerable<Class> parseJSON(System.Net.WebResponse content, int method)
        {
            return JSON.parseClasses(HttpUtils.WebResponseToString(content));
        }
    }
}