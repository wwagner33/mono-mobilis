
using Mobilis.Lib.Model;
using System.Collections.Generic;

namespace Mobilis.Lib.DataServices
{
    class ClassService : RestService<Class>
    {
        public override IEnumerable<Class> parseJSON(string content)
        {
            return null;
        }
    }
}