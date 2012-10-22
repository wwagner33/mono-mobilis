using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib;
using System;

namespace Mobilis.Views
{
    public partial class CoursePage : PhoneApplicationPage
    {
        private CoursesViewModel courseViewModel;

        public CoursePage()
        {
            InitializeComponent();
            Loaded+=MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                if (this.NavigationService.RemoveBackEntry() == null)
                {
                    break;
                }
            }
            ServiceLocator.Dispatcher = new DispatchAdapter();
            courseViewModel = new CoursesViewModel();
            _courses.ItemsSource = courseViewModel.listContent;
            _courses.SelectionChanged -= lstCourses_SelectionChanged;
            _courses.SelectionChanged += new SelectionChangedEventHandler(lstCourses_SelectionChanged);
        }

        public void lstCourses_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if (_courses.SelectedIndex != -1)
            {
                if (courseViewModel.existClasses(_courses.SelectedIndex))
                {
                    NavigationService.Navigate(new Uri("/Views/ClassPage.xaml", UriKind.Relative));
                }

                else
                {
                    courseViewModel.requestClass(() =>
                    {
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            NavigationService.Navigate(new Uri("/Views/ClassPage.xaml", UriKind.Relative));
                        });
                    });
                }
            }
        }
    }
}