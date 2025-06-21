using System.Text.Json.Serialization;

namespace SmartMeetingAPI.Models
{
    public class Attendee
    {
        public int MeetingID { get; set; }
        public int UserID { get; set; }
        public bool IsOrganizer { get; set; }

        [JsonIgnore]
        public Meeting Meeting { get; set; } = null!;

        [JsonIgnore]
        public ApplicationUser User { get; set; } = null!;
    }
}