using System;
using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
    public class CreateActionItemDto
    {
        [Required]
        public int MinutesID { get; set; }

        [Required]
        public int AssignedTo { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public required string Status { get; set; }
    }
}