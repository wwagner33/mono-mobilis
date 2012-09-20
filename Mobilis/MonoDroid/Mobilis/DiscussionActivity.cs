
using Android.App;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using Mobilis.Lib.Util;

namespace Mobilis
{
    [Activity(Label = "Discussion Activity",Theme = "@android:style/Theme.NoTitleBar")]
    public class DiscussionActivity : Activity
    {
        private SimpleListAdapter<Discussion> adapter;
        private DiscussionDao discussionDao;
        private UserDao userDao;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            discussionDao = new DiscussionDao();
            userDao = new UserDao();
            ListView list = FindViewById<ListView>(Resource.Id.list);
            adapter = new SimpleListAdapter<Discussion>(this, discussionDao.getDiscussionFromClass(ContextUtil.Instance.Class));
            list.Adapter = adapter;
        }
    }
}