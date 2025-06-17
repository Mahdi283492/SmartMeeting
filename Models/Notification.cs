using System.Text.Json.Serialization;

namespace SmartMeetingAPI.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public required string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}