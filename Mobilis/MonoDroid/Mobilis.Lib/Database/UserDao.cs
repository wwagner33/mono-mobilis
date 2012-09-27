using Mobilis.Lib.Model;
using System.Collections.Generic;
using System;

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
                MobilisDatabase.getDatabase().Update(user);
            }
        }

        public User getUser() 
        {
            try
            {
                List<User> list = MobilisDatabase.getDatabase().Query<User>("select * from User where _id = 1");
                return list[0];
            }
            catch (Exception e) 
            {
                return null;
            }
        }

        public string getToken() 
        {
            List<User> list = MobilisDatabase.getDatabase().Query<User>("select token from User where _id = 1");
            return list[0].token;
        }

        public bool tokenExists() 
        {
            List<User> list = MobilisDatabase.getDatabase().Query<User>("select token from user where _id = 1");
            return (list.Count > 0) ? true : false;
        }
    }
}