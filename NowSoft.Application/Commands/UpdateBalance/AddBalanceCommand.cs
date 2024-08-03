using MediatR;
using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Commands.UpdateBalance
{
    public class AddBalanceCommand : IRequest<decimal>
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
