using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
    public class CreateUserDto
    {
        [Required]        
        public required string Name { get; set; }

        [Required]
        [EmailAddress]    
        public required string Email { get; set; }

        [Required]
        public required string PasswordHash { get; set; }

        [Required]
        public required string Role { get; set; }
    }
}
