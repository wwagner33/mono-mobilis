
using Android.App;
using Android.OS;
using Com.Actionbarsherlock.App;
using Mobilis.Lib.Database;
using Android.Widget;
using Android.Media;
using Mobilis.Lib.Util;
using Mobilis.Lib.Model;
using Android.Util;
using Mobilis.Lib.DataServices;
using System;
using Mobilis.Lib;
using System.IO;
using System.Threading;
using System.Collections.Specialized;

namespace Mobilis
{
    [Activity(Label = "Response", Theme = "@style/Theme.Mobilis")]
    public class ResponseActivity : SherlockActivity
    {
        private ActionBar actionBar;
        private UserDao userDao;
        private Button submit;
        private ImageButton record;
        private bool recordingExists = false;
        private EditText message;
        private MediaRecorder recorder;
        private ImageView previewRecord;
        private PostService postService;
        private TextView chronometer;
        private bool recording = false;
        private System.Timers.Timer timer;
        int count = 0;
        private SendAudioService audioService;
        private SendPostService deliverPostService;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Response);
            userDao = new UserDao();
            postService = new PostService();
            audioService = new SendAudioService();
            deliverPostService = new SendPostService();
            actionBar = SupportActionBar;
            actionBar.SetHomeButtonEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(false);
            actionBar.SetDisplayUseLogoEnabled(false);
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.Title = "Responder";
            chronometer = FindViewById<TextView>(Resource.Id.recording_lenght);
            previewRecord = FindViewById<ImageView>(Resource.Id.record_image);
            message = FindViewById<EditText>(Resource.Id.criar_topico_conteudo);
            submit = FindViewById<Button>(Resource.Id.criar_topico_submit);
            submit.Click += new System.EventHandler(submit_Click);
            record = FindViewById<ImageButton>(Resource.Id.btn_gravar);
            record.Click += new System.EventHandler(record_Click);
            ServiceLocator.Dispatcher = new DispatchAdapter(this);
            ServiceLocator.Recorder = new RecordAdapter();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ServiceLocator.Dispatcher.invoke(() => 
            {
                count++;
                int sec = count % 60;
                int min = count / 60;
                chronometer.Text = ((min>=10) ? ""+min : "0"+min) +":" + ((sec>=10) ? ""+sec : "0"+sec);
            });
        }

        void record_Click(object sender, System.EventArgs e)
        {
            if (!recording)
            {   
                record.SetImageResource(Resource.Drawable.recorder_active);
                recording = true;
                chronometer.Text = "00:00";
                chronometer.Visibility = Android.Views.ViewStates.Visible;
                count = 0;
                timer.Start();
                ServiceLocator.Recorder.Start();
            }
            else 
            {
                record.SetImageResource(Resource.Drawable.recorder_idle);
                timer.Stop();
                recording = false;
                ServiceLocator.Recorder.Stop();
                chronometer.Visibility = Android.Views.ViewStates.Gone;
                recordingExists = true;
            }
        }

        public void submitAudioRecording(int postId)
        {
            Log.Info("teste", "Enviando áudio");
            ThreadPool.QueueUserWorkItem(status => 
            {
                /*
                audioService.SendAudio(Constants.DeliverAudioURL(postId), userDao.getToken(), File.ReadAllBytes(RecordAdapter.FILE_PATH + RecordAdapter.FILE_NAME), r => 
                {
                    Log.Info("teste", "Envio de áudio OK");
                });
                 */
                audioService.SendAudio2(Constants.DeliverAudioURL(postId), userDao.getToken(), RecordAdapter.FILE_PATH + RecordAdapter.FILE_NAME, "", "audio/3gpp", null);

            });
        }

        void submit_Click(object sender, System.EventArgs e)
        {

            if (!recordingExists && message.Text.Trim().Equals(""))
            {
                Toast.MakeText(this, "Mensagem não pode ser vazia", ToastLength.Short).Show();
            }
            else 
            { 
                string content = JSON.createJSONPostEntity(message.Text, ContextUtil.Instance.Post);
                deliverPostService.sendPost(Constants.DeliverPostURL, userDao.getToken(), content, r => 
                {
                    Log.Info("teste", "post enviado com sucesso");
                    var enumerator = r.Value.GetEnumerator();
                    enumerator.MoveNext();
                    if (recordingExists)
                    {
                        submitAudioRecording(enumerator.Current);
                    }
                    else 
                    {
                        // volta pra lista de postse atualiza +1 nos unread;
                        Toast.MakeText(this, "ir para a tela de posts", ToastLength.Short).Show();
                    }
                });
            }
        }
    }
}