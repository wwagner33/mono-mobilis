using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mobilis.Lib.Database;
using Mobilis.Lib.Util;

namespace Mobilis
{
    [Activity(Label = "Post Activity", Theme = "@android:style/Theme.NoTitleBar")]
    public class PostActivity : Activity
    {
        private PostAdapter adapter;
        private ListView list;
        private PostDao postDao;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Post);
            postDao = new PostDao();
            list = FindViewById<ListView>(Resource.Id.list);
            adapter = new PostAdapter(this, postDao.getPostsFromDiscussion(ContextUtil.Instance.Discussion));
            list.Adapter = adapter;
        }
    }
}