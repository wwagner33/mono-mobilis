using Mobilis.Lib.Model;
using System.Collections.Generic;
using System.Collections;
using Mobilis.Lib.Util;

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