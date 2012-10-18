using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Model;
using Com.Actionbarsherlock.App;
using Com.Actionbarsherlock.View;
using Mobilis.Lib.ViewModel;

namespace Mobilis
{
    [Activity(Label = "CourseActivity", Theme = "@style/Theme.Mobilis")]
    public class CoursesActivity : SherlockActivity
    {
        private SimpleListAdapter<Course> adapter;
        private Intent intent;
        private ProgressDialog dialog;
        private ActionBar actionBar;
        private CoursesViewModel coursesViewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            coursesViewModel = new CoursesViewModel();
            FindViewById<TextView>(Resource.Id.screen_title).Text = "Cursos Disponíveis";
            ListView list = FindViewById<ListView>(Resource.Id.list);
            adapter = new SimpleListAdapter<Course>(this, coursesViewModel.listContent);
            list.Adapter = adapter;
            list.ItemClick += new EventHandler<Android.Widget.AdapterView.ItemClickEventArgs>(list_ItemClick);

            actionBar = SupportActionBar;
            actionBar.SetHomeButtonEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(false);
            actionBar.SetDisplayUseLogoEnabled(false);
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.Title = "Cursos";
        }

        public override bool OnCreateOptionsMenu(Com.Actionbarsherlock.View.IMenu menu)
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
                    intent = new Intent(this,typeof(PreferenceActivity));
                    StartActivity(intent);
                    return true;
                case Resource.Id.menu_logout:
                    coursesViewModel.logout(() => 
                    {
                        intent = new Intent(this, typeof(LoginActivity));
                        intent.SetFlags(ActivityFlags.ClearTop);
                        StartActivity(intent);                    
                    });     
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

        void list_ItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e) 
        {
            if (coursesViewModel.existClasses(e.Position))
            {
                intent = new Intent(this, typeof(ClassActivity));
                StartActivity(intent);
            }

            else 
            {
                dialog = ProgressDialog.Show(this, "Carregando", "Por favor, aguarde...", true);
                coursesViewModel.requestClass(() => 
                {
                    intent = new Intent(this, typeof(ClassActivity));
                    StartActivity(intent);
                });
            }
        }

        public override void OnBackPressed()
        {
            intent = new Intent(this, typeof(SetUpActivity));
            intent.PutExtra("content", SetUpActivity.TERMINATE);
            intent.SetFlags(ActivityFlags.ClearTop);
            StartActivity(intent);
        }
    }
}