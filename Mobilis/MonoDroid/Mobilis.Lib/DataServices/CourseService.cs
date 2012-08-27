
using Mobilis.Lib.Model;
using System.Collections.Generic;
using System.Collections;
using System.Json;
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
            // System.Json
            List<Course> parsedValues = new List<Course>();
            var data = JsonValue.Parse(content);
            System.Diagnostics.Debug.WriteLine("Json value size" + data.Count);

            for (int i = 0; i < data.Count; i++)
            {
                var innerObject = data[i];
                Course course = new Course();
                course._id = innerObject["id"];
                course.offerId = innerObject["offer_id"];
                course.groupId = innerObject["group_id"];
                course.semester = innerObject["semester"];
                course.allocationTagId = innerObject["allocation_tag_id"];
                course.name = innerObject["name"];
                parsedValues.Add(course);
            }
                return parsedValues;
        }
    }
}