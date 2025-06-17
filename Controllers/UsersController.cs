using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.DTOs;
using SmartMeetingAPI.Models;

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }


        /*   [HttpGet("{id}")]
           public async Task<ActionResult<User>> GetUser(int id)
           {
               var user = await _context.Users.FindAsync(id);
               if (user == null)
                   return NotFound();

               return user;
           }*/
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }



        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto input)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Name = input.Name,
                Email = input.Email,
                PasswordHash = input.PasswordHash,
                Role = input.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            return CreatedAtAction(nameof(GetUserById), new { id = user.ID }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User input)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Name = input.Name;
            user.Email = input.Email;
            user.PasswordHash = input.PasswordHash;
            user.Role = input.Role;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
    
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
