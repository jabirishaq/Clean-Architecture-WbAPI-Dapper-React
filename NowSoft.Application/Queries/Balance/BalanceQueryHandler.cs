using MediatR;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Queries.Balance
{
    public class BalanceQueryHandler : IRequestHandler<BalanceQuery, decimal>
    {
        private readonly IUserRepository _userRepository;

        public BalanceQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<decimal> Handle(BalanceQuery request, CancellationToken cancellationToken)
        {
            var balance = await _userRepository.GetBalanceAsync(request.UserId);
           
            return balance;
        }
    }
}
