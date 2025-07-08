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
        [HttpPost("book")]
        public async Task<ActionResult<Booking>> Create([FromBody] CreateBookingDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId))
                return BadRequest("Invalid user ID from token");

            if (!await _context.Rooms.AnyAsync(r => r.ID == input.RoomID))
                return BadRequest($"No Room with ID {input.RoomID}.");
            bool hasConflict = await _context.Bookings.AnyAsync(b =>
b.RoomID == input.RoomID &&
b.StartTime < input.EndTime &&
b.EndTime > input.StartTime);

            if (hasConflict)
                return BadRequest(new { message = "This room is already booked during the selected time." });


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
        [Authorize]
          [HttpGet("my")]
          public async Task<ActionResult<IEnumerable<object>>> GetMyBookings()
          {
              var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
              if (!int.TryParse(userIdStr, out var userId))
                  return BadRequest("Invalid user ID from token");

              var myBookings = await _context.Bookings
                  .Where(b => b.UserID == userId)
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
  
  /*          [HttpGet("my")]
public async Task<ActionResult<IEnumerable<object>>> GetMyBookings()
{
    // TEMP: Use hardcoded user ID 1 to test rendering
    int userId = 2;

    var myBookings = await _context.Bookings
        .Where(b => b.UserID == userId)
        .Include(b => b.Room)
        .OrderByDescending(b => b.StartTime)
        .Select(b => new
        {
            b.ID,
            // RoomName = b.Room != null ? b.Room.Name : "Unknown",
           
            b.StartTime,
            b.EndTime,
            b.Status
        })
        .ToListAsync();

    return Ok(myBookings);
}*/



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
