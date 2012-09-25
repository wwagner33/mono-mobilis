
using Android.App;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using Mobilis.Lib.Util;
using Mobilis.Lib.DataServices;
using Android.Content;

namespace Mobilis
{
    [Activity(Label = "Discussion Activity",Theme = "@android:style/Theme.NoTitleBar")]
    public class DiscussionActivity : Activity
    {
        private SimpleListAdapter<Discussion> adapter;
        private DiscussionDao discussionDao;
        private UserDao userDao;
        private PostDao postDao;
        private PostService postService;
        private Intent intent;
        private ProgressDialog dialog;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            discussionDao = new DiscussionDao();
            userDao = new UserDao();
            postDao = new PostDao();
            postService = new PostService();
            ListView list = FindViewById<ListView>(Resource.Id.list);
            adapter = new SimpleListAdapter<Discussion>(this, discussionDao.getDiscussionFromClass(ContextUtil.Instance.Class));
            list.Adapter = adapter;
            list.ItemClick += new System.EventHandler<AdapterView.ItemClickEventArgs>(list_ItemClick);
        }

        protected override void OnStop()
        {
            if (dialog != null) 
            {
                dialog.Dismiss();
            }
            base.OnStop();
        }

        void list_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Discussion selectedDiscussion = adapter.getItemAtPosition(e.Position);
            ContextUtil.Instance.Discussion = selectedDiscussion._id;
            dialog = ProgressDialog.Show(this, "Carregando", "Por favor, aguarde...", true);
            postService.getPosts(Constants.NewPostURL(Constants.OLD_POST_URL), userDao.getToken(), r => {
                System.Diagnostics.Debug.WriteLine("Posts callback");
                postDao.insertPost(r.Value);
                System.Diagnostics.Debug.WriteLine("Insert OK");
                intent = new Intent(this, typeof(PostActivity));
                StartActivity(intent);
            });
        }
    }
}