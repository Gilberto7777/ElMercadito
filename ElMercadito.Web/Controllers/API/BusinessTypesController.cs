using ElMercadito.Web.Data;
using ElMercadito.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ElMercadito.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public BusinessTypesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/BusinessTypes
        [HttpGet]
        public IEnumerable<BusinessType> GetBusinessTypes()
        {
            return _context.BusinessTypes.OrderBy(bt => bt.Name);
        }


    }
}