using VisitorMVC.Models.DTOs;

namespace VisitorMVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto dto);
    }
}
