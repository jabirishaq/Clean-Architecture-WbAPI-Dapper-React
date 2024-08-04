using MediatR;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace NowSoft.Application.Queries.Authenticate
{
    /// <summary>
    /// Handles the AuthenticateQuery by processing the request to authenticate a user.
    /// Implements the IRequestHandler interface from MediatR for query handling.
    /// </summary>
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, User>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticateQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository">An instance of IUserRepository to interact with the user data store.</param>
        public AuthenticateQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Handles the AuthenticateQuery request by authenticating the user based on the provided credentials.
        /// </summary>
        /// <param name="request">The query containing the username and password for authentication.</param>
        /// <param name="cancellationToken">Cancellation token for managing request cancellation.</param>
        /// <returns>The authenticated User object if successful, otherwise null if authentication fails.</returns>
        public async Task<User> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            // Attempt to authenticate the user using the provided username and password
            User user = await _userRepository.AuthenticateAsync(request.Username, request.Password);

            // Return the user object if authentication is successful; otherwise, return null
            return user;
        }
    }
}
