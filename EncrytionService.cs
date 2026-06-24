using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorWebAPI.Utilities.Security
{
    public static class EncrytionService
    {
        // Hash Password
        public static string EncryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Verify Password
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
}
