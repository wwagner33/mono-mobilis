using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Mobilis.Lib.Model;
using System;
using Mobilis.Lib.Database;
namespace Mobilis.Lib.Util
{
    /*Classe responsável pela Serialização de JSON*/

    public class JSON
    {
        public static string generateLoginObject(string login, string password)
        {
            JObject innerObject = new JObject();
            innerObject.Add("login", login);
            innerObject.Add("password", password);
            JObject outerObject = new JObject();
            outerObject.Add("user", innerObject);
            return outerObject.ToString();
        }

        public static IEnumerable<string> parseToken(string content)
        {
            JObject jObject = JObject.Parse(content);
            var innerObject = jObject.SelectToken("session");
            string token = (string)innerObject.SelectToken("auth_token");
            yield return token;
        }

        public static IEnumerable<Course> parseCourses(string content)
        {
            List<Course> parsedValues = new List<Course>();
            JArray jArray = JArray.Parse(content);
            System.Diagnostics.Debug.WriteLine("JArray size = " + jArray.Count);
            for (int i = 0; i < jArray.Count; i++)
            {
                JObject innerObject = (JObject)jArray[i];
                Course course = new Course();
                course.name = (string)innerObject.SelectToken("name");
                course._id = (int)innerObject.SelectToken("id");
                parsedValues.Add(course);
            }
            return parsedValues;
        }

        public static IEnumerable<Class> parseClasses(string content) 
        {
            System.Diagnostics.Debug.WriteLine(content);
            List<Class> parsedValues = new List<Class>();
            JArray jArray = JArray.Parse(content);
            System.Diagnostics.Debug.WriteLine("JArray size = " + jArray.Count);
            for (int i = 0; i < jArray.Count; i++)
            {
                JObject innerObject = (JObject)jArray[i];
                Class mClass = new Class();
                mClass.code = (string)innerObject.SelectToken("code");
                mClass._id = (int)innerObject.SelectToken("id");
                mClass.courseId = ContextUtil.Instance.Course;
                parsedValues.Add(mClass);
            }
            return parsedValues;
        }

        public static IEnumerable<Discussion> parseDiscussion(string content) 
        {
            List<Discussion> parsedValues = new List<Discussion>();
            JArray jArray = JArray.Parse(content);
            System.Diagnostics.Debug.WriteLine("JArray size = " + jArray.Count);
            for (int i = 0; i < jArray.Count; i++)
            {
                JObject innerObject = (JObject)jArray[i];
                Discussion discussion = new Discussion();
                discussion._id = (int)innerObject.SelectToken("id");
                discussion.name = (string)innerObject.SelectToken("name");
                discussion.description = (string)innerObject.SelectToken("description");
                discussion.status = Convert.ToInt32((string)innerObject.SelectToken("status"));
                discussion.classId = ContextUtil.Instance.Class;
                discussion.lastPostDate = (string)innerObject.SelectToken("last_post_date");
                discussion.startDate = (string)innerObject.SelectToken("start_date");
                discussion.endDate = (string)innerObject.SelectToken("end_date");
                 
                parsedValues.Add(discussion);
            }
            return parsedValues;
        }

        public static IEnumerable<Post> parsePosts(string content) 
        {
            System.Diagnostics.Debug.WriteLine(content);
            List<Post> parsedValues = new List<Post>();
            JArray jArray = JArray.Parse(content);
            System.Diagnostics.Debug.WriteLine("JArray size = " + jArray.Count);

            
            JObject nextAndPreviousPosts = (JObject)jArray[0];
            int postsBefore = (int)nextAndPreviousPosts.SelectToken("before");
            int postsAfter = (int)nextAndPreviousPosts.SelectToken("after");
            ContextUtil.Instance.postsAfter = postsAfter;
            ContextUtil.Instance.postsBefore = postsBefore;

            for (int i = 1; i < jArray.Count; i++)
            {
                JObject innerObject = (JObject)jArray[i];
                Post post = new Post();
                post._id = (int)innerObject.SelectToken("id");
                try
                {
                    post.parentId = (int)innerObject.SelectToken("parent_id");
                }
                catch (Exception e) 
                {
                    System.Diagnostics.Debug.WriteLine("Erro no parentid");
                    post.parentId = 0;
                }

                post.discussionId = ContextUtil.Instance.Discussion;
                System.Diagnostics.Debug.WriteLine("ContextUtil mod");
                post.userId = (int)innerObject.SelectToken("user_id");
                System.Diagnostics.Debug.WriteLine("user_id OK");
                post.content = HttpUtils.Strip((string)innerObject.SelectToken("content"));
                System.Diagnostics.Debug.WriteLine("content OK");
                post.userName = (string)innerObject.SelectToken("user_nick");
                System.Diagnostics.Debug.WriteLine("user_nick OK");
                //post.updatedAt = (string)innerObject.SelectToken("updated_at");
                post.updatedAt = Convert.ToString(innerObject.SelectToken("updated_at"));
                System.Diagnostics.Debug.WriteLine("Updated At OK");
                post.level = (int)innerObject.SelectToken("level");
                System.Diagnostics.Debug.WriteLine("Level OK");
                parsedValues.Add(post);
            }
            return parsedValues;
        }

        public static string createJSONPostEntity(string content,int parentId) 
        {
            JObject innerObject = new JObject();
            innerObject.Add("content", content);
            if (parentId == 0) 
            {
                innerObject.Add("parent_id", string.Empty);
            }
            else
            {
                innerObject.Add("parent_id", parentId);
            }
            JObject entity = new JObject();
            entity.Add("discussion_post", innerObject);
            return entity.ToString();
        }

        public static int parsePostDelivered(string content) 
        {
            JObject json = JObject.Parse(content);
            return (int)json.SelectToken("post_id");
        }
    }
}
