using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.Models;
using SmartMeetingAPI.DTOs;

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotificationsController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetAll()
            => await _context.Notifications.ToListAsync();

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetById(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            return notification is null ? NotFound() : notification;
        }

        
        [HttpPost]
        public async Task<ActionResult<Notification>> Create([FromBody] CreateNotificationDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _context.Users.AnyAsync(u => u.ID == input.UserID))
                return BadRequest($"No User with ID {input.UserID}.");

            var notification = new Notification
            {
                UserID    = input.UserID,
                Message   = input.Message,
                CreatedAt = DateTime.UtcNow,
                IsRead    = input.IsRead
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = notification.ID }, notification);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateNotificationDto input)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification is null)
                return NotFound();

            if (!await _context.Users.AnyAsync(u => u.ID == input.UserID))
                return BadRequest($"No User with ID {input.UserID}.");

            notification.UserID = input.UserID;
            notification.Message = input.Message;
            notification.IsRead = input.IsRead;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification is null)
                return NotFound();

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}