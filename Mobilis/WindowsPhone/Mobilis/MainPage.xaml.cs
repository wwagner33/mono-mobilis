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

namespace Mobilis
{
    public partial class MainPage : PhoneApplicationPage
    {
        private CourseService courseService;
        private LoginService loginService;

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            courseService = new CourseService();
            loginService = new LoginService();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("login = "+ login.Text);
            System.Diagnostics.Debug.WriteLine("password = " + password.Password);
            loginService.getToken(login.Text, password.Password, r => {
                var enumerator = r.Value.GetEnumerator();
                enumerator.MoveNext();
                string token = enumerator.Current;
                System.Diagnostics.Debug.WriteLine("Token = " + enumerator.Current);                
            });
        }
    }
}