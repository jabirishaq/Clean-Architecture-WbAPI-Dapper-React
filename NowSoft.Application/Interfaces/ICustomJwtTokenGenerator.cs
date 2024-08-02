using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Interfaces
{
    public interface ICustomJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
