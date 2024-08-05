using MediatR;
using NowSoft.Application.Interfaces;

namespace NowSoft.Application.Commands.Signup
{
    /// <summary>
    /// Handles the signup command by processing the user registration request.
    /// This class implements the IRequestHandler interface from MediatR.
    /// </summary>
    public class SignupCommandHandler : IRequestHandler<SignupCommand, int>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignupCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">An instance of IUserRepository to interact with the user data store.</param>
        public SignupCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Handles the signup command request.
        /// </summary>
        /// <param name="request">The signup command containing the user details to be registered.</param>
        /// <param name="cancellationToken">Cancellation token for managing request cancellation.</param>
        /// <returns>The ID of the newly created user if successful, otherwise returns 0 if the user already exists.</returns>
        public async Task<int> Handle(SignupCommand request, CancellationToken cancellationToken)
        {
            // Check if the user already exists by username
            bool userExists = (await _userRepository.UserExistAsync(request.User.Username))?.Id > 0;

            if (!userExists)
            {
                // Set initial balance and login time for the new user
                request.User.Balance = 0.0m;
                request.User.IsFirstLogin = true;
                request.User.LoginTime = DateTime.Now;

                // Register the new user and return the user ID
                var userId = await _userRepository.SignUpAsync(request.User);
                return userId;
            }
            else
            {
                // Return 0 if the user already exists
                return 0;
            }
        }
    }
}
