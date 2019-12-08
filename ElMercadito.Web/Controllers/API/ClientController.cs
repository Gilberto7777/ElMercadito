using ElMercadito.Common.Models;
using ElMercadito.Web.Data;
using ElMercadito.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ElMercadito.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ClientController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        [Route("GetClientByEmail")]
        public async Task<IActionResult> GetClientByEmailAsync(EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var client = await _dataContext.Clients
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.User.Email.ToLower() == request.Email.ToLower());

            if (client == null)
            {
                return NotFound();
            }

            var response = new ClientResponse
            {
                Id = client.Id,
                FirstName = client.User.FirstName,
                LastName = client.User.LastName,
                Address = client.User.Address,
                Document = client.User.Document,
                Email = client.User.Email,
                PhoneNumber = client.User.PhoneNumber
         
            };

            return Ok(response);
        }
    }
}
