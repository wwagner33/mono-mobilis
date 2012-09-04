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

        public void insertAll(List<Course> courses)
        {
            MobilisDatabase.getDatabase().InsertAll(courses);
        }

        public void insertAll(IEnumerable<Course> courses)
         {
            MobilisDatabase.getDatabase().InsertAll(courses);
         }

        public void delete(Course course)
        {
            MobilisDatabase.getDatabase().Delete<Course>(course);
        }
    }
}