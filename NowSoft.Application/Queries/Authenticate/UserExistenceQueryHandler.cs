using MediatR;
using NowSoft.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Queries.Authenticate
{
    public class UserExistenceQueryHandler : IRequestHandler<UserExistenceQuery, bool>
    {
        private readonly IUserRepository _userRepository;

        public UserExistenceQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UserExistenceQuery request, CancellationToken cancellationToken)
        {
            bool userExists = (await _userRepository.UserExistAsync(request.Username))?.Id > 0;
            return userExists;
        }
    }
}
