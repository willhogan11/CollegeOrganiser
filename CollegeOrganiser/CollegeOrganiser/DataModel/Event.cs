using Newtonsoft.Json;

namespace CollegeOrganiser.DataModel
{
    /* This class maps getter and setters values from what's been entered in XAML (EventPage)...
     * To values columns in the database stored in both Azure and locally using Sqlite */
    public class Event
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "module")]
        public string Module { get; set; }

        [JsonProperty(PropertyName = "eventDetails")]
        public string EventDetail { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public string PriorityState { get; set; }

        [JsonProperty(PropertyName = "percentOfModule")]
        public int PercentOfModule { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }

        [JsonProperty(PropertyName = "deadline")]
        public string Deadline { get; set; }
    }
}