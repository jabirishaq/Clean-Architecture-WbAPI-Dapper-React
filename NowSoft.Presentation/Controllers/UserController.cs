using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace NowSoft.Presentation.Controllers
{
    //[ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomJwtTokenGenerator _jwtService;

        public UserController(IUserRepository userRepository, ICustomJwtTokenGenerator jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            user.Balance = 0;
            var userId = await _userRepository.SignUpAsync(user);
            return Ok(new { Id = userId });
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var user = await _userRepository.AuthenticateAsync(request.Username, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            decimal currentBalance = await _userRepository.GetBalanceAsync(user.Id); // check the user current balance

            if (currentBalance == 0.0m)
            {
                await _userRepository.AddBalanceAsync(user.Id); //updates the current balance if its first time
            }

            var token = _jwtService.GenerateToken(user);
            return Ok(new
            {
                user.FirstName,
                user.LastName,
                Token = token
            });
        }

        [HttpPost("auth/balance")]
        [Authorize]
        public async Task<IActionResult> GetBalance()
        {
            //for Debugging purposes of claims
            //var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            //Console.WriteLine("Claims: " + JsonSerializer.Serialize(claims)); 


            // Retrieve the user ID from claims
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sid);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            // Parse the user ID
            if (!int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest("Invalid user ID");
            }

            // Get the balance from the repository
            var balance = await _userRepository.GetBalanceAsync(userId);
            return Ok(new { Balance = balance });
        }
    }
}
