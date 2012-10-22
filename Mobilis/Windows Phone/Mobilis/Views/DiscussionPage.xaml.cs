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
using Mobilis.Lib.ViewModel;
using Mobilis.Lib;

namespace Mobilis.Views
{
    public partial class DiscussionPage : PhoneApplicationPage
    {
        private DiscussionsViewModel discussionViewModel;

        public DiscussionPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(DiscussionPage_Loaded);
        }

        void DiscussionPage_Loaded(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Dispatcher = new DispatchAdapter();
            discussionViewModel = new DiscussionsViewModel();
            _discussions.ItemsSource = discussionViewModel.discussions;
            _discussions.SelectionChanged -= _discussions_SelectionChanged;
            _discussions.SelectionChanged += new SelectionChangedEventHandler(_discussions_SelectionChanged);
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
                    discussionViewModel.requestPosts(() => 
                    {
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            NavigationService.Navigate(new Uri("/Views/PostPage.xaml", UriKind.Relative));
                        });  
                    });
                }
            }
        }
    }
}