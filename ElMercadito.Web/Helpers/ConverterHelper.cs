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
        private readonly ICombosHelper _comboshelper;

        public ConverterHelper(DataContext dataContext,
            ICombosHelper comboshelper)
        {
            _dataContext = dataContext;
            _comboshelper = comboshelper;
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

        public ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                Offers =  product.Offers,
                IsAvailable = product.IsAvailable,
                Remarks = product.Remarks,
                Merchant =product.Merchant,
                ProductImages = product.ProductImages,
                BusinessType = product.BusinessType,
                MerchantId = product.Merchant.Id,
                BusinessTypeId = product.BusinessType.Id,
                BusinessTypes = _comboshelper.GetComboBusinessTypes()




            };
        }
    }
}
