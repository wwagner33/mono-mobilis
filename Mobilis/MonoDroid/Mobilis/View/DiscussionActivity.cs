using Android.App;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Model;
using Android.Content;
using Com.Actionbarsherlock.App;
using Com.Actionbarsherlock.View;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib;
using Mobilis.Lib.Messages;

namespace Mobilis
{
    [Activity(Label = "Discussion Activity", Theme = "@style/Theme.Mobilis")]
    public class DiscussionActivity : SherlockActivity
    {
        private SimpleListAdapter<Discussion> adapter;
        private Intent intent;
        private ProgressDialog dialog;
        private ActionBar actionBar;
        private DiscussionsViewModel discussionViewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            discussionViewModel = new DiscussionsViewModel();
            FindViewById<TextView>(Resource.Id.screen_title).Text = "Fóruns Disponíveis";
            ListView list = FindViewById<ListView>(Resource.Id.list);
            adapter = new SimpleListAdapter<Discussion>(this, discussionViewModel.discussions);
            list.Adapter = adapter;
            list.ItemClick += new System.EventHandler<AdapterView.ItemClickEventArgs>(list_ItemClick);

            actionBar = SupportActionBar;
            actionBar.SetHomeButtonEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(false);
            actionBar.SetDisplayUseLogoEnabled(false);
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.Title = "Fóruns";

            ServiceLocator.Messenger.Subscribe<BaseViewMessage>(m =>
            {
                switch (m.Content.message)
                {
                    case BaseViewMessage.MessageTypes.CONNECTION_ERROR:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            Toast.MakeText(this, "Erro de conexão", ToastLength.Short).Show();
                            dialog.Dismiss();
                        });
                        break;

                    case BaseViewMessage.MessageTypes.FUTURE_POSTS_LOADED:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            intent = new Intent(this, typeof(PostActivity));
                            StartActivity(intent);        
                        });
                        break;
                    case BaseViewMessage.MessageTypes.NO_NEW_POSTS:
                        ServiceLocator.Dispatcher.invoke(() =>
                        {
                            Toast.MakeText(this, "Não há novos posts", ToastLength.Short).Show();
                            dialog.Dismiss();
                        });
                        break;
                    default:
                        break;
                }
            });
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
                    discussionViewModel.logout();
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
            if (discussionViewModel.existPostsAtDiscussion(e.Position))
            {
                intent = new Intent(this, typeof(PostActivity));
                StartActivity(intent);
            }
            else
            {
                dialog = ProgressDialog.Show(this, "Carregando", "Por favor, aguarde...", true);
                discussionViewModel.requestPosts();
            }
        }
    }
}