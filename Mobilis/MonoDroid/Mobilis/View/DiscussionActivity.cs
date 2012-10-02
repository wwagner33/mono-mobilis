
using Android.App;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using Mobilis.Lib.Util;
using Mobilis.Lib.DataServices;
using Android.Content;
using Com.Actionbarsherlock.App;
using Com.Actionbarsherlock.View;

namespace Mobilis
{
    [Activity(Label = "Discussion Activity", Theme = "@style/Theme.Mobilis")]
    public class DiscussionActivity : SherlockActivity
    {
        private SimpleListAdapter<Discussion> adapter;
        private DiscussionDao discussionDao;
        private UserDao userDao;
        private PostDao postDao;
        private PostService postService;
        private Intent intent;
        private ProgressDialog dialog;
        private ActionBar actionBar;
        private Discussion selectedDiscussion;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            FindViewById<TextView>(Resource.Id.screen_title).Text = "Fóruns Disponíveis";
            discussionDao = new DiscussionDao();
            userDao = new UserDao();
            postDao = new PostDao();
            postService = new PostService();
            ListView list = FindViewById<ListView>(Resource.Id.list);
            adapter = new SimpleListAdapter<Discussion>(this, discussionDao.getDiscussionsFromClass(ContextUtil.Instance.Class));
            list.Adapter = adapter;
            list.ItemClick += new System.EventHandler<AdapterView.ItemClickEventArgs>(list_ItemClick);

            actionBar = SupportActionBar;
            actionBar.SetHomeButtonEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(false);
            actionBar.SetDisplayUseLogoEnabled(false);
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.Title = "Fóruns";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            Com.Actionbarsherlock.View.MenuInflater inflater = SupportMenuInflater;
            inflater.Inflate(Resource.Menu.options_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_config:
                    intent = new Intent(this, typeof(PreferenceActivity));
                    StartActivity(intent);
                    return true;
                case Resource.Id.menu_logout:
                    User user = userDao.getUser();
                    user.token = null;
                    userDao.addUser(user);
                    intent = new Intent(this, typeof(LoginActivity));
                    intent.SetFlags(ActivityFlags.ClearTop);
                    StartActivity(intent);
                    return true;
                case Resource.Id.menu_refresh:
                    // TODO testar observable collection
                    return true;
                default:
                    return false;
            }
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
            selectedDiscussion = adapter.getItemAtPosition(e.Position);
            ContextUtil.Instance.Discussion = selectedDiscussion._id;
            if (postDao.existPostsAtDiscussion(selectedDiscussion._id))
            {
                intent = new Intent(this, typeof(PostActivity));
                StartActivity(intent);
            }
            else
            {
                dialog = ProgressDialog.Show(this, "Carregando", "Por favor, aguarde...", true);
                postService.getPosts(Constants.NewPostURL(Constants.OLD_POST_DATE), userDao.getToken(), r =>
                {
                    System.Diagnostics.Debug.WriteLine("Posts callback");
                    postDao.insertPost(r.Value);
                    selectedDiscussion.nextPosts = ContextUtil.Instance.postsAfter;
                    selectedDiscussion.previousPosts = ContextUtil.Instance.postsBefore;
                    discussionDao.updateDiscussion(selectedDiscussion);
                    System.Diagnostics.Debug.WriteLine("Insert OK");
                    intent = new Intent(this, typeof(PostActivity));
                    StartActivity(intent);
                });
            }
        }
    }
}