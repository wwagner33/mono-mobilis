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
using Mobilis.Lib.Model;
using MWC.DL.SQLite;
using Mobilis.Lib.Database;

namespace Mobilis
{
    [Activity(Label = "CourseActivity")]
    public class CoursesActivity : Activity
    {
        private List<Course> listContent;
        private SimpleListAdapter<Course> adapter;
        private CourseDao courseDao;
        private Intent intent;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            ListView list = FindViewById<ListView>(Resource.Id.list);
            courseDao = new CourseDao();
            listContent = courseDao.getAllCourses();
            adapter = new SimpleListAdapter<Course>(this, listContent);
            list.Adapter = adapter;

            list.ItemClick += new EventHandler<Android.Widget.AdapterView.ItemClickEventArgs>(list_ItemClick);
        }

        void list_ItemClick(object sender, Android.Widget.AdapterView.ItemClickEventArgs e) 
        {
            Toast.MakeText(this, "itemClick", ToastLength.Short).Show();
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