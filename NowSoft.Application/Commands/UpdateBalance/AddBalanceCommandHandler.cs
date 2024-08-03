using MediatR;
using NowSoft.Application.Commands.Signup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Commands.UpdateBalance
{
    public class AddBalanceCommandHandler : IRequestHandler<AddBalanceCommand, decimal>
    {
        public Task<decimal> Handle(AddBalanceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
