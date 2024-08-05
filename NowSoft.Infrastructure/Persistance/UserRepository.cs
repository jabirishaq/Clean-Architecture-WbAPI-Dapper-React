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
    // The UserRepository class implements the IUserRepository interface
    // and provides data access methods for the User entity using Dapper.
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        // Constructor injecting DapperContext for database connection management
        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        // Method to sign up a new user by inserting user data into the database
        public async Task<int> SignUpAsync(User user)
        {
            // SQL query to insert a new user and retrieve the new user ID
            const string query = @"INSERT INTO Users (Username, Password, FirstName, LastName, Device, IpAddress, Balance, Browser, IsFirstLogin)
                                   VALUES (@Username, @Password, @FirstName, @LastName, @Device, @IpAddress, @Balance, @Browser, @IsFirstLogin);
                                   SELECT CAST(SCOPE_IDENTITY() as int)";

            using var connection = _context.CreateConnection(); // Create a new database connection

            // Execute the query with user data and return the new user ID
            return await connection.QuerySingleAsync<int>(query, user);
        }

        // Method to authenticate a user by verifying username and password
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            // SQL query to select a user matching the provided username and password
            const string query = @"SELECT * FROM Users WHERE Username = @Username AND Password = @Password";

            using var connection = _context.CreateConnection(); // Create a new database connection

            // Execute the query and return the user if found, otherwise return null
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username, Password = password });
        }

        // Method to check if a user exists by username
        public async Task<User> UserExistAsync(string username)
        {
            // SQL query to select a user with the specified username
            const string query = @"SELECT * FROM Users WHERE Username = @Username";

            using var connection = _context.CreateConnection(); // Create a new database connection

            // Execute the query and return the user if found, otherwise return null
            return await connection.QueryFirstOrDefaultAsync<User>(query, new { Username = username });
        }

        // Method to update user authentication info (device, IP address, browser, login time)
        public async Task<int> UpdateUserAuthInfoAsync(User loginRequest)
        {
            // SQL query to update user authentication information in the database
            const string query = @"
            UPDATE Users 
            SET Device = @Device, 
                IsFirstLogin = @IsFirstLogin, 
                IpAddress = @IpAddress, 
                Browser = @Browser, 
                LoginTime = @LoginTime 
            WHERE Id = @Id";

            using var connection = _context.CreateConnection(); // Create a new database connection

            // Execute the update query and return the number of affected rows
            var affectedRows = await connection.ExecuteAsync(query, new
            {
                Device = loginRequest.Device,
                IsFirstLogin = loginRequest.IsFirstLogin,
                IpAddress = loginRequest.IpAddress,
                Browser = loginRequest.Browser,
                LoginTime = loginRequest.LoginTime,
                Id = loginRequest.Id // Ensure this is the correct property for UserId
            });

            return affectedRows;
        }

        // Method to retrieve the balance of a user by their user ID
        public async Task<decimal> GetBalanceAsync(int userId)
        {
            // SQL query to select the balance for the specified user ID
            const string query = @"SELECT Balance FROM Users WHERE Id = @Id";

            using var connection = _context.CreateConnection(); // Create a new database connection

            // Execute the query and return the balance or default if not found
            return await connection.QuerySingleOrDefaultAsync<decimal>(query, new { Id = userId });
        }

        // Method to add a gift balance to a user's account if the current balance is zero
        public async Task<decimal> AddBalanceAsync(int userId)
        {
            decimal giftBalance = 5.0m; // Define the amount to add as a gift balance

            // SQL query to update the balance if it is currently zero
            const string updateBalanceQuery = @"UPDATE Users SET Balance = Balance + 5.0 WHERE Id = @Id AND Balance = 0.0";

            using var connection = _context.CreateConnection(); // Create a new database connection

            // Execute the query to update the user's balance
            await connection.ExecuteAsync(updateBalanceQuery, new { Id = userId });

            return giftBalance; // Return the gift balance amount
        }
    }
}
