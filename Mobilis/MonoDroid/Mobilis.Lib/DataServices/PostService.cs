using Mobilis.Lib.Model;
using System.Collections.Generic;
using Mobilis.Lib.Util;

namespace Mobilis.Lib.DataServices
{
    public class PostService : RestService<Post>
    {
        public void getPosts(string source,string token,ResultCallback<IEnumerable<Post>> callback) 
        {
            Get(source, token, callback);
        }

        public override System.Collections.Generic.IEnumerable<Post> parseJSON(string content)
        {
            System.Diagnostics.Debug.WriteLine("Post content" + content);
            return JSON.parsePosts(content);
        }
    }
}