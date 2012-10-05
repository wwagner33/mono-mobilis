using Mobilis.Lib.Model;
using System.Collections.Generic;
using Mobilis.Lib.Util;
using System;

namespace Mobilis.Lib.DataServices
{
    public class DiscussionService : RestService<Discussion>
    {
        public void getDiscussions(string source,string token,ResultCallback<IEnumerable<Discussion>> callback) 
        {
            Get(source, token, callback);        
        }

        public override System.Collections.Generic.IEnumerable<Discussion> parseJSON(string content,int method)
        {
            return JSON.parseDiscussion(content);
        }
    }
}