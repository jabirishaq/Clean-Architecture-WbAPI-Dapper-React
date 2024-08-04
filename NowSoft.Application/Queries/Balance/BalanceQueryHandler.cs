using MediatR;
using NowSoft.Application.Interfaces;

namespace NowSoft.Application.Queries.Balance
{
    /// <summary>
    /// Handles the BalanceQuery by processing the request to retrieve a user's balance.
    /// Implements the IRequestHandler interface from MediatR for query handling.
    /// </summary>
    public class BalanceQueryHandler : IRequestHandler<BalanceQuery, decimal>
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceQueryHandler"/> class.
        /// </summary>
        /// <param name="userRepository">An instance of IUserRepository to interact with the user data store.</param>
        public BalanceQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Handles the BalanceQuery request by retrieving the user's balance.
        /// </summary>
        /// <param name="request">The query containing the user ID for which the balance is requested.</param>
        /// <param name="cancellationToken">Cancellation token for managing request cancellation.</param>
        /// <returns>A decimal representing the user's balance.</returns>
        public async Task<decimal> Handle(BalanceQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the balance for the specified user ID using the repository
            var balance = await _userRepository.GetBalanceAsync(request.UserId);

            // Return the retrieved balance
            return balance;
        }
    }
}
