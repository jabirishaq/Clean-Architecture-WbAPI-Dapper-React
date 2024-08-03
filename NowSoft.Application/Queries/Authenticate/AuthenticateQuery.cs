using MediatR;
using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Queries.Authenticate
{
    public class AuthenticateQuery : IRequest<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
