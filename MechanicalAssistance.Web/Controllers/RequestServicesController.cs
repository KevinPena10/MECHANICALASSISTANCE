using MechanicalAssistance.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Controllers
{
    public class RequestServicesController : Controller
    {
        private readonly DataContext _context;

        public RequestServicesController(DataContext context)
        {
            _context = context;
        }

        // GET: RequestService
        public async Task<IActionResult> Index()
        {
            return View(await _context.RequestServices.Include(u => u.User).Include(s => s.Service).Include(s => s.Service.User).ToListAsync());
        }
    }
}
