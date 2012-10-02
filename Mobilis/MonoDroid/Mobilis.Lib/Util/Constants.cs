

namespace Mobilis.Lib.Util
{
    public class Constants
    {
        public static int TOTAL_POSTS_TO_LOAD 
        {
            get { return 20;}
        }

        public static string OLD_POST_DATE = "2000101010241010";

        public static string  CoursesURL 
        {
            get 
            {
                return "curriculum_units/list.json";
            }
        }

        public static string ClassesURL 
        {
            get 
            {
                return "curriculum_units/" + ContextUtil.Instance.Course + "/groups.json";
            }
        }
        public static string DiscussionURL 
        {
            get 
            {
                return "groups/" + ContextUtil.Instance.Class + "/discussions.json";
            }
        }

        public static string NewPostURL(string date) 
        {
            return "discussions/" + ContextUtil.Instance.Discussion + "/posts/news/" + date + "/order/asc/limit/" + TOTAL_POSTS_TO_LOAD + ".json";
        }

        public static string HistoryPostURL(string date)
        {
            return "discussions/" + ContextUtil.Instance.Discussion + "/posts/history/" + date + "/order/desc/limit/" + TOTAL_POSTS_TO_LOAD + ".json";
        }
    }
}