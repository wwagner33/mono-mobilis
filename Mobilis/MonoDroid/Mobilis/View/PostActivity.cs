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
using Android.Views;
using Mobilis.Lib.DataServices;
using System.Collections.Generic;
using Android.Util;
using Mobilis.Lib;
using Android.Content;

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
        private ObservableCollection<Post> posts;
        private DiscussionDao discussionDao;
        private View footerRefresh, footerNextPosts, headerPreviousPosts;
        private Discussion selectedDiscussion;
        private PostService postService;
        private UserDao userDao;
        private ProgressDialog dialog;
        private Intent intent;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Post);
            postDao = new PostDao();
            userDao = new UserDao();
            ServiceLocator.Dispatcher = new DispatchAdapter(this);
            postService = new PostService();
            posts = new ObservableCollection<Post>(postDao.getPostsFromDiscussion(ContextUtil.Instance.Discussion));
            list = FindViewById<ListView>(Resource.Id.list);
            adapter = new PostAdapter(this, posts);
            actionBar = SupportActionBar;
            setActionBarIdle();
            list.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(list_ItemClick);
            footerNextPosts = LayoutInflater.Inflate(Resource.Layout.NextPostFooter, list, false);
            footerNextPosts.Click += (o, e) =>
            {
                loadFuturePosts();
            };
            footerRefresh = LayoutInflater.Inflate(Resource.Layout.RefreshFooter, list, false);
            footerRefresh.Click += (o, e) =>
            {
                loadFuturePosts();
            };
            headerPreviousPosts = LayoutInflater.Inflate(Resource.Layout.PreviousPostsHeader, list, false);
            headerPreviousPosts.Click += (o, e) =>
            {
                loadPreviousPosts();
            };
            discussionDao = new DiscussionDao();
            selectedDiscussion = discussionDao.getDiscussion(ContextUtil.Instance.Discussion);
            FindViewById<TextView>(Resource.Id.forum_title).Text = selectedDiscussion.name;
            FindViewById<TextView>(Resource.Id.forum_range).Text = HttpUtils.discussionDateToShowFormat(selectedDiscussion.startDate) + " - "
                + HttpUtils.discussionDateToShowFormat(selectedDiscussion.endDate);
            ContextUtil.Instance.postsBefore = selectedDiscussion.previousPosts;
            ContextUtil.Instance.postsAfter = selectedDiscussion.nextPosts;
            toggleHeader();
            toggleFooter();
            list.Adapter = adapter;
        }

        public void unmarkSelectedPost() 
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
        }

        protected override void OnPause()
        {
            selectedDiscussion.nextPosts = ContextUtil.Instance.postsAfter;
            selectedDiscussion.previousPosts = ContextUtil.Instance.postsBefore;
            discussionDao.updateDiscussion(selectedDiscussion);
            if (dialog != null)
                dialog.Dismiss();
            base.OnPause();
        }

        public void toggleHeader() 
        {
            if (ContextUtil.Instance.postsBefore > 0)
            {
                headerPreviousPosts.FindViewById<TextView>(Resource.Id.load_available_posts).Text = ContextUtil.Instance.postsBefore + " "
                 + Resources.GetString(Resource.String.unloaded_posts_count);
                list.AddHeaderView(headerPreviousPosts);
            }
            else 
            {
                list.RemoveHeaderView(headerPreviousPosts);
            }
        }

        public void toggleFooter() 
        {
            list.RemoveFooterView(footerRefresh);
            list.RemoveFooterView(footerNextPosts);
            if (ContextUtil.Instance.postsAfter > 0)
            {
                list.AddFooterView(footerNextPosts, null, true);
                footerNextPosts.FindViewById<TextView>(Resource.Id.load_available_posts).Text = ContextUtil.Instance.postsAfter + " "
                        + Resources.GetString(Resource.String.unloaded_posts_count);
            }
            else 
            {
                list.AddFooterView(footerRefresh,null,true);
            }
        }

        public void loadFuturePosts() 
        {
            unmarkSelectedPost();
            int oldNext = ContextUtil.Instance.postsAfter;
            int oldPrevious = ContextUtil.Instance.postsBefore;

            string date;
            if (posts.Count == 0)
            {
                date = Constants.OLD_POST_DATE;
            }
            else 
            {
                date = HttpUtils.postDateToServerFormat(posts[posts.Count-1].updatedAt);  
            }
            postService.getPosts(Constants.NewPostURL(date), userDao.getToken(), r => {
                List<Post> newPosts = new List<Post>(r.Value);
                if (newPosts.Count == 0)
                {
                    ServiceLocator.Dispatcher.invoke(() => {
                        Toast.MakeText(this, "Não há novos posts", ToastLength.Short).Show();
                    });
                }
                 ServiceLocator.Dispatcher.invoke(() => {
                    foreach (Post post in newPosts)
                      {
                        posts.Add(post);
                      }
                    });

                 List<Post> postsFromDatabse = postDao.getPostsFromDiscussion(selectedDiscussion._id);
                 postsFromDatabse.AddRange(newPosts);
                    while (postsFromDatabse.Count > 20) 
                    {
                        postsFromDatabse.RemoveAt(0);
                    }
                    ContextUtil.Instance.postsBefore = oldPrevious + newPosts.Count;
                  postDao.insertPost(postsFromDatabse);
                  ServiceLocator.Dispatcher.invoke(() => {
                     toggleFooter();
                 });
            });
        }

        public void loadPreviousPosts() 
        {
            unmarkSelectedPost();
            int oldNext = ContextUtil.Instance.postsAfter;
            int oldPrevious = ContextUtil.Instance.postsBefore;

            string date = HttpUtils.postDateToServerFormat(posts[0].updatedAt);
            postService.getPosts(Constants.HistoryPostURL(date), userDao.getToken(), r => {
                List<Post> oldPosts = new List<Post>(r.Value);
                int newPostCount = oldPosts.Count;
                ServiceLocator.Dispatcher.invoke(() => {
                    foreach (Post post in oldPosts)
                    {
                        posts.Insert(0, post);
                    }
                });
                List<Post> postsFromDatabse = postDao.getPostsFromDiscussion(selectedDiscussion._id);
                oldPosts.AddRange(postsFromDatabse);
                while (oldPosts.Count > 20)
                {
                    oldPosts.RemoveAt(oldPosts.Count-1);
                }
                postDao.insertPost(oldPosts);
                ContextUtil.Instance.postsAfter = oldNext + newPostCount;
                ServiceLocator.Dispatcher.invoke(() =>
                {
                    toggleHeader();
                });
            });
        }

        public override bool OnCreateOptionsMenu(Com.Actionbarsherlock.View.IMenu menu)
        {
            menu.Clear();
            Com.Actionbarsherlock.View.MenuInflater inflater = SupportMenuInflater;
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

        public override bool OnOptionsItemSelected(Com.Actionbarsherlock.View.IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.reply:
                    intent = new Intent(this, typeof(ResponseActivity));
                    StartActivity(intent);
                    return true;
                default:
                    return false;
            }
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
            int position = (list.HeaderViewsCount > 0) ? e.Position - 1 : e.Position;
            ContextUtil.Instance.Post = posts[position]._id;
            if (selectedPosition == -1) 
            {
                posts[position].marked = true;
                selectedPosition = position;
                actionBarSelected = true;
            }
            else if (selectedPosition == position)
            {
                posts[position].marked = false;
                selectedPosition = -1;
                actionBarSelected = false;
            }
            else 
            {
                posts[position].marked = true;
                posts[selectedPosition].marked = false;
                selectedPosition = position;
                actionBarSelected = true;
            }
            adapter.NotifyDataSetChanged();
            SupportInvalidateOptionsMenu();
        }
        public override void OnBackPressed()
        {
            if (selectedPosition != -1)
            {
                unmarkSelectedPost();
            }
            else
                base.OnBackPressed();
        }
    }
}