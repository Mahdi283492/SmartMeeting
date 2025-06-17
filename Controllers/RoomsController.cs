using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.Models;
using SmartMeetingAPI.DTOs;    

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomsController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetAll()
            => await _context.Rooms.ToListAsync();

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetById(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            return room is null ? NotFound() : room;
        }

        
        [HttpPost]
        public async Task<ActionResult<Room>> Create([FromBody] CreateRoomDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var room = new Room
            {
                Name     = input.Name,
                Capacity = input.Capacity,
                Location = input.Location,
                Features = input.Features
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = room.ID },
                room
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateRoomDto input)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room is null)
                return NotFound();

            room.Name     = input.Name;
            room.Capacity = input.Capacity;
            room.Location = input.Location;
            room.Features = input.Features;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room is null)
                return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
