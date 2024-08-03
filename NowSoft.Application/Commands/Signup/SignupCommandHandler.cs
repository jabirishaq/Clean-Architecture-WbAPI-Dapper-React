using MediatR;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Commands.Signup
{
    public class SignupCommandHandler : IRequestHandler<SignupCommand, int>
    {
        private readonly IUserRepository _userRepository;

        public SignupCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Handle(SignupCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userRepository.SignUpAsync(request.User);
            return userId;
        }
    }
}
