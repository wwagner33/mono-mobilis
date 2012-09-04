using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Mobilis.Lib;
using Mobilis.Lib.DataServices;
using Mobilis.Lib.WP7Util;
using Mobilis.Lib.Database;

namespace Mobilis
{
    public partial class MainPage : PhoneApplicationPage
    {
        private CourseService courseService;
        private LoginService loginService;
        private CourseDao courseDao;

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            courseService = new CourseService();
            loginService = new LoginService();
            ServiceLocator.Dispatcher = new DispatchAdapter();
            courseDao = new CourseDao();
        }

        public void getCourses(string token) 
        {
            courseService.getCourses("curriculum_units/list.json", token, r => {
                System.Diagnostics.Debug.WriteLine("Teste");
                courseDao.insertAll(r.Value);
                System.Diagnostics.Debug.WriteLine("Insert OK");
                ServiceLocator.Dispatcher.invoke(() => {
                    NavigationService.Navigate(new Uri("/CoursePage.xaml", UriKind.Relative));
                });
            });
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
               loginService.getToken(login.Text, password.Password, r => {
                var enumerator = r.Value.GetEnumerator();
                enumerator.MoveNext();
                string token = enumerator.Current;
                System.Diagnostics.Debug.WriteLine("Token = " + enumerator.Current);
                ServiceLocator.Dispatcher.invoke(() => {
                    getCourses(token);
                });
             });
        }
    }
}