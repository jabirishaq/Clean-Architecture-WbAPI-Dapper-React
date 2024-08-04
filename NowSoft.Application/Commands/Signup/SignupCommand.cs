using MediatR;
using NowSoft.Domain.Entities;

namespace NowSoft.Application.Commands.Signup
{
    /// <summary>
    /// SignupCommand class represents a request to create a new user in the system.
    /// This command is part of the MediatR pattern and is used to initiate the signup process.
    /// </summary>
    public class SignupCommand : IRequest<int>
    {
        /// <summary>
        /// Gets or sets the User entity containing the details of the user to be signed up.
        /// </summary>
        public User User { get; set; }
    }
}
