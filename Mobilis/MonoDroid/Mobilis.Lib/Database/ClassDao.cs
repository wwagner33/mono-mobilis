using System.Collections.Generic;
using Mobilis.Lib.Model;

namespace Mobilis.Lib.Database
{
    public class ClassDao
    {
        public void insertClasses(IEnumerable<Class> classes) 
        {
            MobilisDatabase.getDatabase().InsertAll(classes);     
        }

        public List<Class> getClassesFromCourse(int courseId) 
        {
            return MobilisDatabase.getDatabase().Query<Class>("select * from Class where courseId = ?", courseId);       
        }

        public bool existClassAtCourse(int courseId)
        {
            List<Class> list = MobilisDatabase.getDatabase().Query<Class>("select * from Class where courseId = ? limit 1", courseId);
            return (list.Count > 0) ? true : false;
        }
    }
}