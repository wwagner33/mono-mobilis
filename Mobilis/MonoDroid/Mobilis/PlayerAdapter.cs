using Mobilis.Lib;
using Android.Media;
using System.Threading;
using Mobilis.Lib.Util;
using System;
using Java.IO;

namespace Mobilis
{
    public class PlayerAdapter : MediaPlayer,IAsyncPlayer, MediaPlayer.IOnCompletionListener,MediaPlayer.IOnErrorListener
    {
        public Mobilis.Lib.TTSManager.BlockFinishedPlaying callback;

        /*Implementação do player de posts na plataforma Android 
          
         * O player deve ter conhecimento do endereço dos blocos a
         * serem tocados e tocar o próximo bloco se já tiver sido
         * baixado.
         * 
         * Os posts estarão localizados na pasta Mobilis/TTS/:(id do post)
         * onde haverá um arquivo para cada bloco.
         * 
         * A pasta, juntamente com os arquivos da mesma, devem ser deletados
         * ao terminar de tocar um bloco.
         * 
         */

        /*
        public override void Start()
        {
            Sobrescrevendo o start para ser rodado em uma thread separada.
            base.Reset();
            ThreadPool.QueueUserWorkItem(state => 
            {
                base.Start();
            });
        }
         */

        public PlayerAdapter() 
        {
            this.SetOnCompletionListener(this);
            this.SetOnErrorListener(this);
        }

        public void play(int blockId,Mobilis.Lib.TTSManager.BlockFinishedPlaying callback) 
        {
            this.callback = callback;
            base.Reset();
            File playingFile = new File(Constants.RECORGING_PATH + blockId + Constants.AUDIO_FILE_EXTENSION);
            base.SetDataSource(playingFile.AbsolutePath);
            base.SetVolume(100, 100);
            base.Prepare();
            base.Start();
        }

        public void stop() 
        {
            base.Stop();
        }

        public void OnCompletion(MediaPlayer mp)
        {
            System.Diagnostics.Debug.WriteLine("Finished player calling callback");
            callback();
        }

        public bool OnError(MediaPlayer mp, MediaError what, int extra)
        {
            System.Diagnostics.Debug.WriteLine("MediaPlayer on ErrorListener");
            return true;
        }

        public void reset() 
        {
            base.Reset();
        }
    }
}