
using Android.App;
using Android.Content;
using Android.OS;
using Mobilis.Lib.Database;
using Mobilis.Lib.Model;
using Mobilis.Lib;
namespace Mobilis
{
    [Activity(Label = "Mobilis", MainLauncher = true, Icon = "@drawable/icon")]
    public class SetUpActivity : Activity
    {
        private const string TAG = "setup";
        private DatabaseHelper helper;
        private Intent intent;
        //private UserDao userDao;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            helper = new DatabaseHelper(this);
            //createDatabase();
            intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }

        /*
        public void createDatabase()
        {
            var database = helper.WritableDatabase;
            database.Close();

            var databaseConnection = new SQLiteConnection(Constants.DATABASE_PATH);
            databaseConnection.CreateTable<Course>();
        }
         */
    }
}