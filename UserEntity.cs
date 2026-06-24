using System;
using System.Collections.Generic;
using System.Text;

namespace VisitorWebAPI.Core.Entities
{
    public class UserEntity
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        //public bool IsActive { get; set; }
    }
}
