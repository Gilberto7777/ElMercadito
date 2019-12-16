using ElMercadito.Common.Models;
using System.Threading.Tasks;

namespace ElMercadito.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetMerchantByEmail(string urlBase, string servicePrefix, string controller, string tokenType, string accessToken, string email);
        Task<Response> GetTokenAsync(string urlBase, string servicePrefix, string controller, TokenRequest request);
    }
}