using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.Models;
using SmartMeetingAPI.DTOs;

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MeetingsController(AppDbContext context)
            => _context = context;

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Meeting>>> GetAll()
            => await _context.Meetings.ToListAsync();

        
        [HttpGet("{id}")]
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

            var meeting = new Meeting
            {
                BookingID = input.BookingID,
                Title     = input.Title,
                Agenda    = input.Agenda
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
            meeting.Title     = input.Title;
            meeting.Agenda    = input.Agenda;

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
