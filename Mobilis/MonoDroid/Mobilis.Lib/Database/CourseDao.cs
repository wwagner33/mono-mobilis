using Mobilis.Lib.Model;
using System.Collections.Generic;
using SQLite;

namespace Mobilis.Lib.Database
{
    public class CourseDao
        {
        /* ----TODO
            inicializar o database connection no construtor
         */

        public List<Course> getAllCourses()
        {
            var database = new SQLiteConnection(Constants.DATABASE_PATH);
            List<Course> list = new List<Course>();
            var enumerator = database.Table<Course>().GetEnumerator();

            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);  
            }
            return list;
        }

        public void insert(Course course)
        {
            var database = new SQLiteConnection(Constants.DATABASE_PATH);
            database.Insert(course);
        }

        public void insertAll(List<Course> courses)
        {
            var database = new SQLiteConnection(Constants.DATABASE_PATH);
            database.InsertAll(courses);
        }

        public void insertAll(IEnumerable<Course> courses)
         {
             var database = new SQLiteConnection(Constants.DATABASE_PATH);
             database.InsertAll(courses);
         }

        public void delete(Course course)
        {
            var database = new SQLiteConnection(Constants.DATABASE_PATH);
            database.Delete<Course>(course);
        }
    }
}