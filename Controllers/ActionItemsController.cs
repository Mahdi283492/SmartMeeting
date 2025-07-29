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
    public class ActionItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActionItemsController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActionItem>>> GetAll()
            => await _context.ActionItems.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<ActionItem>> GetById(int id)
        {
            var item = await _context.ActionItems.FindAsync(id);
            return item is null ? NotFound() : item;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ActionItem>> Create([FromBody] CreateActionItemDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _context.Minutes.AnyAsync(m => m.ID == input.MinutesID))
                return BadRequest($"No Minutes with ID {input.MinutesID}.");
            if (!await _context.Users.AnyAsync(u => u.Id == input.AssignedTo))
                return BadRequest($"No User with ID {input.AssignedTo}.");

            var actionItem = new ActionItem
            {
                MinutesID = input.MinutesID,
                AssignedTo = input.AssignedTo,
                Description = input.Description,
                DueDate = input.DueDate,
                Status = input.Status
            };

            _context.ActionItems.Add(actionItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = actionItem.ID }, actionItem);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateActionItemDto input)
        {
            var actionItem = await _context.ActionItems.FindAsync(id);
            if (actionItem is null)
                return NotFound();

            if (!await _context.Minutes.AnyAsync(m => m.ID == input.MinutesID))
                return BadRequest($"No Minutes with ID {input.MinutesID}.");
            if (!await _context.Users.AnyAsync(u => u.Id == input.AssignedTo))
                return BadRequest($"No User with ID {input.AssignedTo}.");

            actionItem.MinutesID = input.MinutesID;
            actionItem.AssignedTo = input.AssignedTo;
            actionItem.Description = input.Description;
            actionItem.DueDate = input.DueDate;
            actionItem.Status = input.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var actionItem = await _context.ActionItems.FindAsync(id);
            if (actionItem is null)
                return NotFound();

            _context.ActionItems.Remove(actionItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize]
[HttpGet("my")]
public async Task<ActionResult<IEnumerable<object>>> GetMyActionItems()
{
    var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (!int.TryParse(userIdStr, out int userId))
        return BadRequest("Invalid user ID from token.");

    var items = await _context.ActionItems
        .Include(ai => ai.Minutes)
        .ThenInclude(m => m.Meeting)
        .Where(ai => ai.AssignedTo == userId)
        .Select(ai => new
        {
            ai.ID,
            ai.Description,
            ai.DueDate,
            ai.Status,
            MinutesId = ai.MinutesID,
            MeetingTitle = ai.Minutes.Meeting.Title
        })
        .ToListAsync();

    return Ok(items);
}

    }
}
