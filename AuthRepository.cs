using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VisitorWebAPI.Core.Entities;
using VisitorWebAPI.Data.Context;
using VisitorWebAPI.Data.Interfaces;
using Dapper;
using VisitorWebAPI.Utilities.Security;
using Microsoft.Extensions.Logging;

namespace VisitorWebAPI.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(DapperContext context, ILogger<AuthRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserEntity> LoginAsync(string username,string password)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Username", username);

                // Step 1: Get stored hash
                var hashed_password =
                    await connection.QueryFirstOrDefaultAsync<string>(
                        "sp_GetUserPasswordHash",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                if (hashed_password == null)
                {
                    return null;
                }

                // Step 2: Verify password
                bool isValid =
                    EncrytionService.VerifyPassword(
                        password,
                        hashed_password);

                if (!isValid)
                {
                    return null;
                }

                // Step 3: Get user details
                return await connection.QueryFirstOrDefaultAsync<UserEntity>(
                    "sp_GetUserDetails",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "error while getting validation");
                throw;
            }
        }
    }
}
