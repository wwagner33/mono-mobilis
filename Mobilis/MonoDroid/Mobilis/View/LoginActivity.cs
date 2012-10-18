using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Mobilis.Lib;
using Mobilis.Lib.Util;
using Mobilis.Lib.DataServices;
using Android.Content;
using Mobilis.Lib.Database;
using Android.Util;
using Mobilis.Lib.Model;
using Com.Actionbarsherlock.App;
using System;
using Mobilis.Lib.ViewModel;

namespace Mobilis
{
    [Activity(Theme = "@style/Theme.Mobilis")]
    public class LoginActivity : SherlockActivity, Android.Views.View.IOnClickListener
    {
        private Button submit;
        private EditText loginField, passwordField;
        private Intent intent;
        public const string TAG = "mobilis";
        private ProgressDialog dialog;

        private LoginViewModel loginViewModel;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SupportActionBar.Hide();
            SetContentView(Resource.Layout.Login);
            loginViewModel = new LoginViewModel();
            ServiceLocator.Dispatcher = new DispatchAdapter(this);
            loginField = FindViewById<EditText>(Resource.Id.username);
            passwordField = FindViewById<EditText>(Resource.Id.password);
            submit = FindViewById<Button>(Resource.Id.submit);
            submit.SetOnClickListener(this);
        }

        protected override void OnStop()
        {
            if (dialog != null) 
            {
                dialog.Dismiss();
            }
            base.OnStop();
        }

        public void OnClick(Android.Views.View v)
        {
            dialog = ProgressDialog.Show(this, "Carregando", "Por favor, aguarde...", true);
            loginViewModel.submitLoginData(loginField.Text, passwordField.Text, () => 
            {
                ServiceLocator.Dispatcher.invoke(() =>
                {
                    getCourses();
                });
            });
        }

        public void getCourses()
        {
            loginViewModel.requestCourses(() => 
            {
                intent = new Intent(this, typeof(CoursesActivity));
                intent.SetFlags(ActivityFlags.ClearTop);
                StartActivity(intent);
            });
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

