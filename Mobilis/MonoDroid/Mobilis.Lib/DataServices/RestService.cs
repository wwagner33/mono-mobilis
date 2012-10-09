using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using Mobilis.Lib.Util;
using System.Collections.Specialized;
using System.Web;

namespace Mobilis.Lib.DataServices
{
    /* Classe abstrata que serve como base para acesso a rede.
       Serialização deve ser implementada na subclasse*/

    public abstract class RestService<T>
    {
        //public string _baseUrl = "http://apolo11teste.virtual.ufc.br/solar/";
        //public abstract IEnumerable<T> parseJSON(string content,int method);
        public abstract IEnumerable<T> parseJSON(WebResponse content, int method);
        public static int METHOD_GET = 1;
        public static int METHOD_POST = 2;
        public static string CONTENT_TYPE_AUDIO = "audio/3gpp";
        public static string CONTENT_TYPE_JSON = "application/json";

        protected void Get(string source,string token,ResultCallback<IEnumerable<T>> callback) 
        {
            string tokenUrl = (token == null || token.Equals(string.Empty)) ? string.Empty : ("?auth_token=" + token);
            //string tokenUrl = "?auth_token=" + token;
            var webRequest = (HttpWebRequest)WebRequest.Create(source + tokenUrl);
            System.Diagnostics.Debug.WriteLine("URL = " + source + tokenUrl);

            webRequest.BeginGetResponse(responseResult =>
                {
                    try
                    {
                        var response = webRequest.EndGetResponse(responseResult);
                        if (response != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Response not null");
                            var result = parseJSON(response, METHOD_GET);
                            response.Close();
                            callback(new Result<IEnumerable<T>>(result));
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                        System.Diagnostics.Debug.WriteLine("Response IS null");
                        callback(new Result<IEnumerable<T>>(ex));
                    }

                },webRequest);
        }

        protected void Post(string source,string token, string content,string contentType, ResultCallback<IEnumerable<T>> callback)
        {
            ThreadPool.QueueUserWorkItem(state => 
            {
                Post(source,token,HttpUtils.toByteArray(content),contentType,callback);
            });
        }

        public void Post(string source,string token, byte[] content,string contentType, ResultCallback<IEnumerable<T>> callback)
        {
            string tokenUrl = (token == null || token.Equals(string.Empty)) ? string.Empty : ("?auth_token=" + token);
            var webRequest = (HttpWebRequest)WebRequest.Create(source + tokenUrl);
            System.Diagnostics.Debug.WriteLine("URL = " + source);
            System.Diagnostics.Debug.WriteLine("Temanho do array de bytes = " + content.Length);
            webRequest.ContentType = contentType;
            webRequest.ContentLength = content.Length;
            webRequest.Method = "POST";
            webRequest.BeginGetRequestStream(responseResult =>
            {
                try
                {
                    // Envia dados ao servidor
                    Stream streamResponse = webRequest.EndGetRequestStream(responseResult);
                    byte[] byteArray = content;
                    streamResponse.Write(byteArray, 0, content.Length);
                    streamResponse.Close();

                    webRequest.BeginGetResponse(result =>
                    {
                        var response = (HttpWebResponse)webRequest.EndGetResponse(result);
                        System.Diagnostics.Debug.WriteLine("StatusCode = " + response.StatusCode.ToString());

                        if (response != null)
                        {
                            var postResult = parseJSON(response, METHOD_POST);
                            response.Close();
                            callback(new Result<IEnumerable<T>>(postResult));
                        }
                    }, webRequest);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("PostException");
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    callback(new Result<IEnumerable<T>>(ex));
                }
            }, webRequest);
        }

        public void getAudio(string teste,int id,ResultCallback<IEnumerable<T>> callback) 
        {

            WebRequest webRequest = WebRequest.Create(teste);
            HttpWebResponse response = null;
            System.Diagnostics.Debug.WriteLine("BING URL =" + teste);

                webRequest.BeginGetResponse(responseResult =>
                {
                    try
                    {
                        response = (HttpWebResponse)webRequest.EndGetResponse(responseResult);
                        if (response != null) 
                        {
                            System.Diagnostics.Debug.WriteLine("Bing status code = " + response.StatusCode);
                            HttpUtils.SaveFileToStorage(response,id);
                            // TODO REMOVER ESSE CALLBACK
                            var result = parseJSON(response, METHOD_GET);
                            callback(new Result<IEnumerable<T>>(result));
                        }
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("Exception no Bing " + e.StackTrace);
                        throw;
                    }
                    finally 
                    {
                        if (response != null) 
                        {
                            response.Close();
                            response = null;
                        }
                    }

                },webRequest);
        }
        
        /*
        private IEnumerable<T> ParseResult(WebResponse response, int method)
        {
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
            string result = responseReader.ReadToEnd();
            System.Diagnostics.Debug.WriteLine(result);
            return parseJSON(result,method);
        }
         * */
    }
}