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
    }
}