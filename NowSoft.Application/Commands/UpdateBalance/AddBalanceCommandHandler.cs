using MediatR;
using NowSoft.Application.Commands.Signup;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Commands.UpdateBalance
{
    public class AddBalanceCommandHandler : IRequestHandler<AddBalanceCommand, decimal>
    {
        private readonly IUserRepository _userRepository;

        public AddBalanceCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<decimal> Handle(AddBalanceCommand request, CancellationToken cancellationToken)//        public async Task<int> Handle(SignupCommand request, CancellationToken cancellationToken)

        {
            await _userRepository.AddBalanceAsync(request.UserId);
            return decimal.MaxValue;
        }
    }
}
