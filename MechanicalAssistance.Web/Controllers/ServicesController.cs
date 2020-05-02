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

        public ServicesController(
         DataContext context,
         IImageHelper imageHelper,
         IUserHelper userHelper,
         IConverterHelper converterHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            return View(await _context.Services.Include(s => s.User).ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mechanicalServiceEntity = await _context.Services
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mechanicalServiceEntity == null)
            {
                return NotFound();
            }

            return View(mechanicalServiceEntity);
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

                MechanicalServiceEntity serviceEntity =  _converterHelper.ToServiceEntity(serviceViewModel, path, true);

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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
