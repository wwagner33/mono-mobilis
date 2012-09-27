using Mobilis.Lib.Model;
using System.Collections.Generic;
using MWC.DL.SQLite;

namespace Mobilis.Lib.Database
{
    public class CourseDao
        {

        public List<Course> getAllCourses()
        {
            List<Course> list = new List<Course>();
            var enumerator = MobilisDatabase.getDatabase().Table<Course>().GetEnumerator();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }
            return list;
        }

        public void insert(Course course)
        {
            MobilisDatabase.getDatabase().Insert(course);
        }

        public void insertAll(IEnumerable<Course> courses)
         {
            MobilisDatabase.getDatabase().CreateCommand("delete from Course", 0).ExecuteNonQuery();
            System.Diagnostics.Debug.WriteLine("courses deleted");
            MobilisDatabase.getDatabase().InsertAll(courses);
         }

        public void delete(Course course)
        {
            MobilisDatabase.getDatabase().Delete<Course>(course);
        }

        public bool existCourses() 
        {
            List<Course> list = MobilisDatabase.getDatabase().Query<Course>("select * from Course limit 1");
            return (list.Count > 0) ? true : false;
        }
    }
}