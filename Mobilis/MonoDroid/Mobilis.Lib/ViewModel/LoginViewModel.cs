using Mobilis.Lib.DataServices;
using System.Collections.Generic;
using Mobilis.Lib.Model;
using Mobilis.Lib.Util;
using Mobilis.Lib.Database;
using System;
using Mobilis.Lib.Messages;

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

        public void submitLoginData(string name, string password) 
        {
                loginService.getToken(Constants.tokenURL, name, password, r =>
                {
                    if (r.hasError())
                    {
                        ServiceLocator.Messenger.Publish<BaseViewMessage>(new BaseViewMessage(this, new Message(BaseViewMessage.MessageTypes.CONNECTION_ERROR)));
                    }
                    else
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
                            ServiceLocator.Messenger.Publish<BaseViewMessage>(new BaseViewMessage(this, new Message(BaseViewMessage.MessageTypes.LOGIN_CONNECTION_OK)));
                        }
                    }
                });
            }

        public void requestCourses()
        {
            courseService.getCourses(Constants.CoursesURL, userDao.getToken(), r =>
            {
                courseDao.insertAll(r.Value);
                ServiceLocator.Messenger.Publish<BaseViewMessage>(new BaseViewMessage(this, new Message(BaseViewMessage.MessageTypes.COURSE_CONNECTION_OK)));
            });
        } 
    }
}