
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

namespace Mobilis
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar")]
    public class LoginActivity : Activity, View.IOnClickListener
    {
        private Button submit;
        private EditText loginField, passwordField;
        private LoginService loginService;
        private CourseService courseService;
        private Intent intent;
        private CourseDao courseDao;
        private const string TAG = "login";
        private UserDao userDao;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
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

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.submit) {
                string loginData = JSON.generateLoginObject(loginField.Text, passwordField.Text);
                System.Diagnostics.Debug.WriteLine("User data = " + loginData);
                loginService.getToken(loginField.Text, passwordField.Text, r => {
                    var enumerator = r.Value.GetEnumerator();
                    enumerator.MoveNext();
                    string token = enumerator.Current;
                    System.Diagnostics.Debug.WriteLine("Token = " + enumerator.Current);

                    User user = new User();
                    user.token = token;
                    user._id = 1;

                    userDao.addUser(user);

                    ServiceLocator.Dispatcher.invoke( () => {
                        getCourses(token);
                   });
                });
            }
        }

        public void getCourses(string token)
        {
            courseService.getCourses(Constants.CoursesURL, token, r => {
                courseDao.insertAll(r.Value);
                System.Diagnostics.Debug.WriteLine("Insert OK");
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

