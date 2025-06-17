using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
    public class CreateAttendeeDto
    {
        [Required]
        public int MeetingID { get; set; }

        [Required]
        public int UserID { get; set; }

        public bool IsOrganizer { get; set; } = false;
    }
}