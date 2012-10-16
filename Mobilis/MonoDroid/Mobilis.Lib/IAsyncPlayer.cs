namespace Mobilis.Lib
{
    public interface IAsyncPlayer
    {
       void play(int blockId,Mobilis.Lib.TTSManager.BlockFinishedPlaying callback);
       void stop();
       void reset();
    }
}