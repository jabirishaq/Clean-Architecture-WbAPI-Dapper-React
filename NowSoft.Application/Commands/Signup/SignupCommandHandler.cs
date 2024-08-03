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
            request.User.LoginTime = DateTime.Now;
            request.User.IpAddress = "172.23.5.67";
            request.User.Device = "12fdr112233";
            request.User.Browser = "Chrome";
            var userId = await _userRepository.SignUpAsync(request.User);
            return userId;
        }
    }
}
