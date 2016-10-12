using CollegeOrganiser.Data;
using System.Diagnostics;
using System.IO;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CollegeOrganiser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string path;
        private SQLite.Net.SQLiteConnection conn;

        public MainPage()
        {
            this.InitializeComponent();
            dbConnection();
            conn.CreateTable<Event>();
            conn.CreateTable<Meeting>();
        }

        public void dbConnection()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.collegeOrganiser");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Debug.WriteLine("Test");
        }

        // Closes this MainPage when the below function is called to open the MenuPage
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        // Navigates to the Menu Page when Tapped
        private void Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(MenuPage));
        }

        // Triggers the Sqlite Database and Table creation, defined in the loadDatabase.cs class in the Data Folder
        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // dbConnection();
        }
    }
}
