
using Android.App;
using Android.OS;
using Com.Actionbarsherlock.App;
using Mobilis.Lib.Database;
using Android.Widget;
using Android.Media;

namespace Mobilis
{
    [Activity(Label = "Response", Theme = "@style/Theme.Mobilis")]
    public class ResponseActivity : SherlockActivity
    {
        private ActionBar actionBar;
        private UserDao userDao;
        private Button submit;
        private ImageButton record;
        private bool recordingExists;
        private EditText message;
        private MediaRecorder recorder;
        private ImageView previewRecord;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Response);
            userDao = new UserDao();
            actionBar = SupportActionBar;
            actionBar.SetHomeButtonEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(false);
            actionBar.SetDisplayUseLogoEnabled(false);
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.Title = "Responder";
            previewRecord = FindViewById<ImageView>(Resource.Id.record_image);
            message = FindViewById<EditText>(Resource.Id.criar_topico_conteudo);
            submit = FindViewById<Button>(Resource.Id.criar_topico_submit);
            submit.Click += new System.EventHandler(submit_Click);
            record = FindViewById<ImageButton>(Resource.Id.btn_gravar);
            record.Click += new System.EventHandler(record_Click);
        }

        void record_Click(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "record", ToastLength.Short).Show();
        }

        void submit_Click(object sender, System.EventArgs e)
        {
            //Toast.MakeText(this, "submit", ToastLength.Short).Show();
            if (!recordingExists && submit.Text.Length == 0)
            {
                Toast.MakeText(this, "Mensagem não pode ser vazia", ToastLength.Short).Show();
            }
            else 
            { 
                // Construir objeto de envio ao WS
            }
        }
    }
}