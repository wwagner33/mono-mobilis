using Mobilis.Lib.Model;
using System.Collections.Generic;

namespace Mobilis.Lib.Database
{
    public class DiscussionDao
    {
        public List<Discussion> getDiscussionFromClass(int classId) 
        {
            return MobilisDatabase.getDatabase().Query<Discussion>("select * from Discussion where classId = ?", classId);       
        }

        public void insertDiscussion(IEnumerable<Discussion> discussions) 
        {
            MobilisDatabase.getDatabase().InsertAll(discussions);
        }
    }
}