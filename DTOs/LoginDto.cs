using System.ComponentModel.DataAnnotations;
namespace SmartMeetingAPI.DTOs
{
  public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
}
