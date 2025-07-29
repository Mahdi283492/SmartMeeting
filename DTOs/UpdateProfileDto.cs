using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
    public class UpdateProfileDto
    {
        [Required]
        public string Name { get; set; } = default!;

        public string? NewPassword { get; set; }
    }
}
