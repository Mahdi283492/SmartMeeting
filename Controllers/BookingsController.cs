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
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        private async Task<int?> GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdStr, out int userId))
                return userId;

            // fallback: try email claim
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                return user?.Id;
            }

            return null;
        }

      [AllowAnonymous]
[HttpGet]
public async Task<ActionResult<IEnumerable<object>>> GetAll()
{
    var allBookings = await _context.Bookings
        .Include(b => b.Room)
        .OrderByDescending(b => b.StartTime)
        .Select(b => new
        {
            b.ID,
            RoomName = b.Room.Name,
            b.StartTime,
            b.EndTime,
            b.Status
        })
        .ToListAsync();

    return Ok(allBookings);
}


        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetById(int id)
        {
            var booking = await _context.Bookings.Include(b => b.Room).FirstOrDefaultAsync(b => b.ID == id);
            return booking is null ? NotFound() : booking;
        }

        [HttpPost("book")]
        public async Task<ActionResult<Booking>> Create([FromBody] CreateBookingDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = await GetCurrentUserId();
            if (userId == null)
                return BadRequest("Unable to determine current user.");

            if (!await _context.Rooms.AnyAsync(r => r.ID == input.RoomID))
                return BadRequest($"No Room with ID {input.RoomID}.");

            bool hasConflict = await _context.Bookings.AnyAsync(b =>
                b.RoomID == input.RoomID &&
                b.StartTime < input.EndTime &&
                b.EndTime > input.StartTime);

            if (hasConflict)
                return Conflict(new { message = "This room is already booked during the selected time." });

            var booking = new Booking
            {
                UserID = userId.Value,
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
            var userId = await GetCurrentUserId();
            if (userId == null)
                return BadRequest("Unable to determine current user.");

            var booking = await _context.Bookings.FindAsync(id);
            if (booking is null)
                return NotFound();

            if (booking.UserID != userId.Value)
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

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<object>>> GetMyBookings()
        {
            var userId = await GetCurrentUserId();
            if (userId == null)
                return BadRequest("Unable to determine current user.");

            var myBookings = await _context.Bookings
                .Where(b => b.UserID == userId.Value)
                .Include(b => b.Room)
                .OrderByDescending(b => b.StartTime)
                .Select(b => new
                {
                    b.ID,
                    RoomName = b.Room.Name,
                    b.StartTime,
                    b.EndTime,
                    b.Status
                })
                .ToListAsync();

            return Ok(myBookings);
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
