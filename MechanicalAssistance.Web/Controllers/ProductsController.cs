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
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly ICombosHelper _combosHelper;

        public ProductsController(
          DataContext context,
          IImageHelper imageHelper,
          IConverterHelper converterHelper,
          ICombosHelper combosHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
            _combosHelper = combosHelper;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(P => P.ProductBrand).ToListAsync());
        }



        // GET: Products/Create
        public IActionResult Create()
        {
            ProductViewModel model = new ProductViewModel
            {
                ProductBrands = _combosHelper.GetComboProductsBrand()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (productViewModel.ProductFile != null)
                {
                    path = await _imageHelper.UploadImageAsync(productViewModel.ProductFile, "Products");
                }


                ProductEntity productEntity =  await _converterHelper.ToProductEntityAsync(productViewModel, path, true);
               
                _context.Add(productEntity);

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

            productViewModel.ProductBrands = _combosHelper.GetComboProductsBrand();

            return View(productViewModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productEntity = await _context.Products.Include(p => p.ProductBrand).FirstOrDefaultAsync(p => p.Id == id);

           
            if (productEntity == null)
            {
                return NotFound();
            }

            ProductViewModel productViewModel = _converterHelper.ToProductViewModel(productEntity);
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                return NotFound();
            }

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
                    return RedirectToAction(nameof(Index));
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

            var productEntity = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productEntity == null)
            {
                return NotFound();
            }

            _context.Products.Remove(productEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductEntityExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
