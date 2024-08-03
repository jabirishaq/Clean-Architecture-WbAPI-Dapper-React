using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Queries.Authenticate
{
    public class UserExistenceQuery : IRequest<bool>
    {
        public string Username { get; set; }
    }
}
