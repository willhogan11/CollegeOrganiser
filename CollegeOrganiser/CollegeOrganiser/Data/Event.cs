using SQLite.Net.Attributes;

namespace CollegeOrganiser.Data
{
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string module { get; set; }
        public string eventTask { get; set; }
        public int percentComplete { get; set; }
        public string deadline { get; set; }
    }
}