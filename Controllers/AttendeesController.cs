using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.Models;
using SmartMeetingAPI.DTOs;

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AttendeesController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendee>>> GetAll()
            => await _context.Attendees.ToListAsync();

        
        [HttpGet("{meetingId}/{userId}")]
        public async Task<ActionResult<Attendee>> GetById(int meetingId, int userId)
        {
            var attendee = await _context.Attendees
                .FindAsync(meetingId, userId);
            return attendee is null ? NotFound() : attendee;
        }

        
        [HttpPost]
        public async Task<ActionResult<Attendee>> Create([FromBody] CreateAttendeeDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _context.Meetings.AnyAsync(m => m.ID == input.MeetingID))
                return BadRequest($"No Meeting with ID {input.MeetingID}.");
            if (!await _context.Users.AnyAsync(u => u.ID == input.UserID))
                return BadRequest($"No User with ID {input.UserID}.");

            var attendee = new Attendee
            {
                MeetingID = input.MeetingID,
                UserID    = input.UserID,
                IsOrganizer = input.IsOrganizer
            };

            _context.Attendees.Add(attendee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { meetingId = attendee.MeetingID, userId = attendee.UserID },
                attendee);
        }

        
        [HttpPut("{meetingId}/{userId}")]
        public async Task<IActionResult> Update(int meetingId, int userId, [FromBody] CreateAttendeeDto input)
        {
            var attendee = await _context.Attendees.FindAsync(meetingId, userId);
            if (attendee is null)
                return NotFound();

            attendee.IsOrganizer = input.IsOrganizer;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpDelete("{meetingId}/{userId}")]
        public async Task<IActionResult> Delete(int meetingId, int userId)
        {
            var attendee = await _context.Attendees.FindAsync(meetingId, userId);
            if (attendee is null)
                return NotFound();

            _context.Attendees.Remove(attendee);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}