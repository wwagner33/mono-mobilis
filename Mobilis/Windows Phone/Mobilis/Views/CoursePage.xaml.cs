using System.Windows;
using Microsoft.Phone.Controls;
using System.Windows.Controls;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib;
using System;
using Mobilis.Lib.Messages;
using Coding4Fun.Phone.Controls;

namespace Mobilis.Views
{
    public partial class CoursePage : PhoneApplicationPage
    {
        private CoursesViewModel courseViewModel;
        private PerformanceProgressBar progressBar;

        public CoursePage()
        {
            InitializeComponent();
            Loaded+=MainPage_Loaded;
            Unloaded += CoursePage_Unloaded;
        }

        void CoursePage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ContentPanel.Children.Contains(progressBar))
                this.ContentPanel.Children.Remove(progressBar);
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

            progressBar = new PerformanceProgressBar();
            progressBar.IsIndeterminate = true;
            ServiceLocator.Dispatcher = new DispatchAdapter();
            courseViewModel = new CoursesViewModel();
            _courses.ItemsSource = courseViewModel.listContent;
            _courses.SelectionChanged -= lstCourses_SelectionChanged;
            _courses.SelectionChanged += new SelectionChangedEventHandler(lstCourses_SelectionChanged);

            ServiceLocator.Messenger.Subscribe<BaseViewMessage>(m =>
            {
                switch (m.Content.message)
                {
                    case BaseViewMessage.MessageTypes.CONNECTION_ERROR:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            ToastPrompt toast = new ToastPrompt();
                            toast.Message = "Erro de conexão";
                            toast.Show();
                            if (ContentPanel.Children.Contains(progressBar))
                                this.ContentPanel.Children.Remove(progressBar);
                            _courses.SelectedIndex = -1;
                        });
                        break;
                    case BaseViewMessage.MessageTypes.CLASS_CONNECTION_OK:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            ServiceLocator.Dispatcher.invoke(() =>
                            {
                                NavigationService.Navigate(new Uri("/Views/ClassPage.xaml", UriKind.Relative));
                            });
                        });
                        break;
                    case BaseViewMessage.MessageTypes.COURSE_CONNECTION_OK:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            //TODO refresh.
                        });
                        break;
                    default:
                        break;
                }
            });
        }

        public void lstCourses_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {
            if (_courses.SelectedIndex != -1)
            {
                if (courseViewModel.existClasses(_courses.SelectedIndex))
                {
                    this.ContentPanel.Children.Remove(progressBar);
                    NavigationService.Navigate(new Uri("/Views/ClassPage.xaml", UriKind.Relative));
                }

                else
                {
                    this.ContentPanel.Children.Add(progressBar);
                    courseViewModel.requestClass();
                }
            }
        }
    }
}