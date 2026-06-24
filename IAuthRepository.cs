using System;
using System.Collections.Generic;
using System.Text;
using VisitorWebAPI.Core.Entities;

namespace VisitorWebAPI.Data.Interfaces
{
    public interface IAuthRepository
    {
        Task<UserEntity> LoginAsync(string username,string password);
    }
}
