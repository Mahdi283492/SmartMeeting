using System;
using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
    public class CreateNotificationDto
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public required string Message { get; set; }

        public bool IsRead { get; set; } = false;
    }
}