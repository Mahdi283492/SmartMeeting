using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
    public class CreateMeetingDto
    {
        [Required]        
        public int BookingID { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Agenda { get; set; }
    }
}
