using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.Models;

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
public async Task<IActionResult> GetSummary()
{
    var now = DateTime.UtcNow;

    var bookingsPerMonth = await _context.Bookings
        .Where(b => b.StartTime >= now.AddMonths(-6))
        .GroupBy(b => new { b.StartTime.Year, b.StartTime.Month })
        .Select(g => new
        {
            Year = g.Key.Year,
            Month = g.Key.Month,
            Count = g.Count()
        })
        .OrderBy(g => g.Year).ThenBy(g => g.Month)
        .ToListAsync();

    var mostUsedRoom = await _context.Bookings
        .GroupBy(b => b.RoomID)
        .Select(g => new { RoomID = g.Key, Count = g.Count() })
        .OrderByDescending(g => g.Count)
        .FirstOrDefaultAsync();

    var mostUsedRoomName = mostUsedRoom != null
        ? (await _context.Rooms.FindAsync(mostUsedRoom.RoomID))?.Name ?? "Unknown"
        : "N/A";

    var totalMeetings = await _context.Meetings.CountAsync();
    var totalRooms = await _context.Rooms.CountAsync();
    var totalBookings = await _context.Bookings.CountAsync();
    var totalUsers = await _context.Users.CountAsync();

    return Ok(new
    {
        TotalMeetings = totalMeetings,
        TotalRooms = totalRooms,
        TotalBookings = totalBookings,
        TotalUsers = totalUsers,
        MostUsedRoom = mostUsedRoomName,
        BookingsPerMonth = bookingsPerMonth
    });
}

    }
}
