using System.Json;
using System.Collections.Generic;
using Mobilis.Lib.Model;
namespace Mobilis.Lib.Util
{
    public class JSON
    {
        public JSON() { }

        public static string generateLoginObject(string login,string password) 
        {
            JsonObject innerObject = new JsonObject();
            innerObject.Add("login", login);
            innerObject.Add("password", password);
            JsonObject outerObject = new JsonObject();
            outerObject.Add("user", innerObject);
            return outerObject.ToString();
        }

        public static IEnumerable<string> parseToken(string content) 
        {
            var json = JsonValue.Parse(content);
            var session = json["session"];
            string token = session["auth_token"];
            yield return token;
        }

        public static IEnumerable<Course> parseCourses(string content)
        {
            System.Diagnostics.Debug.WriteLine("Content novo = " + content);
            
            List<Course> parsedValues = new List<Course>();
            var data = JsonValue.Parse(content);
            System.Diagnostics.Debug.WriteLine("Json value size" + data.Count);

            for (int i = 0; i < data.Count; i++)
            {
                var innerObject = data[i];
                Course course = new Course();
                course._id = innerObject["id"];
                course.curriculumUnitTypeId = innerObject["curriculum_unit_type_id"];
                course.allocationTagId = innerObject["allocation_tag_id"];
                course.name = innerObject["name"];
                parsedValues.Add(course);
            }
            return parsedValues;
        }
    }
}