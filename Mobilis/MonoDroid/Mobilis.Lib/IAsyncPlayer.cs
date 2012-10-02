namespace Mobilis.Lib
{
    public interface IAsyncPlayer
    {
        public void play();
        public void next();
        public void previous();
        public void pause();
        public void stop();
    }
}