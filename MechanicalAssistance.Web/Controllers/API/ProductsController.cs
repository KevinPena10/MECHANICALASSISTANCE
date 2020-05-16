using MechanicalAssistance.Common.Models;
using MechanicalAssistance.Web.Data;
using MechanicalAssistance.Web.Data.Entities;
using MechanicalAssistance.Web.Helpers;
using MechanicalAssistance.Web.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;
        public ProductsController(DataContext dataContext, IUserHelper userHelper, IConverterHelper converterHelper, IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> PostProducts([FromBody] ProductRequest productRequest)
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


            CultureInfo cultureInfo = new CultureInfo(productRequest.CultureInfo);
            Resource.Culture = cultureInfo;

            MechanicalServiceEntity serviceEntity = await _dataContext.Services.FindAsync(productRequest.ServiceId);

            if (serviceEntity == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = Resource.ServiceValidation
                });
            }

            ProductBrandEntity productBrandEntity = await _dataContext.ProductBrands.FindAsync(productRequest.ProductBrandId);

            if (productBrandEntity == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = Resource.ProductBrandValidation
                });
            }

            string picturePath = string.Empty;
            if (productRequest.Photo != null && productRequest.Photo.Length > 0)
            {
                picturePath = _imageHelper.UploadImage(productRequest.Photo, "Products");
            }

            ProductEntity product = new ProductEntity
            {
                ProductName = productRequest.ProductName,
                Description = productRequest.Description,
                Price = productRequest.Price,
                Photo = picturePath,
                Service = await _dataContext.Services.FirstOrDefaultAsync(s => s.Id == productRequest.ServiceId),
                ProductBrand = await _dataContext.ProductBrands.FirstOrDefaultAsync(p => p.Id == productRequest.ProductBrandId),
            };

            _dataContext.Products.Add(product);
            await _dataContext.SaveChangesAsync();

            return Ok(new Response
            {
                IsSuccess = true,
                Message = Resource.ProductMessage
            });

        }

    }
}
