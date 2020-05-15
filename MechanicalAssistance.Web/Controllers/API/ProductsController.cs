using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Web.Data;
using MechanicalAssistance.Web.Data.Entities;
using MechanicalAssistance.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;
        public ProductsController(DataContext dataContext, IUserHelper userHelper, IConverterHelper converterHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            List<ProductEntity> product = await _dataContext.Products.
                                      Include(b => b.ProductBrand).
                                      Include(u => u.Service).
                                      ToListAsync();

            return Ok(_converterHelper.ToProductsResponse(product));
        }


    }
}
