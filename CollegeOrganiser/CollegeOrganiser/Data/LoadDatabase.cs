using System.IO;
using System.Diagnostics;

namespace CollegeOrganiser.Data
{
    public class LoadDatabase
    {
        // Instance Variables that are required to Setup a path and a new sqlite Database connection
        private string path;
        private SQLite.Net.SQLiteConnection conn;

        public LoadDatabase()
        {
            setPath();
            dbConnection();
            conn.CreateTable<Event>();
            conn.CreateTable<Meeting>();
            Debug.WriteLine(conn);
            // closeDBconnection(); // Need to be careful here, connection needs to be closed after transaction is completed and not now. 
        }

        // Set the path to the sqlite database
        public void setPath()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.collegeOrganiser");
        }

        public void dbConnection()
        {
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            #if DEBUG
                Debug.WriteLine(path); // Console write the path, for testing purposes
            #endif
        }

        // Close / Dispose the DB connections for each operation
        public void closeDBconnection()
        {
            conn.Commit();
            conn.Dispose();
            conn.Close();
        }
    }
}
