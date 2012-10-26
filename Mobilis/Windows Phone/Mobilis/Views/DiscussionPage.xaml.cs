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
    public partial class DiscussionPage : PhoneApplicationPage
    {
        private DiscussionsViewModel discussionViewModel;
        private PerformanceProgressBar progressBar;

        public DiscussionPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(DiscussionPage_Loaded);
            Unloaded += DiscussionPage_Unloaded;
        }

        void DiscussionPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ContentPanel.Children.Contains(progressBar))
                this.ContentPanel.Children.Remove(progressBar);
        }

        void DiscussionPage_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar = new PerformanceProgressBar();
            progressBar.IsIndeterminate = true;
            ServiceLocator.Dispatcher = new DispatchAdapter();
            discussionViewModel = new DiscussionsViewModel();
            _discussions.ItemsSource = discussionViewModel.discussions;
            _discussions.SelectionChanged -= _discussions_SelectionChanged;
            _discussions.SelectionChanged += new SelectionChangedEventHandler(_discussions_SelectionChanged);

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
                            _discussions.SelectedIndex = -1;
                        });
                        break;

                    case BaseViewMessage.MessageTypes.FUTURE_POSTS_LOADED:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            NavigationService.Navigate(new Uri("/Views/PostPage.xaml", UriKind.Relative));
                        });
                        break;
                    case BaseViewMessage.MessageTypes.NO_NEW_POSTS:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            ToastPrompt noNewMessages = new ToastPrompt();
                            noNewMessages.Message = "Não há novos posts";
                            noNewMessages.Show();
                        });
                        break;
                    default:
                        break;
                }
            });
        }

        void _discussions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_discussions.SelectedIndex != -1) 
            {
                if (discussionViewModel.existPostsAtDiscussion(_discussions.SelectedIndex))
                {
                    NavigationService.Navigate(new Uri("/Views/PostPage.xaml", UriKind.Relative));
                }
                else 
                {
                    this.ContentPanel.Children.Add(progressBar);
                    discussionViewModel.requestPosts();
                }
            }
        }
    }
}