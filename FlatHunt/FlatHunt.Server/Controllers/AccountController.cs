using FlatHunt.Server.Dto;
using FlatHunt.Server.Models;
using FlatHunt.Server.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlatHunt.Server.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUser)
        {
            var user = await _userManager.FindByEmailAsync(loginUser.Email);

            if (user == null
                || user.Deleted)
            {
                return Unauthorized();
            }

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginUser.Password, lockoutOnFailure: true);
            if (!checkPassword.Succeeded)
            {
                return Unauthorized();
            }

            var (access, refresh) = await _jwtTokenService.IssueTokensAsync(user);

            return Ok(new
            {
                AccessToken = access,
                RefreshToken = refresh
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var result = await _jwtTokenService.RefreshAsync(refreshToken);

            return result is null
                    ? Unauthorized()
                    : Ok(new
                    {
                        access = result.Value.accessToken,
                        refresh = result.Value.refreshToken
                    });
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            await _jwtTokenService.RevokeAsync(refreshToken);
            return Ok();
        }

#if DEBUG
        [Authorize]
        [HttpGet("whoami")]
        public IActionResult WhoAmI() =>
            Ok(User.Claims.Select(c => new { c.Type, c.Value }));
#endif
    }
}
