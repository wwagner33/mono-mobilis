using MWC.DL.SQLite;

namespace Mobilis.Lib.Model
{
    public class Class
    {
        [PrimaryKey]
        private int _id { get; set;}
        private int courseId { get; set;}
        private string code { get; set;}
        private string semester { get; set;}

        public override string ToString()
        {
            return code;
        }
    }
}