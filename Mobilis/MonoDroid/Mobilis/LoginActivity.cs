
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

namespace Mobilis
{
    [Activity(Label = "Login")]
    public class LoginActivity : Activity, View.IOnClickListener
    {
        private Button submit;
        private EditText loginField, passwordField;
        private LoginService loginService;
        private CourseService courseService;
        private Intent intent;
        private DatabaseHelper helper;
        private CourseDao courseDao;
        private const string TAG = "login";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);
            helper = new DatabaseHelper(this);
            courseDao = new CourseDao();
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
                    ServiceLocator.Dispatcher.invoke( () => {
                        getCourses(token);
                   });
                });
            }
             
        }

        public void getCourses(string token)
        {
            courseService.getCourses("curriculum_units/list.json", token, r => {
                courseDao.insertAll(r.Value);
                System.Diagnostics.Debug.WriteLine("Insert OK");
                intent = new Intent(this, typeof(CoursesActivity));
                intent.PutExtra("activity", Constants.ACTIVITY_COURSES);
                StartActivity(intent);
            });
        }

        /*
        private void findNearestAirport() 
        {
            var coordinates = new MonoMobile.Extensions.GeoLocation();
            coordinates.getCurrentPosition(positionAvaliable);
        }

        private void PositionAvaliable(Position position) 
        {
            Console.WriteLine("{0},{1}", 
                position.Coords.Latitude,
                position.Coords.Longitude);
        }
        */
    }
}

