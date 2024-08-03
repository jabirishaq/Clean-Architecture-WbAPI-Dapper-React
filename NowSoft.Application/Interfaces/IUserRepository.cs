using NowSoft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<int> SignUpAsync(User user);
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> UserExistAsync(string username);
        Task<int> UpdateUserAuthInfoAsync(User loginRequest);
        Task<decimal> GetBalanceAsync(int userId);
        Task<decimal> AddBalanceAsync(int userId);
    }
}
