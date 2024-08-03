using MediatR;
using NowSoft.Application.Interfaces;
using NowSoft.Application.Queries.Authenticate;
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
        private ISender _mediator;


        public SignupCommandHandler(IUserRepository userRepository, ISender mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async Task<int> Handle(SignupCommand request, CancellationToken cancellationToken)
        {
            //checked the user is existing or not
            var userAuthObj = await _mediator.Send(new UserExistenceQuery { Username = request.User.Username });

            if (!userAuthObj)
            {
                request.User.Balance = 0.0m;
                request.User.LoginTime = DateTime.Now;
                request.User.IpAddress = "172.23.5.67";
                request.User.Device = "12fdr112233";
                request.User.Browser = "Chrome";
                var userId = await _userRepository.SignUpAsync(request.User);
                return userId;
            }
            else
            {
                return 0;
            }            
        }
    }
}
