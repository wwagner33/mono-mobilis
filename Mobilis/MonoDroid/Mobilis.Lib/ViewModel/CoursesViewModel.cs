using Mobilis.Lib.Model;
using System.Collections.Generic;
using Mobilis.Lib.Database;
using Mobilis.Lib.DataServices;
using Mobilis.Lib.Util;

namespace Mobilis.Lib.ViewModel
{
    public class CoursesViewModel
    {
        public List<Course> listContent {get; private set;}
        private CourseDao courseDao;
        private UserDao userDao;
        private ClassService classService;
        private ClassDao classDao;

        public CoursesViewModel() 
        {
            courseDao = new CourseDao();
            userDao = new UserDao();
            classService = new ClassService();
            classDao = new ClassDao();
            listContent = courseDao.getAllCourses();
        }

        public  void logout()
        {
            User user = userDao.getUser();
            user.token = null;
            userDao.addUser(user);
        }

        public bool existClasses(int coursePosition) 
        {
            Course selectedCourse = listContent[coursePosition];
            ContextUtil.Instance.Course = selectedCourse._id;
            return classDao.existClassAtCourse(selectedCourse._id);
        }

        public void requestClass(NotifyView callback) 
        {
            classService.getClasses(Constants.ClassesURL, userDao.getToken(), r =>
             {
               classDao.insertClasses(r.Value);
               callback();
            });
        }
    }
}
