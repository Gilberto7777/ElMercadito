using System;
using System.Linq;
using System.Threading.Tasks;
using ElMercadito.Web.Data.Entities;
using ElMercadito.Web.Helpers;

namespace ElMercadito.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(
            DataContext context,
            IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRoles();
            var manager = await CheckUserAsync("1010", "Juan", "Zuluaga", "jzuluaga55@gmail.com", "350 634 2747", "Calle Luna Calle Sol", "Manager");
            var merchant = await CheckUserAsync("2020", "Juan", "Zuluaga", "jzuluaga55@hotmail.com", "350 634 2747", "Calle Luna Calle Sol", "Merchant");
            var client = await CheckUserAsync("2020", "Juan", "Zuluaga", "carlos.zuluaga@globant.com", "350 634 2747", "Calle Luna Calle Sol", "Client");
            await CheckBusinessTypesAsync();
            await CheckManagerAsync(manager);
            await CheckMerchantsAsync(merchant);
            await CheckClientsAsync(client);
            await CheckProductsAsync();
            await CheckOffersAsync();
        }

        private async Task CheckOffersAsync()
        {
            var merchant = _context.Merchants.FirstOrDefault();
            var client = _context.Clients.FirstOrDefault();
            var product = _context.Products.FirstOrDefault();
            if (!_context.Offers.Any())
            {
                _context.Offers.Add(new Offer
                {
                    OfferName = "Mayoreo solo hoy",
                    IsAvailable = "yes",
                    Client = client,
                    Merchant = merchant,
                    Price = 800000M,
                    Product = product,
                    Remarks = "El Mejor calcetin."
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckManagerAsync(User user)
        {
            if (!_context.Managers.Any())
            {
                _context.Managers.Add(new Manager { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, string role)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }


        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Manager");
            await _userHelper.CheckRoleAsync("Merchant");
            await _userHelper.CheckRoleAsync("Client");
        }

        private async Task CheckClientsAsync(User user)
        {
            if (!_context.Clients.Any())
            {
                _context.Clients.Add(new Client { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckProductsAsync()
        {
            var merchant = _context.Merchants.FirstOrDefault();
            var businessType = _context.BusinessTypes.FirstOrDefault();
            if (!_context.Products.Any())
            {
                AddProduct("calcetin", 20,"si","de caballero", businessType, merchant);
                AddProduct("tin", 10, "si", "de niño", businessType, merchant);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckBusinessTypesAsync()
        {
            if (!_context.BusinessTypes.Any())
            {
                _context.BusinessTypes.Add(new BusinessType { Name = "Calcetines", DescriptionBusiness ="", HomeService ="", WalkingOrLocal ="", WorkingHours ="" });
                _context.BusinessTypes.Add(new BusinessType { Name = "Carniceria", DescriptionBusiness = "", HomeService = "", WalkingOrLocal = "", WorkingHours = "" });
                _context.BusinessTypes.Add(new BusinessType { Name = "Papeleria", DescriptionBusiness = "", HomeService = "", WalkingOrLocal = "", WorkingHours = "" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckMerchantsAsync(User user)
        {
            if (!_context.Merchants.Any())
            {
                _context.Merchants.Add(new Merchant { User = user });
                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string productName, decimal price, string isAvailable,string remarks, BusinessType businessType,  Merchant merchant)
        {
            _context.Products.Add(new Product
            {
                ProductName = productName,
                Price = price,
                IsAvailable = isAvailable,
                Remarks = remarks,
                BusinessType = businessType,
                Merchant = merchant



            });
        }
    }
}

