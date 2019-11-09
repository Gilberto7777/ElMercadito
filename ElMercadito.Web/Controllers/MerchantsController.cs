using ElMercadito.Web.Data;
using ElMercadito.Web.Data.Entities;
using ElMercadito.Web.Helpers;
using ElMercadito.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMercadito.Web.Controllers
{
    [Authorize(Roles = "Manager")]
    public class MerchantsController : Controller
    {

        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;

        public MerchantsController(
            DataContext dataContext,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
        }

        // GET: Merchants
        public IActionResult Index()
        {
            return View(_dataContext.Merchants
                .Include(m => m.User)
                .Include(m => m.Products)
                .Include(m => m.Offers));
        }

        // GET: Merchants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _dataContext.Merchants
                .Include(m => m.User)
                .Include(m => m.Products)
                .ThenInclude(p => p.ProductImages)
                .Include(o => o.Offers)
                .ThenInclude(c => c.Client)
                .ThenInclude(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (merchant == null)
            {
                return NotFound();
            }

            return View(merchant);
        }

        // GET: Merchants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Merchants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await CreateUserAsync(model);
                if (user != null)
                {
                    var merchant = new Merchant
                    {
                        Offers = new List<Offer>(),
                        Products = new List<Product>(),
                        User = user
                    };
                    _dataContext.Merchants.Add(merchant);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "User with this email already exist.");
            }
            return View(model);
        }

        private async Task<User> CreateUserAsync(AddUserViewModel model)
        {
            var user = new User
            {
                Address = model.Address,
                Document = model.Document,
                Email = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Username

            };
            var result = await _userHelper.AddUserAsync(user, model.Password);
            if (result.Succeeded)
            {
                user = await _userHelper.GetUserByEmailAsync(model.Username);
                await _userHelper.AddUserToRoleAsync(user, "Merchant");
                return user;
            }
            return null;
        }

        // GET: Merchants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _dataContext.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }
            return View(merchant);
        }

        // POST: Merchants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Merchant merchant)
        {
            if (id != merchant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(merchant);
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MerchantExists(merchant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(merchant);
        }

        // GET: Merchants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _dataContext.Merchants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (merchant == null)
            {
                return NotFound();
            }

            return View(merchant);
        }

        // POST: Merchants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var merchant = await _dataContext.Merchants.FindAsync(id);
            _dataContext.Merchants.Remove(merchant);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MerchantExists(int id)
        {
            return _dataContext.Merchants.Any(e => e.Id == id);
        }
        public async Task<IActionResult> AddProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var merchant = await _dataContext.Merchants.FindAsync(id);
            if (merchant == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                MerchantId = merchant.Id,
                BusinessTypes = _combosHelper.GetComboBusinessTypes()

            };

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await _converterHelper.ToProductAsync(model, true);
                _dataContext.Products.Add(product);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Details/{model.Merchant.Id}");
            }

            return View(model);
        
        }


    }
}
