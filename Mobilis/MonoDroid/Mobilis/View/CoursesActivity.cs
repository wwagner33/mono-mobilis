using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Model;
using Mobilis.Lib.Database;
using Mobilis.Lib.DataServices;
using Mobilis.Lib.Util;
using Com.Actionbarsherlock.App;
using Com.Actionbarsherlock.View;

namespace Mobilis
{
    [Activity(Label = "CourseActivity", Theme = "@style/Theme.Mobilis")]
    public class CoursesActivity : SherlockActivity
    {
        private List<Course> listContent;
        private SimpleListAdapter<Course> adapter;
        private CourseDao courseDao;
        private UserDao userDao;
        private ClassService classService;
        private ClassDao classDao;
        private Intent intent;
        private ProgressDialog dialog;
        private ActionBar actionBar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            FindViewById<TextView>(Resource.Id.screen_title).Text = "Cursos Disponíveis";
            ListView list = FindViewById<ListView>(Resource.Id.list);
            classService = new ClassService();
            courseDao = new CourseDao();
            userDao = new UserDao();
            classDao = new ClassDao();
            listContent = courseDao.getAllCourses();
            adapter = new SimpleListAdapter<Course>(this, listContent);
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
            Course selectedCourse = adapter.getItemAtPosition(e.Position);
            ContextUtil.Instance.Course = selectedCourse._id;
            if (classDao.existClassAtCourse(selectedCourse._id))
            {
                intent = new Intent(this, typeof(ClassActivity));
                StartActivity(intent);
            }
            else
            {
                dialog = ProgressDialog.Show(this, "Carregando", "Por favor, aguarde...", true);
                classService.getClasses(Constants.ClassesURL, userDao.getToken(), r =>
                {
                    System.Diagnostics.Debug.WriteLine("Classes callback");
                    classDao.insertClasses(r.Value);
                    System.Diagnostics.Debug.WriteLine("Insert OK");
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