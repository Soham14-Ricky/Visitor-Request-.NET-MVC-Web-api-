using Humanizer;
using Newtonsoft.Json;
using System.Text;
using VisitorMVC.Models.DTOs;
using VisitorMVC.Services.Interfaces;

namespace VisitorMVC.Services.Repositories
{
    public class VisitorRequestService: IVisitorRequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public VisitorRequestService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;

            _configuration = configuration;

            _httpContextAccessor = httpContextAccessor;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient();

            var token =
                _httpContextAccessor.HttpContext
                .Session.GetString("JWToken");

            client.DefaultRequestHeaders
                .Authorization =
                new System.Net.Http.Headers
                .AuthenticationHeaderValue(
                    "Bearer",
                    token);

            return client;
        }

        public async Task<bool> CreateRequestAsync( VisitorRequestDto dto)
        {
            var client = CreateClient();

            var apiUrl =
                _configuration
                ["ApiSettings:BaseUrl"];

            var content =
                new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json");

            var response =
                await client.PostAsync(
                    $"{apiUrl}VisitorRequest/create",
                    content);

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<VisitorRequestDto>> GetMyRequestsAsync()
        {
            var client = CreateClient();

            var apiUrl =
                _configuration
                ["ApiSettings:BaseUrl"];

            var response =
                await client.GetAsync(
                    $"{apiUrl}VisitorRequest/myrequests");

            if (!response.IsSuccessStatusCode)
            {
                return new List<VisitorRequestDto>();
            }

            var json =
                await response.Content
                .ReadAsStringAsync();

            return JsonConvert.DeserializeObject <IEnumerable<VisitorRequestDto>>(json);
        }

        public async Task<bool> UpdateRequestAsync(VisitorRequestDto dto)
        {
            var client = CreateClient();

            var apiUrl =
                _configuration
                ["ApiSettings:BaseUrl"];

            var content =
                new StringContent(
                    JsonConvert.SerializeObject(dto),
                    Encoding.UTF8,
                    "application/json");

            var response =
                await client.PutAsync(
                    $"{apiUrl}VisitorRequest/update",
                    content);

            return response.IsSuccessStatusCode;
        }



        public async Task<IEnumerable<VisitorRequestDto>> GetPendingRequestsAsync()
        {
            var client = CreateClient();

            var apiUrl =
                _configuration
                ["ApiSettings:BaseUrl"];

            var response =
                await client.GetAsync(
                    $"{apiUrl}VisitorRequest/pending");

            if (!response.IsSuccessStatusCode)
            {
                return new List<VisitorRequestDto>();
            }

            var json =
                await response.Content
                .ReadAsStringAsync();

            return JsonConvert.DeserializeObject
                <List<VisitorRequestDto>>(json);
        }

        //public async Task<bool> ApproveRequestAsync(int requestId)
        //{
        //    var client = CreateClient();

        //    var apiUrl =
        //        _configuration
        //        ["ApiSettings:BaseUrl"];

        //    var response =
        //        await client.PutAsync(
        //            $"{apiUrl}VisitorRequest/approve",
        //            null);

        //    return response.IsSuccessStatusCode;
        //}


        public async Task<bool> ApproveRequestAsync(int requestId)
        {
            var client = CreateClient();

            var apiUrl =
                _configuration["ApiSettings:BaseUrl"];

            var content =
                new StringContent(
                    JsonConvert.SerializeObject(requestId),
                    Encoding.UTF8,
                    "application/json");

            var response =
                await client.PutAsync(
                    $"{apiUrl}VisitorRequest/approve",
                    content);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> RejectRequestAsync(RejectRequestDto dto)

        {

            var client = CreateClient();

            var apiUrl =

                _configuration["ApiSettings:BaseUrl"];

            var content =

                new StringContent(

                    JsonConvert.SerializeObject(dto),

                    Encoding.UTF8,

                    "application/json");

            var response =

                await client.PutAsync(

                    $"{apiUrl}VisitorRequest/reject",

                    content);

            return response.IsSuccessStatusCode;

        }


        public async Task<VisitorRequestDto> GetRequestByIdAsync(int requestId)
        {
            var client = CreateClient();

            var apiUrl =
                _configuration
                ["ApiSettings:BaseUrl"];

            var content = new StringContent(
    JsonConvert.SerializeObject(requestId),
    Encoding.UTF8,
    "application/json");

            var response = await client.PostAsync(
                $"{apiUrl}VisitorRequest/getbyid",
                content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json =
                await response.Content
                .ReadAsStringAsync();

            return JsonConvert.DeserializeObject
                <VisitorRequestDto>(json);
        }



        public async Task<bool> DeleteRequestAsync(int requestId)
        {
            var client = CreateClient();

            var apiUrl =
                _configuration["ApiSettings:BaseUrl"];

            var content = new StringContent(
                JsonConvert.SerializeObject(requestId),
                Encoding.UTF8,
                "application/json");

            var request = new HttpRequestMessage(
                HttpMethod.Delete,
                $"{apiUrl}VisitorRequest/delete");

            request.Content = content;

            var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
    }
}