using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.Models;
using SmartMeetingAPI.DTOs;

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

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Minutes>>> GetAll()
            => await _context.Minutes.ToListAsync();

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Minutes>> GetById(int id)
        {
            var minutes = await _context.Minutes.FindAsync(id);
            return minutes is null ? NotFound() : minutes;
        }

        
        [HttpPost]
        public async Task<ActionResult<Minutes>> Create([FromBody] CreateMinutesDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

           
            if (!await _context.Meetings.AnyAsync(m => m.ID == input.MeetingID))
                return BadRequest($"No Meeting with ID {input.MeetingID}.");

            var minutes = new Minutes
            {
                MeetingID        = input.MeetingID,
                DiscussionPoints = input.DiscussionPoints,
                Decisions        = input.Decisions
            };

            _context.Minutes.Add(minutes);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = minutes.ID }, minutes);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateMinutesDto input)
        {
            var minutes = await _context.Minutes.FindAsync(id);
            if (minutes is null)
                return NotFound();

            if (!await _context.Meetings.AnyAsync(m => m.ID == input.MeetingID))
                return BadRequest($"No Meeting with ID {input.MeetingID}.");

            minutes.MeetingID        = input.MeetingID;
            minutes.DiscussionPoints = input.DiscussionPoints;
            minutes.Decisions        = input.Decisions;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        
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
    }
}