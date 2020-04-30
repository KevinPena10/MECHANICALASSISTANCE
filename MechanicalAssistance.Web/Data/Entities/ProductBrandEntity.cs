using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MechanicalAssistance.Web.Data.Entities
{
    public class ProductBrandEntity
    {
        [Display(Name = "#")]
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        public ICollection<ProductEntity> Products { get; set; }
    }
}
