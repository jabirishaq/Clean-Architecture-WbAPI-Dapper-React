using MediatR;

namespace NowSoft.Application.Commands.UpdateBalance
{
    /// <summary>
    /// Represents a command to add balance to a user's account.
    /// This command is part of the MediatR pattern and is used to trigger the balance update process.
    /// </summary>
    public class AddBalanceCommand : IRequest<decimal>
    {
        /// <summary>
        /// Gets or sets the user ID for which the balance will be updated.
        /// </summary>
        public int UserId { get; set; }
    }
}
