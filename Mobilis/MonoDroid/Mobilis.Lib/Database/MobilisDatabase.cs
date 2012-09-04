
using Mobilis.Lib.Model;
using System;
using System.IO;
using MWC.DL.SQLite;

namespace Mobilis.Lib.Database
{
    class MobilisDatabase : SQLiteConnection
    {
        protected static MobilisDatabase me = null;
        protected static string dbLocation;

        protected MobilisDatabase (string path) : base (path)
		{
			// create the tables
			CreateTable<Course>();
		}

        static MobilisDatabase() 
        {
            dbLocation = DatabaseFilePath;
            me = new MobilisDatabase(dbLocation);
        }

        public static MobilisDatabase getDatabase()
        {
            dbLocation = DatabaseFilePath;
            me = new MobilisDatabase(dbLocation);
            return me;
        }

        public static string DatabaseFilePath 
        {
            get 
            {
                #if SILVERLIGHT
                var path = "MwcDB.db3";
                #else

                #if MONODROID
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                #else
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
			    // (they don't want non-user-generated data in Documents)
			    string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
		    	string libraryPath = Path.Combine (documentsPath, "../Library/");
                #endif
                var path = Path.Combine(libraryPath, "MwcDB.db3");
                #endif
                return path;  
            }
        }
    }
}