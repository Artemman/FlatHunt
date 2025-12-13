using FlatHunt.Server.Auth;
using FlatHunt.Server.Dto;
using FlatHunt.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlatHunt.Server.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            UserManager<User> userManager,
            ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("add-broker")]
        public async Task<IActionResult> CreateBroker([FromBody] CreateBrokerRequest request)
        {
            if (request is null)
            {
                return BadRequest(new { message = "Request body is required." });
            }

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email and Password are required." });
            }

            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing != null)
            {
                return Conflict(new { message = "A user with this email already exists." });
            }

            var userName = string.IsNullOrWhiteSpace(request.UserName)
                ? request.Email.Split('@').FirstOrDefault() ?? request.Email
                : request.UserName;

            var user = new User
            {
                UserName = userName,
                Email = request.Email,
                EmailConfirmed = request.EmailConfirmed,
                FullName = request.FullName,
                Deleted = false
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return BadRequest(new { errors = createResult.Errors.Select(e => e.Description) });
            }

            var addToRole = await _userManager.AddToRoleAsync(user, Roles.Broker);
            if (!addToRole.Succeeded)
            {
                _logger.LogError("Failed to assign role to user {UserId}. Errors: {Errors}", user.Id, string.Join("; ", addToRole.Errors.Select(e => e.Description)));
                return StatusCode(500, new { message = "User created but failed to assign role." });
            }

            return Ok(user);
        }
    }
}