using ElMercadito.Common.Models;
using ElMercadito.Web.Data;
using ElMercadito.Web.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ElMercadito.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class MerchantsController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public MerchantsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("GetMerchantByEmail")]
        public async Task<IActionResult> GetMerchantByEmailAsync(EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var merchant = await _dataContext.Merchants
                .Include(o => o.User)
                .Include(o => o.Products)
                .ThenInclude(p => p.BusinessType)
                .Include(o => o.Products)
                .ThenInclude(p => p.ProductImages)
                .FirstOrDefaultAsync(o => o.User.Email.ToLower() == request.Email.ToLower());

            if (merchant == null)
            {
                return NotFound();
            }

            var response = new MerchantResponse
            {
                Id = merchant.Id,
                FirstName = merchant.User.FirstName,
                LastName = merchant.User.LastName,
                Address = merchant.User.Address,
                Document = merchant.User.Document,
                Email = merchant.User.Email,
                PhoneNumber = merchant.User.PhoneNumber,
                Products = merchant.Products?.Select(p => new ProductResponse
                {
                    
                    Id = p.Id,
                    ProductName = p.ProductName,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    Remarks = p.Remarks,
                    ProductImages = p.ProductImages?.Select(pi => new ProductImageResponse
                    {
                        Id = pi.Id,
                        ImageUrl = pi.ImageFullPath
                    }).ToList(),
                    BusinessType = p.BusinessType.Name,
                    
                }).ToList()
            };

            return Ok(response);
        }

        private ClientResponse ToClientsResponse(Client client)
        {
            return new ClientResponse
            {
                Id = client.Id,
                Address = client.User.Address,
                Document = client.User.Document,
                Email = client.User.Email,
                FirstName = client.User.FirstName,
                LastName = client.User.LastName,
                PhoneNumber = client.User.PhoneNumber
            };
        }
    }
}

 

