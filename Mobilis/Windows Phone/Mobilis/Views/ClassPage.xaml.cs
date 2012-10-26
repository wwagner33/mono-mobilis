using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib;
using Mobilis.Lib.Messages;
using Coding4Fun.Phone.Controls;

namespace Mobilis.Views
{
    public partial class ClassPage : PhoneApplicationPage
    {
        private ClassViewModel classViewModel;
        private PerformanceProgressBar progressBar;

        public ClassPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(ClassPage_Loaded);
            Unloaded += ClassPage_Unloaded;
        }

        void ClassPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ContentPanel.Children.Contains(progressBar))
                this.ContentPanel.Children.Remove(progressBar);
        }

        void ClassPage_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar = new PerformanceProgressBar();
            progressBar.IsIndeterminate = true;
            classViewModel = new ClassViewModel();
            ServiceLocator.Dispatcher = new DispatchAdapter();
            _classes.ItemsSource = classViewModel.classes;
            _classes.SelectionChanged -= _classes_SelectionChanged;
            _classes.SelectionChanged += new SelectionChangedEventHandler(_classes_SelectionChanged);

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
                            _classes.SelectedIndex = -1;
                        });
                        break;

                    case BaseViewMessage.MessageTypes.DISCUSSION_CONNECTION_OK:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            NavigationService.Navigate(new Uri("/Views/DiscussionPage.xaml", UriKind.Relative));
                        });
                        break;
                    default:
                        break;
                }
            });
        }

        void _classes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_classes.SelectedIndex != -1)
            {
                if (classViewModel.existDiscussionsAtClass(_classes.SelectedIndex))
                {
                    NavigationService.Navigate(new Uri("/Views/DiscussionPage.xaml", UriKind.Relative));
                }

                else 
                {
                    this.ContentPanel.Children.Add(progressBar);
                    classViewModel.requestDiscussions();           
                }
            }
        }
    }
}