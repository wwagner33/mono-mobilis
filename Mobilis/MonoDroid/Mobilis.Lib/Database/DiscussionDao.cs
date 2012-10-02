using Mobilis.Lib.Model;
using System.Collections.Generic;

namespace Mobilis.Lib.Database
{
    public class DiscussionDao
    {
        public List<Discussion> getDiscussionsFromClass(int classId) 
        {
            return MobilisDatabase.getDatabase().Query<Discussion>("select * from Discussion where classId = ?", classId);       
        }

        public void insertDiscussion(IEnumerable<Discussion> discussions) 
        {
            MobilisDatabase.getDatabase().InsertAll(discussions);
        }

        public Discussion getDiscussion(int discussionId) 
        {
            return MobilisDatabase.getDatabase().Query<Discussion>("select * from Discussion where _id = ?", discussionId)[0];
        }

        public void updateDiscussion(Discussion discussion) 
        {
            MobilisDatabase.getDatabase().Update(discussion);
        }

        public bool existDiscussionsAtClass(int classId) 
        {
            List<Discussion> list = MobilisDatabase.getDatabase().Query<Discussion>("select * from Discussion where classId = ? limit 1", classId);
            return (list.Count > 0) ? true : false;
        }
    }
}