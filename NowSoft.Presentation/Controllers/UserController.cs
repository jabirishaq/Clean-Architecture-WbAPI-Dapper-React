using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NowSoft.Application.Commands.Signup;
using NowSoft.Application.Commands.UpdateBalance;
using NowSoft.Application.Commands.UserAuthenticationInfo;
using NowSoft.Application.Interfaces;
using NowSoft.Application.Queries.Authenticate;
using NowSoft.Application.Queries.Balance;
using NowSoft.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NowSoft.Presentation.Controllers
{
    //[ApiController] // this enforces model validation and better error handling
    [Route("users")]
    public class UserController : ControllerBase
    {
        // MediatR is used to implement CQRS pattern for decoupling requests from handlers
        private ISender _mediator;
        private readonly ICustomJwtTokenGenerator _jwtService;

        // Constructor injects MediatR and JWT service for handling commands and generating tokens
        public UserController(IMediator mediator, ICustomJwtTokenGenerator jwtService)
        {
            _mediator = mediator;
            _jwtService = jwtService;
        }

        // Endpoint to handle user signup requests
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            try
            {
                // Sends a signup command to the handler via MediatR
                var userId = await _mediator.Send(new SignupCommand { User = user });

                // If a valid user ID is returned, the signup was successful
                if (userId > 0)
                {
                    return Ok(); // Return HTTP 200 status for successful signup
                }
                else
                {
                    // Return HTTP 409 conflict if user already exists
                    return Conflict(new { error = "User already exists." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception (not shown) and return HTTP 500 with the exception message
                return StatusCode(500, new { error = ex.Message }); // Handle unexpected exceptions
            }
        }

        // Endpoint to handle user authentication requests
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User request)
        {
            try
            {
                // Check for null or empty username
                if (String.IsNullOrEmpty(request.Username))
                {
                    return BadRequest(new { message = "Username cannot be null" });
                }

                // Check for null or empty password
                if (String.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Password cannot be null" });
                }

                // Authenticate user using a query sent to MediatR
                var user = await _mediator.Send(new AuthenticateQuery { Username = request.Username, Password = request.Password });

                // If user is not found, return unauthorized status
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }

                //// Retrieve current balance for the authenticated user
                //decimal currentBalance = await _mediator.Send(new BalanceQuery { UserId = user.Id });

                // If balance is zero, add a default balance amount
                //if (currentBalance == 0.0m)
                //{
                //    await _mediator.Send(new AddBalanceCommand { UserId = user.Id });
                //}

                if (user.IsFirstLogin)
                {
                    await _mediator.Send(new AddBalanceCommand { UserId = user.Id });
                }

                // Generate a JWT token for the authenticated user
                var token = _jwtService.GenerateToken(user);

                // Update user information with device, browser, and IP address
                user.Browser = request.Browser;
                user.IpAddress = request.IpAddress;
                user.Device = request.Device;

                // Save user authentication info using MediatR command
                await _mediator.Send(new UserAuthenticationInfoCommand { UserInfoObj = user });

                // Return the token and user information
                return Ok(new
                {
                    user.FirstName,
                    user.LastName,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                // Log the exception (not shown) and return HTTP 500 with the exception message
                return StatusCode(500, new { error = ex.Message }); // Handle unexpected exceptions
            }
        }

        // Protected endpoint to retrieve user's balance
        [HttpPost("auth/balance")]
        [Authorize] // Ensure that the request is authenticated
        public async Task<IActionResult> GetBalance()
        {
            try
            {
                // Retrieve the user ID from the JWT claims
                var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sid);
                if (userIdClaim == null)
                {
                    return Unauthorized(); // Return unauthorized if the claim is missing
                }

                // Attempt to parse the user ID from the claims
                if (!int.TryParse(userIdClaim.Value, out var userId))
                {
                    return BadRequest("Invalid user ID"); // Return bad request if parsing fails
                }

                // Retrieve the user's balance using a MediatR query
                var balance = await _mediator.Send(new BalanceQuery { UserId = userId });

                // Return the balance formatted as GBP
                return Ok(new { Balance = balance + " GBP" });
            }
            catch (Exception ex)
            {
                // Log the exception (not shown) and return HTTP 500 with the exception message
                return StatusCode(500, new { error = ex.Message }); // Handle unexpected exceptions
            }
        }
    }
}
