using SQLite;

namespace Mobilis.Lib.Model
{
    public class Course
    {
        [PrimaryKey]
        public int _id { get; set; }

        public int offerId { get; set; }
        public int groupId { get; set; }
        public int allocationTagId { get; set; }
        public string semester { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}