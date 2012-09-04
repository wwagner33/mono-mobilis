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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            ListView list = FindViewById<ListView>(Resource.Id.list);
            courseDao = new CourseDao();
            listContent = courseDao.getAllCourses();
            System.Diagnostics.Debug.WriteLine("name = " + listContent[0].name);
            adapter = new SimpleListAdapter<Course>(this, listContent);
            list.Adapter = adapter;
        }
    }
}