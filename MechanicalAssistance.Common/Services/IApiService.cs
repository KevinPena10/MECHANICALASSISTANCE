using MechanicalAssistance.Common.Models;
using System.Threading.Tasks;

namespace MechanicalAssistance.Common.Services
{
    public interface IApiService
    {
        Task<bool> CheckConnectionAsync(string url);
        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);

        Task<Response> RecoverPasswordAsync(string urlBase, string servicePrefix, string controller, EmailRequest emailRequest);

        Task<Response> PutAsync<T>(string urlBase, string servicePrefix, string controller, T model, string tokenType, string accessToken);

        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken);
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
        Task<Response> ChangePasswordAsync(string urlBase, string servicePrefix, string controller, ChangePasswordRequest changePasswordRequest, string tokenType, string accessToken);

        Task<Response> RegisterUserAsync(string urlBase, string servicePrefix, string controller, UserRequest userRequest);

        Task<Response> GetUserByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, EmailRequest email);

        Task<Response> NewProductAsync(string urlBase, string servicePrefix, string controller, ProductRequest productlRequest, string tokenType, string accessToken);


    }
}
