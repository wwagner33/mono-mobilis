using System.Collections.Generic;
using Mobilis.Lib.Model;
using Mobilis.Lib.Util;

namespace Mobilis.Lib.Database
{
    public class PostDao
    {
        public void insertPost(IEnumerable<Post> posts) 
        {
            List<Post> teste = new List<Post>(posts);
            System.Diagnostics.Debug.WriteLine("POSTS SIZE " + teste.Count);
            System.Diagnostics.Debug.WriteLine("antes do delete");
            MobilisDatabase.getDatabase().CreateCommand("delete from Post where discussionId = ?", ContextUtil.Instance.Discussion).ExecuteNonQuery();
            System.Diagnostics.Debug.WriteLine("depoid do delete");
            MobilisDatabase.getDatabase().InsertAll(teste);
            System.Diagnostics.Debug.WriteLine("depoid do insert");
        }

        public List<Post> getPostsFromDiscussion(int discussionId) 
        {
            return MobilisDatabase.getDatabase().Query<Post>("select * from Post where discussionId = ?", discussionId);
        }

        public bool existPostsAtDiscussion(int discussionId)
        {
            List<Post> list = MobilisDatabase.getDatabase().Query<Post>("select * from Post where discussionId= ? limit 1", discussionId);
            return (list.Count > 0) ? true : false;                    
        }
    }
}