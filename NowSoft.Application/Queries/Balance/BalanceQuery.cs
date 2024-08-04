using MediatR;

namespace NowSoft.Application.Queries.Balance
{
    /// <summary>
    /// Represents a query to retrieve the balance of a specific user.
    /// This query is part of the MediatR pattern and is used to request the user's balance.
    /// </summary>
    public class BalanceQuery : IRequest<decimal>
    {
        /// <summary>
        /// Gets or sets the user ID for which the balance is requested.
        /// </summary>
        public int UserId { get; set; }
    }
}
