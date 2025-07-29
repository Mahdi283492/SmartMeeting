using System.ComponentModel.DataAnnotations;

namespace SmartMeetingAPI.DTOs
{
  public class UpdateUserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}

}
