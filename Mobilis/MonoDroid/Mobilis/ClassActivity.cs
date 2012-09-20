
using Android.App;
using Android.OS;
using Android.Widget;
using Mobilis.Lib.Database;
using Mobilis.Lib.Model;
using Mobilis.Lib.Util;

namespace Mobilis
{
    [Activity(Label = "ClassActivity",Theme = "@android:style/Theme.NoTitleBar")]
    public class ClassActivity : Activity
    {
        private ClassDao classDao;
        private SimpleListAdapter<Class> adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SimpleList);
            classDao = new ClassDao();
            ListView list = FindViewById<ListView>(Resource.Id.list);
            adapter = new SimpleListAdapter<Class>(this, classDao.getClassesFromCourse(ContextUtil.Instance.Course));
            list.Adapter = adapter;
        }
    }
}