using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;

namespace NowSoft.Presentation.Controllers
{
    //[ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                        
            await _userRepository.AddBalanceAsync(user.Id);         // change this logic later    

            //var token = _jwtService.GenerateSecurityToken(user);
            return Ok(new
            {
                user.FirstName,
                user.LastName,
                //Token = token
            });
        }

        [HttpPost("auth/balance")]
        [Authorize]
        public async Task<IActionResult> GetBalance()
        {
            var userId = int.Parse(User.Identity.Name);
            var balance = await _userRepository.GetBalanceAsync(userId);
            return Ok(new { Balance = balance });
        }
    }
}
