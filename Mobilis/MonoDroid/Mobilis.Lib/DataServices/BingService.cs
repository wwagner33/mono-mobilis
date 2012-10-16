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
        string BingOptions = "&format=" + HttpUtility.UrlEncode("audio/wav") + "&options=MaxQuality";
        const string BingLanguage = "&language=pt";

        /*
        public void GetAsAudio(string text,ResultCallback<IEnumerable<int>> callback) 
        {
            string sentence = HttpUtility.HtmlEncode(text);
            string url = BingURL + sentence + BingLanguage;
            Get(url, string.Empty, callback);
        }
         * */

        public void GetAsAudio2(string text,int blockId, AudioCallback callback) 
        {
            string sentence = HttpUtility.HtmlEncode(text);
            string url = BingURL + sentence + BingLanguage + BingOptions;
            getAudio(url,blockId,callback);
        }

        public override System.Collections.Generic.IEnumerable<int> parseJSON(System.Net.WebResponse content, int method)
        {
            // REMOVER CALLBACK
            //HttpUtils.SaveFileToStorage(content);
            System.Diagnostics.Debug.WriteLine("BING CALLBACK");
            yield return 0;
        }

        /*
               Log.Info("teste", "TESTE");
               BingService bs = new BingService();
               bs.GetAsAudio2("teste",0, r => 
               {
                   System.Diagnostics.Debug.WriteLine("retorno");
                   var teste = r.Value.GetEnumerator();
                   teste.MoveNext();
                   Log.Info("teste", "BING RETORNO = " + teste.Current);
               });
         */
    }
}