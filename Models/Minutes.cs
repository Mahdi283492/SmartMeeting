using System.Text.Json.Serialization;

namespace SmartMeetingAPI.Models
{
    public class Minutes
    {
        public int ID { get; set; }
        public int MeetingID { get; set; }
        public required string DiscussionPoints { get; set; }
        public required string Decisions { get; set; }

        [JsonIgnore]
        public Meeting Meeting { get; set; } = null!;

        [JsonIgnore]
        public ICollection<ActionItem> ActionItems { get; set; } = new List<ActionItem>();
    }
}