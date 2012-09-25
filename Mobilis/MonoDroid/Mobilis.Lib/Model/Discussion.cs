using MWC.DL.SQLite;
using System;

namespace Mobilis.Lib.Model
{
    public class Discussion
    {
        [PrimaryKey]
	    public int _id {get;set;}
        public string name { get; set; }
        public int status { get; set; }
        public int classId { get; set; }
        public string description { get; set; }
        public int nextPosts { get; set; }
        public int previousPosts { get; set; }
        [Ignore]
        public bool hasNewPosts { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string lastPostDate { get; set; }

    public override string ToString()
        {
            return name;
        }
    }
}