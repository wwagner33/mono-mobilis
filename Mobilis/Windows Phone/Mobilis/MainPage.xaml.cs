using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Mobilis.Lib;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib.Messages;
using Coding4Fun.Phone.Controls;

namespace Mobilis
{
    public partial class MainPage : PhoneApplicationPage
    {
        private LoginViewModel loginViewModel;
        private PerformanceProgressBar progressBar;

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
            Unloaded += MainPage_Unloaded;
        }

        void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ContentPanel.Children.Contains(progressBar))
                this.ContentPanel.Children.Remove(progressBar);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Dispatcher = new DispatchAdapter();
            loginViewModel = new LoginViewModel();
            progressBar = new PerformanceProgressBar();
            progressBar.IsIndeterminate = true;
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
                        });
                        break;
                    case BaseViewMessage.MessageTypes.LOGIN_CONNECTION_OK:
                        getCourses();
                        break;
                    case BaseViewMessage.MessageTypes.COURSE_CONNECTION_OK:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            NavigationService.Navigate(new Uri("/Views/CoursePage.xaml", UriKind.Relative));
                        });
                        break;
                    default:
                        break;
                }
            });
        }


        public void getCourses() 
        {
            loginViewModel.requestCourses();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            loginViewModel.submitLoginData(login.Text, password.Password);
            this.ContentPanel.Children.Add(progressBar);
        }
    }
}