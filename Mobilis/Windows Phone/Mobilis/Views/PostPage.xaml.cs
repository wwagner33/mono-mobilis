using Microsoft.Phone.Controls;
using Mobilis.Lib.ViewModel;

namespace Mobilis.Views
{
    public partial class PostPage : PhoneApplicationPage
    {
        private PostsViewModel postsViewModel;

        public PostPage()
        {
            InitializeComponent();
            Loaded += new System.Windows.RoutedEventHandler(PostPage_Loaded);
        }

        void PostPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            postsViewModel = new PostsViewModel(null);
            _posts.ItemsSource = postsViewModel.posts;
        }
    }
}