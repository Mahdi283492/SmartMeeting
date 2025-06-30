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


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Booking>> Create([FromBody] CreateBookingDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid user ID from token");

            if (!await _context.Rooms.AnyAsync(r => r.ID == input.RoomID))
                return BadRequest($"No Room with ID {input.RoomID}.");

            var booking = new Booking
            {
                UserID = userId,
                RoomID = input.RoomID,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                Status = input.Status
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = booking.ID }, booking);
        }



        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateBookingDto input)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid user ID from token");

            var booking = await _context.Bookings.FindAsync(id);
            if (booking is null)
                return NotFound();

            
            if (booking.UserID != userId)
                return Forbid("You can only update your own bookings.");

            if (!await _context.Rooms.AnyAsync(r => r.ID == input.RoomID))
                return BadRequest($"No Room with ID {input.RoomID}.");

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
