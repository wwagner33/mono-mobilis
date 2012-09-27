using Android.App;
using Android.Content;
using Android.OS;
using Mobilis.Lib.Database;
using Mobilis.Lib.Model;
using Mobilis.Lib;
using System;
using Android.Widget;

namespace Mobilis
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar",Label = "Mobilis")]
    public class SetUpActivity : Activity

    {
        private const string TAG = "setup";
        private Intent intent;
        public const int TERMINATE = 1;
        private CourseDao courseDao;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            courseDao = new CourseDao();
            int flag = Intent.GetIntExtra("content",0);

            if (flag == TERMINATE)
            {
                this.Finish();
            }
            else 
            {
                if (courseDao.existCourses())
                {
                    intent = new Intent(this, typeof(CoursesActivity));
                    StartActivity(intent);
                }
                else
                {
                    intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);
                }
           }
        }
    }
}