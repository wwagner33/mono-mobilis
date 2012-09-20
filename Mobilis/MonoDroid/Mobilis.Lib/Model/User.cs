
using MWC.DL.SQLite;
namespace Mobilis.Lib.Model
{
    public class User
    {
        [PrimaryKey]
        public int _id { get; set; }
        public string token { get; set; }
    }
}