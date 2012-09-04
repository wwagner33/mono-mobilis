using Mobilis.Lib.Model;
using System.Collections.Generic;
using System.Collections;
#if MONODROID || MONOTOUCH
using Mobilis.Lib.Util;
#endif
#if SILVERLIGHT
using Mobilis.Lib.WP7Util;
#endif

namespace Mobilis.Lib.DataServices
{
    public class CourseService : RestService<Course>
    {
        public void getCourses(string source,string token,ResultCallback<IEnumerable<Course>> callback) 
        {
            Get(source,token, callback);
        }

        public override IEnumerable<Course> parseJSON(string content)
        {
            return JSON.parseCourses(content);
        }
    }
}