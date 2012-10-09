using Mobilis.Lib.Model;
using System.Collections.Generic;
using System.Collections;
using Mobilis.Lib.Util;
using System;

namespace Mobilis.Lib.DataServices
{
    public class CourseService : RestService<Course>
    {
        public void getCourses(string source,string token,ResultCallback<IEnumerable<Course>> callback) 
        {
            Get(source,token, callback);
        }

        /*
        public override IEnumerable<Course> parseJSON(string content,int method)
        {
            return JSON.parseCourses(content);
        }
         */
        public override IEnumerable<Course> parseJSON(System.Net.WebResponse content, int method)
        {
            return JSON.parseCourses(HttpUtils.WebResponseToString(content));
        }
    }
}