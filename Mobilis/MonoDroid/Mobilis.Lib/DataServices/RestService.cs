using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Mobilis.Lib.DataServices
{
    public abstract class RestService<T> where T : class
    {
        private string _baseUrl = "http://apolo11teste.virtual.ufc.br/solar/";
        public abstract IEnumerable<T> parseJSON(string content);

        protected void Get(string source,string token,ResultCallback<IEnumerable<T>> callback) 
        {
            string tokenUrl = "?auth_token=" + token;
            var webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + source + tokenUrl);

            webRequest.BeginGetResponse(responseResult =>
                {
                    try
                    {
                        var response = webRequest.EndGetResponse(responseResult);
                        if (response != null)
                        {
                            var result = ParseResult(response);
                            response.Close();
                            callback(new Result<IEnumerable<T>>(result));
                        }
                    }
                    catch (Exception ex)
                    {
                        callback(new Result<IEnumerable<T>>(ex));
                    }

                },webRequest);
        }

        protected void Post(string source, string content, ResultCallback<IEnumerable<T>> callback)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + source);

            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";

            webRequest.BeginGetRequestStream(responseResult => 
            {
                try
                {
                    // Push data to server
                    Stream streamResponse = webRequest.EndGetRequestStream(responseResult);
                    byte[] byteArray = Encoding.UTF8.GetBytes(content);
                    streamResponse.Write(byteArray, 0, content.Length);
                    streamResponse.Close();

                    webRequest.BeginGetResponse(result => {
                        var response = (HttpWebResponse)webRequest.EndGetResponse(result);
                        System.Diagnostics.Debug.WriteLine("StatusCode = " + response.StatusCode.ToString());

                        if (response != null)
                        {
                            var postResult = ParseResult(response);
                            response.Close();
                            callback(new Result<IEnumerable<T>>(postResult));
                        }
                    }, webRequest);

                    
                    /*
                    var response = (HttpWebResponse)webRequest.GetResponse();
                    System.Diagnostics.Debug.WriteLine("StatusCode = " + response.StatusCode.ToString());

                    if (response != null)
                    {
                        var result = ParseResult(response);
                        response.Close();
                        callback(new Result<IEnumerable<T>>(result));
                    }
                     */
                }
                catch (Exception ex)
                {
                    callback(new Result<IEnumerable<T>>(ex));
                }
            }, webRequest);
        }

        private IEnumerable<T> ParseResult(WebResponse response)
        {
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
            string result = responseReader.ReadToEnd();
            System.Diagnostics.Debug.WriteLine("Result from server = " + result);
            return parseJSON(result);
        }
    }
}