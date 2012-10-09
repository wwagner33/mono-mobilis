using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
namespace Mobilis.Lib.DataServices
{
    public class SendAudioService : RestService<int>
    {
        public void SendAudio(string source,string token, byte[] content,ResultCallback<IEnumerable<int>> callback) 
        {
            Post(source, token, content,CONTENT_TYPE_AUDIO, callback);   
        }


        public override IEnumerable<int> parseJSON(WebResponse content, int method)
        {
            yield return 1;
        }
    }
}