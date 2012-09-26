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
using Com.Actionbarsherlock.App;

namespace Mobilis
{
    [Activity(Label = "Post Activity", Theme = "@style/Theme.Mobilis")]
    public class PostActivity : SherlockActivity
    {
        private PostAdapter adapter;
        private ListView list;
        private PostDao postDao;
        private ActionBar actionBar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Post);
            postDao = new PostDao();
            list = FindViewById<ListView>(Resource.Id.list);
            adapter = new PostAdapter(this, postDao.getPostsFromDiscussion(ContextUtil.Instance.Discussion));
            list.Adapter = adapter;

            actionBar = SupportActionBar;
            actionBar.SetHomeButtonEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(false);
            actionBar.SetDisplayUseLogoEnabled(false);
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.Title = "Postagens";

            list.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(list_ItemClick);
        }

        void list_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
           // TODO marcar post   
        }
    }
}