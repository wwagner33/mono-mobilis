#if MONODROID
using Android.OS;
#endif
using System.IO;

namespace Mobilis.Lib.Util
{
    public class Constants
    {
        private const string MOBILIS_BASE_URL = "http://apolo11teste.virtual.ufc.br/solar/";
        public const string AUDIO_FILE_EXTENSION = ".wav";
        public static string RECORGING_PATH
        {
            get
            {
                #if MONODROID
                string path = Environment.ExternalStorageDirectory.AbsolutePath + "/Mobilis/TTS/";
                return path;
                #else
                return "";
                #endif
            }
        }

        public static int TOTAL_POSTS_TO_LOAD 
        {
            get { return 20;}
        }

        public static string OLD_POST_DATE = "2000101010241010";

        public static string tokenURL 
        {
            get 
            {
                return MOBILIS_BASE_URL + "sessions.json";
            }
        }

        public static string  CoursesURL 
        {
            get 
            {
                return MOBILIS_BASE_URL + "curriculum_units/list.json";
            }
        }

        public static string ClassesURL 
        {
            get 
            {
                return MOBILIS_BASE_URL + "curriculum_units/" + ContextUtil.Instance.Course + "/groups.json";
            }
        }
        public static string DiscussionURL 
        {
            get 
            {
                return MOBILIS_BASE_URL + "groups/" + ContextUtil.Instance.Class + "/discussions.json";
            }
        }

        public static string NewPostURL(string date) 
        {
            return MOBILIS_BASE_URL + "discussions/" + ContextUtil.Instance.Discussion + "/posts/news/" + date + "/order/asc/limit/" + TOTAL_POSTS_TO_LOAD + ".json";
        }

        public static string HistoryPostURL(string date)
        {
            return MOBILIS_BASE_URL + "discussions/" + ContextUtil.Instance.Discussion + "/posts/history/" + date + "/order/desc/limit/" + TOTAL_POSTS_TO_LOAD + ".json";
        }

        public static string DeliverPostURL 
        {
            get
            {
                return MOBILIS_BASE_URL + "discussions/" + ContextUtil.Instance.Discussion + "/posts.json";
            }
        }

        public static string DeliverAudioURL(int postId) 
        {
            return MOBILIS_BASE_URL + "posts/" + postId + "/post_files";
        }
    }
}