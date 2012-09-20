

namespace Mobilis.Lib.Util
{
    public class Constants
    {
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
    }
}