using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
    public class CreateRoomDto
    {
        [Required]  
        public required string Name { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public required string Location { get; set; }

        
        public required string Features { get; set; }
    }
}
