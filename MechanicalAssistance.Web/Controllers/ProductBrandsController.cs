using MechanicalAssistance.Web.Data;
using MechanicalAssistance.Web.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Controllers
{
    public class ProductBrandsController : Controller
    {
        private readonly DataContext _context;

        public ProductBrandsController(DataContext context)
        {
            _context = context;
        }

        // GET: ProductBrands
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductBrands.ToListAsync());
        }

        // GET: ProductBrands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBrandEntity = await _context.ProductBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productBrandEntity == null)
            {
                return NotFound();
            }

            return View(productBrandEntity);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductBrandEntity productBrandEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productBrandEntity);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "This name of the brand already exists");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }

            }
            return View(productBrandEntity);
        }


        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBrandEntity = await _context.ProductBrands.FindAsync(id);
            if (productBrandEntity == null)
            {
                return NotFound();
            }
            return View(productBrandEntity);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductBrandEntity productBrandEntity)
        {
            if (id != productBrandEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(productBrandEntity);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    if (ex.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "This name of the brand already exists");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                    }
                }
            }
            return View(productBrandEntity);
        }


        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productBrandEntity = await _context.ProductBrands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productBrandEntity == null)
            {
                return NotFound();
            }

            _context.ProductBrands.Remove(productBrandEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool ProductBrandEntityExists(int id)
        {
            return _context.ProductBrands.Any(e => e.Id == id);
        }
    }
}
