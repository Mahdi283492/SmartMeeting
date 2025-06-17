using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartMeetingAPI.Models
{
    public class ActionItem
    {
        public int ID { get; set; }
        public int MinutesID { get; set; }
        public int AssignedTo { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
        public required string Status { get; set; }

        [JsonIgnore]
        public Minutes Minutes { get; set; } = null!;

        [JsonIgnore]
        public User User { get; set; } = null!;
    }
}