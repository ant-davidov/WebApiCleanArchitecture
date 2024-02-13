using MVC.Contracts;
using System.Net.Http.Headers;

namespace MVC.Services.Base
{
    public class BaseHttpService
    {
        protected readonly ILocalStorageService _localStorage;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected IClient _client;

        public BaseHttpService(IClient client, ILocalStorageService localStorage,IHttpContextAccessor httpContextAccessor)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _localStorage = localStorage;

        }

        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException ex)
        {
            if (ex.StatusCode == 400)
            {
                return new Response<Guid>() { Message = "Validation errors have occured.", ValidationErrors = ex.Response, Success = false };
            }
            else if (ex.StatusCode == 404)
            {
                return new Response<Guid>() { Message = "The requested item could not be found.", Success = false };
            }
            else
            {
                return new Response<Guid>() { Message = "Something went wrong, please try again.", Success = false };
            }
        }

        protected void AddBearerToken()
        {
          
            _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Request.Cookies["AuthToken"]);

        }
    }
}
