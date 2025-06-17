using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
    public class CreateMinutesDto
    {
        [Required]
        public int MeetingID { get; set; }

        [Required]
        public required string DiscussionPoints { get; set; }

        [Required]
        public required string Decisions { get; set; }
    }
}