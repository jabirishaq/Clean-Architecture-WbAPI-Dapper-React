using MediatR;
using NowSoft.Domain.Entities;

namespace NowSoft.Application.Queries.Authenticate
{
    /// <summary>
    /// Represents a query to authenticate a user in the system.
    /// This query is part of the MediatR pattern and is used to initiate the authentication process.
    /// </summary>
    public class AuthenticateQuery : IRequest<User>
    {
        /// <summary>
        /// Gets or sets the username of the user to be authenticated.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password of the user to be authenticated.
        /// </summary>
        public string Password { get; set; }
    }
}
