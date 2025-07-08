using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.DTOs;
using SmartMeetingAPI.Models;

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<AdminDashboardSummaryDto>> GetSummary()
        {
            var totalRooms = await _context.Rooms.CountAsync();
            var totalBookings = await _context.Bookings.CountAsync();
            var totalUsers = await _context.Users.CountAsync();

            
            var mostUsedRoom = await _context.Bookings
                .GroupBy(b => b.RoomID)
                .OrderByDescending(g => g.Count())
                .Select(g => g.First().Room.Name)
                .FirstOrDefaultAsync();

            var summary = new AdminDashboardSummaryDto
            {
                TotalRooms = totalRooms,
                TotalBookings = totalBookings,
                TotalUsers = totalUsers,
                MostUsedRoom = mostUsedRoom ?? "N/A"
            };

            return Ok(summary);
        }
    }
}
