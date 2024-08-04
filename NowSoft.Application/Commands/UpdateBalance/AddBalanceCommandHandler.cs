using MediatR;
using NowSoft.Application.Interfaces;

namespace NowSoft.Application.Commands.UpdateBalance
{
    /// <summary>
    /// Handles the AddBalanceCommand by processing the request to add a balance to a user's account.
    /// Implements the IRequestHandler interface from MediatR for command handling.
    /// </summary>
    public class AddBalanceCommandHandler : IRequestHandler<AddBalanceCommand, decimal>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddBalanceCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">An instance of IUserRepository to interact with the user data store.</param>
        public AddBalanceCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Handles the AddBalanceCommand request by updating the user's balance.
        /// </summary>
        /// <param name="request">The command containing the user ID for which the balance should be added.</param>
        /// <param name="cancellationToken">Cancellation token for managing request cancellation.</param>
        /// <returns>A decimal representing the result of the operation, currently returns decimal.MaxValue as a placeholder.</returns>
        public async Task<decimal> Handle(AddBalanceCommand request, CancellationToken cancellationToken)
        {
            // Add balance to the user's account using the repository
            await _userRepository.AddBalanceAsync(request.UserId);

            // Return a placeholder value to signify completion of the operation
            return decimal.MaxValue;
        }
    }
}
