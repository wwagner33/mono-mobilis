using System;
using Android.App;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Util;
using Com.Actionbarsherlock.App;
using Android.Graphics.Drawables;
using Android.Views;
using Mobilis.Lib;
using Android.Content;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib.Messages;

namespace Mobilis
{
    [Activity(Label = "Post Activity", Theme = "@style/Theme.Mobilis")]
    public class PostActivity : SherlockActivity
    {
        private PostAdapter adapter;
        private ListView list;
        private ActionBar actionBar;
        private View footerRefresh, footerNextPosts, headerPreviousPosts;
        private ProgressDialog dialog;
        private Intent intent;
        private ImageButton play, next, prev, stop;
        private PostsViewModel postsViewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Post);
            postsViewModel = new PostsViewModel(new PlayerAdapter());
            ServiceLocator.Dispatcher = new DispatchAdapter(this);
            list = FindViewById<ListView>(Resource.Id.list);
            adapter = new PostAdapter(this, postsViewModel.posts);
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
            FindViewById<TextView>(Resource.Id.forum_title).Text = postsViewModel.selectedDiscussion.name;
            FindViewById<TextView>(Resource.Id.forum_range).Text = HttpUtils.discussionDateToShowFormat(postsViewModel.selectedDiscussion.startDate) + " - "
                + HttpUtils.discussionDateToShowFormat(postsViewModel.selectedDiscussion.endDate);
            toggleHeader();
            toggleFooter();
            list.Adapter = adapter;
            play = FindViewById<ImageButton>(Resource.Id.button_play);

            play.Click += (o, e) => 
            {
                postsViewModel.playSelectedPost();
            };

            stop = FindViewById<ImageButton>(Resource.Id.button_stop);

            stop.Click += (o, e) =>
            {
                play.SetImageResource(Resource.Drawable.playback_play);
                togglePlayerBar(false);
                postsViewModel.releaseResources();
                postsViewModel.removeSelection();
            };

            next = FindViewById<ImageButton>(Resource.Id.button_next);

            next.Click += (o, e) =>
            {
                play.SetImageResource(Resource.Drawable.playback_pause);
                postsViewModel.playNextInSelection();
            };

            prev = FindViewById<ImageButton>(Resource.Id.button_prev);

            prev.Click += (o, e) =>
            {
                play.SetImageResource(Resource.Drawable.playback_pause);
                postsViewModel.playPreviousInSelection();
            };

            ServiceLocator.Messenger.Subscribe<BaseViewMessage>(m =>
            {
                switch (m.Content.message) 
                {
                    case BaseViewMessage.MessageTypes.NO_NEW_POSTS:
                    Toast.MakeText(this, "Não há novos posts", ToastLength.Short).Show();
                    break;
                    case BaseViewMessage.MessageTypes.FUTURE_POSTS_LOADED:
                    toggleFooter();
                    break;
                    case BaseViewMessage.MessageTypes.PREVIOUS_POSTS_LOADED:
                    toggleHeader();
                    break;
                    case BaseViewMessage.MessageTypes.FINISHED_PLAYING:
                    togglePlayerBar(false);
                    break;
                    case BaseViewMessage.MessageTypes.UPDATE_SCREEN:
                    updateScreen();
                    break;
                    default:
                    break;
                }            
            });
        }

        protected override void OnPause()
        {
            postsViewModel.saveSelection();
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

        public void togglePlayerBar(bool status) 
        {
            if (status)
            {
                play.Visibility = ViewStates.Visible;
                stop.Visibility = ViewStates.Visible;
                prev.Visibility = ViewStates.Visible;
                next.Visibility = ViewStates.Visible;
            }
            else 
            {
                play.Visibility = ViewStates.Gone;
                stop.Visibility = ViewStates.Gone;
                prev.Visibility = ViewStates.Gone;
                next.Visibility = ViewStates.Gone;
            }
        }

        public void loadFuturePosts()
        {
            postsViewModel.removeSelection();
            postsViewModel.loadFuturePosts();
        }

        public void loadPreviousPosts() 
        {
            postsViewModel.removeSelection();
            postsViewModel.loadPreviousPosts();
        }

        public override bool OnCreateOptionsMenu(Com.Actionbarsherlock.View.IMenu menu)
        {
            menu.Clear();
            Com.Actionbarsherlock.View.MenuInflater inflater = SupportMenuInflater;
            if (postsViewModel.contextualSelection)
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
                case Resource.Id.play:
                    togglePlayerBar(true);
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
            postsViewModel.togglePostMarked(position);
            updateScreen();
        }
        public override void OnBackPressed()
        {
            if (postsViewModel.contextualSelection)
            {
                postsViewModel.removeSelection();
            }
            else
                base.OnBackPressed();
        }

        public void updateScreen() 
        {
            adapter.NotifyDataSetChanged();
            SupportInvalidateOptionsMenu();
        }
    }
}