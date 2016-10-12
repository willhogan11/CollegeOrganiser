using SQLite.Net.Attributes;

namespace CollegeOrganiser.Data
{
    public class Meeting
    {
        [PrimaryKey, AutoIncrement]
        public int meetingId { get; set; }
        public string location { get; set; }
        public string timeDate { get; set; }
        public string meetingWho { get; set; }
        public string topics { get; set; }
    }
}
