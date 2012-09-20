using Mobilis.Lib.Model;
using System.Collections.Generic;

namespace Mobilis.Lib.Database
{
    public class UserDao
    {
        public void addUser(User user) 
        {
            int number = MobilisDatabase.getDatabase().Table<User>().Count();
            if (number == 0)
            {
                MobilisDatabase.getDatabase().Insert(user);
            }
            else 
            { 
                // TODO UPDATE USER
            }
        }

        public string getToken() 
        {
            List<User> list = MobilisDatabase.getDatabase().Query<User>("select * from User where _id = 1");
            return list[0].token;
        }
    }
}