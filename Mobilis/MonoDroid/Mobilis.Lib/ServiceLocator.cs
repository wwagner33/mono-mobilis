namespace Mobilis.Lib
{
    public static class ServiceLocator
    {
        public static IDispatchOnUIThread Dispatcher { get; set; }
        public static IRecordAudio Recorder { get; set; }
    }
}