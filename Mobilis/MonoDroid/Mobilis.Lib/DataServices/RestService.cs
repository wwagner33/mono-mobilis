using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using Mobilis.Lib.Util;
using System.Collections.Specialized;

namespace Mobilis.Lib.DataServices
{
    /* Classe abstrata que serve como base para acesso a rede.
       Serialização deve ser implementada na subclasse*/

    public abstract class RestService<T>
    {
        private string _baseUrl = "http://apolo11teste.virtual.ufc.br/solar/";
        public abstract IEnumerable<T> parseJSON(string content,int method);
        public static int METHOD_GET = 1;
        public static int METHOD_POST = 2;
        public static string CONTENT_TYPE_AUDIO = "audio/3gpp";
        public static string CONTENT_TYPE_JSON = "application/json";

        protected void Get(string source,string token,ResultCallback<IEnumerable<T>> callback) 
        {
            string tokenUrl = "?auth_token=" + token;
            var webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + source + tokenUrl);
            System.Diagnostics.Debug.WriteLine("URL = " + _baseUrl + source + tokenUrl);

            webRequest.BeginGetResponse(responseResult =>
                {
                    try
                    {
                        var response = webRequest.EndGetResponse(responseResult);
                        if (response != null)
                        {
                            var result = ParseResult(response,METHOD_GET);
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

        protected void Post(string source,string token, string content,string contentType, ResultCallback<IEnumerable<T>> callback)
        {
            ThreadPool.QueueUserWorkItem(state => 
            {
                Post(source,token,HttpUtils.toByteArray(content),contentType,callback);
            });
        }

        public void Post(string source,string token, byte[] content,string contentType, ResultCallback<IEnumerable<T>> callback)
        {
            string tokenUrl = (token == null) ? string.Empty : ("?auth_token=" + token);
            var webRequest = (HttpWebRequest)WebRequest.Create(_baseUrl + source + tokenUrl);
            System.Diagnostics.Debug.WriteLine("URL = " + _baseUrl + source);
            // webRequest.ContentType = "application/json";
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
                            var postResult = ParseResult(response,METHOD_POST);
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

        public void PostFile(string url,string token, string filePath, string paramName, string contentType, NameValueCollection nvc)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Uploading {0} to {1}", filePath, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");


            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create((_baseUrl + url + "?auth_token=" + token));
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, filePath, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                System.Diagnostics.Debug.WriteLine(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error uploading file", ex);
                System.Diagnostics.Debug.WriteLine("Error uploading file", ex.Message);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
        }

        private IEnumerable<T> ParseResult(WebResponse response, int method)
        {
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
            string result = responseReader.ReadToEnd();
            System.Diagnostics.Debug.WriteLine(result);
            return parseJSON(result,method);
        }
    }
}