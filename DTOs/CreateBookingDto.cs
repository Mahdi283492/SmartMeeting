using System;
using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
    public class CreateBookingDto
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public int RoomID { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public required string Status { get; set; }
    }
}
