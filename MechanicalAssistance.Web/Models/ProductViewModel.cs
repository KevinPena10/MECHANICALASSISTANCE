using MechanicalAssistance.Web.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MechanicalAssistance.Web.Models
{
    public class ProductViewModel : ProductEntity
    {
        [Display(Name = "Product Photo")]
        public IFormFile ProductFile { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Brand of the product")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Brand.")]
        public int ProductBrandId { get; set; }

        public IEnumerable<SelectListItem> ProductBrands { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public int ServiceId { get; set; }
    }
}
