using Android.App;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Database;
using Mobilis.Lib.Model;
using Mobilis.Lib.Util;
using Mobilis.Lib.DataServices;
using Android.Content;
using Com.Actionbarsherlock.App;
using Com.Actionbarsherlock.View;

namespace Mobilis
{
    [Activity(Label = "ClassActivity",Theme = "@style/Theme.Mobilis")]
    public class ClassActivity : SherlockActivity
    {
        private ClassDao classDao;
        private UserDao userDao;
        private DiscussionDao discussionDao;
        private DiscussionService discussionService;
        private SimpleListAdapter<Class> adapter;
        private Intent intent;
        private ProgressDialog dialog;
        private ActionBar actionBar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            FindViewById<TextView>(Resource.Id.screen_title).Text = "Turmas Disponíveis";
            classDao = new ClassDao();
            userDao = new UserDao();
            discussionDao = new DiscussionDao();
            discussionService = new DiscussionService();
            ListView list = FindViewById<ListView>(Resource.Id.list);
            adapter = new SimpleListAdapter<Class>(this, classDao.getClassesFromCourse(ContextUtil.Instance.Course));
            list.Adapter = adapter;
            list.ItemClick += new System.EventHandler<AdapterView.ItemClickEventArgs>(list_ItemClick);

            actionBar = SupportActionBar;
            actionBar.SetHomeButtonEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(false);
            actionBar.SetDisplayUseLogoEnabled(false);
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.Title = "Turmas";
           
        }

        protected override void OnStop()
        {
            if (dialog != null) 
            {
                dialog.Dismiss();
            }
            base.OnStop();
        }

        public void list_ItemClick(object sender, AdapterView.ItemClickEventArgs e) 
        {
            System.Diagnostics.Debug.WriteLine("Click item de turmas");
            Class selectedClass = adapter.getItemAtPosition(e.Position);
            ContextUtil.Instance.Class = selectedClass._id;
            if (discussionDao.existDiscussionsAtClass(selectedClass._id))
            {
                intent = new Intent(this, typeof(DiscussionActivity));
                StartActivity(intent);
            }
            else
            {
                dialog = ProgressDialog.Show(this, "Carregando", "Por favor, aguarde...", true);
                discussionService.getDiscussions(Constants.DiscussionURL, userDao.getToken(), r =>
                {
                    System.Diagnostics.Debug.WriteLine("Discussions callback");
                    discussionDao.insertDiscussion(r.Value);
                    System.Diagnostics.Debug.WriteLine("Insert OK");
                    intent = new Intent(this, typeof(DiscussionActivity));
                    StartActivity(intent);
                });
            }
        }
    }
}