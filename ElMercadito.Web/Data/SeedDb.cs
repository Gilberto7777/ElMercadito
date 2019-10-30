using ElMercadito.Web.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ElMercadito.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckBusinessTypesAsync();
            await CheckMerchantsAsync();
            await CheckClientsAsync();
            await CheckProductsAsync();
        }

        private async Task CheckBusinessTypesAsync()
        {
            if (!_context.BusinessTypes.Any())
            {
                _context.BusinessTypes.Add(new Entities.BusinessType { Name = "Calcetines", DescriptionBusiness = "Buenos", HomeService = "", WalkingOrLocal = "", WorkingHours = "" });
                _context.BusinessTypes.Add(new Entities.BusinessType { Name = "Carniceria", DescriptionBusiness = "rica", HomeService = "", WalkingOrLocal = "", WorkingHours = "" });
                _context.BusinessTypes.Add(new Entities.BusinessType { Name = "Papeleria", DescriptionBusiness = "basica", HomeService = "", WalkingOrLocal = "", WorkingHours = "" });
                await _context.SaveChangesAsync();
            }
        }



        private async Task CheckClientsAsync()
        {
            if (!_context.Clients.Any())
            {
                AddClient("1", "Ramon", "Gamboa", "234323", "Calle Luna Calle Sol");
                await _context.SaveChangesAsync();
            }
        }

        private void AddClient(
            string document,
            string firstName,
            string lastName,
            string cellPhone,
            string address)
        {
            _context.Clients.Add(new Client
            {
                Address = address,
                CellPhone = cellPhone,
                Document = document,
                FirstName = firstName,
                LastName = lastName
            });
        }

        private async Task CheckProductsAsync()
        {
            var merchant = _context.Merchants.FirstOrDefault();
            var businessType = _context.BusinessTypes.FirstOrDefault();
            if (!_context.Products.Any())
            {
                AddProduct("Calcetines", merchant, businessType, "buenos", 10M, "si");
                AddProduct("tines", merchant, businessType, "niño", 10M, "no");
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckMerchantsAsync()
        {
            if (!_context.Merchants.Any())
            {
                AddMerchant("8989898", "Juan", "Zuluaga", "310 322 3221", "Calle Luna Calle Sol");
                AddMerchant("7655544", "Jose", "Cardona", "300 322 3221", "Calle 77 #22 21");
                AddMerchant("6565555", "Maria", "López", "350 322 3221", "Carrera 56 #22 21");
                await _context.SaveChangesAsync();
            }
        }

        private void AddMerchant(
            string document,
            string firstName,
            string lastName,
            string cellPhone,
            string address)
        {
            _context.Merchants.Add(new Merchant
            {
                Address = address,
                CellPhone = cellPhone,
                Document = document,
                FirstName = firstName,
                LastName = lastName
            });
        }

        private void AddProduct(
            string productName,
            Merchant merchant,
            BusinessType businessType,
            string remarks,
            decimal price,
            string isAvailable
            )
        {
            _context.Products.Add(new Product
            {
                ProductName = productName,
                Remarks = remarks,
                IsAvailable = isAvailable,
                Merchant = merchant,
                Price = price,
                BusinessType = businessType,
            });
        }
    }
}
