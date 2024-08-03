using MediatR;
using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Commands.UserAuthenticationInfo
{
    public class UserAuthenticationInfoCommand : IRequest
    {
        public User UserInfoObj { get; set; }
    }
}
