﻿
// #define OFFLINE_SYNC_ENABLED

using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using CollegeOrganiser.DataModel;
using Windows.UI.Popups;
using static CollegeOrganiser.DataModel.EnumData;

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
        private IMobileServiceSyncTable<Event> eventTable = App.MobileService.GetSyncTable<Event>(); // offline sync
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

        // For debug purposes: Prints to console all details of each Event being sent to database.
        public void displayEventList()
        {
            string debugValues = "";
            int urgCount = 0, normCount = 0, lowCnt = 0, noCount = 0, totalCnt = 0;

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
                totalCnt++;

                urgentCount.Text = urgCount.ToString();
                normalCount.Text = normCount.ToString();
                lowCount.Text = lowCnt.ToString();
                noPCount.Text = noCount.ToString();
                totalCount.Text = totalCnt.ToString();


#if DEBUG
                debugValues = string.Format("No: " + totalCnt + " Module: " + evnt.Module + " EventDetail: " + evnt.EventDetail + " % of Module: " + evnt.PercentOfModule + " Priority Level: " + evnt.PriorityState + " Deadline:" + evnt.Deadline);
                Debug.WriteLine(debugValues);
                Debug.WriteLine("Urgent Count: " + urgCount);
                Debug.WriteLine("Normal Count: " + normCount);
                Debug.WriteLine("Low Count: " + lowCnt);
                Debug.WriteLine("No Priority Count: " + noCount);
                Debug.WriteLine("Total Count: " + totalCnt);
#endif
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

        // A method that loads all the data associated with comboBoxes
        public void loadData()
        {
            foreach (DayEnum day in Enum.GetValues(typeof(DayEnum)))
            {
                dayComboBox.Items.Add(day);
            }

            foreach (MonthEnum month in Enum.GetValues(typeof(MonthEnum)))
            {
                monthComboBox.Items.Add(month);
            }

            foreach (PriorityEnum p in Enum.GetValues(typeof(PriorityEnum)))
            {
                priorityLevelComboBox.Items.Add(p);
            }

            for (int i = 1; i <= 31; i++)
            {
                dayNumComboBox.Items.Add(i);
            }

            for (int i = 0; i <= 100; i += 5)
            {
                percentOfModuleComboBox.Items.Add(i);
            }

            yearComboBox.Items.Add(2016);
            yearComboBox.Items.Add(2017);
            yearComboBox.Items.Add(2018);
            yearComboBox.Items.Add(2019);
            yearComboBox.Items.Add(2020);
            yearComboBox.Items.Add(2021);
            yearComboBox.Items.Add(2022);
        }


        // On page load, loads percentage completed values into the percentageCompleted comboBox
        // and Adds each enum priority instance into the Priority comboBox
        // and also Refreshes the Event Details Listview with Database values that currently exist on Azure
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();
            

#if OFFLINE_SYNC_ENABLED
            await InitLocalStoreAsync(); // offline sync
#endif
            await RefreshEventDetails();
        }


        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
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
                moduleTitleTextBox.Text = "";
                eventNameTextBox.Text = "";
                percentOfModuleComboBox.SelectedItem = 0;

                await InsertEvent(eventDetails);
                displayEventList();
            }
            catch (Exception)
            { }
        }


        private async Task UpdateCheckedEventDetails(Event eventDetails)
        {
            // This code takes a freshly completed Event Detail and updates the database.
            // After the MobileService client responds, the item is removed from the list.
            await eventTable.UpdateAsync(eventDetails);
            events.Remove(eventDetails);
            displayEventList();
            EventDetails.Focus(Windows.UI.Xaml.FocusState.Unfocused);

            if (events.Count == 0)
            {
                urgentCount.Text = "0";
                lowCount.Text = "0";
                normalCount.Text = "0";
                noPCount.Text = "0";
                totalCount.Text = "0";
            }    

#if OFFLINE_SYNC_ENABLED
            await App.MobileService.SyncContext.PushAsync(); // offline sync
#endif
        }


        private async void CheckBoxComplete_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Event events = cb.DataContext as Event;
            displayEventList();
            await UpdateCheckedEventDetails(events);  
        }


        private async void Sync_Click(object sender, RoutedEventArgs e)
        {
            displayEventList();
            await RefreshEventDetails();

#if OFFLINE_SYNC_ENABLED
            await SyncAsync(); // offline sync
#endif
        }


        #region Offline sync
#if OFFLINE_SYNC_ENABLED
        private async Task InitLocalStoreAsync()
        {
            if (!App.MobileService.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("collegeOrganiser.db");
                store.DefineTable<Event>();
                await App.MobileService.SyncContext.InitializeAsync(store);
            }

            await SyncAsync();
        }

        private async Task SyncAsync()
        {
            await App.MobileService.SyncContext.PushAsync();
            await eventTable.PullAsync("eventDetails", eventTable.CreateQuery());
        }
#endif
        #endregion
    }
}