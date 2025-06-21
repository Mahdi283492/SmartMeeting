using System.Text.Json.Serialization;

namespace SmartMeetingAPI.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoomID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public required string Status { get; set; }

        [JsonIgnore]
        public ApplicationUser User { get; set; } = null!;

        [JsonIgnore]
        public Room Room { get; set; } = null!;

        [JsonIgnore]
        public Meeting? Meeting { get; set; }
    }
}