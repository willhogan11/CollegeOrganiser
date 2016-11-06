using Newtonsoft.Json;

namespace CollegeOrganiser.DataModel
{
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


        /* Need to figure out how DatTime is handled by Azure 
           Based on this, have either string or DateTime format */

        // public DateTime deadline { get; set; }
        // public string deadline { get; set; }
    }
}