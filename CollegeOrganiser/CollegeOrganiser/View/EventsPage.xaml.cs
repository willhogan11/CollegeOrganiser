
#define OFFLINE_SYNC_ENABLED

using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using CollegeOrganiser.DataModel;
using Windows.UI.Popups;
using static CollegeOrganiser.DataModel.EnumData;
using Windows.UI.Xaml.Navigation;

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
        // Declares an Instance of the remote (or below local) table, with associated Key Value pair format
        private MobileServiceCollection<Event, Event> events;

#if OFFLINE_SYNC_ENABLED
        private IMobileServiceSyncTable<Event> eventTable = App.MobileService.GetSyncTable<Event>(); // offline sync
#else
        private IMobileServiceTable<Event> eventTable = App.MobileService.GetTable<Event>();
#endif

        public EventsPage()
        {
            this.InitializeComponent();
        }

        // This code inserts a new event into the database. After the operation completes
        // and the mobile app backend has assigned an id, the item is added to the CollectionView.
        private async Task InsertEvent(Event eventDetails)
        {
            await eventTable.InsertAsync(eventDetails);
            events.Add(eventDetails);

#if OFFLINE_SYNC_ENABLED
            await App.MobileService.SyncContext.PushAsync(); // offline sync
#endif
        }


        // This code refreshes the entries in the list view by querying the TodoItems table.
        // The query excludes completed eventDetails.
        private async Task RefreshEventDetails()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                events = await eventTable
                    .Where(eventDetails => eventDetails.Complete == false)
                    .ToCollectionAsync();

                displayEventList(); 
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
            }
        }


        /* Function used for to do 2 things;
           * 1 Check to see what was being processed (Prints to console all details of each Event being sent to database). Very useful for debugging
           * 2 To store the count for each priority level assigned to each event.  
           */
        public void displayEventList()
        {
            string debugValues = "";
            int urgCount = 0, normCount = 0, lowCnt = 0, noCount = 0, totalCnt = 0; // Declare count variables

            // For each event in the list, chech which priority level was assigned to it and increment the count for that particular level
            foreach (var evnt in events)
            {
                if (evnt.PriorityState.Equals("URGENT"))
                    urgCount++;
                if (evnt.PriorityState.Equals("NORMAL"))
                    normCount++;
                if (evnt.PriorityState.Equals("LOW"))
                    lowCnt++;
                if (evnt.PriorityState.Equals("None"))
                    noCount++;
                totalCnt++; // Counts the total number of events

                // Maps the counts for each priority level with specific textBlock on this page
                urgentCount.Text = urgCount.ToString();
                normalCount.Text = normCount.ToString();
                lowCount.Text = lowCnt.ToString();
                noPCount.Text = noCount.ToString();
                totalCount.Text = totalCnt.ToString();

                // For debug purposes... Will be ignored at deployment level
#if DEBUG
                debugValues = string.Format("No: " + totalCnt + " Module: " + evnt.Module + " EventDetail: " + evnt.EventDetail + " % of Module: " +
                                                     evnt.PercentOfModule + " Priority Level: " + evnt.PriorityState + " Deadline:" + evnt.Deadline);
                Debug.WriteLine(debugValues); //  Display the above String
                Debug.WriteLine("Urgent Count: " + urgCount);
                Debug.WriteLine("Normal Count: " + normCount);
                Debug.WriteLine("Low Count: " + lowCnt);
                Debug.WriteLine("No Priority Count: " + noCount);
                Debug.WriteLine("Total Count: " + totalCnt);
#endif
            }
        }


        // Method that listens for any keydown event on the "Add" button 
        private void moduleTitleTextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Add.Focus(FocusState.Programmatic);
            }
        }


        // A method that loads all the data into the various associated comboBoxes on this page
        public void loadData()
        {
            // Populates the day combobox with values from the Enum class
            foreach (DayEnum day in Enum.GetValues(typeof(DayEnum)))
            {
                dayComboBox.Items.Add(day);
            }

            // Populates the month combobox with values from the Enum class
            foreach (MonthEnum month in Enum.GetValues(typeof(MonthEnum)))
            {
                monthComboBox.Items.Add(month);
            }

            // Populates the priority combobox with values from the Enum class
            foreach (PriorityEnum p in Enum.GetValues(typeof(PriorityEnum)))
            {
                priorityLevelComboBox.Items.Add(p);
            }

            // Fills the date combobox with values
            for (int i = 1; i <= 31; i++)
            {
                dayNumComboBox.Items.Add(i);
            }

            // Fills the percent of module combobox with values
            for (int i = 0; i <= 100; i += 5)
            {
                percentOfModuleComboBox.Items.Add(i);
            }

            // Fills the year combobox with values
            for (int i = 0; i <= 50; i++)
            {
                yearComboBox.Items.Add(2010 + i);
            }
        }


        // On page load, loads percentage completed values into the percentageCompleted comboBox
        // and Adds each enum priority instance into the Priority comboBox
        // and also Refreshes the Event Details Listview with Database values that currently exist on Azure
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadData(); //  Calls the declared function above

#if OFFLINE_SYNC_ENABLED
            await InitLocalStoreAsync(); // offline sync
#endif
            await RefreshEventDetails(); //  Refresh the list of events in the either database and display them in the XAML listView
        }



        /*
         * Function that sends a successfully entered event to either database
         * Firstly, check that each required field has been filled, if not displays the approriate error message on screen to the user. 
         * At this point they won't be able to proceed until the specified criteria have been met. 
         */
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check that all fields have been filled, if not display appropriate message
                if (!IsPresent(moduleTitleTextBox) || !IsPresent(eventNameTextBox) || !ComboValuePresent(dayComboBox) || !ComboValuePresent(priorityLevelComboBox) ||
                    !ComboValuePresent(dayNumComboBox) || !ComboValuePresent(monthComboBox) || !ComboValuePresent(yearComboBox) )
                {
                    MessageDialog message = new MessageDialog("Required Fields missing!");
                    await message.ShowAsync();
                    requiredField.Visibility = Visibility.Visible;
                }
                else
                {
                    /* Create a new Instance of the Event class and map each instance variable 
                       to the relevant TextBox / ComboBox  on this page */
                    var eventDetails = new Event
                    {
                        Module = moduleTitleTextBox.Text,
                        EventDetail = eventNameTextBox.Text,
                        PercentOfModule = Convert.ToInt32(percentOfModuleComboBox.SelectedItem),
                        PriorityState = priorityLevelComboBox.SelectedItem.ToString(),
                        Deadline = dayComboBox.SelectedItem.ToString() + " "
                                 + dayNumComboBox.SelectedItem + " "
                                 + monthComboBox.SelectedItem.ToString() + " "
                                 + yearComboBox.SelectedItem.ToString()
                    };

                    // Reset the TextBoxes
                    moduleTitleTextBox.Text = "";
                    eventNameTextBox.Text = "";
                    percentOfModuleComboBox.SelectedItem = 0;

                    // Start a new thread that inserts the details, 
                    // Increment the count for each prioirty level, collapse the textbox..
                    // ... that holds the error message should some fields be missing values
                    await InsertEvent(eventDetails);
                    displayEventList();
                    requiredField.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception){ }
        }


        // This code takes a freshly completed Event Detail and updates the database.
        // After the MobileService client responds, the item is removed from the list (but not deleted in either database, just marked as complete)
        private async Task UpdateCheckedEventDetails(Event eventDetails)
        {
            // Start a new Thread that updates the eventTable in the database, 
            // ..and remove from the actual list declared at the top of this page
            await eventTable.UpdateAsync(eventDetails);
            events.Remove(eventDetails);
                        
            displayEventList();
            EventDetails.Focus(Windows.UI.Xaml.FocusState.Unfocused);

            // Zero the counters, if there are no more events left
            if (events.Count == 0)
            {
                urgentCount.Text = "0";
                lowCount.Text = "0";
                normalCount.Text = "0";
                noPCount.Text = "0";
                totalCount.Text = "0";

                // Inform the user that they have completed all events 
                MessageDialog message = new MessageDialog("All Events Completed!");
                await message.ShowAsync();
            }    

#if OFFLINE_SYNC_ENABLED
            await App.MobileService.SyncContext.PushAsync(); // offline sync
#endif
        }
         

        // Function that checks if en event checkbox has been ticked, removes from the list and marks as completed, 
        // which stops any completed events from being returned back to the user during a refresh.  
        private async void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = (CheckBox)sender;
                Event events = cb.DataContext as Event;
                displayEventList();
                await UpdateCheckedEventDetails(events);
            }
            catch (Exception){}
        }


        // This function performs a sync with the Azure cloud database and retrieves whats there
        private async void Sync_Click(object sender, RoutedEventArgs e)
        {
            displayEventList();
            await RefreshEventDetails();

#if OFFLINE_SYNC_ENABLED
            await SyncAsync(); // offline sync
#endif
        }



        // Checks to Ensure that all TextBox fields have data in them, returns false if not
        // Using the shortend Ternary operators conditional statement
        // Ref: https://msdn.microsoft.com/en-us/library/ty67wk28.aspx

        private bool IsPresent(TextBox textBox)
        {
            return (textBox.Text == "") ? false : true;
        }





        // Checks to Ensure that all ComboBoxes have data selected, returns false if not
        // Using the shortend Ternary operators conditional statement
        // Ref: https://msdn.microsoft.com/en-us/library/ty67wk28.aspx

        private bool ComboValuePresent(ComboBox comboBox)
        {
            return (comboBox.SelectedItem == null) ? false : true;
        }




        // Having an issue installing sqlite from the nuget package manager, working the issue.
        // Having an issue installing sqlite from the nuget package manager, working the issue.
        // Stored the data offline using SQLite

        #region Offline sync
#if OFFLINE_SYNC_ENABLED
        private async Task InitLocalStoreAsync()
        {
            try
            {
                if (!App.MobileService.SyncContext.IsInitialized)
                {
                    var store = new MobileServiceSQLiteStore("collegeOrganiser.db");
                    store.DefineTable<Event>();
                    await App.MobileService.SyncContext.InitializeAsync(store);
                }

                await SyncAsync();
            }
            catch (Exception){}
        }



        private async Task SyncAsync()
        {
            try
            {
                await App.MobileService.SyncContext.PushAsync();
                await eventTable.PullAsync("eventDetails", eventTable.CreateQuery());
            }
            catch (Exception)
            {
                MessageDialog message = new MessageDialog("You Can't Sync events to the Cloud at this time.\nEvents Will be Stored locally");
                await message.ShowAsync();
            }
        }
#endif
        #endregion
    }
}