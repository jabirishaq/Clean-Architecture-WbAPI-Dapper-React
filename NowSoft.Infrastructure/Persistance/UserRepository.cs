using Dapper;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using NowSoft.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> SignUpAsync(User user)
        {
            const string query = @"INSERT INTO Users (Username, Password, FirstName, LastName, Device, IpAddress, Balance)
                               VALUES (@Username, @Password, @FirstName, @LastName, @Device, @IpAddress, @Balance);
                               SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleAsync<int>(query, user);
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            const string query = @"SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username, Password = password });
        }

        public async Task<decimal> GetBalanceAsync(int userId)
        {
            const string query = @"SELECT Balance FROM Users WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<decimal>(query, new { Id = userId });
        }
        public async Task<decimal> AddBalanceAsync(int userId)
        {
            decimal giftBalance = 5.0m;

            // Define the SQL query to update the balance
            const string updateBalanceQuery = @"UPDATE Users SET Balance = Balance + 5.0 WHERE Id = @Id AND Balance = 0.0";

            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync(updateBalanceQuery, new { Id = userId });         
           
            return giftBalance;
        }
    }

}
