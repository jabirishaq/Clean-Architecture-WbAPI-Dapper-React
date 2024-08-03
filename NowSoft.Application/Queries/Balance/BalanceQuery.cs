using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Queries.Balance
{
    public class BalanceQuery : IRequest<decimal>
    {
        public int UserId { get; set; }
    }
}
