using Mobilis.Lib.DataServices;
using System.Collections.Generic;
using Mobilis.Lib.Model;
using Mobilis.Lib.Util;
using Mobilis.Lib.Database;
using System;

namespace Mobilis.Lib.ViewModel
{
    public class LoginViewModel
    {
        private LoginService loginService;
        private UserDao userDao;
        private CourseDao courseDao;
        private CourseService courseService;

        public LoginViewModel() 
        {
            loginService = new LoginService();
            courseService = new CourseService();
            userDao = new UserDao();
            courseDao = new CourseDao();
        }

        public void submitLoginData(string name, string password,NotifyView callback) 
        {
             loginService.getToken(Constants.tokenURL,name, password, r => 
             {
                    var enumerator = r.Value.GetEnumerator();
                    enumerator.MoveNext();
                    string token = enumerator.Current;
                    try
                    {
                        // atualiza o token do usuário
                        User user = userDao.getUser();
                        user.token = token;
                        userDao.addUser(user);
                    }
                    catch (Exception e)
                    {
                        // cria usuário novo
                        User user = new User();
                        user.token = token;
                        user._id = 1;
                        user.autoLogin = true;
                        userDao.addUser(user);
                    }
                    finally 
                    {
                        callback();
                    }
             });
        }

        public void requestCourses(NotifyView callback)
        {
            courseService.getCourses(Constants.CoursesURL, userDao.getToken(), r =>
            {
                courseDao.insertAll(r.Value);
                callback();
            });
        } 
    }
}