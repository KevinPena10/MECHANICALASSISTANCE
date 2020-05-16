using MechanicalAssistance.Web.Data;
using MechanicalAssistance.Web.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBrandsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductBrandsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<ProductBrandEntity> GetProductBrands()
        {

            return _context.ProductBrands.OrderBy(p => p.BrandName);
        }

    }
}
