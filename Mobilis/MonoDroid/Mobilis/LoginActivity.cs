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

namespace Mobilis
{
    [Activity(Theme = "@style/Theme.Mobilis")]
    public class LoginActivity : SherlockActivity, View.IOnClickListener
    {
        private Button submit;
        private EditText loginField, passwordField;
        private LoginService loginService;
        private CourseService courseService;
        private Intent intent;
        private CourseDao courseDao;
        public const string TAG = "mobilis";
        private UserDao userDao;
        private ProgressDialog dialog;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SupportActionBar.Hide();
            SetContentView(Resource.Layout.Login);
            courseDao = new CourseDao();
            userDao = new UserDao();
            ServiceLocator.Dispatcher = new DispatchAdapter(this);
            loginService = new LoginService();
            courseService = new CourseService();
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

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.submit) {
                string loginData = JSON.generateLoginObject(loginField.Text, passwordField.Text);
                Log.Info(TAG,"User data = " + loginData);
                dialog = ProgressDialog.Show(this,"Carregando","Por favor, aguarde...",true);
                loginService.getToken(loginField.Text, passwordField.Text, r => {
                    var enumerator = r.Value.GetEnumerator();
                    enumerator.MoveNext();
                    string token = enumerator.Current;
                    Log.Info(TAG,"Token = " + enumerator.Current);
                    User user = new User();
                    user.token = token;
                    user._id = 1;
                    userDao.addUser(user);
                    ServiceLocator.Dispatcher.invoke(() => {
                        getCourses(token);
                   });
                });
            }
        }

        public void getCourses(string token)
        {
            courseService.getCourses(Constants.CoursesURL, token, r => {
                courseDao.insertAll(r.Value);
                Log.Info(TAG, "Insert OK");
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

