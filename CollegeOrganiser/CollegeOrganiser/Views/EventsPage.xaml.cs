using CollegeOrganiser.Data;
using System;
using System.Diagnostics;
using System.IO;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace CollegeOrganiser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventsPage : Page
    {
        private string path;
        private SQLite.Net.SQLiteConnection conn;

        public EventsPage()
        {
            this.InitializeComponent();
        }

        // Displays the System back button's Visibility to Visible
        // If there is a page to go back to, then navigate back through the stack
        // If not then the user is at the main first page. 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += MenuPage_BackRequested;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void MenuPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;

            // Navigate back if possible, and if the event has not already been handled...
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        // Populates the Percentage completed dropdown box with values incrementing in 5's
        private void percentComplete_DropDownOpened(object sender, object e)
        {
            //for (int i = 5; i <= 100; i += 5)
            //{
            //    percentCompleteComboBox.Items.Add(i);
            //}     
        }

        public void Add_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            dbConnection();
            var addEvent = conn.Insert(new Event()
            {
                module = moduleTitleTextBox.Text,
                eventTask = eventNameTextBox.Text,
                percentComplete = Convert.ToInt32(percentCompleteComboBox.SelectedItem),
                deadline = datePickerCombo.DataContext
                // deadline = (DateTime)datePickerCombo.DataContext

            });
        }

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            for (int i = 5; i <= 100; i += 5)
            {
                percentCompleteComboBox.Items.Add(i);
            }
        }


        public void dbConnection()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.collegeOrganiser");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Debug.WriteLine("Test");
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
