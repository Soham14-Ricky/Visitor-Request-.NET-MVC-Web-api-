using Microsoft.AspNetCore.Mvc;
using VisitorWebAPI.Core.DTOs;
using VisitorWebAPI.Data.Interfaces;
using VisitorWebAPI.Utilities.Security;

namespace VisitorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthController(IAuthRepository authRepository,JwtTokenGenerator jwtTokenGenerator)
        {
            _authRepository = authRepository;

            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user =
                await _authRepository.LoginAsync(
                    loginRequestDto.Username,
                    loginRequestDto.Password);

            if (user == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid username or password"
                });
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            var response =
                new LoginResponseDto
                {
                    UserId = user.UserId,

                    Username = user.Username,

                    FullName = user.FullName,

                    Role = user.Role,

                    Token = token
                };

            return Ok(new
            {
                Token = token,
                Role = user.Role,
                Username = user.Username
            });
        }
    }
}
