using FinanceApp.Api.Utils;
using FinanceApp.Data;
using Microsoft.AspNetCore.Mvc;
using FinanceApp.DTO;
using FinanceApp.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace FinanceApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUnitOfWork unitOfWork, IAuthenticationService authenticationService, JwtSettings jwtSettings) : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IAuthenticationService authenticationService = authenticationService;
        private readonly JwtSettings jwtSettings = jwtSettings;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await unitOfWork.Users.GetUserAsync(loginDTO.Username);

            if (user == null || !PasswordUtils.VerifyPassword(loginDTO.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            var token = AuthenticationService.GenerateJwtToken(user, jwtSettings);
            return Ok(new { token });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            await authenticationService.RegisterUser(registerDTO);
            return Ok("User registered successfully.");
        }
    }
}
