using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ElMercadito.Web.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
    }
}
