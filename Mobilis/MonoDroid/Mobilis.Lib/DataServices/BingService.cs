using System.Collections;
using System;
using System.Collections.Generic;
using System.Web;
using Mobilis.Lib.Util;
using Mobilis.Lib.Model;
namespace Mobilis.Lib.DataServices
{
    public class BingService : RestService<int>
    {
        const string BingURL = "http://api.microsofttranslator.com/v2/Http.svc/Speak?appId=03CAF44417913E4B9D82BE6202DBFBD768B8C5E1&text=";
        const string BingLanguage = "&language=pt";

        public void GetAsAudio(string text,ResultCallback<IEnumerable<int>> callback) 
        {
            string sentence = HttpUtility.HtmlEncode(text);
            string url = BingURL + sentence + BingLanguage;
            Get(url, string.Empty, callback);
        }

        public void GetAsAudio2(string text,int blockId, ResultCallback<IEnumerable<int>> callback) 
        {
            string sentence = HttpUtility.HtmlEncode(text);
            string url = BingURL + sentence + BingLanguage;
            getAudio(url,blockId,callback);
        }

        public override System.Collections.Generic.IEnumerable<int> parseJSON(System.Net.WebResponse content, int method)
        {
            // REMOVER CALLBACK
            //HttpUtils.SaveFileToStorage(content);
            System.Diagnostics.Debug.WriteLine("BING CALLBACK");
            yield return 0;
        }
    }
}