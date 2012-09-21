using MWC.DL.SQLite;

namespace Mobilis.Lib.Model
{
   public class Post
    {
        [PrimaryKey]
        public int _id {get;set;}
        public int parentId { get; set;}
        public int userId { get; set;}
        public int discussionId { get; set; }
        public string content { get; set;}
        public int level { get; set; }
        public string userName { get; set; }
        public string updatedAt { get; set; }
    }
}