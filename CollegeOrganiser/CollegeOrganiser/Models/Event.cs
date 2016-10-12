using SQLite.Net.Attributes;
using System;
using Windows.UI.Xaml.Controls;

namespace CollegeOrganiser.Data
{
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int eventId { get; set; }
        public string module { get; set; }
        public string eventTask { get; set; }
        public int percentComplete { get; set; }
        public DateTime deadline { get; set; }
        // public string deadline { get; set; }
    }
}