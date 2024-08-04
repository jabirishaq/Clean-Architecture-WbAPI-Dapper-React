using MediatR;
using NowSoft.Domain.Entities;

namespace NowSoft.Application.Commands.UserAuthenticationInfo
{
    /// <summary>
    /// Represents a command to update user authentication information in the system.
    /// This command is part of the MediatR pattern and is used to trigger updates for user authentication details.
    /// </summary>
    public class UserAuthenticationInfoCommand : IRequest
    {
        /// <summary>
        /// Gets or sets the User entity containing authentication details to be updated.
        /// </summary>
        public User UserInfoObj { get; set; }
    }
}
