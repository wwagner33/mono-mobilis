using Mobilis.Lib;
using Android.Media;
using System.Threading;

namespace Mobilis
{
    public class PlayerAdapter : MediaPlayer,IAsyncPlayer
    {
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
        //public void init()

        public override void Start()
        {
            base.Reset();
            ThreadPool.QueueUserWorkItem(state => 
            {
                base.Start();
            });
        }

        public void play() 
        {
            base.Start();
        }

        /*
        public void pause() 
        {
            base.Pause();
        }
        */
        public void stop() 
        {
            base.Stop();
        }
        public void previous() 
        { 
        }
        public void next() 
        { 
        }
    }
}