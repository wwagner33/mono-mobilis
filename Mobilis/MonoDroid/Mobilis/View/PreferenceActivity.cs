
using Android.App;
using Android.OS;
using Com.Actionbarsherlock.App;
using Android.Preferences;
using Mobilis.Lib.Database;
using Android.Util;
using Mobilis.Lib.Model;

namespace Mobilis
{
    [Activity(Label = "PreferencesActiviy", Theme = "@style/Theme.Mobilis")]
    public class PreferenceActivity : SherlockPreferenceActivity
    {
        private CheckBoxPreference autoLogin;
        private UserDao userDao;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SupportActionBar.Hide();
            userDao = new UserDao();
            AddPreferencesFromResource(Resource.Layout.Config);
            autoLogin = (CheckBoxPreference)FindPreference("checkbox_preference");
            autoLogin.PreferenceChange += new System.EventHandler<Preference.PreferenceChangeEventArgs>(autoLogin_PreferenceChange);
            autoLogin.Checked = userDao.isAutologinEnabled();
        }

        void autoLogin_PreferenceChange(object sender, Preference.PreferenceChangeEventArgs e)
        {
            User user = userDao.getUser();
            user.autoLogin = (bool)e.NewValue;
            userDao.addUser(user);
        }
    }
}