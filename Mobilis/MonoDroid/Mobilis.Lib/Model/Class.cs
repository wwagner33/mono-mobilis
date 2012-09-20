using MWC.DL.SQLite;

namespace Mobilis.Lib.Model
{
    public class Class
    {
        [PrimaryKey]
        public int _id { get; set;}
        public int courseId { get; set;}
        public string code { get; set;}
        public bool status { get; set; }

        public override string ToString()
        {
            return code;
        }
    }
}