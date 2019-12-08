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
        private readonly IImageHelper _imageHelper;

        public MerchantsController(
            DataContext dataContext,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
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

            var merchant = await _dataContext.Merchants
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (merchant == null)
            {
                return NotFound();
            }

            var model = new EditUserViewModel
            {
                Address = merchant.User.Address,
                Document = merchant.User.Document,
                FirstName = merchant.User.FirstName,
                Id = merchant.Id,
                LastName = merchant.User.LastName,
                PhoneNumber = merchant.User.PhoneNumber
            };

            return View(model);
        }


        // POST: Merchants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var merchant = await _dataContext.Merchants
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == model.Id);

                merchant.User.Document = model.Document;
                merchant.User.FirstName = model.FirstName;
                merchant.User.LastName = model.LastName;
                merchant.User.Address = model.Address;
                merchant.User.PhoneNumber = model.PhoneNumber;

                await _userHelper.UpdateUserAsync(merchant.User);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }


        // GET: Merchants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var merchant = await _dataContext.Merchants
                .Include(o => o.User)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (merchant == null)
            {
                return NotFound();
            }
            if (merchant.Products.Count != 0)
            {
                ModelState.AddModelError(string.Empty, "El comerciante no se puede borrar tiene productos.");
                return RedirectToAction(nameof(Index));
            }


                

            _dataContext.Merchants.Remove(merchant);
            await _dataContext.SaveChangesAsync();
            await _userHelper.DeleteUserAsync(merchant.User.Email);
            return RedirectToAction(nameof(Index));
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
                return RedirectToAction($"Details/{model.MerchantId}");
            }

            model.BusinessTypes = _combosHelper.GetComboBusinessTypes();
            return View(model);
        
        }
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _dataContext.Products
                .Include(p => p.Merchant)
                .Include(p => p.BusinessType)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var model = _converterHelper.ToProductViewModel(product);

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await _converterHelper.ToProductAsync(model, false);
                _dataContext.Products.Update(product);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Details/{model.MerchantId}");
            }

            return View(model);

        }

        public async Task<IActionResult> DetailsProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _dataContext.Products
                .Include(o => o.Merchant)
                .ThenInclude(o => o.User)
                .Include(o => o.Offers)
                .ThenInclude(c => c.Client)
                .ThenInclude(l => l.User)
                .Include(o => o.BusinessType)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }



        public async Task<IActionResult> AddImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _dataContext.Products.FindAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductImageViewModel
            {
                Id = product.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(ProductImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);
                }

                var productImage = new ProductImage
                {
                    ImageUrl = path,
                    Product = await _dataContext.Products.FindAsync(model.Id)
                };

                _dataContext.ProductImages.Add(productImage);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"{nameof(DetailsProduct)}/{model.Id}");
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _dataContext.ProductImages
                .Include(pi => pi.Product)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (productImage == null)
            {
                return NotFound();
            }

            _dataContext.ProductImages.Remove(productImage);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(DetailsProduct)}/{productImage.Product.Id}");
        }


        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _dataContext.Products
                .Include(p => p.Merchant)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(pi => pi.Id == id.Value);
            if (product == null)
            {
                return NotFound();
            }

            _dataContext.ProductImages.RemoveRange(product.ProductImages);
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(Details)}/{product.Merchant.Id}");
        }
    }
}
