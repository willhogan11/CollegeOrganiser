using System.IO;
using System.Diagnostics;


namespace CollegeOrganiser.Data
{
    public class LoadDatabase
    {
        // Instance Variables that are required to Setup a path and a new sqlite Database connection
        private string path;
        // private SQLite.Net.SQLiteConnection conn;

        //public LoadDatabase()
        //{
        //    path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.collegeOrganiser");
        //}

        public void dbConnection()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.collegeOrganiser");
            #if DEBUG
                Debug.WriteLine(path);
            #endif
            // conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
        }
    }
}
