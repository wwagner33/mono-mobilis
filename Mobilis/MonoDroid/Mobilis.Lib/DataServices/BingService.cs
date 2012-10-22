//using System.Web;

using Mobilis.Lib.Util;
namespace Mobilis.Lib.DataServices
{
    public class BingService : RestService<int>
    {
        const string BingURL = "http://api.microsofttranslator.com/v2/Http.svc/Speak?appId=03CAF44417913E4B9D82BE6202DBFBD768B8C5E1&text=";
        string BingOptions = "&format=" + HttpUtils.UrlEncode("audio/wav") + "&options=MaxQuality";
        const string BingLanguage = "&language=pt";

        public void GetAsAudio2(string text,int blockId, AudioCallback callback) 
        {
            string sentence = HttpUtils.HtmlEncode(text);
            string url = BingURL + sentence + BingLanguage + BingOptions;
            getAudio(url,blockId,callback);
        }

        public override System.Collections.Generic.IEnumerable<int> parseJSON(System.Net.WebResponse content, int method)
        {
            // REMOVER CALLBACK
            System.Diagnostics.Debug.WriteLine("BING CALLBACK");
            yield return 0;
        }
    }
}