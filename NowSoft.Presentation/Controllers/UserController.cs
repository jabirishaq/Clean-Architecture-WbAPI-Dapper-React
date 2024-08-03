using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NowSoft.Application.Commands.Signup;
using NowSoft.Application.Commands.UpdateBalance;
using NowSoft.Application.Interfaces;
using NowSoft.Application.Queries.Authenticate;
using NowSoft.Application.Queries.Balance;
using NowSoft.Domain.Entities;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace NowSoft.Presentation.Controllers
{
    //[ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private ISender _mediator;
        private readonly ICustomJwtTokenGenerator _jwtService;

        public UserController(IMediator mediator, ICustomJwtTokenGenerator jwtService)
        {
            _mediator = mediator;
            _jwtService = jwtService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            user.Balance = 0;
            bool IsUserExisting = false;

            var userAuthObj = await _mediator.Send(new UserExistenceQuery { Username = user.Username });

            if(!userAuthObj)
            {
                var userId = await _mediator.Send(new SignupCommand { User = user });
                return Ok();
            }
            else
            {
                return Conflict(new { error = "User already exists." });
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {

            var user = await _mediator.Send(new AuthenticateQuery { Username = request.Username, Password = request.Password }); // using the CQRS via mediatr

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            decimal currentBalance = await _mediator.Send(new BalanceQuery { UserId = user.Id }); // using the CQRS via mediatr

            if (currentBalance == 0.0m)
            {
                await _mediator.Send(new AddBalanceCommand { UserId = user.Id });
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

            var balance = await _mediator.Send(new BalanceQuery { UserId = userId }); // using the CQRS via mediatr

            return Ok(new { Balance = balance + " GBP" });
        }
    }
}
