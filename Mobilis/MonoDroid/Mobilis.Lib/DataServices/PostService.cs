using Mobilis.Lib.Model;
using System.Collections.Generic;
using Mobilis.Lib.Util;
using System;

namespace Mobilis.Lib.DataServices
{
    public class PostService : RestService<Post>
    {
        private bool ignoreResult = false;

        public void getPosts(string source,string token,ResultCallback<IEnumerable<Post>> callback) 
        {
            Get(source, token, callback);
        }

        /*
        public void sendPost(string source, string token, string content, ResultCallback<IEnumerable<Post>> callback) 
        {
            Post(source, token, content, callback);
        }

        public void sendRecording(string source, string token, byte[] content, ResultCallback<IEnumerable<Post>> callback) 
        {
            Post(source, token, content, callback);
        }
        */
         
        public override System.Collections.Generic.IEnumerable<Post> parseJSON(string content,int method)
        {
            if (method == METHOD_GET)
            {
                System.Diagnostics.Debug.WriteLine("GET");
                System.Diagnostics.Debug.WriteLine("Post content" + content);
                return JSON.parsePosts(content);
            }
            if (method == METHOD_POST) 
            {
                System.Diagnostics.Debug.WriteLine("POST");
                System.Diagnostics.Debug.WriteLine("Resultado do post" + content);
                Post newPost = new Post();
                newPost._id = 1;
            }
            return null;
        }
    }
}