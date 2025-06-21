using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.Models;
using SmartMeetingAPI.DTOs;

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAll()
        {
            return await _context.Bookings.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetById(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            return booking is null ? NotFound() : booking;
        }


        [HttpPost]
        public async Task<ActionResult<Booking>> Create([FromBody] CreateBookingDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await _context.Users.AnyAsync(u => u.Id == input.UserID))
                return BadRequest($"No User with ID {input.UserID}.");

            if (!await _context.Rooms.AnyAsync(r => r.ID == input.RoomID))
                return BadRequest($"No Room with ID {input.RoomID}.");
            var booking = new Booking
            {
                UserID = input.UserID,
                RoomID = input.RoomID,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                Status = input.Status
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = booking.ID }, booking);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateBookingDto input)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking is null)
                return NotFound();
            if (!await _context.Users.AnyAsync(u => u.Id == input.UserID))
                return BadRequest($"No User with ID {input.UserID}.");

            if (!await _context.Rooms.AnyAsync(r => r.ID == input.RoomID))
                return BadRequest($"No Room with ID {input.RoomID}.");

            booking.UserID = input.UserID;
            booking.RoomID = input.RoomID;
            booking.StartTime = input.StartTime;
            booking.EndTime = input.EndTime;
            booking.Status = input.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking is null)
                return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
