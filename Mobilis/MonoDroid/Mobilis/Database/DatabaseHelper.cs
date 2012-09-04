
using Android.Database.Sqlite;
using Android.Content;
using MWC.DL.SQLite;
using Mobilis.Lib.Model;

namespace Mobilis.Lib.Database
{
   public class DatabaseHelper : SQLiteOpenHelper
    {
       private const string DATABASE_NAME = "mobilis.sqlite";
       private const int DATABASE_VERSION = 1;

       public DatabaseHelper(Context context) : base(context,DATABASE_NAME,null,DATABASE_VERSION)
       {
       }

       public override void OnCreate(SQLiteDatabase db)
       {
           System.Diagnostics.Debug.WriteLine("OnCreateDatabase");
       }

       public void insertTables()
       {
           var database = new SQLiteConnection("/data/data/Mobilis.Mobilis/databases/mobilis.sqlite");
           database.CreateTable<Course>();
       }

       public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
       {
           throw new System.NotImplementedException();
       }

    }
}