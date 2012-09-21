using System.Collections.Generic;
using Mobilis.Lib.Model;

namespace Mobilis.Lib.Database
{
    public class PostDao
    {
        public void insertPost(IEnumerable<Post> posts) 
        {
            MobilisDatabase.getDatabase().InsertAll(posts);
        }

        public List<Post> getPostsFromDiscussion(int discussionId) 
        {
            return MobilisDatabase.getDatabase().Query<Post>("select * from Post where discussionId = ?", discussionId);
        }
    }
}