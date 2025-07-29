using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.Models;
using SmartMeetingAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MinutesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MinutesController(AppDbContext context)
        {
            _context = context;
        }

        // Admin only - fetch all minutes
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Minutes>>> GetAll()
            => await _context.Minutes.ToListAsync();

        // Admin only - get raw minute record
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Minutes>> GetById(int id)
        {
            var minutes = await _context.Minutes.FindAsync(id);
            return minutes is null ? NotFound() : minutes;
        }

        // Authenticated users (Admin/User) - get detailed minute view
        [Authorize]
        [HttpGet("{id}/details")]
        public async Task<ActionResult<object>> GetMinutesDetails(int id)
        {
            var minutes = await _context.Minutes
                .Include(m => m.ActionItems)
                    .ThenInclude(ai => ai.User)
                .Include(m => m.Meeting)
                    .ThenInclude(mt => mt.Booking)
                        .ThenInclude(b => b.Room)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (minutes == null)
                return NotFound();

            return Ok(new
            {
                minutes.ID,
                minutes.DiscussionPoints,
                minutes.Decisions,
                MeetingTitle = minutes.Meeting.Title,
                StartTime = minutes.Meeting.Booking.StartTime,
                RoomName = minutes.Meeting.Booking.Room.Name,
                ActionItems = minutes.ActionItems.Select(ai => new
                {
                    ai.ID,
                    ai.Description,
                    ai.DueDate,
                    ai.Status,
                    AssignedUser = ai.User.Name
                })
            });
        }

        // Admin only - create minutes
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Minutes>> Create([FromBody] CreateMinutesDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _context.Meetings.AnyAsync(m => m.ID == input.MeetingID))
                return BadRequest($"No Meeting with ID {input.MeetingID}.");

            var minutes = new Minutes
            {
                MeetingID = input.MeetingID,
                DiscussionPoints = input.DiscussionPoints,
                Decisions = input.Decisions
            };

            _context.Minutes.Add(minutes);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = minutes.ID }, minutes);
        }

        // Admin only - update minutes
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateMinutesDto input)
        {
            var minutes = await _context.Minutes.FindAsync(id);
            if (minutes is null)
                return NotFound();

            if (!await _context.Meetings.AnyAsync(m => m.ID == input.MeetingID))
                return BadRequest($"No Meeting with ID {input.MeetingID}.");

            minutes.MeetingID = input.MeetingID;
            minutes.DiscussionPoints = input.DiscussionPoints;
            minutes.Decisions = input.Decisions;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Admin only - delete minutes
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var minutes = await _context.Minutes.FindAsync(id);
            if (minutes is null)
                return NotFound();

            _context.Minutes.Remove(minutes);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Users - get only minutes from meetings they booked
        [Authorize(Roles = "User,Admin")]
        [HttpGet("mine")]
        public async Task<ActionResult<IEnumerable<object>>> GetMyMinutes()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out int userId))
                return BadRequest("Invalid user ID from token.");

            var myMinutes = await _context.Minutes
                .Include(m => m.Meeting)
                .ThenInclude(mt => mt.Booking)
                .Where(m => m.Meeting.Booking.UserID == userId)
                .Select(m => new
                {
                    m.ID,
                    m.DiscussionPoints,
                    m.Decisions,
                    MeetingTitle = m.Meeting.Title,
                    StartTime = m.Meeting.Booking.StartTime,
                    RoomName = m.Meeting.Booking.Room.Name
                })
                .ToListAsync();

            return Ok(myMinutes);
        }
    }
}
