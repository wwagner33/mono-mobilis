using System.Collections.Generic;
using System.Collections.Specialized;
namespace Mobilis.Lib.DataServices
{
    public class SendAudioService : RestService<int>
    {
        public void SendAudio(string source,string token, byte[] content,ResultCallback<IEnumerable<int>> callback) 
        {
            Post(source, token, content,CONTENT_TYPE_AUDIO, callback);   
        }

        public void SendAudio2(string url,string token,string filePath,string paramName,string contentType,NameValueCollection nvc) 
        {
            PostFile(url, token, filePath, paramName, contentType, nvc);        
        }

        public override System.Collections.Generic.IEnumerable<int> parseJSON(string content, int method)
        {
           yield return 0;
        }
    }
}