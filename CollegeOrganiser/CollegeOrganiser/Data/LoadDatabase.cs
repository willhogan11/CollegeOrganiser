using System.IO;
using System.Diagnostics;

namespace CollegeOrganiser.Data
{
    public class LoadDatabase
    {
        //// Instance Variables that are required to Setup a path and a new sqlite Database connection
        //private string path;
        //private SQLite.Net.SQLiteConnection conn;

        //public LoadDatabase()
        //{
        //    dbConnection();
        //    conn.CreateTable<Event>();
        //    conn.CreateTable<Meeting>();
            
        //    #if DEBUG
        //        Debug.WriteLine(conn);
        //    #endif
            
        //    closeDBconnection(); // Need to be careful here, connection needs to be closed after transaction is completed and not now. 
        //}


        //public void dbConnection()
        //{
        //    path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.collegeOrganiser");
        //    conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
        //    Debug.WriteLine("Test");
        //}


        //// Close / Dispose the DB connections for each operation
        //public void closeDBconnection()
        //{
        //    conn.Commit();
        //    conn.Dispose();
        //    conn.Close();
        //}
    }
}
