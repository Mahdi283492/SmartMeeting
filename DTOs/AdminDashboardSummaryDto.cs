
namespace SmartMeetingAPI.DTOs
{
    public class AdminDashboardSummaryDto
    {
        public int TotalRooms { get; set; }
        public int TotalBookings { get; set; }
        public int TotalUsers { get; set; }
        public string? MostUsedRoom { get; set; }
    }
}
