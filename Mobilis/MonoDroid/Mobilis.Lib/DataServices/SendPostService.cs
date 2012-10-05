using System.Collections.Generic;
using Mobilis.Lib.Util;

namespace Mobilis.Lib.DataServices
{
    public class SendPostService : RestService<int>
    {
        public void sendPost(string source,string token,string content,ResultCallback<IEnumerable<int>> callback) 
        {
            Post(source, token, content,CONTENT_TYPE_JSON, callback);
        }

        public override System.Collections.Generic.IEnumerable<int> parseJSON(string content, int method)
        {
            System.Diagnostics.Debug.WriteLine("Post return = " + content);
            yield return JSON.parsePostDelivered(content);
        }
    }
}