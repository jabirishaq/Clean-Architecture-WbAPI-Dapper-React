using MediatR;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Queries.Authenticate
{
    public class AuthenticateQueryHandler : IRequestHandler<AuthenticateQuery, User>
    {
        private readonly IUserRepository _userRepository;

        public AuthenticateQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


       public async Task<User> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.AuthenticateAsync(request.Username, request.Password);
            return user;
        }
    }
}
