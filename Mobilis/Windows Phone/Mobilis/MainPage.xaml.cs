using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Mobilis.Lib;
using Mobilis.Lib.ViewModel;

namespace Mobilis
{
    public partial class MainPage : PhoneApplicationPage
    {
        //private CourseService courseService;
       // private LoginService loginService;
        //rivate CourseDao courseDao;
        private LoginViewModel loginViewModel;

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Dispatcher = new DispatchAdapter();
            loginViewModel = new LoginViewModel();
        }

        public void getCourses() 
        {
            loginViewModel.requestCourses(() => 
            {
                ServiceLocator.Dispatcher.invoke(() =>
                {
                    NavigationService.Navigate(new Uri("/Views/CoursePage.xaml", UriKind.Relative));
                });
            });
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            loginViewModel.submitLoginData(login.Text, password.Password, () => 
            {
                getCourses();
            });
        }
    }
}