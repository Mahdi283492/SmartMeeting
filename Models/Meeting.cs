using System.Text.Json.Serialization;

namespace SmartMeetingAPI.Models
{

    public class Meeting
    {
        public int ID { get; set; }
        public int BookingID { get; set; }
        public required string Title { get; set; }
        public required string Agenda { get; set; }

        [JsonIgnore]
        public Booking Booking { get; set; } = null!;

        [JsonIgnore]
        public Minutes? Minutes { get; set; }

        [JsonIgnore]
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}