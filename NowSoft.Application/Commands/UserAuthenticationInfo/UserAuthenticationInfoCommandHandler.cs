using MediatR;
using NowSoft.Application.Interfaces;

namespace NowSoft.Application.Commands.UserAuthenticationInfo
{
    /// <summary>
    /// Handles the UserAuthenticationInfoCommand by updating user authentication information in the data store.
    /// Implements the IRequestHandler interface from MediatR for command handling.
    /// </summary>
    public class UserAuthenticationInfoCommandHandler : IRequestHandler<UserAuthenticationInfoCommand>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAuthenticationInfoCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">An instance of IUserRepository to interact with the user data store.</param>
        public UserAuthenticationInfoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Handles the UserAuthenticationInfoCommand request by updating the user's authentication details.
        /// </summary>
        /// <param name="request">The command containing the user authentication information to be updated.</param>
        /// <param name="cancellationToken">Cancellation token for managing request cancellation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Handle(UserAuthenticationInfoCommand request, CancellationToken cancellationToken)
        {
            // Set the login time to the current date and time
            request.UserInfoObj.LoginTime = DateTime.Now;
            request.UserInfoObj.IsFirstLogin = false;

            // Update the user's authentication information in the repository
            await _userRepository.UpdateUserAuthInfoAsync(request.UserInfoObj);
        }
    }
}
