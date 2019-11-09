using ElMercadito.Web.Data;
using ElMercadito.Web.Data.Entities;
using ElMercadito.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElMercadito.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;

        public ConverterHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Product> ToProductAsync(ProductViewModel model, bool isNew)
        {
            return new Product
            {
                Id = isNew ? 0 : model.Id,
                ProductName = model.ProductName,
                Price = model.Price,
                Offers = isNew ? new List<Offer>() : model.Offers,
                IsAvailable = model.IsAvailable,
                Remarks = model.Remarks,
                Merchant = await _dataContext.Merchants.FindAsync(model.MerchantId),
                ProductImages = isNew ? new List<ProductImage>() : model.ProductImages,
                BusinessType = await _dataContext.BusinessTypes.FindAsync(model.BusinessTypeId
                ),




            };
        }
    }
}
