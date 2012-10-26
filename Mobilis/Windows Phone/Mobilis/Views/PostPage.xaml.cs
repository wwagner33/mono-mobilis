using Microsoft.Phone.Controls;
using Mobilis.Lib.ViewModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Mobilis.Lib;
using Mobilis.Lib.Messages;
using Coding4Fun.Phone.Controls;

namespace Mobilis.Views
{
    public partial class PostPage : PhoneApplicationPage
    {
        private PostsViewModel postsViewModel;
        private Button loadMore;
        private PerformanceProgressBar progressBar;

        public PostPage()
        {
            InitializeComponent();
            Loaded += new System.Windows.RoutedEventHandler(PostPage_Loaded);
        }

        void PostPage_Loaded(object sender, System.Windows.RoutedEventArgs re)
        {
            postsViewModel = new PostsViewModel(null);
            _posts.ItemsSource = postsViewModel.posts;
            loadMore = FindChild<Button>(_posts, "_load_more");
            loadMore.Click += new RoutedEventHandler((o, e) => 
            {
                postsViewModel.loadFuturePosts();
            });

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
                            _posts.SelectedIndex = -1;
                        });
                        break;

                    case BaseViewMessage.MessageTypes.FUTURE_POSTS_LOADED:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            _posts.ItemsSource = postsViewModel.posts;
                        });
                        break;
                    default:
                        break;
                }
            });
        }

        public static T FindChild<T>(DependencyObject parent, string childName)
        where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                var childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                    foundChild = FindChild<T>(child, childName);
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }
    }
}