using MechanicalAssistance.Web.Data;
using MechanicalAssistance.Web.Data.Entities;
using MechanicalAssistance.Web.Helpers;
using MechanicalAssistance.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;
        public int IdService;

        public ServicesController(
         DataContext context,
         IImageHelper imageHelper,
         IUserHelper userHelper,
         IConverterHelper converterHelper,
         ICombosHelper combosHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _combosHelper = combosHelper;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.Include(s => s.User).ToListAsync());
        }


        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceViewModel serviceViewModel)
        {

            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (serviceViewModel.ServiceFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(serviceViewModel.ServiceFile, "Services");
                }

                serviceViewModel.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                MechanicalServiceEntity serviceEntity = _converterHelper.ToServiceEntity(serviceViewModel, path, true);

                _context.Add(serviceEntity);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }

            return View(serviceViewModel);


        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanicalServiceEntity = await _context.Services.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
            if (mechanicalServiceEntity == null)
            {
                return NotFound();
            }

            ServiceViewModel serviceViewModel = _converterHelper.ToServiceViewModel(mechanicalServiceEntity);
            return View(serviceViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServiceViewModel serviceViewModel)
        {
            if (id != serviceViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string path = serviceViewModel.LogoPath;

                if (serviceViewModel.ServiceFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(serviceViewModel.ServiceFile, "Services");
                }

                serviceViewModel.User = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                MechanicalServiceEntity serviceEntity = _converterHelper.ToServiceEntity(serviceViewModel, path, false);

                _context.Update(serviceEntity);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }

            return View(serviceViewModel);

        }

        public async Task<IActionResult> Details(int id)
        {
    
            var model = await _context.Services
               .Include(p => p.Products)
               .ThenInclude(p => p.ProductBrand)
               .FirstOrDefaultAsync(s => s.Id == id);
           
            return View(model);
        }

        // GET:
        public IActionResult CreateProduct(int id)
        {

            ProductViewModel model = new ProductViewModel
            {
                ProductBrands = _combosHelper.GetComboProductsBrand(),
                ServiceId = id
                 
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (productViewModel.ProductFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(productViewModel.ProductFile, "Products");
                }

                ProductEntity productEntity = await _converterHelper.ToProductEntityAsync(productViewModel, path, true);

                _context.Add(productEntity);

                try
                {
                    await _context.SaveChangesAsync();

                    return RedirectToAction($"{nameof(Details)}/{productViewModel.ServiceId}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }

            productViewModel.ProductBrands = _combosHelper.GetComboProductsBrand();

            return View(productViewModel);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productEntity = await _context.Products.Include(p => p.ProductBrand).Include(s => s.Service).FirstOrDefaultAsync(p => p.Id == id);


            if (productEntity == null)
            {
                return NotFound();
            }

            ProductViewModel productViewModel = _converterHelper.ToProductViewModel(productEntity);
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductViewModel productViewModel)
        {

            if (ModelState.IsValid)
            {
                string path = productViewModel.Photo;

                if (productViewModel.ProductFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(productViewModel.ProductFile, "Products");
                }

                ProductEntity productEntity = await _converterHelper.ToProductEntityAsync(productViewModel, path, false);
                _context.Update(productEntity);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}/{productViewModel.ServiceId}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }

            return View(productViewModel);


        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var productEntity = await _context.Products.Where(p => p.Service.Id == id).ToListAsync();

            _context.Products.RemoveRange(productEntity);

            var mechanicalServiceEntity = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mechanicalServiceEntity == null)
            {
                return NotFound();
            }

            _context.Services.Remove(mechanicalServiceEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool MechanicalServiceEntityExists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
