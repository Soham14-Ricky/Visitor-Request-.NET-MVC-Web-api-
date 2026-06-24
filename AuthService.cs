using Newtonsoft.Json;
using System.Text;
using VisitorMVC.Models.DTOs;
using VisitorMVC.Services.Interfaces;

namespace VisitorMVC.Services.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        public AuthService( IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;

            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var client = _httpClientFactory.CreateClient();

            var apiUrl = _configuration["ApiSettings:BaseUrl"];

            var content = new StringContent(JsonConvert.SerializeObject(dto),Encoding.UTF8,"application/json");

            var response =await client.PostAsync($"{apiUrl}Auth/login",content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
