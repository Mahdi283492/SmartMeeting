using System.Text.Json.Serialization;

namespace SmartMeetingAPI.Models
{
public class Room
{
    public int ID { get; set; }  
    public required string Name { get; set; }
    public int Capacity { get; set; }
    public required string Location { get; set; }
    public required string Features { get; set; }

  
      
        [JsonIgnore]                     
        public ICollection<Booking> Bookings { get; set; }
            = new List<Booking>();
}

}