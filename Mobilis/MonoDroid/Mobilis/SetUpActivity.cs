
using Android.App;
using Android.Content;
using Android.OS;
using Mobilis.Lib.Database;
using Mobilis.Lib.Model;
using Mobilis.Lib;
using System;
namespace Mobilis
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar",Label = "Mobilis")]
    public class SetUpActivity : Activity

    {
        private const string TAG = "setup";
        private DatabaseHelper helper;
        private Intent intent;
        public const int TERMINATE = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            int flag = Intent.GetIntExtra("content",0);

            if (flag == TERMINATE)
            {
                this.Finish();
            }

            else 
            {
            helper = new DatabaseHelper(this);
            intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
           }
        }
    }
}