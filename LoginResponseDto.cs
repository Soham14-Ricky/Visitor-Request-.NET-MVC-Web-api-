using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorWebAPI.Core.DTOs
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
