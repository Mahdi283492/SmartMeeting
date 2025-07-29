using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMeetingAPI.DTOs;
using SmartMeetingAPI.Models;
using System.Security.Claims;

namespace SmartMeetingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

     
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<object>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                result.Add(new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    Roles = roles
                });
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                Roles = roles
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _userManager.FindByEmailAsync(input.Email) != null)
                return BadRequest(new { message = "Email already exists." });

            var newUser = new ApplicationUser
            {
                UserName = input.Email,
                Email = input.Email,
                Name = input.Name
            };

            var createResult = await _userManager.CreateAsync(newUser, input.Password);
            if (!createResult.Succeeded)
                return BadRequest(createResult.Errors);

            if (!await _roleManager.RoleExistsAsync(input.Role))
                await _roleManager.CreateAsync(new IdentityRole<int>(input.Role));

            await _userManager.AddToRoleAsync(newUser, input.Role);

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, new
            {
                newUser.Id,
                newUser.Name,
                newUser.Email,
                Roles = new[] { input.Role }
            });
        }

 [HttpPut("{id}")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto input)
{
    var user = await _userManager.FindByIdAsync(id.ToString());
    if (user == null)
        return NotFound();

    user.Name = input.Name;
    user.Email = input.Email;
    user.UserName = input.Email;

    var updateResult = await _userManager.UpdateAsync(user);
    if (!updateResult.Succeeded)
        return BadRequest(updateResult.Errors);

    var currentRoles = await _userManager.GetRolesAsync(user);
    await _userManager.RemoveFromRolesAsync(user, currentRoles);

    if (!await _roleManager.RoleExistsAsync(input.Role))
        await _roleManager.CreateAsync(new IdentityRole<int>(input.Role));

    await _userManager.AddToRoleAsync(user, input.Role);

    return NoContent();
}


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? NoContent() : BadRequest(result.Errors);
        }

     
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                Roles = roles
            });
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto input)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            user.Name = input.Name;
            if (!string.IsNullOrWhiteSpace(input.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, token, input.NewPassword);
                if (!passwordResult.Succeeded)
                    return BadRequest(passwordResult.Errors);
            }

            var updateResult = await _userManager.UpdateAsync(user);
            return updateResult.Succeeded ? NoContent() : BadRequest(updateResult.Errors);
        }
    }
}
