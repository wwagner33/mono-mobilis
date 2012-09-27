using Android.App;
using Android.Content;
using Android.OS;
using Mobilis.Lib.Database;
using Mobilis.Lib.Model;
using Mobilis.Lib;
using System;
using Android.Widget;
using Android.Util;

namespace Mobilis
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar",Label = "Mobilis")]
    public class SetUpActivity : Activity

    {
        private const string TAG = "setup";
        private Intent intent;
        public const int TERMINATE = 1;
        private CourseDao courseDao;
        private UserDao userDao;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            courseDao = new CourseDao();
            userDao = new UserDao();
            int flag = Intent.GetIntExtra("content",0);

            if (flag == TERMINATE)
            {
                this.Finish();
            }
            else 
            {
                try
                {
                    User user = userDao.getUser();
                    if ((user.token != null) && user.autoLogin)
                    {
                        Log.Info("mobilis","user token = " + user.token + "auto login on");
                        intent = new Intent(this, typeof(CoursesActivity));
                        StartActivity(intent);
                    }
                    else
                    {
                        Log.Info("mobilis", "auto login off");
                        intent = new Intent(this, typeof(LoginActivity));
                        StartActivity(intent);
                    }
                }
                catch (Exception e) 
                {
                    Log.Info("mobilis", "setup Exception");
                    intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);
                }
           }
        }
    }
}