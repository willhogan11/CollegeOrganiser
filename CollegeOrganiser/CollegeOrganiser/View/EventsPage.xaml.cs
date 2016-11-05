using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using CollegeOrganiser.DataModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;


// #define OFFLINE_SYNC_ENABLED


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238


#if OFFLINE_SYNC_ENABLED
    using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
    using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync
#endif


namespace CollegeOrganiser.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventsPage : Page
    {
        private MobileServiceCollection<Event, Event> events;

#if OFFLINE_SYNC_ENABLED
        private IMobileServiceSyncTable<TodoItem> eventTable = App.MobileService.GetSyncTable<TodoItem>(); // offline sync
        // private IMobileServiceSyncTable<Event> eventTable = App.MobileService.GetSyncTable<Event>(); // offline sync
#else
        private IMobileServiceTable<Event> eventTable = App.MobileService.GetTable<Event>();
        // private IMobileServiceTable<Event> eventTable = App.MobileService.GetTable<Event>();
#endif

        public EventsPage()
        {
            this.InitializeComponent();
        }


        private async Task InsertEvent(Event eventDetails)
        {
            // This code inserts a new TodoItem into the database. After the operation completes
            // and the mobile app backend has assigned an id, the item is added to the CollectionView.
            await eventTable.InsertAsync(eventDetails);
            events.Add(eventDetails);

#if OFFLINE_SYNC_ENABLED
            await App.MobileService.SyncContext.PushAsync(); // offline sync
#endif
        }


        private async Task RefreshEventDetails()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems.
                events = await eventTable
                    .Where(eventDetails => eventDetails.Complete == false)
                    .ToCollectionAsync();

                displayTaskList();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                EventDetails.ItemsSource = events;
                this.Add.IsEnabled = true;
            }
        }



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                        await InitLocalStoreAsync(); // offline sync
#endif
                await RefreshEventDetails();
            }
            catch (Exception)
            {
                // ButtonRefresh_Click(this, null);
            }
        }



        public void displayTaskList()
        {
            string debugValues = "";
            int counter = 0;

            foreach (var evnt in events)
            {
                counter++;
                debugValues = string.Format("No: " + counter + " Module: " + evnt.Module + " EventDetail: " + evnt.EventDetail);
                Debug.WriteLine(debugValues);
            }
        }


        // Method that listens for any any keydown event on the "Add" button 
        private void moduleTitleTextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Add.Focus(FocusState.Programmatic);
            }
        }

        // On page load, loads percentage completed values into the percentageCompleted comboBox
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 5; i <= 100; i += 5)
            {
                percentCompleteComboBox.Items.Add(i);
            }
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var eventDetails = new Event
                {
                    Module = moduleTitleTextBox.Text,
                    EventDetail = eventNameTextBox.Text,
                    PercentComplete = Convert.ToInt32(percentCompleteComboBox.SelectedItem)
                };

                moduleTitleTextBox.Text = "";
                eventNameTextBox.Text = "";
                percentCompleteComboBox.SelectedItem = 0;
                await InsertEvent(eventDetails);
            }
            catch (Exception)
            { }
        }


        private async Task UpdateCheckedEventDetails(Event eventDetails)
        {
            // This code takes a freshly completed TodoItem and updates the database.
            // After the MobileService client responds, the item is removed from the list.
            await eventTable.UpdateAsync(eventDetails);
            events.Remove(eventDetails);
            EventDetails.Focus(Windows.UI.Xaml.FocusState.Unfocused);

#if OFFLINE_SYNC_ENABLED
            await App.MobileService.SyncContext.PushAsync(); // offline sync
#endif
        }


        private async void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Event events = cb.DataContext as Event;
            await UpdateCheckedEventDetails(events);
        }
    }
}