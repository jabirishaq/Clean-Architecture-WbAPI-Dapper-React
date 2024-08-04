using MediatR;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Commands.UserAuthenticationInfo
{
    public class UserAuthenticationInfoCommandHandler : IRequestHandler<UserAuthenticationInfoCommand>
    {
        private readonly IUserRepository _userRepository;

        public UserAuthenticationInfoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(UserAuthenticationInfoCommand request, CancellationToken cancellationToken)
        {
            request.UserInfoObj.LoginTime = DateTime.Now;

            await _userRepository.UpdateUserAuthInfoAsync(request.UserInfoObj);
        }
    }
}
