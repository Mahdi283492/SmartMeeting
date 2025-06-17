
using System.Text.Json.Serialization;

namespace SmartMeetingAPI.Models
{
    public class User
    {
        public int ID { get; set; }  
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }

         [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        [JsonIgnore]
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        [JsonIgnore]
        public ICollection<ActionItem> AssignedItems { get; set; } = new List<ActionItem>();

        [JsonIgnore]
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();       
    }
}