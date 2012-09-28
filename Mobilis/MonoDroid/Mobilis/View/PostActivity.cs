using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Database;
using Mobilis.Lib.Util;
using Com.Actionbarsherlock.App;
using Com.Actionbarsherlock.View;
using System.Collections.ObjectModel;
using Mobilis.Lib.Model;
using Android.Graphics.Drawables;

namespace Mobilis
{
    [Activity(Label = "Post Activity", Theme = "@style/Theme.Mobilis")]
    public class PostActivity : SherlockActivity
    {
        private PostAdapter adapter;
        private ListView list;
        private PostDao postDao;
        private ActionBar actionBar;
        private bool actionBarSelected = false;
        private int selectedPosition = -1;
        ObservableCollection<Post> posts;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Post);
            postDao = new PostDao();
            posts = new ObservableCollection<Post>(postDao.getPostsFromDiscussion(ContextUtil.Instance.Discussion));
            list = FindViewById<ListView>(Resource.Id.list);
            adapter = new PostAdapter(this, posts);
            list.Adapter = adapter;

            actionBar = SupportActionBar;
            setActionBarIdle();

            list.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(list_ItemClick);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Clear();
            MenuInflater inflater = SupportMenuInflater;
            if (actionBarSelected)
            {
                inflater.Inflate(Resource.Menu.action_bar_selected, menu);
                setActionBarSelected();
            }
            else 
            {
                inflater.Inflate(Resource.Menu.options_menu_action, menu);
                setActionBarIdle();
            }
            return true;
        }

        public void setActionBarSelected() 
        {
            actionBar.SetHomeButtonEnabled(false);
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.SetDisplayShowTitleEnabled(false);
            actionBar.SetDisplayUseLogoEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(false);
            actionBar.SetDisplayShowCustomEnabled(true);
            actionBar.SetBackgroundDrawable(new ColorDrawable(Resources
                   .GetColor(Resource.Color.action_bar_active)));
        }

        public void setActionBarIdle() 
        {
            actionBar.SetDisplayShowTitleEnabled(true);
            actionBar.SetHomeButtonEnabled(false);
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.Title = "Postagens";
            actionBar.SetBackgroundDrawable(new ColorDrawable(Resources
                    .GetColor(Resource.Color.action_bar_idle)));
        }

        void list_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (selectedPosition == -1) 
            {
                posts[e.Position].marked = true;
                selectedPosition = e.Position;
                actionBarSelected = true;
            }
            else if (selectedPosition == e.Position) 
            {
                posts[e.Position].marked = false;
                selectedPosition = -1;
                actionBarSelected = false;
            }
            else 
            {
                posts[e.Position].marked = true;
                posts[selectedPosition].marked = false;
                selectedPosition = e.Position;
                actionBarSelected = true;
            }
            adapter.NotifyDataSetChanged();
            SupportInvalidateOptionsMenu();
        }
        public override void OnBackPressed()
        {
            if (selectedPosition != -1)
            {
                posts[selectedPosition].marked = false;
                adapter.NotifyDataSetChanged();
                actionBarSelected = false;
                selectedPosition = -1;
                setActionBarIdle();
                SupportInvalidateOptionsMenu();
            }
            else
                base.OnBackPressed();
        }
    }
}