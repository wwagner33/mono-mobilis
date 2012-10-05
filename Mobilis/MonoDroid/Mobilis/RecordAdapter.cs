using Mobilis.Lib;
using Android.Media;
using System.Threading;
using System.IO;
using Android.OS;
namespace Mobilis
{
    public class RecordAdapter : IRecordAudio
    {
        public static string FILE_PATH = Environment
			.ExternalStorageDirectory.AbsolutePath
			+ "/Mobilis/Recordings/";
        public const string FILE_NAME = "recording.3gp";
        private MediaRecorder recorder;

        public RecordAdapter() 
        {
            recorder = new MediaRecorder();
            if (!Directory.Exists(FILE_PATH))
                Directory.CreateDirectory(FILE_PATH);
        }

        private void initRecorder() 
        {
            recorder.Reset();
            recorder.SetAudioSource(AudioSource.Mic);
            recorder.SetOutputFormat(OutputFormat.ThreeGpp);
            recorder.SetAudioEncoder(AudioEncoder.AmrNb);
            recorder.SetOutputFile(FILE_PATH + FILE_NAME);
            recorder.Prepare();
        }

        public void Start() 
        {
            initRecorder();
            recordAsync();
        }

        public void Stop() 
        {
            recorder.Stop();
        }

        private void recordAsync() 
        {
            ThreadPool.QueueUserWorkItem(state => 
            {
                recorder.Start();
            });
        }
    }
}