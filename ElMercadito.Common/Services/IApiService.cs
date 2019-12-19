using ElMercadito.Common.Models;
using System.Threading.Tasks;

namespace ElMercadito.Common.Services
{
    public interface IApiService
    {
        Task<Response<MerchantResponse>> GetMerchantByEmail(
            string urlBase, 
            string servicePrefix, 
            string controller, 
            string tokenType, 
            string accessToken, 
            string email);
       
        Task<Response<TokenResponse>> GetTokenAsync(
            string urlBase, 
            string servicePrefix, 
            string controller, 
            TokenRequest request);
    }
}