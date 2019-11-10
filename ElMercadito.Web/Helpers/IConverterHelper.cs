using System.Threading.Tasks;
using ElMercadito.Web.Data.Entities;
using ElMercadito.Web.Models;

namespace ElMercadito.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Product> ToProductAsync(ProductViewModel model, bool isNew);
        
        ProductViewModel ToProductViewModel(Product product);
    }
}