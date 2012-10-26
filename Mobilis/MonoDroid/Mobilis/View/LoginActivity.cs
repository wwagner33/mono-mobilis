using Android.App;
using Android.Widget;
using Android.OS;
using Mobilis.Lib;
using Android.Content;
using Com.Actionbarsherlock.App;
using Mobilis.Lib.ViewModel;
using Mobilis.Lib.Messages;

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

            ServiceLocator.Messenger.Subscribe<BaseViewMessage>(m => 
            {
                switch (m.Content.message) 
                {
                    case BaseViewMessage.MessageTypes.CONNECTION_ERROR:
                        Toast.MakeText(this, "Erro de conexão", ToastLength.Short).Show();
                        break;
                    case BaseViewMessage.MessageTypes.LOGIN_CONNECTION_OK:
                        getCourses();
                        break;
                    case BaseViewMessage.MessageTypes.COURSE_CONNECTION_OK:
                        intent = new Intent(this, typeof(CoursesActivity));
                        intent.SetFlags(ActivityFlags.ClearTop);
                        StartActivity(intent);
                        break;
                    default:
                        break;
                }            
            });
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
            loginViewModel.submitLoginData(loginField.Text, passwordField.Text);
        }

        public void getCourses()
        {
            loginViewModel.requestCourses();
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

