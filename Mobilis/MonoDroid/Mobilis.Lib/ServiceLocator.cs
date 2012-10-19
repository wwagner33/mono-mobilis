using TinyMessenger;

namespace Mobilis.Lib
{
    public static class ServiceLocator
    {
        public static IDispatchOnUIThread Dispatcher { get; set; }
        public static IRecordAudio Recorder { get; set; }
        public static ITinyMessengerHub Messenger { get; private set; }

        static ServiceLocator() 
        {
           Messenger = new TinyMessengerHub();
        }
    }
}