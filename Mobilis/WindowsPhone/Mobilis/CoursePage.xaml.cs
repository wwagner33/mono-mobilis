using System.Windows;
using Microsoft.Phone.Controls;
using Mobilis.Lib.Database;
using System.Collections.Generic;
using Mobilis.Lib.Model;

namespace Mobilis
{
    public partial class CoursePage : PhoneApplicationPage
    {
        private CourseDao courseDao;

        public CoursePage()
        {
            InitializeComponent();
            Loaded+=MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            courseDao = new CourseDao();
            List<Course> courses = courseDao.getAllCourses();
            System.Diagnostics.Debug.WriteLine("Número de posts = " + courses.Count);
            _courses.ItemsSource = courses;
        }
    }
}