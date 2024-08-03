using MediatR;
using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Commands.Signup
{
    public class SignupCommand : IRequest<int>
    {
        public User User { get; set; }
    }
}
