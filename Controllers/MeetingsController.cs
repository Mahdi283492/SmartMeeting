using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.Models;
using SmartMeetingAPI.DTOs;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace SmartMeetingAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MeetingsController(AppDbContext context)
            => _context = context;

        
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<MeetingSummaryDto>>> GetUpcoming()
        {
                Console.WriteLine($"Authorization header: {Request.Headers["Authorization"]}");

            Console.WriteLine("===== Incoming User Claims =====");
            foreach (var c in User.Claims)
            {
                Console.WriteLine($"Type: {c.Type}, Value: {c.Value}");
            }

            var userIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            var userIdStr = userIdClaim?.Value;
            Console.WriteLine($"Parsed UserId string (manual): {userIdStr}");
Console.WriteLine("User.Identity.IsAuthenticated: " + User.Identity?.IsAuthenticated);

            if (!int.TryParse(userIdStr, out int userId))
                return BadRequest("Invalid user ID in token.");

            var now = DateTime.UtcNow;

            var list = await _context.Meetings
                .Include(m => m.Booking).ThenInclude(b => b.Room)
                .Where(m => m.Booking.UserID == userId && m.Booking.StartTime >= now)
                .OrderBy(m => m.Booking.StartTime)
                .Select(m => new MeetingSummaryDto
                {
                    Id = m.ID,
                    Title = m.Title,
                    Start = m.Booking.StartTime,
                    RoomName = m.Booking.Room.Name
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meeting>>> GetAll()
            => await _context.Meetings.ToListAsync();


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Meeting>> GetById(int id)
        {
            var m = await _context.Meetings.FindAsync(id);
            return m is null ? NotFound() : m;
        }


        [HttpPost]
        public async Task<ActionResult<Meeting>> Create([FromBody] CreateMeetingDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _context.Bookings.AnyAsync(b => b.ID == input.BookingID))
                return BadRequest($"No Booking with ID {input.BookingID}.");
            if (await _context.Meetings.AnyAsync(m => m.BookingID == input.BookingID))
                return Conflict($"Booking {input.BookingID} already has a meeting.");
            var meeting = new Meeting
            {
                BookingID = input.BookingID,
                Title = input.Title,
                Agenda = input.Agenda
            };

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = meeting.ID }, meeting);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateMeetingDto input)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting is null) return NotFound();

            if (!await _context.Bookings.AnyAsync(b => b.ID == input.BookingID))
                return BadRequest($"No Booking with ID {input.BookingID}.");

            meeting.BookingID = input.BookingID;
            meeting.Title = input.Title;
            meeting.Agenda = input.Agenda;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting is null) return NotFound();

            _context.Meetings.Remove(meeting);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
